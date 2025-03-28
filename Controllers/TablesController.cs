using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using Microsoft.Extensions.Configuration;
using OfficeOpenXml;
using static QuizeManagement_Project.Controllers.FormsController;
using QuizeManagement_Project.Filters;
using System; 

namespace QuizeManagement_Project.Controllers
{
    [AuthorizeSession]
    public class TablesController : Controller
    {
        private IConfiguration configuration;

        public TablesController(IConfiguration _configuration)
        {
            configuration = _configuration;
        }

        public IActionResult QuizList()
        {
            string connectionString = this.configuration.GetConnectionString("ConnectionString");
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "[dbo].[MST_Quiz_SelectAll]";
            SqlDataReader reader = command.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);
            return View(table);
        }

        public IActionResult QuizDelete(int QuizID)
        {
            try
            {
                string connectionString = configuration.GetConnectionString("ConnectionString");
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = connection.CreateCommand();
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "[dbo].[MST_Quiz_DeleteByPK]";
                    command.Parameters.Add("@QuizID", SqlDbType.Int).Value = QuizID;
                    command.ExecuteNonQuery();
                }

                TempData["SuccessMessage"] = "Quiz deleted successfully.";
                return RedirectToAction("QuizList");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "ERROR: This Quiz cannot be deleted because Questions are associated with it. " +
                    "Please remove Questions from this Quiz First.";
                return RedirectToAction("QuizList");
            }
        }
        public IActionResult QuizExportToExcel()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            string connectionString = configuration.GetConnectionString("ConnectionString");
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();
            SqlCommand sqlCommand = sqlConnection.CreateCommand();
            sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
            sqlCommand.CommandText = "MST_Quiz_SelectAll";
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            DataTable data = new DataTable();
            data.Load(sqlDataReader);

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("QuizData");

                // Add headers
                worksheet.Cells[1, 1].Value = "QuizID";
                worksheet.Cells[1, 2].Value = "QuizName";
                worksheet.Cells[1, 3].Value = "TotalQuestions";
                worksheet.Cells[1, 4].Value = "UserID";
                worksheet.Cells[1, 5].Value = "Created";
                worksheet.Cells[1, 6].Value = "Modified";

                // Add data
                int row = 2;
                foreach (DataRow item in data.Rows)
                {
                    worksheet.Cells[row, 1].Value = item["QuizID"];
                    worksheet.Cells[row, 2].Value = item["QuizName"];
                    worksheet.Cells[row, 3].Value = item["TotalQuestions"];
                    worksheet.Cells[row, 4].Value = item["UserID"];
                    worksheet.Cells[row, 5].Value = item["Created"];
                    worksheet.Cells[row, 6].Value = item["Modified"];
                    row++;
                }

                var stream = new MemoryStream();
                package.SaveAs(stream);
                stream.Position = 0;

                string excelName = $"QuizData-{DateTime.Now:yyyyMMddHHmmss}.xlsx";
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
            }
        }
        public IActionResult QuestionList()
        {
            string connectionString = this.configuration.GetConnectionString("ConnectionString");
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "[dbo].[MST_Question_SelectAll]";
            SqlDataReader reader = command.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);
            return View(table);
        }

        public IActionResult QuestionDelete(int QuestionID)
        {
            try
            {
                string connectionString = configuration.GetConnectionString("ConnectionString");
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = connection.CreateCommand();
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "[dbo].[MST_Question_DeleteByPK]";
                    command.Parameters.Add("@QuestionID", SqlDbType.Int).Value = QuestionID;


                    command.ExecuteNonQuery();
                }

                TempData["SuccessMessage"] = "Question deleted successfully.";
                return RedirectToAction("QuestionList");
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("FOREIGN KEY constraint", StringComparison.OrdinalIgnoreCase) || ex.Message.Contains("REFERENCE constraint", StringComparison.OrdinalIgnoreCase))
                {
                    TempData["ErrorMessage"] = "ERROR: This Question cannot be deleted because it is linked with a Quiz. " +
                                                "Please unlink this Question from associated Quizzes first.";
                }
                else
                {
                    TempData["ErrorMessage"] = "An error occurred while deleting the Question: " + ex.Message;
                }
                return RedirectToAction("QuestionList");
            }
        }

        public IActionResult QuestionExportToExcel()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            string connectionString = configuration.GetConnectionString("ConnectionString");
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();
            SqlCommand sqlCommand = sqlConnection.CreateCommand();
            sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
            sqlCommand.CommandText = "[dbo].[MST_Question_SelectAll]";
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            DataTable data = new DataTable();
            data.Load(sqlDataReader);

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("QuestionData");

                // Add headers
                worksheet.Cells[1, 1].Value = "QuestionID";
                worksheet.Cells[1, 2].Value = "QuestionText";
                worksheet.Cells[1, 3].Value = "QuestionLevelID";
                worksheet.Cells[1, 4].Value = "OptionA";
                worksheet.Cells[1, 5].Value = "OptionB";
                worksheet.Cells[1, 6].Value = "OptionC";
                worksheet.Cells[1, 7].Value = "OptionD";
                worksheet.Cells[1, 8].Value = "CorrectOption";
                worksheet.Cells[1, 9].Value = "QuestionMarks";
                worksheet.Cells[1, 10].Value = "IsActive";
                worksheet.Cells[1, 11].Value = "UserID";
                worksheet.Cells[1, 12].Value = "Created";
                worksheet.Cells[1, 13].Value = "Modified";

                // Add data
                int row = 2;
                foreach (DataRow item in data.Rows)
                {
                    worksheet.Cells[row, 1].Value = item["QuestionID"];
                    worksheet.Cells[row, 2].Value = item["QuestionText"];
                    worksheet.Cells[row, 3].Value = item["QuestionLevelID"];
                    worksheet.Cells[row, 4].Value = item["OptionA"];
                    worksheet.Cells[row, 5].Value = item["OptionB"];
                    worksheet.Cells[row, 6].Value = item["OptionC"];
                    worksheet.Cells[row, 7].Value = item["OptionD"];
                    worksheet.Cells[row, 8].Value = item["CorrectOption"];
                    worksheet.Cells[row, 9].Value = item["QuestionMarks"];
                    worksheet.Cells[row, 10].Value = item["IsActive"];
                    worksheet.Cells[row, 11].Value = item["UserID"];
                    worksheet.Cells[row, 12].Value = item["Created"];
                    worksheet.Cells[row, 13].Value = item["Modified"];
                    row++;
                }

                var stream = new MemoryStream();
                package.SaveAs(stream);
                stream.Position = 0;

                string excelName = $"QuestionData-{DateTime.Now:yyyyMMddHHmmss}.xlsx";
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
            }
        }

        public IActionResult QuestionLevelList()
        {
            string connectionString = this.configuration.GetConnectionString("ConnectionString");
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "[dbo].[MST_QuestionLevel_SelectAll]";
            SqlDataReader reader = command.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);
            return View(table);
        }

        public IActionResult QuestionLevelDelete(int QuestionLevelID)
        {
            try
            {
                string connectionString = configuration.GetConnectionString("ConnectionString");
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = connection.CreateCommand();
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "[dbo].[MST_QuestionLevel_DeleteByPK]";
                    command.Parameters.Add("@QuestionLevelID", SqlDbType.Int).Value = QuestionLevelID;
                    command.ExecuteNonQuery();
                }

                TempData["SuccessMessage"] = "Question Level deleted successfully.";
                return RedirectToAction("QuestionLevelList");
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("FOREIGN KEY constraint", StringComparison.OrdinalIgnoreCase) || ex.Message.Contains("REFERENCE constraint", StringComparison.OrdinalIgnoreCase))
                {
                    TempData["ErrorMessage"] = "ERROR: This Question Level cannot be deleted because Questions are associated with it. " +
                                                "Please remove this Question level from Questions first.";

                }
                else
                {
                    TempData["ErrorMessage"] = "An error occurred while deleting the QuestionLevel: " + ex.Message;
                }
                return RedirectToAction("QuestionLevelList");
            }
        }

        public IActionResult QuestionLevelExportToExcel()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            string connectionString = configuration.GetConnectionString("ConnectionString");
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();
            SqlCommand sqlCommand = sqlConnection.CreateCommand();
            sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
            sqlCommand.CommandText = "[dbo].[MST_QuestionLevel_SelectAll]";
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            DataTable data = new DataTable();
            data.Load(sqlDataReader);

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("QuestionLevelData");

                worksheet.Cells[1, 1].Value = "QuestionLevelID";
                worksheet.Cells[1, 2].Value = "QuestionLevel";
                worksheet.Cells[1, 3].Value = "UserID";
                worksheet.Cells[1, 4].Value = "Created";
                worksheet.Cells[1, 5].Value = "Modified";

                // Add data
                int row = 2;
                foreach (DataRow item in data.Rows)
                {
                    worksheet.Cells[row, 1].Value = item["QuestionLevelID"];
                    worksheet.Cells[row, 2].Value = item["QuestionLevel"];
                    worksheet.Cells[row, 3].Value = item["UserID"];
                    worksheet.Cells[row, 4].Value = item["Created"];
                    worksheet.Cells[row, 5].Value = item["Modified"];
                    row++;
                }

                var stream = new MemoryStream();
                package.SaveAs(stream);
                stream.Position = 0;

                string excelName = $"QuestionLevelData-{DateTime.Now:yyyyMMddHHmmss}.xlsx";
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
            }
        }

        public IActionResult AddQuizwiseQuestionsList()
        {
            string connectionString = this.configuration.GetConnectionString("ConnectionString");
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "[dbo].[MST_QuizWiseQuestions_SelectAll]";
            SqlDataReader reader = command.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);
            return View(table);
        }

        public IActionResult QuizwiseQuestionsDelete(int QuizWiseQuestionsID)
        {
            try
            {
                string connectionString = configuration.GetConnectionString("ConnectionString");
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = connection.CreateCommand();
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "[dbo].[MST_QuizWiseQuestions_DeleteByPK]";
                    command.Parameters.Add("@QuizWiseQuestionsID", SqlDbType.Int).Value = QuizWiseQuestionsID;
                    command.ExecuteNonQuery();
                }

                TempData["SuccessMessage"] = "Quiz Wise Questions deleted successfully.";
                return RedirectToAction("AddQuizwiseQuestionsList");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while deleting the Quiz Wise Questions: " + ex.Message;
                return RedirectToAction("AddQuizwiseQuestionsList");
            }
        }

        public IActionResult QuizWiseQuestionsExportToExcel()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            string connectionString = configuration.GetConnectionString("ConnectionString");
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();
            SqlCommand sqlCommand = sqlConnection.CreateCommand();
            sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
            sqlCommand.CommandText = "[dbo].[MST_QuizWiseQuestions_SelectAll]";
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            DataTable data = new DataTable();
            data.Load(sqlDataReader);

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("QuizWiseQuestionsData");

                worksheet.Cells[1, 1].Value = "QuizWiseQuestionsID";
                worksheet.Cells[1, 2].Value = "QuizID";
                worksheet.Cells[1, 3].Value = "QuizName";
                worksheet.Cells[1, 4].Value = "QuestionID";
                worksheet.Cells[1, 5].Value = "QuestionText";
                worksheet.Cells[1, 6].Value = "UserID";
                worksheet.Cells[1, 7].Value = "Created";
                worksheet.Cells[1, 8].Value = "Modified";

                // Add data
                int row = 2;
                foreach (DataRow item in data.Rows)
                {
                    worksheet.Cells[row, 1].Value = item["QuizWiseQuestionsID"];
                    worksheet.Cells[row, 2].Value = item["QuizID"];
                    worksheet.Cells[row, 3].Value = item["QuizName"];
                    worksheet.Cells[row, 4].Value = item["QuestionID"];
                    worksheet.Cells[row, 5].Value = item["QuestionText"];
                    worksheet.Cells[row, 6].Value = item["UserID"];
                    worksheet.Cells[row, 7].Value = item["Created"];
                    worksheet.Cells[row, 8].Value = item["Modified"];
                    row++;
                }

                var stream = new MemoryStream();
                package.SaveAs(stream);
                stream.Position = 0;

                string excelName = $"QuizWiseQuestionsDataWithDetails-{DateTime.Now:yyyyMMddHHmmss}.xlsx";
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
            }
        }
    }
}
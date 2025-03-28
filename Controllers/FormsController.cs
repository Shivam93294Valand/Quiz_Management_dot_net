using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using QuizeManagement_Project.Models;
using System.Reflection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering; 

namespace QuizeManagement_Project.Controllers
{
    public class FormsController : Controller
    {
        private IConfiguration configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public FormsController(IConfiguration _configuration, IHttpContextAccessor httpContextAccessor)
        {
            configuration = _configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        public IActionResult CreateAccountForm()
        {
            return View();
        }
        public IActionResult LoginAccountForm()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateAccountAddEdit(CreateAccountModel model)
        {
            if (ModelState.IsValid)
            {
                string connectionString = this.configuration.GetConnectionString("ConnectionString");
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "[dbo].[MST_User_Insert]";

                        command.Parameters.Add("@UserName", SqlDbType.VarChar).Value = model.userName;
                        command.Parameters.Add("@Password", SqlDbType.VarChar).Value = model.password;
                        command.Parameters.Add("@Mobile", SqlDbType.VarChar).Value = model.mobile;
                        command.Parameters.Add("@Email", SqlDbType.VarChar).Value = model.email;
                        command.Parameters.Add("@IsActive", SqlDbType.Bit).Value = true;
                        command.Parameters.Add("@IsAdmin", SqlDbType.Bit).Value = false;
                        command.Parameters.Add("@Created", SqlDbType.DateTime).Value = DateTime.Now;
                        command.Parameters.Add("@Modified", SqlDbType.DateTime).Value = DateTime.Now;

                        command.ExecuteNonQuery();

                        return RedirectToAction("LoginAccountForm");
                    }
                }
            }

            return View("CreateAccountForm", model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult LoginAccountAddEdit(LoginAccountModel model)
        {
            if (ModelState.IsValid)
            {
                string connectionString = this.configuration.GetConnectionString("ConnectionString");
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "[dbo].[MST_User_SelectByUserNamePassword]";
                        command.Parameters.Add("@UserName", SqlDbType.VarChar).Value = model.Username;
                        command.Parameters.Add("@password", SqlDbType.VarChar).Value = model.Password;

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                reader.Read();
                                HttpContext.Session.SetInt32("UserID", Convert.ToInt32(reader["UserID"]));

                                return RedirectToAction("Index", "Home");
                            }
                            else
                            {
                                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                                return View("LoginAccountForm", model);
                            }
                        }
                    }
                }
            }
            return View("LoginAccountForm", model);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("LoginAccountForm");
        }


        public static class CommonVariable
        {
            public static int UserID(IHttpContextAccessor httpContextAccessor)
            {
                var session = httpContextAccessor.HttpContext?.Session;
                return session?.GetInt32("UserID") ?? 0;
            }
        }

        public void QuestionLevelDropDown()
        {
            string connectionString = this.configuration.GetConnectionString("ConnectionString");
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "[dbo].[MST_QuestionLevel_SelectAll]"; 
            SqlDataReader reader = command.ExecuteReader();
            DataTable dataTable = new DataTable();
            dataTable.Load(reader);
            List<QuestionLevelDropDownModel> questionLevelList = new List<QuestionLevelDropDownModel>();

            string[] levelColors = { "text-primary", "text-success", "text-danger", "text-warning", "text-info", "text-secondary" };
            int colorIndex = 0;

            foreach (DataRow data in dataTable.Rows)
            {
                QuestionLevelDropDownModel model = new QuestionLevelDropDownModel();
                model.QuestionLevelID = Convert.ToInt32(data["QuestionLevelID"]);
                model.QuestionLevel = data["QuestionLevel"].ToString();
                model.ColorClass = levelColors[colorIndex % levelColors.Length]; 
                questionLevelList.Add(model);
                colorIndex++;
            }
            ViewBag.QuestionLevelsList = questionLevelList;
        }

        public IActionResult CreateQuizForm()
        {
            var model = new CreateQuizModel
            {
                UserID = CommonVariable.UserID(_httpContextAccessor)
            };
            return View(model);
        }


        [HttpPost]
        public IActionResult CreateQuizAddEdit(CreateQuizModel model)
        {
            if (ModelState.IsValid)
            {
                string connectionString = this.configuration.GetConnectionString("ConnectionString");
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;

                if (model.QuizID == null || model.QuizID == 0)
                {
                    command.CommandText = "[dbo].[MST_Quiz_Insert]";
                }
                else
                {
                    command.CommandText = "[dbo].[MST_Quiz_UpdateByPK]";
                    command.Parameters.Add("@QuizID", SqlDbType.Int).Value = model.QuizID;
                }

                command.Parameters.Add("@QuizName", SqlDbType.VarChar).Value = model.QuizName;
                command.Parameters.Add("@TotalQuestions", SqlDbType.Int).Value = model.TotalQuestions;
                command.Parameters.Add("@QuizDate", SqlDbType.DateTime).Value = model.QuizDate;
                command.Parameters.Add("@UserID", SqlDbType.Int).Value = CommonVariable.UserID(_httpContextAccessor);


                command.ExecuteNonQuery();
                return RedirectToAction("QuizList", "Tables");
            }
            return View("CreateQuizForm", model);
        }

        public IActionResult EditQuizForm(int? QuizID)
        {
            if (QuizID == null)
            {
                return RedirectToAction("CreateQuizForm");
            }

            string connectionString = this.configuration.GetConnectionString("ConnectionString");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "[dbo].[MST_Quiz_SelectByPK]";
                    command.Parameters.AddWithValue("@QuizID", QuizID);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        DataTable table = new DataTable();
                        table.Load(reader);

                        CreateQuizModel model = new CreateQuizModel();

                        if (table.Rows.Count > 0)
                        {
                            DataRow dataRow = table.Rows[0];
                            model.QuizID = Convert.ToInt32(dataRow["QuizID"]);
                            model.QuizName = dataRow["QuizName"].ToString();
                            model.TotalQuestions = Convert.ToInt32(dataRow["TotalQuestions"]);
                            model.QuizDate = Convert.ToDateTime(dataRow["QuizDate"]);
                            model.UserID = Convert.ToInt32(dataRow["UserID"]);
                        }
                        else
                        {
                            return RedirectToAction("QuizList", "Tables");
                        }

                        return View("CreateQuizForm", model);
                    }
                }
            }
        }
        public IActionResult AddQuestionForm()
        {
            QuestionLevelDropDown(); 

            var model = new AddQuestionModel
            {
                IsActive = true,
                UserID = CommonVariable.UserID(_httpContextAccessor)
            };

            return View(model);
        }


        [HttpPost]
        public IActionResult AddQuestionAddEdit(AddQuestionModel model)
        {
            if (ModelState.IsValid)
            {
                string connectionString = this.configuration.GetConnectionString("ConnectionString");
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;

                if (model.QuestionID == 0)
                {
                    command.CommandText = "[dbo].[MST_Question_Insert]";
                }
                else
                {
                    command.CommandText = "[dbo].[MST_Question_UpdateByPK]";
                    command.Parameters.Add("@QuestionID", SqlDbType.Int).Value = model.QuestionID;
                }
                command.Parameters.Add("@QuestionText", SqlDbType.VarChar).Value = model.QuestionText;
                command.Parameters.Add("@QuestionLevelID", SqlDbType.Int).Value = model.QuestionLevelID;
                command.Parameters.Add("@OptionA", SqlDbType.VarChar).Value = model.OptionA;
                command.Parameters.Add("@OptionB", SqlDbType.VarChar).Value = model.OptionB;
                command.Parameters.Add("@OptionC", SqlDbType.VarChar).Value = model.OptionC;
                command.Parameters.Add("@OptionD", SqlDbType.VarChar).Value = model.OptionD;
                command.Parameters.Add("@CorrectOption", SqlDbType.VarChar).Value = model.CorrectOption;
                command.Parameters.Add("@QuestionMarks", SqlDbType.Int).Value = model.QuestionMarks;
                command.Parameters.Add("@IsActive", SqlDbType.Bit).Value = model.IsActive;
                command.Parameters.Add("@UserID", SqlDbType.Int).Value = CommonVariable.UserID(_httpContextAccessor);

                command.ExecuteNonQuery();
                return RedirectToAction("QuestionList", "Tables");
            }

            QuestionLevelDropDown(); 
            return View("AddQuestionForm", model);
        }

        public IActionResult EditQuestionForm(int? QuestionID)
        {
            if (QuestionID == null)
            {
                return RedirectToAction("AddQuestionForm");
            }

            QuestionLevelDropDown(); 

            string connectionString = this.configuration.GetConnectionString("ConnectionString");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "[dbo].[MST_Question_SelectByPK]";
                    command.Parameters.AddWithValue("@QuestionID", QuestionID);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        DataTable table = new DataTable();
                        table.Load(reader);

                        AddQuestionModel model = new AddQuestionModel();

                        if (table.Rows.Count > 0)
                        {
                            DataRow dataRow = table.Rows[0];
                            model.QuestionID = Convert.ToInt32(dataRow["QuestionID"]);
                            model.QuestionText = dataRow["QuestionText"].ToString();
                            model.QuestionLevelID = Convert.ToInt32(dataRow["QuestionLevelID"]);
                            model.OptionA = dataRow["OptionA"].ToString();
                            model.OptionB = dataRow["OptionB"].ToString();
                            model.OptionC = dataRow["OptionC"].ToString();
                            model.OptionD = dataRow["OptionD"].ToString();
                            model.CorrectOption = dataRow["CorrectOption"].ToString();
                            model.QuestionMarks = Convert.ToInt32(dataRow["QuestionMarks"]);
                            model.IsActive = Convert.ToBoolean(dataRow["IsActive"]);
                            model.UserID = Convert.ToInt32(dataRow["UserID"]);
                        }
                        else
                        {
                            return RedirectToAction("QuestionList", "Tables");
                        }


                        return View("AddQuestionForm", model);
                    }
                }
            }
        }

        public IActionResult AddQuestionLevelForm()
        {
            var model = new AddQuestionLevelModel
            {
                UserID = CommonVariable.UserID(_httpContextAccessor)
            };
            return View(model);
        }

        public IActionResult EditQuestionLevelForm(int? QuestionLevelID)
        {
            if (QuestionLevelID == null)
            {
                return RedirectToAction("AddQuestionLevelForm");
            }

            string connectionString = this.configuration.GetConnectionString("ConnectionString");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "[dbo].[MST_QuestionLevel_SelectByPK]";
                    command.Parameters.AddWithValue("@QuestionLevelID", QuestionLevelID);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        DataTable table = new DataTable();
                        table.Load(reader);

                        AddQuestionLevelModel model = new AddQuestionLevelModel();

                        if (table.Rows.Count > 0)
                        {
                            DataRow dataRow = table.Rows[0];
                            model.QuestionLevelID = Convert.ToInt32(dataRow["QuestionLevelID"]);
                            model.QuestionLevel = dataRow["QuestionLevel"].ToString();
                            model.UserID = Convert.ToInt32(dataRow["UserID"]);
                        }
                        else
                        {
                            return RedirectToAction("QuestionLevelList", "Tables");
                        }

                        return View("AddQuestionLevelForm", model);
                    }
                }
            }
        }


        [HttpPost]
        public IActionResult AddQuestionLevelAddEdit(AddQuestionLevelModel model)
        {
            if (ModelState.IsValid)
            {
                string connectionString = this.configuration.GetConnectionString("ConnectionString");
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;

                if (model.QuestionLevelID == 0)
                {
                    command.CommandText = "[dbo].[MST_QuestionLevel_Insert]";
                }
                else
                {
                    command.CommandText = "[dbo].[MST_QuestionLevel_UpdateByPK]";
                    command.Parameters.Add("@QuestionLevelID", SqlDbType.Int).Value = model.QuestionLevelID;
                }

                command.Parameters.Add("@QuestionLevel", SqlDbType.VarChar).Value = model.QuestionLevel;
                command.Parameters.Add("@UserID", SqlDbType.Int).Value = CommonVariable.UserID(_httpContextAccessor);

                command.ExecuteNonQuery();
                return RedirectToAction("QuestionLevelList", "Tables");
            }

            return View("AddQuestionLevelForm", model);
        }

        #region Quiz Dropdown
        public void QuizDropDown()
        {
            string connectionString = this.configuration.GetConnectionString("ConnectionString");
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "[dbo].[MST_Quiz_SelectAll]"; 
            SqlDataReader reader = command.ExecuteReader();
            DataTable dataTable = new DataTable();
            dataTable.Load(reader);
            List<QuizDropDownModel> quizList = new List<QuizDropDownModel>();
            foreach (DataRow data in dataTable.Rows)
            {
                QuizDropDownModel model = new QuizDropDownModel();
                model.QuizID = Convert.ToInt32(data["QuizID"]);
                model.QuizName = data["QuizName"].ToString();
                quizList.Add(model);
            }
            ViewBag.QuizzesList = quizList;
        }
        #endregion

        #region Question Dropdown
        public void QuestionDropDown()
        {
            string connectionString = this.configuration.GetConnectionString("ConnectionString");
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "[dbo].[MST_Question_SelectAll]"; 
            SqlDataReader reader = command.ExecuteReader();
            DataTable dataTable = new DataTable();
            dataTable.Load(reader);
            List<QuestionDropDownModel> questionList = new List<QuestionDropDownModel>();
            foreach (DataRow data in dataTable.Rows)
            {
                QuestionDropDownModel model = new QuestionDropDownModel();
                model.QuestionID = Convert.ToInt32(data["QuestionID"]);
                model.QuestionText = data["QuestionText"].ToString();
                questionList.Add(model);
            }
            ViewBag.QuestionsList = questionList;
        }
        #endregion

        public IActionResult AddQuizwiseQuestionsForm()
        {
            QuizDropDown(); 
            QuestionDropDown(); 

            var model = new AddQuizwiseQuestionModel
            {
                UserID = CommonVariable.UserID(_httpContextAccessor)
            };

            return View(model);
        }


        public IActionResult EditQuizwiseQuestionsForm(int? QuizWiseQuestionsID)
        {
            if (QuizWiseQuestionsID == null)
            {
                return RedirectToAction("AddQuizwiseQuestionsForm", "Forms");
            }

            QuizDropDown();
            QuestionDropDown(); 


            string connectionString = this.configuration.GetConnectionString("ConnectionString");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "[dbo].[MST_QuizWiseQuestions_SelectByPK]";
                    command.Parameters.AddWithValue("@QuizWiseQuestionsID", QuizWiseQuestionsID);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        DataTable table = new DataTable();
                        table.Load(reader);

                        AddQuizwiseQuestionModel model = new AddQuizwiseQuestionModel();

                        if (table.Rows.Count > 0)
                        {
                            DataRow dataRow = table.Rows[0];
                            model.QuizWiseQuestionsID = Convert.ToInt32(dataRow["QuizWiseQuestionsID"]);
                            model.QuizID = Convert.ToInt32(dataRow["QuizID"]);
                            model.QuestionID = Convert.ToInt32(dataRow["QuestionID"]);
                            model.UserID = Convert.ToInt32(dataRow["UserID"]);
                        }
                        else
                        {
                            return RedirectToAction("AddQuizwiseQuestionsList", "Tables");
                        }

                        return View("AddQuizwiseQuestionsForm", model);
                    }
                }
            }
        }


        [HttpPost]
        public IActionResult AddQuizwiseQuestionsAddEdit(AddQuizwiseQuestionModel model)
        {
            if (ModelState.IsValid)
            {
                string connectionString = this.configuration.GetConnectionString("ConnectionString");
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;

                if (model.QuizWiseQuestionsID == 0)
                {
                    command.CommandText = "[dbo].[MST_QuizWiseQuestions_Insert]";
                }
                else
                {
                    command.CommandText = "[dbo].[MST_QuizWiseQuestions_UpdateByPK]";
                    command.Parameters.Add("@QuizWiseQuestionsID", SqlDbType.Int).Value = model.QuizWiseQuestionsID;
                }
                command.Parameters.Add("@QuizID", SqlDbType.Int).Value = model.QuizID;
                command.Parameters.Add("@QuestionID", SqlDbType.Int).Value = model.QuestionID;
                command.Parameters.Add("@UserID", SqlDbType.Int).Value = CommonVariable.UserID(_httpContextAccessor);

                command.ExecuteNonQuery();
                return RedirectToAction("AddQuizwiseQuestionsList", "Tables");
            }

            QuizDropDown(); 
            QuestionDropDown(); 

            return View("AddQuizwiseQuestionsForm", model);
        }
    }
}
<div align="center">

# 🎯 Quiz Management System

### A Full-Stack ASP.NET Core MVC Web Application

[![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/)
[![C#](https://img.shields.io/badge/C%23-12.0-239120?style=for-the-badge&logo=csharp&logoColor=white)](https://learn.microsoft.com/en-us/dotnet/csharp/)
[![SQL Server](https://img.shields.io/badge/SQL_Server-CC2927?style=for-the-badge&logo=microsoftsqlserver&logoColor=white)](https://www.microsoft.com/en-us/sql-server)
[![Bootstrap](https://img.shields.io/badge/Bootstrap-5.3-7952B3?style=for-the-badge&logo=bootstrap&logoColor=white)](https://getbootstrap.com/)
[![License](https://img.shields.io/badge/License-MIT-yellow?style=for-the-badge)](LICENSE)

<br/>

<img src="https://raw.githubusercontent.com/devicons/devicon/master/icons/dotnetcore/dotnetcore-original.svg" width="80" alt=".NET Core"/>
&nbsp;&nbsp;&nbsp;
<img src="https://raw.githubusercontent.com/devicons/devicon/master/icons/csharp/csharp-original.svg" width="80" alt="C#"/>
&nbsp;&nbsp;&nbsp;
<img src="https://raw.githubusercontent.com/devicons/devicon/master/icons/microsoftsqlserver/microsoftsqlserver-plain-wordmark.svg" width="80" alt="SQL Server"/>
&nbsp;&nbsp;&nbsp;
<img src="https://raw.githubusercontent.com/devicons/devicon/master/icons/bootstrap/bootstrap-original.svg" width="80" alt="Bootstrap"/>
&nbsp;&nbsp;&nbsp;
<img src="https://raw.githubusercontent.com/devicons/devicon/master/icons/html5/html5-original.svg" width="80" alt="HTML5"/>

<br/><br/>

> **Quiz Management System** is a robust, feature-rich web application built with **ASP.NET Core 8.0 MVC** that enables users to create quizzes, manage questions with difficulty levels, link questions to quizzes, and export data to Excel — all with a beautiful modern UI using the **NiceAdmin** template.

</div>

---

## 📋 Table of Contents

- [✨ Features](#-features)
- [🏗️ Architecture Overview](#️-architecture-overview)
- [🗂️ Project Structure](#️-project-structure)
- [🔄 Application Workflow](#-application-workflow)
- [🗄️ Database Schema](#️-database-schema)
- [🧩 MVC Component Map](#-mvc-component-map)
- [⚙️ Tech Stack](#️-tech-stack)
- [🚀 Getting Started](#-getting-started)
- [📸 Screenshots](#-screenshots)
- [🤝 Contributing](#-contributing)

---

## ✨ Features

<table>
<tr>
<td width="50%">

### 🔐 Authentication & Security
- User Registration with validation
- Session-based Login / Logout
- Custom `[AuthorizeSession]` action filter
- Anti-forgery token protection (CSRF)
- 30-minute session timeout

</td>
<td width="50%">

### 📝 Quiz Management
- Create, Edit, Delete quizzes
- Set quiz name, total questions & date
- View all quizzes in data tables
- Search & filter quizzes
- Export quiz data to Excel (.xlsx)

</td>
</tr>
<tr>
<td width="50%">

### ❓ Question Management
- Add questions with 4 MCQ options (A-D)
- Set correct answer & marks
- Assign difficulty levels
- Active/Inactive status toggle
- Full CRUD operations

</td>
<td width="50%">

### 🔗 Quiz-Question Linking
- Link questions to specific quizzes
- Edit/Delete linked associations
- Dropdown selection for quizzes & questions
- Export linked data to Excel
- Constraint protection on delete

</td>
</tr>
<tr>
<td width="50%">

### 📊 Dashboard
- Summary cards (Quizzes, Questions, Levels)
- Progress indicators
- Modern glassmorphism UI
- Dark/Light theme support

</td>
<td width="50%">

### 📤 Data Export
- Export Quizzes to Excel
- Export Questions to Excel
- Export Question Levels to Excel
- Export Linked Questions to Excel
- Powered by EPPlus library

</td>
</tr>
</table>

---

## 🏗️ Architecture Overview

This project follows the **MVC (Model-View-Controller)** architectural pattern with **SQL Server** as the backend database, using **Stored Procedures** for all data operations.

```mermaid
graph TB
    subgraph "🖥️ Client Layer"
        Browser["🌐 Web Browser"]
    end

    subgraph "🔒 Middleware Pipeline"
        HTTPS["HTTPS Redirection"]
        Static["Static Files"]
        Routing["Routing"]
        Session["Session Management"]
        Auth["Authorization"]
    end

    subgraph "🎮 Controller Layer"
        FC["FormsController<br/>Authentication & Forms"]
        HC["HomeController<br/>Dashboard"]
        TC["TablesController<br/>Data Tables & Export"]
    end

    subgraph "🛡️ Filters"
        ASA["AuthorizeSession<br/>Attribute"]
    end

    subgraph "📦 Model Layer"
        CAM["CreateAccountModel"]
        LAM["LoginAccountModel"]
        CQM["CreateQuizModel"]
        AQM["AddQuestionModel"]
        AQLM["AddQuestionLevelModel"]
        AQWM["AddQuizwiseQuestionModel"]
        DDM["DropDownModels"]
    end

    subgraph "🎨 View Layer"
        SL["Shared Layouts<br/>_Layout · _LoginLayout"]
        FV["Form Views<br/>Login · Register · Quiz<br/>Question · Level · Link"]
        TV["Table Views<br/>QuizList · QuestionList<br/>LevelList · LinkedList"]
        HV["Home Views<br/>Dashboard · Privacy"]
    end

    subgraph "🗄️ Database Layer"
        SP["SQL Stored Procedures"]
        DB[("SQL Server<br/>EN_374 Database")]
    end

    Browser --> HTTPS --> Static --> Routing --> Session --> Auth
    Auth --> FC & HC & TC
    ASA -.-> HC & TC
    FC --> CAM & LAM & CQM & AQM & AQLM & AQWM & DDM
    FC --> FV
    HC --> HV
    TC --> TV
    FC & TC --> SP --> DB

    style Browser fill:#E3F2FD,stroke:#1565C0,color:#000
    style DB fill:#FFF3E0,stroke:#E65100,color:#000
    style ASA fill:#FCE4EC,stroke:#C62828,color:#000
```

---

## 🗂️ Project Structure

```
📦 Quiz_Management_dot_net
├── 📁 Controllers/                    # MVC Controllers
│   ├── 📄 FormsController.cs          # Auth + all form CRUD operations
│   ├── 📄 HomeController.cs           # Dashboard & home pages
│   └── 📄 TablesController.cs         # Data listing & Excel export
│
├── 📁 Filters/                        # Custom Action Filters
│   └── 📄 AuthorizeSessionAttribute.cs # Session-based auth guard
│
├── 📁 Models/                         # Data Models with Validation
│   ├── 📄 CreateAccountModel.cs       # User registration model
│   ├── 📄 LoginAccountModel.cs        # User login model
│   ├── 📄 CreateQuizModel.cs          # Quiz creation/edit model
│   ├── 📄 AddQuestionModel.cs         # Question CRUD model
│   ├── 📄 AddQuestionLevelModel.cs    # Difficulty level model
│   ├── 📄 AddQuizwiseQuestionModel.cs # Quiz-question link model
│   ├── 📄 DropDownModel.cs            # Dropdown list models
│   └── 📄 ErrorViewModel.cs           # Error display model
│
├── 📁 Views/                          # Razor Views
│   ├── 📁 Forms/                      # Input form views
│   │   ├── 📄 LoginAccountForm.cshtml
│   │   ├── 📄 CreateAccountForm.cshtml
│   │   ├── 📄 CreateQuizForm.cshtml
│   │   ├── 📄 AddQuestionForm.cshtml
│   │   ├── 📄 AddQuestionLevelForm.cshtml
│   │   └── 📄 AddQuizwiseQuestionsForm.cshtml
│   │
│   ├── 📁 Tables/                     # Data table views
│   │   ├── 📄 QuizList.cshtml
│   │   ├── 📄 QuestionList.cshtml
│   │   ├── 📄 QuestionLevelList.cshtml
│   │   └── 📄 AddQuizwiseQuestionsList.cshtml
│   │
│   ├── 📁 Home/                       # Home pages
│   │   ├── 📄 Index.cshtml            # Dashboard
│   │   └── 📄 Privacy.cshtml
│   │
│   ├── 📁 Shared/                     # Shared layouts
│   │   ├── 📄 _Layout.cshtml          # Main app layout (sidebar + navbar)
│   │   ├── 📄 _Layout.cshtml.css      # Layout styles
│   │   ├── 📄 _LoginLayout.cshtml     # Login/Register layout (particles bg)
│   │   └── 📄 Error.cshtml
│   │
│   ├── 📄 _ViewImports.cshtml         # Global using directives
│   └── 📄 _ViewStart.cshtml           # Default layout assignment
│
├── 📁 Properties/                     # Launch settings
├── 📄 Program.cs                      # App entry point & middleware config
├── 📄 appsettings.json                # Connection strings & config
├── 📄 QuizeManagement_Project.csproj  # Project dependencies
└── 📄 QuizeManagement_Project.sln     # Solution file
```

---

## 🔄 Application Workflow

### 🔑 Authentication Flow

```mermaid
sequenceDiagram
    actor User
    participant Browser
    participant FormsController
    participant Session
    participant Database

    User->>Browser: Navigate to App
    Browser->>FormsController: GET /Forms/LoginAccountForm
    FormsController-->>Browser: Login Page (with _LoginLayout)

    alt New User
        User->>Browser: Click "Create Account"
        Browser->>FormsController: GET /Forms/CreateAccountForm
        FormsController-->>Browser: Registration Form
        User->>Browser: Fill form & Submit
        Browser->>FormsController: POST /Forms/CreateAccountAddEdit
        FormsController->>Database: EXEC MST_User_Insert
        Database-->>FormsController: Success
        FormsController-->>Browser: Redirect → Login Page
    end

    User->>Browser: Enter credentials & Submit
    Browser->>FormsController: POST /Forms/LoginAccountAddEdit
    FormsController->>Database: EXEC MST_User_SelectByUserNamePassword
    
    alt Valid Credentials
        Database-->>FormsController: User Record Found
        FormsController->>Session: Set Session["UserID"]
        FormsController-->>Browser: Redirect → /Home/Index (Dashboard)
    else Invalid Credentials
        Database-->>FormsController: No Record
        FormsController-->>Browser: Error "Invalid login attempt"
    end

    Note over Session: Session expires after 30 min idle
    
    User->>Browser: Click Logout
    Browser->>FormsController: GET /Forms/Logout
    FormsController->>Session: Session.Clear()
    FormsController-->>Browser: Redirect → Login Page
```

### 📝 Quiz & Question Management Flow

```mermaid
flowchart LR
    subgraph "📝 Create"
        A["Create Quiz"] --> B["Create Questions"]
        B --> C["Set Difficulty Levels"]
        C --> D["Link Questions to Quiz"]
    end

    subgraph "📋 Manage"
        E["View Quiz List"]
        F["View Question List"]
        G["View Level List"]
        H["View Linked List"]
    end

    subgraph "⚡ Actions"
        I["✏️ Edit"]
        J["🗑️ Delete"]
        K["📤 Export Excel"]
        L["🔍 Search & Filter"]
    end

    D --> E & F & G & H
    E & F & G & H --> I & J & K & L

    style A fill:#E8F5E9,stroke:#2E7D32,color:#000
    style B fill:#E3F2FD,stroke:#1565C0,color:#000
    style C fill:#FFF8E1,stroke:#F9A825,color:#000
    style D fill:#F3E5F5,stroke:#7B1FA2,color:#000
    style K fill:#C8E6C9,stroke:#388E3C,color:#000
```

### 🛡️ Session Authorization Flow

```mermaid
flowchart TD
    A["🌐 Incoming Request"] --> B{"Has [AuthorizeSession]<br/>Attribute?"}
    B -->|No| C["✅ Proceed to Action"]
    B -->|Yes| D{"Session['UserID']<br/>exists & > 0?"}
    D -->|Yes| C
    D -->|No| E["⚠️ Set TempData AuthError"]
    E --> F["🔄 Redirect to<br/>LoginAccountForm"]

    style A fill:#E3F2FD,stroke:#1565C0,color:#000
    style C fill:#E8F5E9,stroke:#2E7D32,color:#000
    style F fill:#FFEBEE,stroke:#C62828,color:#000
```

---

## 🗄️ Database Schema

```mermaid
erDiagram
    MST_User {
        int UserID PK
        varchar UserName
        varchar Password
        varchar Mobile
        varchar Email
        bit IsActive
        bit IsAdmin
        datetime Created
        datetime Modified
    }

    MST_Quiz {
        int QuizID PK
        varchar QuizName
        int TotalQuestions
        datetime QuizDate
        int UserID FK
        datetime Created
        datetime Modified
    }

    MST_Question {
        int QuestionID PK
        varchar QuestionText
        varchar OptionA
        varchar OptionB
        varchar OptionC
        varchar OptionD
        varchar CorrectOption
        int QuestionMarks
        bit IsActive
        int QuestionLevelID FK
        int UserID FK
    }

    MST_QuestionLevel {
        int QuestionLevelID PK
        varchar QuestionLevel
        int UserID FK
    }

    MST_QuizWiseQuestions {
        int QuizWiseQuestionsID PK
        int QuizID FK
        int QuestionID FK
        int UserID FK
    }

    MST_User ||--o{ MST_Quiz : creates
    MST_User ||--o{ MST_Question : creates
    MST_User ||--o{ MST_QuestionLevel : creates
    MST_QuestionLevel ||--o{ MST_Question : categorizes
    MST_Quiz ||--o{ MST_QuizWiseQuestions : contains
    MST_Question ||--o{ MST_QuizWiseQuestions : "assigned to"
```

### 📦 Stored Procedures Used

| Stored Procedure | Description |
|:---|:---|
| `MST_User_Insert` | Register a new user |
| `MST_User_SelectByUserNamePassword` | Authenticate user login |
| `MST_Quiz_SelectAll` | Fetch all quizzes |
| `MST_Quiz_InsertUpdate` | Create or update a quiz |
| `MST_Quiz_DeleteByPK` | Delete a quiz by ID |
| `MST_Quiz_SelectByPK` | Get quiz details by ID |
| `MST_Question_SelectAll` | Fetch all questions |
| `MST_Question_InsertUpdate` | Create or update a question |
| `MST_Question_DeleteByPK` | Delete a question by ID |
| `MST_Question_SelectByPK` | Get question details by ID |
| `MST_QuestionLevel_SelectAll` | Fetch all difficulty levels |
| `MST_QuestionLevel_InsertUpdate` | Create or update a level |
| `MST_QuestionLevel_DeleteByPK` | Delete a level by ID |
| `MST_QuestionLevel_SelectByPK` | Get level details by ID |
| `MST_QuizWiseQuestions_SelectAll` | Fetch all quiz-question links |
| `MST_QuizWiseQuestions_InsertUpdate` | Create or update a link |
| `MST_QuizWiseQuestions_DeleteByPK` | Delete a link by ID |
| `MST_QuizWiseQuestions_SelectByPK` | Get link details by ID |

---

## 🧩 MVC Component Map

```mermaid
graph LR
    subgraph "🎮 Controllers"
        FC["FormsController"]
        HC["HomeController"]
        TC["TablesController"]
    end

    subgraph "📦 Models"
        M1["CreateAccountModel"]
        M2["LoginAccountModel"]
        M3["CreateQuizModel"]
        M4["AddQuestionModel"]
        M5["AddQuestionLevelModel"]
        M6["AddQuizwiseQuestionModel"]
        M7["DropDownModels"]
    end

    subgraph "🎨 Views"
        V1["LoginAccountForm"]
        V2["CreateAccountForm"]
        V3["CreateQuizForm"]
        V4["AddQuestionForm"]
        V5["AddQuestionLevelForm"]
        V6["AddQuizwiseQuestionsForm"]
        V7["QuizList"]
        V8["QuestionList"]
        V9["QuestionLevelList"]
        V10["QuizwiseQuestionsList"]
        V11["Dashboard Index"]
    end

    FC --> M1 & M2 & M3 & M4 & M5 & M6 & M7
    FC --> V1 & V2 & V3 & V4 & V5 & V6
    HC --> V11
    TC --> V7 & V8 & V9 & V10

    style FC fill:#BBDEFB,stroke:#1565C0,color:#000
    style HC fill:#C8E6C9,stroke:#2E7D32,color:#000
    style TC fill:#FFE0B2,stroke:#E65100,color:#000
```

---

## ⚙️ Tech Stack

| Layer | Technology | Version |
|:---:|:---|:---|
| 🖥️ **Runtime** | .NET | 8.0 |
| 🔤 **Language** | C# | 12.0 |
| 🏗️ **Framework** | ASP.NET Core MVC | 8.0 |
| 🎨 **Frontend** | Razor Views + Bootstrap | 5.3.3 |
| 🗄️ **Database** | Microsoft SQL Server | Express |
| 📡 **Data Access** | ADO.NET (SqlClient) | 4.9.0 |
| 📊 **Excel Export** | EPPlus | 7.6.1 |
| 🎭 **UI Template** | NiceAdmin | Bootstrap 5 |
| 🔤 **Fonts** | Google Fonts (Inter, Poppins) | — |
| 🎨 **Icons** | Font Awesome + Bootstrap Icons | 6.x |

---

## 🚀 Getting Started

### Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) or later
- [SQL Server Express](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (or any SQL Server edition)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) (recommended) or VS Code

### Installation

**1. Clone the repository**
```bash
git clone https://github.com/Shivam93294Valand/Quiz_Management_dot_net.git
cd Quiz_Management_dot_net
```

**2. Set up the Database**

Create a database named `EN_374` in SQL Server and execute all the required stored procedures (listed in the [Stored Procedures](#-stored-procedures-used) section).

**3. Update Connection String**

Edit `appsettings.json` and update the connection string to match your SQL Server instance:

```json
{
  "ConnectionStrings": {
    "ConnectionString": "Data Source=YOUR_SERVER_NAME; Initial Catalog=EN_374; Integrated Security=true;"
  }
}
```

**4. Restore dependencies & Run**
```bash
dotnet restore
dotnet run
```

**5. Open in browser**
```
https://localhost:5001
```

> 💡 The app starts on the **Login page** (`/Forms/LoginAccountForm`) by default. Create an account first to access the dashboard.

---

## 📸 Screenshots

| Page | Description |
|:---|:---|
| 🔐 **Login** | Beautiful dark-themed login page with particle.js animated background |
| 📝 **Register** | User registration form with validation |
| 📊 **Dashboard** | Overview cards showing total quizzes, questions & levels |
| 📋 **Quiz List** | Data table with search, edit, delete & Excel export |
| ❓ **Question Form** | MCQ question creation with 4 options & difficulty level |
| 🔗 **Link Manager** | Associate questions to quizzes via dropdown selectors |

---

## 🛣️ Route Map

| HTTP Method | Route | Controller | Action | Auth Required |
|:---:|:---|:---|:---|:---:|
| GET | `/` | Forms | LoginAccountForm | ❌ |
| GET | `/Forms/CreateAccountForm` | Forms | CreateAccountForm | ❌ |
| POST | `/Forms/CreateAccountAddEdit` | Forms | CreateAccountAddEdit | ❌ |
| POST | `/Forms/LoginAccountAddEdit` | Forms | LoginAccountAddEdit | ❌ |
| GET | `/Forms/Logout` | Forms | Logout | ❌ |
| GET | `/Home/Index` | Home | Index | ✅ |
| GET | `/Forms/CreateQuizForm` | Forms | CreateQuizForm | ✅ |
| POST | `/Forms/CreateQuizAddEdit` | Forms | CreateQuizAddEdit | ✅ |
| GET | `/Tables/QuizList` | Tables | QuizList | ✅ |
| GET | `/Tables/QuizDelete/{id}` | Tables | QuizDelete | ✅ |
| GET | `/Tables/QuizExportToExcel` | Tables | QuizExportToExcel | ✅ |
| GET | `/Forms/AddQuestionForm` | Forms | AddQuestionForm | ✅ |
| GET | `/Tables/QuestionList` | Tables | QuestionList | ✅ |
| GET | `/Tables/QuestionExportToExcel` | Tables | QuestionExportToExcel | ✅ |
| GET | `/Forms/AddQuestionLevelForm` | Forms | AddQuestionLevelForm | ✅ |
| GET | `/Tables/QuestionLevelList` | Tables | QuestionLevelList | ✅ |
| GET | `/Forms/AddQuizwiseQuestionsForm` | Forms | AddQuizwiseQuestionsForm | ✅ |
| GET | `/Tables/AddQuizwiseQuestionsList` | Tables | AddQuizwiseQuestionsList | ✅ |

---

## 🤝 Contributing

Contributions are welcome! Here's how you can help:

1. **Fork** the repository
2. **Create** a feature branch: `git checkout -b feature/amazing-feature`
3. **Commit** your changes: `git commit -m 'Add amazing feature'`
4. **Push** to the branch: `git push origin feature/amazing-feature`
5. **Open** a Pull Request

---

<div align="center">

### ⭐ Star this repo if you found it useful!

Made with ❤️ by [Shivam Valand](https://github.com/Shivam93294Valand)

</div>

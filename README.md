# Employee Management Helpdesk

An ASP.NET Core Helpdesk application for managing employees and support calls. The application provides employee and call management, PDF report generation, and SQL Server database integration.

---

# Architecture

The solution is organized into **5 logical layers**:

1. **View Layer**
   - HTML pages and JavaScript
   - `home.html`
   - `employee.html` / `employee.js`
   - `call.html` / `call.js`
   - `report.html` / `report.js`

2. **Web Layer**
   - ASP.NET Core API Controllers
   - Employee Controller
   - Department Controller
   - Call Controller
   - Problem Controller
   - Report Controller

3. **ViewModels Layer**
   - `HelpdeskViewModels.dll`
   - Employee ViewModel
   - Department ViewModel
   - Call ViewModel
   - Problem ViewModel

4. **Data Access Layer**
   - `HelpdeskDAL.dll`
   - Repositories
   - DAOs
   - Entity Framework Core entities
   - `HelpdeskContext`

5. **Data Layer**
   - SQL Server LocalDB (`HelpdeskDB`)

---

# Features

## Employee Management

- Create, read, update, and delete employee records
- Department selection using dropdown lists
- Upload and update employee photos
- Search employees by last name

## Call Management

- Create, read, update, and delete support calls
- Select employees, technicians, and problems from dropdown lists
- Three call modes:
  - Add
  - Update (open calls)
  - View (closed calls)
- Closing a call automatically:
  - Records the close date
  - Prevents further editing
- Search calls by employee last name

## Reporting

- Generate PDF reports using **iText7**
- Employee Report (portrait layout)
- Calls Report (landscape layout)
- Reports include:
  - Company logo
  - Custom fonts and colors
  - Date and time generated
- Reports open in a new browser tab

## Additional Features

- Custom UI theme
- Responsive design
- Shared navigation bar across all pages

---

# Technology Stack

| Component | Technology |
|-----------|------------|
| Backend | ASP.NET Core, C#, Entity Framework Core, LINQ |
| Frontend | JavaScript, jQuery, Bootstrap 5 |
| Database | SQL Server LocalDB |
| PDF Generation | iText7 (v7.2.5) |

---

# Requirements

- Visual Studio 2022 (17.10 or later recommended)
- .NET 7.0 or later
- SQL Server LocalDB (SQL Server 2019+)

---

# NuGet Packages

Packages are restored automatically when the solution is built.

If installing manually, target the **HelpdeskDAL** project:

```powershell
Install-Package Microsoft.EntityFrameworkCore
Install-Package Microsoft.EntityFrameworkCore.SqlServer
Install-Package Microsoft.EntityFrameworkCore.Tools
Install-Package Microsoft.EntityFrameworkCore.Proxies
```

---

# Database Setup

1. Open **SQL Server Object Explorer** in Visual Studio.
2. Create a new LocalDB database named **HelpdeskDB**.
3. Right-click the database and select **New Query**.
4. Execute the contents of `helpdeskSQL.txt`.
5. Update the connection string in `appsettings.json` (or `HelpdeskContext`).
6. Build and run the solution.

---

# Running the Project

1. Clone the repository.

```bash
git clone <repository-url>
```

2. Open `EmployeeManagementHelpdesk.sln` in Visual Studio.

3. Restore NuGet packages.

4. Configure the database.

5. Press **F5** to run the application.
https://github.com/3eisa/task-management-mvc

# TaskManagementSystem

## Overview

TaskManagementSystem is a small ASP.NET Core MVC web application built for a university assignment. It lets a signed-in user manage a list of people (assignable users) and a list of tasks, where each task has a title, description, priority, creation date, and an assigned user. The project is intentionally minimal: it uses a simple session-based dummy login (no ASP.NET Identity), Entity Framework Core with SQLite for storage, and standard Razor views with Bootstrap.

The codebase is organised so each piece of functionality (scaffold, data layer, authentication, users CRUD, tasks CRUD, sorting/validation) was developed on its own feature branch and merged back into `development` / `main`.

## Features

- Dummy login system with register, login, and logout (session-based, no Identity)
- User CRUD for the people who can be assigned to tasks
- Task CRUD with priority and user assignment
- Sorting tasks by creation date and by priority
- Basic validation on all forms (Data Annotations + `ModelState.IsValid`)
- Automatic database creation on first run (EF Core `Database.Migrate()` at startup)

## Technologies Used

- ASP.NET Core MVC
- .NET 8
- Entity Framework Core
- SQLite
- Razor Views
- C#

## Project Structure

```
TaskManagementSystem/
  Controllers/        AccountController, BaseController, HomeController, UsersController, TasksController
  Data/               AppDbContext (EF Core)
  Migrations/         EF Core migrations (InitialCreate)
  Models/             AppUser, User, TaskItem, Priority enum, ErrorViewModel
  ViewModels/         LoginViewModel, RegisterViewModel, TaskFormViewModel
  Views/              Razor views (Home, Account, Users, Tasks, Shared)
  wwwroot/            Static assets (CSS, JS, Bootstrap, jQuery)
  appsettings.json    Connection string (Data Source=app.db)
  Program.cs          Service registration, session, auto-migration on startup
docs/
  testing-checklist.md  Manual testing checklist
```

## Data Models

- **`AppUser`** — represents a login account. Fields: `Id`, `Username` (required, max 50), `Password` (required, max 100). Used only for dummy auth; passwords are stored in plain text.
- **`User`** — represents a person who can be assigned to tasks. Fields: `Id`, `Name` (required, max 100), `Email` (required, valid email, max 150), and a `Tasks` navigation collection.
- **`TaskItem`** — represents a task. Fields: `Id`, `Title` (required, max 100), `Description` (optional, max 500), `Priority` (required, enum), `CreatedAt` (set on creation only), `UserId` (required), and a `User` navigation property.
- **`Priority`** — enum with values `Low`, `Medium`, `High` (stored as `INTEGER` in SQLite).

## Authentication

Authentication is intentionally minimal:

- `AccountController` handles `Register`, `Login`, and `Logout`.
- Registration creates an `AppUser` after checking the username is not already taken.
- Login looks up the matching `Username`/`Password` and stores the username in the session (`HttpContext.Session.SetString("Username", ...)`).
- Logout calls `Session.Clear()`.
- A small `BaseController` is used by `UsersController` and `TasksController`. In `OnActionExecuting`, it redirects to `/Account/Login` when the session has no `Username`, so all user/task pages are protected.
- The shared layout shows **Login** / **Register** when logged out, and **Hello, {username}** + **Logout** when logged in.

## User Management

Implemented in `UsersController` and `Views/Users/`. All pages require an authenticated session.

- **Index** — lists users (Name, Email) with Edit and Delete actions.
- **Create** — form for Name and Email, with validation.
- **Edit** — same fields as Create, with a hidden `Id`. Returns `NotFound()` if the user does not exist.
- **Delete** — shows a confirmation page, then deletes via POST. Returns `NotFound()` if the user does not exist.
- `ModelState.IsValid` is checked on every POST.

## Task Management

Implemented in `TasksController`, `TaskFormViewModel`, and `Views/Tasks/`. All pages require an authenticated session.

- **Index** — lists tasks with Title, Description, Priority (shown as a small coloured label), Assigned User, Created At, and Edit/Delete actions.
- **Create** — form bound to `TaskFormViewModel` with Title, Description, Priority dropdown (`Low`/`Medium`/`High`), and Assigned User dropdown (built from `_db.Users`). `CreatedAt` is set to `DateTime.Now` only on creation.
- **Edit** — same form as Create plus a hidden `Id`. The existing entity is loaded and only `Title`, `Description`, `Priority`, `UserId` are overwritten — `CreatedAt` is never modified. Returns `NotFound()` if the task does not exist.
- **Delete** — shows a confirmation page with task details, then deletes via POST. Returns `NotFound()` if the task does not exist.
- All POST actions check `ModelState.IsValid` and re-render the form (with the user dropdown re-populated) on invalid input.

## Sorting and Validation

### Sorting

`TasksController.Index(string? sortBy)` accepts a `sortBy` query parameter:

| Value           | Result                                                      |
| --------------- | ----------------------------------------------------------- |
| `date_desc`     | Newest first (**default** when `sortBy` is null or unknown) |
| `date_asc`      | Oldest first                                                |
| `priority_asc`  | Priority Low → Medium → High                                |
| `priority_desc` | Priority High → Medium → Low                                |

The Tasks index page has a small GET form with a `<select>` named `sortBy`. The currently selected option is preserved after submitting using `ViewData["SortBy"]`.

### Validation

- Data Annotations on every model and view model (`[Required]`, `[MaxLength]`, `[EmailAddress]`, `[Compare]`).
- Every POST action checks `ModelState.IsValid` and returns the same view with the model on failure.
- Razor views include `<div asp-validation-summary="ModelOnly">` and `<span asp-validation-for="...">` for field-level messages.
- `_ValidationScriptsPartial` is included on form pages for client-side validation.
- Edit and Delete actions return `NotFound()` when the requested record does not exist.

## How to Run Locally

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download)

### Steps

From the repo root:

```bash
# 1. Restore NuGet packages
dotnet restore

# 2. Restore local tools (dotnet-ef) and apply migrations to create the database
dotnet tool restore
dotnet ef database update --project TaskManagementSystem

# 3. Run the app
dotnet run --project TaskManagementSystem
```

The app starts on `http://localhost:5078` (see `TaskManagementSystem/Properties/launchSettings.json`). On startup, `Program.cs` also calls `Database.Migrate()`, so if the database does not exist it will be created automatically — running `dotnet ef database update` manually is therefore optional but explicitly listed above for clarity.

To reset the database, delete `TaskManagementSystem/app.db` and run the app again.

### Using the App

1. Open `http://localhost:5078`.
2. Click **Register** and create an account.
3. **Login** with that account.
4. Use the **Users** page to create the people who can be assigned to tasks.
5. Use the **Tasks** page to create tasks and assign them to users. Use the **Sort** dropdown to change order.
6. Use **Logout** to clear the session.

## Testing

There are no automated tests in this project — testing is performed manually using a written checklist.

- The manual testing checklist lives at [`docs/testing-checklist.md`](docs/testing-checklist.md).
- It covers Authentication, Users, Tasks, Sorting, and Build steps.
- The build itself can be verified with:

  ```bash
  dotnet build TaskManagementSystem/TaskManagementSystem.csproj
  ```

  A passing build prints `Build succeeded.` with `0 Warning(s)` and `0 Error(s)`.

## Screenshots

Screenshots of the running application will be added here for the final report:

- Register page
- Login page
- Users list
- Create / Edit user form
- Tasks list with sort dropdown
- Create / Edit task form
- Delete confirmation page

> Place screenshot files under `docs/screenshots/` and reference them here, for example:
> As seen in screenshots the url is hosted and served via cloudlflare
>
> ```markdown
> ![Tasks list](docs/screenshots/tasks-list.png)
> ```

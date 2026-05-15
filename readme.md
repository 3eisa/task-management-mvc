# TaskManagementSystem

A minimal ASP.NET Core MVC task management system built with .NET 8, Entity Framework Core, and SQLite.

## Features

- Dummy login system (session-based, no ASP.NET Identity)
- User CRUD (task-assigned users)
- Task CRUD with priority levels and user assignment
- Sorting by creation date and priority
- Basic validation on all forms

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- (Optional, for manual migrations) `dotnet-ef` is already pinned as a **local tool** in `.config/dotnet-tools.json` — restore it with:

  ```bash
  dotnet tool restore
  ```

## Getting Started

From the repo root:

```bash
# 1. Restore packages (run once after cloning)
dotnet restore

# 2. Run the app
dotnet run --project TaskManagementSystem
```

The app starts on `http://localhost:5078` (see `TaskManagementSystem/Properties/launchSettings.json`).

### Database

SQLite is used and the database file is `TaskManagementSystem/app.db`. **You don't need to run any migration commands manually** — the app calls `Database.Migrate()` at startup, so the database file and schema are created/updated automatically on the first run.

If you ever want to manage the database manually (e.g. inspecting it or recreating from scratch):

```bash
# From TaskManagementSystem/
dotnet tool restore                  # only needed once
dotnet ef database update            # apply migrations manually
dotnet ef migrations add <Name>      # add a new migration after model changes
```

To reset the database, simply delete `TaskManagementSystem/app.db` and run the app again.

## Basic Usage

1. Open `http://localhost:5078` in your browser.
2. Click **Register** in the top-right and create an account (username + password).
3. Click **Login** and sign in with the account you just created.
4. Once logged in, the navbar shows **Users** and **Tasks** links:
   - **Users**: create the people who can be assigned to tasks (Name + Email).
   - **Tasks**: create tasks with title, description, priority (Low / Medium / High), and an assigned user.
5. On the Tasks page, use the **Sort** dropdown to sort by:
   - Newest first (default)
   - Oldest first
   - Priority low to high
   - Priority high to low
6. Use **Logout** in the top-right to clear your session.

> Note: the login system is intentionally simple for assignment purposes — passwords are stored in plain text and there is no real authentication framework.

## Project Structure

```
TaskManagementSystem/
  Controllers/        # AccountController, BaseController, HomeController, UsersController, TasksController
  Data/               # AppDbContext (EF Core)
  Migrations/         # EF Core migrations (InitialCreate)
  Models/             # AppUser, User, TaskItem, Priority enum
  ViewModels/         # LoginViewModel, RegisterViewModel, TaskFormViewModel
  Views/              # Razor views (Home, Account, Users, Tasks, Shared)
  wwwroot/            # Static assets (CSS, JS, Bootstrap, jQuery)
  appsettings.json    # Connection string (Data Source=app.db)
  Program.cs          # Service registration, session, auto-migration on startup
```

## Development Plan

The project is developed in phases using feature branches. Completed phases are documented below to support the final university report.

## Completed Phases

- **Phase 1 — Project scaffold**: created the ASP.NET Core MVC project targeting .NET 8 with the default `HomeController`, layout, and views.
- **Phase 2 — Models and database**: added `AppUser`, `User`, `TaskItem`, and `Priority` enum; configured `AppDbContext` with EF Core + SQLite; added the initial migration.
- **Phase 3 — Authentication**: added minimal session-based dummy auth (no Identity) with `AccountController`, `LoginViewModel`, `RegisterViewModel`, login/logout/register views, and session-aware navbar.
- **Phase 4 — Users CRUD**: added `UsersController` and Razor views for listing, creating, editing, and deleting task-assigned users; pages protected by session check.
- **Phase 5 — Tasks CRUD**: added `TasksController`, `TaskFormViewModel`, and Razor views for listing, creating, editing, and deleting tasks; `CreatedAt` set only on creation; user dropdown for assignment.
- **Phase 6 — Sorting, validation, and UI polish**: added `sortBy` support to the Tasks index (date asc/desc, priority asc/desc, default newest first), audited validation across forms and POST actions, and applied a minimal CSS cleanup with small priority labels.

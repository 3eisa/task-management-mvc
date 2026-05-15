# Manual Testing Checklist

Use this checklist to manually verify the core functionality of the TaskManagementSystem app before tagging a release or handing it in. Start with a clean state (e.g. delete `TaskManagementSystem/app.db` so the database is fresh, then run `dotnet run --project TaskManagementSystem`).

## Authentication

- [ ] Register page loads at `/Account/Register`
- [ ] User can register with a valid username and password (redirected to login)
- [ ] Login page loads at `/Account/Login`
- [ ] User can log in with the registered credentials (redirected to home, navbar shows username)
- [ ] User can log out from the navbar (session cleared, Login/Register links reappear)
- [ ] Visiting `/Users` while logged out redirects to `/Account/Login`
- [ ] Visiting `/Tasks` while logged out redirects to `/Account/Login`

## Users

- [ ] User list loads at `/Users` (logged in)
- [ ] User can be created via `/Users/Create` with a valid Name and Email
- [ ] User can be edited via `/Users/Edit/{id}` and changes are persisted
- [ ] User can be deleted via `/Users/Delete/{id}` after the confirmation page
- [ ] Submitting the Create or Edit form with invalid input (empty Name, invalid Email, etc.) shows validation error messages on the form

## Tasks

- [ ] Task list loads at `/Tasks` (logged in)
- [ ] Task can be created via `/Tasks/Create` with a valid Title
- [ ] Task can be assigned to a user via the Assigned User dropdown
- [ ] Task priority can be selected from Low / Medium / High
- [ ] Task can be edited via `/Tasks/Edit/{id}` and changes are persisted (CreatedAt does not change)
- [ ] Task can be deleted via `/Tasks/Delete/{id}` after the confirmation page
- [ ] Submitting the Create or Edit form with invalid input (empty Title, missing Priority, no User selected, etc.) shows validation error messages on the form

## Sorting

- [ ] Selecting "Newest first" sorts tasks by CreatedAt descending (default)
- [ ] Selecting "Oldest first" sorts tasks by CreatedAt ascending
- [ ] Selecting "Priority low to high" sorts tasks Low → Medium → High
- [ ] Selecting "Priority high to low" sorts tasks High → Medium → Low
- [ ] Selected sort option stays visible in the dropdown after submit

## Build

- [ ] `dotnet build TaskManagementSystem/TaskManagementSystem.csproj` succeeds with 0 warnings and 0 errors
- [ ] `dotnet run --project TaskManagementSystem` starts the app and serves `http://localhost:5078`

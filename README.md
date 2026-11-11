# Loan Management System (LMS) — Backend

ASP.NET Core 8 Web API backend built with Clean Architecture, Entity Framework Core (Code First), and Microsoft SQL Server.

---

## Project Structure

```text
LMS.sln
├── LMS.Domain          # Entities (BaseEntity + 17 domain entities)
├── LMS.Application     # Service interfaces, repository interfaces, DTOs, business logic
├── LMS.Infrastructure  # ApplicationDbContext, EF Core configurations, Migrations, SeedData
└── LMS.API             # Controllers, Program.cs, appsettings.json, API startup
```

---

## Prerequisites (On Any Laptop)

Before running the project on a new laptop, install:

1. **.NET 8 SDK** (8.0 or later)
   - Verify with:
     ```powershell
     dotnet --list-sdks
     ```
2. **SQL Server** (one of the following):
   - **SQL Server Express LocalDB** (comes with Visual Studio or SQL Server Express installer) — default configured in `LMS.API/appsettings.json`
   - Or **SQL Server / SQL Server Express** (`localhost` or `.\SQLEXPRESS`)
3. **Git**
4. **EF Core CLI tool (`dotnet-ef`)** — installed once per laptop:
   ```powershell
   dotnet tool install --global dotnet-ef --version 8.*
   ```
   *(If `dotnet-ef` is already installed and you want to update it, run `dotnet tool update --global dotnet-ef --version 8.*`.)*

---

## Part 1: Pushing This Project to Git (First Laptop)

Before pushing to GitHub/GitLab/Azure Repos, make sure build artifacts (`bin/`, `obj/`) and local user secrets are not committed.

1. Open a terminal in the solution root folder (the folder containing `LMS.sln`).
2. Generate a standard .NET `.gitignore` if you don't have one yet:
   ```powershell
   dotnet new gitignore
   ```
3. Initialize Git (if not already initialized), stage, commit, and push:
   ```powershell
   git init
   git add .
   git commit -m "Initial LMS backend scaffolding, EF Core model, migration, and admin seed"
   git branch -M main
   git remote add origin <YOUR_GIT_REPOSITORY_URL>
   git push -u origin main
   ```

> **Note:** Make sure the `LMS.Infrastructure/Migrations/` folder **is** committed to Git. That is what lets another laptop recreate the exact database schema.

---

## Part 2: Cloning & Creating the Database on Another Laptop

Follow these steps in order after cloning on a second laptop:

### Step 1: Clone the repository and open the solution root

```powershell
git clone <YOUR_GIT_REPOSITORY_URL>
cd "<REPO_FOLDER_NAME>"
```

Make sure you are in the directory that contains `LMS.sln`.

### Step 2: Restore NuGet packages and build the solution

```powershell
dotnet restore LMS.sln
dotnet build LMS.sln
```

Verify the build succeeds with `0 Error(s)`.

### Step 3: Verify `dotnet-ef` is installed

```powershell
dotnet ef --version
```

If it says command not found, install it:

```powershell
dotnet tool install --global dotnet-ef --version 8.*
```

### Step 4: Check your SQL Server / LocalDB connection string

Open `LMS.API/appsettings.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=LoanManagementSystemDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
}
```

- **If using LocalDB (default):** Make sure the LocalDB instance is started:
  ```powershell
  sqllocaldb info
  sqllocaldb start MSSQLLocalDB
  ```
- **If using SQL Server Express (`.\SQLEXPRESS`):** Update `DefaultConnection` (or set it via `appsettings.Development.json` / user-secrets) to:
  ```text
  Server=.\SQLEXPRESS;Database=LoanManagementSystemDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True
  ```
- **If using SQL Server with username & password:**
  ```text
  Server=localhost;Database=LoanManagementSystemDb;User Id=sa;Password=<YOUR_PASSWORD>;TrustServerCertificate=True;MultipleActiveResultSets=true
  ```

### Step 5: Create the database by running EF Core migrations

Run this command from the solution root folder (where `LMS.sln` is located):

```powershell
dotnet ef database update --project LMS.Infrastructure --startup-project LMS.API
```

This command will:
1. Create the `LoanManagementSystemDb` database in SQL Server if it does not exist.
2. Create all **17 tables**, relationships, decimal precisions, and filtered unique indexes.
3. Seed the **26 workflow statuses** in `Statuses`.

### Step 6: Run the API to seed the initial Admin role and Admin user

```powershell
dotnet run --project LMS.API
```

On startup, `LMS.API/Program.cs` runs `SeedData.InitializeAsync(...)`, which ensures:
- **Only** the `Admin` role (`Key = "ADMIN"`) is created in `Roles`.
- **Only** the initial `Admin` user is created in `Users` (with a cryptographic PBKDF2 password hash).
- Other roles (such as `Loan Officer`, `Customer`) and user profiles are **not** pre-seeded—the Admin creates them after logging in.

#### Default Development Admin Credentials (`SeedData.cs`)
- **Full Name:** `Suhaib`
- **Email:** `admin@lms.local`
- **Password:** `Admin@12345`

*(Optional)* To override the seeded Admin credentials or configure a JWT signing key locally without modifying committed files, use `dotnet user-secrets`:

```powershell
dotnet user-secrets init --project LMS.API
dotnet user-secrets set "SeedAdmin:FullName" "Suhaib" --project LMS.API
dotnet user-secrets set "SeedAdmin:Email" "admin@lms.local" --project LMS.API
dotnet user-secrets set "SeedAdmin:Password" "<StrongPasswordHere>" --project LMS.API
dotnet user-secrets set "Jwt:Key" "<AtLeast32CharactersSecretKey>" --project LMS.API
```

> **Production / Non-Development Note:** Outside the `Development` environment, `SeedData` does **not** use fallback credentials—you must provide `SeedAdmin:Email` and `SeedAdmin:Password` via environment variables or a secret store.

### Step 7: Open Swagger UI (in Development)

Once `dotnet run --project LMS.API` prints the listening URL (for example `https://localhost:7xxx` or `http://localhost:5xxx`), open:

```text
https://localhost:<PORT>/swagger
```

---

## Common EF Core Migration Commands (Quick Reference)

Always run these from the solution root folder (next to `LMS.sln`):

| Task | Command |
| --- | --- |
| Apply all pending migrations to DB | `dotnet ef database update --project LMS.Infrastructure --startup-project LMS.API` |
| Create a new migration after editing entities | `dotnet ef migrations add <MigrationName> --project LMS.Infrastructure --startup-project LMS.API` |
| List all migrations and check applied status | `dotnet ef migrations list --project LMS.Infrastructure --startup-project LMS.API` |
| Check if DbContext has unmigrated model changes | `dotnet ef migrations has-pending-model-changes --project LMS.Infrastructure --startup-project LMS.API` |
| Undo the last migration (before applying to DB) | `dotnet ef migrations remove --project LMS.Infrastructure --startup-project LMS.API` |
| Drop the local development database completely | `dotnet ef database drop --force --project LMS.Infrastructure --startup-project LMS.API` |

---

## Troubleshooting

1. **`Could not execute because the specified command or file was not found` (`dotnet ef`)**
   - Install the EF Core CLI tool globally:
     ```powershell
     dotnet tool install --global dotnet-ef --version 8.*
     ```
2. **Database connection failure on LocalDB**
   - Verify LocalDB is installed and running:
     ```powershell
     sqllocaldb info
     sqllocaldb start MSSQLLocalDB
     ```
3. **`An Application Control policy has blocked this file (0x800711C7)` on Windows 11**
   - Windows **Smart App Control** or WDAC may block newly compiled unsigned `.dll` files (especially inside `OneDrive\Desktop`).
   - Clone the repository into a standard non-OneDrive local folder (such as `C:\Projects\LMS`) or a **Windows Dev Drive**, or check **Windows Security -> App & browser control -> Smart App Control settings**.
4. **Resetting the local database completely**
   ```powershell
   dotnet ef database drop --force --project LMS.Infrastructure --startup-project LMS.API
   dotnet ef database update --project LMS.Infrastructure --startup-project LMS.API
   dotnet run --project LMS.API
   ```


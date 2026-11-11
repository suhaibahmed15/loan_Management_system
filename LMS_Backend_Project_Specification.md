# Loan Management System (LMS) --- Backend Project Specification

**Project:** Loan Management System\
**Backend:** ASP.NET Core Web API (.NET 8)\
**Architecture:** Clean Architecture (minimal, team-friendly
implementation)\
**Database:** Microsoft SQL Server\
**ORM:** Entity Framework Core --- Code First\
**Authentication:** JWT Bearer Authentication\
**Document status:** Consolidated working specification based on the
decisions made so far

------------------------------------------------------------------------

## 1. Project Overview

The Loan Management System is a web-based system for managing the
complete loan lifecycle: user access, loan products, applications,
supporting documents, guarantors, review and approval, loan creation,
repayment schedules, repayments, and audit history.

The system must also support **dynamic permissions**. Administrators can
decide which permissions belong to a role and can grant additional
permissions to an individual user. A direct user permission remains
assigned until an administrator manually revokes it.

The implementation should remain straightforward and suitable for a
small development team. Avoid unnecessary abstractions, generic
repositories, CQRS, MediatR, or extra projects.

## 2. Confirmed Technology and Architecture Decisions

  -------------------------------------------------------------------------------
  Area                                Decision
  ----------------------------------- -------------------------------------------
  Framework                           ASP.NET Core Web API, .NET 8

  Database                            SQL Server

  Data access                         Entity Framework Core

  Database approach                   Code First

  Architecture                        Clean Architecture

  Authentication                      JWT Bearer

  Primary/foreign keys                `int`

  Status handling                     Central `Statuses` table; do not use enums
                                      for workflow statuses

  Delete behavior                     Soft delete using `IsDeleted`

  Repository interfaces               `LMS.Application/Repositories/Interfaces`

  Repository implementations          `LMS.Infrastructure/Repositories`

  Services                            Application layer

  Controllers                         API layer

  Mapping                             Keep implementation simple; do not
                                      introduce an unnecessary mapping layer

  Complexity                          No CQRS, MediatR, generic repository, or
                                      extra abstraction layers
  -------------------------------------------------------------------------------

**Database name:** `LoanManagementSystemDb`

### Architecture dependency direction

``` text
LMS.Domain
    ↑
LMS.Application
    ↑
LMS.Infrastructure
    ↑
LMS.API
```

-   `LMS.Domain` contains entities and domain-level definitions.
-   `LMS.Application` contains service contracts, repository interfaces,
    DTOs where needed, and business logic.
-   `LMS.Infrastructure` contains EF Core, database configuration, and
    repository implementations.
-   `LMS.API` contains HTTP controllers, authentication/authorization
    configuration, and application startup.

The Domain project must not depend on the other projects. Application
depends on Domain. Infrastructure implements Application's repository
interfaces. API is the composition root and references the required
projects.

## 3. User Roles

The current minimal role set is:

1.  **Admin**
    -   Manages users and roles.
    -   Manages permissions and role-permission assignments.
    -   Grants and revokes direct permissions for individual users.
    -   Manages system configuration and monitors system activity.
2.  **Loan Officer**
    -   Reviews submitted loan applications.
    -   Checks applicant information and supporting documents.
    -   Records verification findings.
    -   Handles guarantor verification and forwards applications for the
        required decision.
3.  **Customer**
    -   Registers/logs in.
    -   Views available loan types and loan information.
    -   Submits loan applications and supporting documents.
    -   Provides guarantor information.
    -   Views application progress, repayment schedule, and payment
        history.

A **guarantor is not a system role**. A guarantor is represented by a
`LoanGuarantor` record linked to a loan application. A guarantor may be
an external person and does not automatically need a system login.

> Earlier discussions considered additional internal roles such as
> Manager and Accountant. The later minimal role decision was Admin,
> Loan Officer, and Customer. If the team later approves separate
> Manager or Accountant roles, they can be added as data in `Roles`; the
> application should not require hard-coded role IDs.

## 4. Main Backend Modules

  Module                Responsibility
  --------------------- -------------------------------------------------
  Authentication        Login, JWT generation, current-user identity
  Users and Roles       User management and role assignment
  Statuses              Central list of workflow statuses
  Loan Types            Configure loan products and terms
  Loan Applications     Create and process applications
  Loan Documents        Store document metadata and file references
  Loan Guarantors       Capture and verify guarantor information
  Loan Approvals        Record approval/rejection decisions
  Loans                 Manage approved/disbursed loans
  Repayment Schedules   Store expected installments
  Repayments            Record received payments and allocations
  Menus                 Manage application navigation items
  Role Menus            Control which menus are available to roles
  Audit Logs            Record important system actions
  Permissions           Define granular system capabilities
  Role Permissions      Assign capabilities to roles
  User Permissions      Grant additional capabilities directly to users

## 5. Backend Project Structure

Keep the solution to four projects:

``` text
LMS
│
├── LMS.Domain
│   ├── Entities
│   │   ├── BaseEntity.cs
│   │   ├── Role.cs
│   │   ├── User.cs
│   │   ├── Status.cs
│   │   ├── LoanType.cs
│   │   ├── LoanApplication.cs
│   │   ├── LoanDocument.cs
│   │   ├── LoanGuarantor.cs
│   │   ├── LoanApproval.cs
│   │   ├── Loan.cs
│   │   ├── RepaymentSchedule.cs
│   │   ├── Repayment.cs
│   │   ├── Menu.cs
│   │   ├── RoleMenu.cs
│   │   ├── AuditLog.cs
│   │   ├── Permission.cs
│   │   ├── RolePermission.cs
│   │   └── UserPermission.cs
│   └── [Domain-level definitions only when required]
│
├── LMS.Application
│   ├── DTOs
│   │   ├── Auth
│   │   ├── User
│   │   ├── Role
│   │   ├── LoanType
│   │   ├── LoanApplication
│   │   ├── LoanDocument
│   │   ├── LoanGuarantor
│   │   ├── LoanApproval
│   │   ├── Loan
│   │   ├── RepaymentSchedule
│   │   ├── Repayment
│   │   ├── Menu
│   │   └── Permission
│   │
│   ├── Repositories
│   │   └── Interfaces
│   │       ├── IRoleRepository.cs
│   │       ├── IUserRepository.cs
│   │       ├── IStatusRepository.cs
│   │       ├── ILoanTypeRepository.cs
│   │       ├── ILoanApplicationRepository.cs
│   │       ├── ILoanDocumentRepository.cs
│   │       ├── ILoanGuarantorRepository.cs
│   │       ├── ILoanApprovalRepository.cs
│   │       ├── ILoanRepository.cs
│   │       ├── IRepaymentScheduleRepository.cs
│   │       ├── IRepaymentRepository.cs
│   │       ├── IMenuRepository.cs
│   │       ├── IRoleMenuRepository.cs
│   │       ├── IAuditLogRepository.cs
│   │       ├── IPermissionRepository.cs
│   │       ├── IRolePermissionRepository.cs
│   │       └── IUserPermissionRepository.cs
│   │
│   ├── Services
│   │   ├── Interfaces
│   │   │   ├── IAuthService.cs
│   │   │   ├── IUserService.cs
│   │   │   ├── IRoleService.cs
│   │   │   ├── ILoanTypeService.cs
│   │   │   ├── ILoanApplicationService.cs
│   │   │   ├── ILoanDocumentService.cs
│   │   │   ├── ILoanGuarantorService.cs
│   │   │   ├── ILoanApprovalService.cs
│   │   │   ├── ILoanService.cs
│   │   │   ├── IRepaymentScheduleService.cs
│   │   │   ├── IRepaymentService.cs
│   │   │   ├── IMenuService.cs
│   │   │   ├── IPermissionService.cs
│   │   │   ├── IRolePermissionService.cs
│   │   │   ├── IUserPermissionService.cs
│   │   │   └── IPermissionAuthorizationService.cs
│   │   │
│   │   ├── AuthService.cs
│   │   ├── UserService.cs
│   │   ├── RoleService.cs
│   │   ├── LoanTypeService.cs
│   │   ├── LoanApplicationService.cs
│   │   ├── LoanDocumentService.cs
│   │   ├── LoanGuarantorService.cs
│   │   ├── LoanApprovalService.cs
│   │   ├── LoanService.cs
│   │   ├── RepaymentScheduleService.cs
│   │   ├── RepaymentService.cs
│   │   ├── MenuService.cs
│   │   ├── PermissionService.cs
│   │   ├── RolePermissionService.cs
│   │   ├── UserPermissionService.cs
│   │   ├── PermissionAuthorizationService.cs
│   │   └── DependencyInjection.cs
│   │
│   └── [Application-level validation/helpers only when required]
│
├── LMS.Infrastructure
│   ├── Data
│   │   ├── ApplicationDbContext.cs
│   │   └── Configurations
│   │       ├── RoleConfiguration.cs
│   │       ├── UserConfiguration.cs
│   │       ├── StatusConfiguration.cs
│   │       ├── LoanTypeConfiguration.cs
│   │       ├── LoanApplicationConfiguration.cs
│   │       ├── LoanDocumentConfiguration.cs
│   │       ├── LoanGuarantorConfiguration.cs
│   │       ├── LoanApprovalConfiguration.cs
│   │       ├── LoanConfiguration.cs
│   │       ├── RepaymentScheduleConfiguration.cs
│   │       ├── RepaymentConfiguration.cs
│   │       ├── MenuConfiguration.cs
│   │       ├── RoleMenuConfiguration.cs
│   │       ├── AuditLogConfiguration.cs
│   │       ├── PermissionConfiguration.cs
│   │       ├── RolePermissionConfiguration.cs
│   │       └── UserPermissionConfiguration.cs
│   │
│   ├── Repositories
│   │   ├── RoleRepository.cs
│   │   ├── UserRepository.cs
│   │   ├── StatusRepository.cs
│   │   ├── LoanTypeRepository.cs
│   │   ├── LoanApplicationRepository.cs
│   │   ├── LoanDocumentRepository.cs
│   │   ├── LoanGuarantorRepository.cs
│   │   ├── LoanApprovalRepository.cs
│   │   ├── LoanRepository.cs
│   │   ├── RepaymentScheduleRepository.cs
│   │   ├── RepaymentRepository.cs
│   │   ├── MenuRepository.cs
│   │   ├── RoleMenuRepository.cs
│   │   ├── AuditLogRepository.cs
│   │   ├── PermissionRepository.cs
│   │   ├── RolePermissionRepository.cs
│   │   └── UserPermissionRepository.cs
│   │
│   └── DependencyInjection.cs
│
└── LMS.API
    ├── Controllers
    │   ├── AuthController.cs
    │   ├── UsersController.cs
    │   ├── RolesController.cs
    │   ├── StatusesController.cs
    │   ├── LoanTypesController.cs
    │   ├── LoanApplicationsController.cs
    │   ├── LoanDocumentsController.cs
    │   ├── LoanGuarantorsController.cs
    │   ├── LoanApprovalsController.cs
    │   ├── LoansController.cs
    │   ├── RepaymentSchedulesController.cs
    │   ├── RepaymentsController.cs
    │   ├── MenusController.cs
    │   ├── RoleMenusController.cs
    │   ├── PermissionsController.cs
    │   ├── RolePermissionsController.cs
    │   └── UserPermissionsController.cs
    │
    ├── Authorization
    │   └── PermissionAuthorizationHandler.cs
    ├── Program.cs
    └── appsettings.json
```

This is the target structure, not a requirement to create every file
immediately. Create a module's files when implementing that module. Do
not create empty or unused classes just to match the tree.

## 6. Entities and Database Tables

The agreed core entity set contains 14 entities. The dynamic permission
feature adds 3 more entities, giving **17 entities/tables** in the full
target design.

### 6.1 BaseEntity

All entities inherit from `BaseEntity` to share common fields.

``` csharp
public abstract class BaseEntity
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public bool IsDeleted { get; set; }
}
```

**Important implementation note:** The exact primary-key property names
for concrete entities should remain consistent with the agreed entity
files and database column names. If the project currently uses
entity-specific keys such as `RoleId` and `UserId`, do not silently
replace them with `Id`. Decide whether `BaseEntity` owns the key or only
the common audit/soft-delete fields, then apply that choice consistently
before creating migrations. The final entity files and EF configuration
are the source of truth.

`IsDeleted` is used for soft deletion: - New records start with
`IsDeleted = false`. - Delete operations set `IsDeleted = true`. -
Records are not physically removed in normal application workflows. -
Normal read queries exclude deleted records. - Unique
constraints/indexes must be designed with the soft-delete strategy in
mind.

### 6.2 Core entities

The following is the agreed entity inventory and relationship
responsibility. Use the existing entity files as the authoritative
source for exact property spelling and types; do not add speculative
columns.

  ------------------------------------------------------------------------
  Entity / table          Responsibility          Main relationships
  ----------------------- ----------------------- ------------------------
  `Role`                  Defines system roles    One role has many users
                                                  and role mappings

  `User`                  Stores system/customer  Belongs to one role;
                          login users             linked to approvals,
                                                  audit logs, and direct
                                                  permission grants

  `Status`                Central lookup for      Referenced by
                          workflow statuses       applications,
                                                  guarantors, approvals,
                                                  loans, schedules, and
                                                  repayments

  `LoanType`              Defines available loan  Has many loan
                          products and terms      applications

  `LoanApplication`       Stores a customer's     Linked to applicant user
                          loan request            and loan type; parent of
                                                  documents, guarantors,
                                                  approvals; may produce
                                                  one loan

  `LoanDocument`          Stores application      Belongs to a loan
                          document                application
                          metadata/reference      

  `LoanGuarantor`         Stores guarantor        Belongs to a loan
                          details and             application; may be
                          verification state      reviewed by a user

  `LoanApproval`          Stores review/decision  Belongs to an
                          records                 application and
                                                  references the
                                                  responsible user and
                                                  decision status

  `Loan`                  Represents the          One-to-one with the
                          approved/disbursed loan originating application;
                                                  parent of repayment
                                                  schedules

  `RepaymentSchedule`     Represents an expected  Belongs to a loan; has a
                          installment             status; parent of
                                                  repayments/allocations
                                                  as defined by the final
                                                  entity design

  `Repayment`             Records a received      Belongs to the relevant
                          repayment               loan/schedule and has a
                                                  status

  `Menu`                  Represents a frontend   May have a parent menu;
                          navigation/menu item    parent-child
                                                  relationship is
                                                  self-referencing

  `RoleMenu`              Maps roles to menus     Links one role and one
                                                  menu

  `AuditLog`              Records important       References the user who
                          user/system actions     performed the action
  ------------------------------------------------------------------------

### 6.3 Dynamic permission entities

These three entities extend the original 14-table model.

#### Permission

Defines an individual capability in the system.

Agreed fields from the design: - `PermissionId` - `PermissionName` (or
the final entity's agreed `Name` property) - `PermissionKey` / `Key` ---
unique machine-readable identifier - `Description` - `IsActive` - Common
`BaseEntity` fields, including `IsDeleted`

Examples of permission keys (illustrative names; align them with actual
controller actions): - `LOAN_APPLICATION_VIEW` -
`LOAN_APPLICATION_CREATE` - `LOAN_APPLICATION_REVIEW` -
`LOAN_APPLICATION_APPROVE` - `LOAN_APPLICATION_REJECT` -
`LOAN_DISBURSE` - `REPAYMENT_VIEW` - `REPAYMENT_RECORD` -
`USER_MANAGE` - `ROLE_PERMISSION_MANAGE`

#### RolePermission

Maps permissions to roles.

Agreed relationship fields: - `RolePermissionId` - `RoleId` --- FK to
`Roles` - `PermissionId` --- FK to `Permissions` - Common `BaseEntity`
fields

A role can have many permissions, and a permission can belong to many
roles through this mapping table.

Add a unique constraint/index on `(RoleId, PermissionId)` so the same
permission cannot be assigned twice to the same role.

#### UserPermission

Maps additional permissions directly to an individual user.

Agreed relationship fields: - `UserPermissionId` - `UserId` --- FK to
`Users` - `PermissionId` --- FK to `Permissions` - Assignment/revocation
metadata such as `AssignedBy`, `AssignedAt`, `IsActive`, `RevokedBy`,
and `RevokedAt` where included in the final entity design - Common
`BaseEntity` fields

Add a unique constraint/index on `(UserId, PermissionId)` for the active
assignment model. If historical grant/revoke records are retained as
separate rows, use an appropriate filtered unique index for active
assignments instead.

**Duration rule:** Direct user permissions have no automatic expiry.
They remain effective until an administrator manually revokes them. Do
not add an expiry date or temporary-duration feature unless the
requirement changes.

## 7. Important Relationships

The EF Core model must configure the relationships explicitly and
consistently.

``` text
Role 1 ───── * User

User 1 ───── * LoanApplication       (applicant relationship)
LoanType 1 ─ * LoanApplication

LoanApplication 1 ─ * LoanDocument
LoanApplication 1 ─ * LoanGuarantor
LoanApplication 1 ─ * LoanApproval
LoanApplication 1 ─ 0..1 Loan

User 1 ───── * LoanGuarantor         (review/verification user, if configured)
User 1 ───── * LoanApproval          (decision maker)
Loan 1 ───── * RepaymentSchedule
RepaymentSchedule 1 ─ * Repayment    (according to final repayment model)

Menu 1 ───── * Menu                  (ParentMenuId self-reference)
Role 1 ───── * RoleMenu
Menu 1 ───── * RoleMenu

Role 1 ───── * RolePermission
Permission 1 ─ * RolePermission

User 1 ───── * UserPermission
Permission 1 ─ * UserPermission

User 1 ───── * AuditLog
```

Workflow records use `StatusId` foreign keys to the central `Statuses`
table. Configure delete behavior deliberately; use restrictive/no-action
behavior for important historical and financial relationships rather
than cascading away loan history.

## 8. Status Management

Do not use C# enums for workflow statuses. Store status records in the
central `Statuses` table and reference them through integer `StatusId`
foreign keys.

The status table should contain the agreed status identity/name fields
and common base fields. Seed the required status rows with stable
identifiers or keys. Business logic should resolve statuses safely
rather than scattering unexplained numeric IDs throughout the code.

Suggested workflow status categories to validate with the team:

-   Application: Draft, Submitted, UnderReview, DocumentsRequired,
    Approved, Rejected, Cancelled
-   Guarantor: Pending, Verified, Rejected
-   Approval: Pending, Approved, Rejected
-   Loan: Approved, Disbursed, Active, Closed, Defaulted
-   Repayment schedule: Pending, PartiallyPaid, Paid, Overdue
-   Repayment: Pending, Completed, Failed, Reversed

These are suggested values for the workflow, not a claim that every
value has already been finalized. If the project already has approved
status names, keep those exact names.

## 9. End-to-End Business Flow

### 9.1 Public/customer journey

1.  A visitor can view available loan types and basic loan information.
2.  A visitor may use a loan estimate/calculator if that feature is
    included in the agreed scope.
3.  The visitor registers or logs in before submitting an application.
4.  The customer creates a loan application and selects a loan type.
5.  The customer enters the required information and submits the
    application.
6.  The customer uploads required supporting documents.
7.  The customer provides guarantor information if required by the loan
    type/policy.
8.  The customer can view application progress and later view
    loan/repayment information.

### 9.2 Internal processing journey

1.  A submitted application becomes available to a Loan Officer.
2.  The Loan Officer checks application details and documents.
3.  The Loan Officer verifies or rejects guarantor information as
    required.
4.  Missing information may be requested from the customer.
5.  The application is forwarded for the required approval decision.
6.  The responsible authorized user records the approval/rejection in
    `LoanApprovals`.
7.  If approved, the system creates the corresponding `Loan`.
8.  The loan is disbursed according to the business process.
9.  A repayment schedule is generated and stored.
10. Received payments are recorded and applied to the relevant
    installment/loan.
11. When the outstanding balance is settled, the loan is marked closed.

The final approval authority and whether a separate Manager role is
required remain business decisions. Do not allow a Loan Officer to
approve their own reviewed application unless the business explicitly
permits it.

## 10. Dynamic Permission System

### 10.1 Why it exists

Static role checks are not flexible enough when an administrator needs
to change what a role can do without changing code. The permission
tables allow role capabilities to be managed as data.

The system has two permission sources:

1.  **Role permissions:** capabilities assigned to a role through
    `RolePermissions`.
2.  **Direct user permissions:** additional capabilities assigned to a
    specific user through `UserPermissions`.

### 10.2 Effective permission calculation

``` text
Effective permissions for a user
    = Active permissions assigned to the user's role
      UNION
      Active permissions assigned directly to that user
```

The resulting list should be distinct so a permission granted through
both sources appears only once.

Direct user permissions are additive. They do not deny or override
permissions inherited from the role. A deny/exception model has not been
requested.

### 10.3 Admin business flow

**Role permission assignment** 1. Admin selects a role. 2. API returns
available permissions and the role's currently assigned permissions. 3.
Admin selects or deselects permissions. 4. API validates the role and
permission IDs. 5. API saves the mapping in `RolePermissions`. 6. Future
authorization checks use the updated mapping.

**Direct user permission assignment** 1. Admin selects a user. 2. API
returns available permissions and the user's direct assignments. 3.
Admin selects additional permissions. 4. API records the grant and who
assigned it. 5. The permission remains active permanently until manually
revoked.

**Direct user permission revocation** 1. Admin opens the user's direct
permission assignments. 2. Admin selects a permission to revoke. 3. API
deactivates/revokes that assignment and records revocation metadata
where supported. 4. The permission is no longer included in that user's
effective permissions unless it is still inherited from the user's role.

### 10.4 Authorization requirements

-   Only an authorized administrator can manage roles, permissions, role
    assignments, and direct user assignments.
-   A user must be authenticated before protected endpoints are
    accessed.
-   Reject inactive or soft-deleted users.
-   Check permissions on the backend API; hiding a frontend menu is not
    security.
-   Load current effective permissions from the database (or use a cache
    with a deliberate invalidation strategy). Do not rely indefinitely
    on stale JWT permission claims after an administrator changes an
    assignment.
-   Protect each endpoint/action with its required permission.
-   Avoid hard-coded role IDs. Use role names/keys or permission keys
    where a role-specific rule is genuinely required.
-   A direct user grant must not automatically grant unrelated
    permissions.

## 11. Menu Access vs API Permissions

`Menus` and `RoleMenus` control navigation visibility. `Permissions`,
`RolePermissions`, and `UserPermissions` control what a user is
authorized to do.

These are related but not interchangeable:

-   A menu may be visible to a role through `RoleMenus`.
-   A user may receive an extra capability through `UserPermissions`.
-   The backend must still enforce permission checks even if a frontend
    menu is hidden.
-   Do not treat menu visibility as proof of API authorization.

## 12. API Endpoint Plan

The routes below are a proposed REST-style endpoint plan. Keep route
naming consistent with the controller names already in the project.

  -----------------------------------------------------------------------
  Controller                          Example endpoints
  ----------------------------------- -----------------------------------
  `AuthController`                    `POST /api/auth/login`,
                                      `POST /api/auth/register`,
                                      `GET /api/auth/me`

  `UsersController`                   `GET /api/users`,
                                      `GET /api/users/{id}`,
                                      `POST /api/users`,
                                      `PUT /api/users/{id}`,
                                      `DELETE /api/users/{id}`

  `RolesController`                   `GET /api/roles`,
                                      `GET /api/roles/{id}`,
                                      `POST /api/roles`,
                                      `PUT /api/roles/{id}`

  `StatusesController`                `GET /api/statuses`

  `LoanTypesController`               CRUD for `/api/loantypes`

  `LoanApplicationsController`        create, get own applications, get
                                      details, submit, review/update
                                      workflow

  `LoanDocumentsController`           upload/register, list by
                                      application, remove/deactivate
                                      document

  `LoanGuarantorsController`          add, list by application,
                                      verify/reject

  `LoanApprovalsController`           record and retrieve approval
                                      decisions

  `LoansController`                   list/details, disburse, close

  `RepaymentSchedulesController`      list schedules by loan, get
                                      installment details

  `RepaymentsController`              record repayment, list payments,
                                      view payment details

  `MenusController`                   list and manage menus

  `RoleMenusController`               get menus for role, assign/remove
                                      role menu

  `PermissionsController`             list, get by ID, create, update,
                                      soft delete

  `RolePermissionsController`         get role permissions, assign
                                      permissions, remove role permission

  `UserPermissionsController`         get direct user permissions, grant,
                                      revoke
  -----------------------------------------------------------------------

Use request/response DTOs for API boundaries where they prevent exposing
sensitive entity fields. Do not bind a public registration or update
request directly to an entity if that would allow a caller to set fields
such as `RoleId`, `IsDeleted`, or password hash.

## 13. Authentication and Security

-   Use JWT Bearer authentication.
-   Store password hashes only; never store plain-text passwords.
-   Validate credentials before issuing a token.
-   Include the minimum identity claims needed to identify the user.
-   Validate token signature, issuer, audience, and expiry.
-   Use HTTPS outside local development.
-   Restrict administrative endpoints.
-   Validate all incoming IDs and request data.
-   Do not return password hashes or sensitive document storage paths in
    API responses.
-   Apply authorization at the API/service boundary for every protected
    operation.

JWT configuration values belong in configuration and secrets should not
be committed to source control. Use development user-secrets or
environment variables for credentials and signing keys.

## 14. EF Core and Database Rules

-   Use `ApplicationDbContext` in `LMS.Infrastructure/Data`.
-   Register all entity sets as `DbSet<T>`.
-   Configure relationships, indexes, precision, and delete behavior
    using Fluent API configuration classes or `OnModelCreating`,
    consistently.
-   Use `int` primary and foreign keys as agreed.
-   Configure required fields and maximum string lengths.
-   Configure monetary values using an explicit decimal precision, such
    as `decimal(18,2)`, after confirming the agreed database precision.
-   Add unique indexes for natural identifiers and mapping pairs where
    appropriate.
-   Configure `StatusId` relationships to `Statuses`.
-   Configure `Menu.ParentMenuId` as an optional self-reference.
-   Avoid cascade deletion for loan, repayment, approval, and audit
    history.
-   Apply `IsDeleted` filtering consistently to normal read operations.
-   Do not physically delete financial records through normal API
    operations.

### Migration workflow

Run EF Core commands from the solution directory or Package Manager
Console with the correct startup and target projects.

Typical CLI commands:

``` powershell
dotnet ef migrations add InitialCreate `
  --project LMS.Infrastructure `
  --startup-project LMS.API

dotnet ef database update `
  --project LMS.Infrastructure `
  --startup-project LMS.API
```

If the EF Core Design package or `dotnet-ef` tool is missing, install
the version compatible with EF Core 8 before running migrations.

## 15. Dependency Injection

`LMS.Application/DependencyInjection.cs` registers application services.

`LMS.Infrastructure/DependencyInjection.cs` registers the database
context and repository implementations.

The API startup should call both extension methods:

``` csharp
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
```

Register each service against its interface and each repository against
its interface. Keep registrations in the relevant project; do not
register Infrastructure implementations inside the Application project.

Example responsibility split:

``` text
Application:
    IPermissionService -> PermissionService
    IRolePermissionService -> RolePermissionService
    IUserPermissionService -> UserPermissionService

Infrastructure:
    IPermissionRepository -> PermissionRepository
    IRolePermissionRepository -> RolePermissionRepository
    IUserPermissionRepository -> UserPermissionRepository
```

## 16. Repository and Service Responsibilities

### Repository

A repository: - Reads and writes database records using EF Core. -
Contains database queries and persistence operations. - Applies query
filters/conditions needed by its methods. - Does not contain HTTP
response logic. - Does not decide API status codes.

### Service

A service: - Implements business rules. - Validates whether an operation
is allowed. - Coordinates one or more repositories. - Calculates
effective permissions. - Decides application workflow transitions. -
Does not depend on MVC controller types.

### Controller

A controller: - Receives HTTP requests. - Validates/binds request
DTOs. - Calls the application service. - Returns suitable HTTP
responses. - Does not contain database queries or core business rules.

Keep repository interfaces in `LMS.Application/Repositories/Interfaces`
and implementations in `LMS.Infrastructure/Repositories`, as agreed.

## 17. Soft Delete and Audit History

`IsDeleted` is the soft-delete flag on every entity.

For ordinary deletion: 1. Find the record. 2. Return not found if it
does not exist or is already deleted. 3. Set `IsDeleted = true`. 4. Set
`UpdatedAt` if the common base fields are configured. 5. Save changes.

Do not cascade soft deletion automatically unless the business rule
explicitly requires it. For example, deleting a loan application must
not silently erase its approvals or repayment history.

`AuditLog` is for meaningful business/security events, such as: - User
login or important authentication events (subject to the audit
policy). - Application submission and status changes. - Document
verification. - Approval/rejection. - Loan disbursement and closure. -
Repayment recording or reversal. - Role/permission assignment and
revocation.

Do not log passwords, tokens, or unnecessary sensitive document
contents.

## 18. Financial and Workflow Integrity

The system must preserve financial history and prevent inconsistent
state changes.

-   An application must satisfy required validation before submission.
-   Only an authorized user can perform a review/approval action.
-   An approval decision should record who made it and when, using the
    fields defined in the final entity.
-   A loan should be created only after the required approval.
-   Disbursement must not be performed twice for the same loan.
-   Repayments must be validated against the loan and its outstanding
    amount according to the approved business policy.
-   Repayment allocation must be traceable to the relevant
    installment/loan.
-   A fully settled loan can be closed.
-   Financial records should not be physically deleted.
-   Use database transactions for multi-step operations that must
    succeed or fail together.

Exact interest calculation, penalty, late-fee, partial-payment,
overpayment, and allocation rules must be confirmed by the business
before implementation. Do not invent financial formulas.

## 19. Recommended Development Sequence

Build and test one module at a time.

  ------------------------------------------------------------------------
                         Order Module / work         Status
  ---------------------------- --------------------- ---------------------
                             1 Solution and          Existing /
                               four-project Clean    established
                               Architecture setup    

                             2 Domain entities and   In progress /
                               `BaseEntity`          established

                             3 EF Core DbContext and In progress
                               relationships         

                             4 Initial migration and Pending confirmation
                               database verification 

                             5 Authentication and    Pending
                               users                 

                             6 Roles and statuses    Pending

                             7 Loan types            Pending

                             8 Loan applications     Pending

                             9 Documents and         Pending
                               guarantors            

                            10 Review and approval   Pending

                            11 Loan creation and     Pending
                               disbursement          

                            12 Repayment schedules   Pending
                               and repayments        

                            13 Menus and role menus  Pending

                            14 Permission CRUD       Permission interface,
                                                     service registration,
                                                     and controller have
                                                     been worked on;
                                                     verify end-to-end

                            15 Role permission       Next
                               assignments           dynamic-permission
                                                     module

                            16 Direct user           After role
                               permission            permissions
                               assignments           

                            17 Backend permission    After assignment
                               authorization         services

                            18 Audit logging and     Pending
                               integration testing   

                            19 End-to-end API        Pending
                               testing and handover  
  ------------------------------------------------------------------------

### Current dynamic-permission progress

-   `IPermissionService.cs` exists in
    `LMS.Application/Services/Interfaces`.
-   `PermissionService.cs` is registered in
    `LMS.Application/DependencyInjection.cs`.
-   The permission controller has been created and the user confirmed
    that the current errors are resolved and the controller work is
    already done.
-   Next planned module: `RolePermission`.
-   After that: `UserPermission`.
-   Then: backend permission authorization and end-to-end tests.

## 20. Immediate Next Files --- RolePermission

Create these files in this order, reusing the existing `RolePermission`
entity if it already exists:

1.  `LMS.Application/Repositories/Interfaces/IRolePermissionRepository.cs`
2.  `LMS.Infrastructure/Repositories/RolePermissionRepository.cs`
3.  `LMS.Application/Services/Interfaces/IRolePermissionService.cs`
4.  `LMS.Application/Services/RolePermissionService.cs`
5.  Update `LMS.Application/DependencyInjection.cs`
6.  Update `LMS.Infrastructure/DependencyInjection.cs`
7.  `LMS.API/Controllers/RolePermissionsController.cs`

Expected capabilities: - Get permissions assigned to a role. - Assign
one or more permissions to a role. - Remove a permission from a role. -
Prevent duplicate role-permission assignments. - Validate that the role
and permission exist and are active/not deleted. - Restrict assignment
changes to Admin. - Keep the role's other assignments unchanged when
adding/removing a specific permission.

Then implement the corresponding UserPermission files and endpoints.

## 21. Testing Checklist

### Authentication

-   Valid login returns a JWT.
-   Invalid credentials are rejected.
-   Expired/invalid tokens are rejected.
-   Inactive or soft-deleted users cannot access protected endpoints.

### CRUD and soft delete

-   Create and retrieve records.
-   Update valid records.
-   Reject invalid IDs and invalid input.
-   Delete marks `IsDeleted = true`.
-   Normal list/get operations do not return soft-deleted records.

### Loan workflow

-   Customer can submit an application.
-   Loan Officer can retrieve and review assigned/available applications
    according to policy.
-   Required documents and guarantor information are validated.
-   Unauthorized users cannot approve/reject.
-   Approved application can produce one loan.
-   Loan disbursement cannot happen twice.
-   Repayment schedule and repayment records remain consistent.

### Dynamic permissions

-   Admin can create/list/update permissions.
-   Admin can assign permissions to a role.
-   Duplicate role-permission assignment is prevented.
-   Admin can grant a direct permission to a user.
-   Direct permission remains active until manually revoked.
-   Revoked direct permission is no longer effective unless inherited
    from the role.
-   Effective permissions are the distinct union of role and direct user
    permissions.
-   Non-admin users cannot manage assignments.
-   API endpoints enforce permissions even if the frontend hides a menu.

## 22. Decisions Still Requiring Business Confirmation

The architecture and main module structure are established, but these
business rules should not be guessed during implementation:

1.  Whether a separate Loan Manager/Approver role is required, or
    whether approval is assigned to an appropriately authorized user.
2.  Exact loan types and product-specific eligibility rules.
3.  Interest calculation method and rate rules.
4.  Installment frequency and schedule-generation rules.
5.  Late fees, penalties, partial payments, overpayments, and repayment
    allocation priority.
6.  Required document types for each loan type.
7.  Whether guarantors are mandatory for all loans or only selected
    products.
8.  File storage provider/path and allowed document formats/size.
9.  Exact final status names and allowed state transitions.
10. Whether a customer can edit or withdraw an application after
    submission.
11. Whether approval requires one decision or multiple approval levels.
12. Exact fields for each entity where the final entity source code has
    not yet been reviewed.

## 23. Non-Negotiable Implementation Guidelines

-   Keep the architecture minimal and consistent.
-   Do not create a fifth project.
-   Do not move repository interfaces into Infrastructure; they belong
    in Application.
-   Do not put repository implementations in Application; they belong in
    Infrastructure.
-   Do not put business logic in controllers.
-   Do not put EF Core database queries in controllers or services.
-   Do not add generic repositories, CQRS, MediatR, or unnecessary
    patterns.
-   Do not use enums for workflow statuses; use the `Statuses` table.
-   Use `int` PK/FK as agreed.
-   Apply `IsDeleted` soft deletion consistently.
-   Keep entity property names and database column names synchronized.
-   Do not invent extra entity columns; confirm them against the agreed
    entity files.
-   Do not hard-code database IDs for roles, statuses, or permissions.
-   Enforce authorization in the backend, not only in the frontend.
-   Keep direct user permissions permanent until an administrator
    manually revokes them.
-   Preserve loan, approval, repayment, and audit history.

------------------------------------------------------------------------

**Working principle:** implement the smallest complete version of each
module, test it, then move to the next module. The database entities,
relationships, and business rules should remain consistent across
Domain, Application, Infrastructure, and API.

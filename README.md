# AcxiomCRM — ASP.NET Core MVC

A starter implementation based on the AcxiomCRM assignment specification. It uses ASP.NET Core MVC/Razor, ASP.NET Core Identity, Entity Framework Core, SQL Server, Bootstrap 5, and Chart.js.

## Implemented in this starter
- Identity login/register/logout, framework-managed password hashes, password policy and lockout.
- Admin, Manager and SalesExecutive roles (seeded on startup).
- Dashboard KPI cards and Chart.js lead/opportunity charts.
- Customer CRUD with required/email/phone validation and duplicate email/phone checks.
- Lead CRUD with status/source/priority fields.
- Opportunity CRUD with amount, probability and expected-close-date business validation.
- Follow-up scheduling, past-date validation and completion.
- Audit logging for core CRM create/update/delete/completion actions; Admin-only audit page.
- Authenticated REST Customer API: GET list, GET by id, POST, PUT and role-protected DELETE/deactivation.
- Search for customer, lead, opportunity and audit lists.
- Anti-forgery tokens on MVC state-changing forms and EF Core parameterized data access.

## Prerequisites
- .NET 8 SDK
- SQL Server LocalDB (default Windows setup), or change `ConnectionStrings:DefaultConnection` in `appsettings.json` to your SQL Server instance.

## Run locally
1. Open a terminal in this folder.
2. Restore packages:
   ```bash
   dotnet restore
   ```
3. Install EF tooling if needed:
   ```bash
   dotnet tool install --global dotnet-ef
   ```
4. Create the database:
   ```bash
   dotnet ef migrations add InitialCreate
   dotnet ef database update
   ```
5. Run:
   ```bash
   dotnet run
   ```
6. Open the HTTPS URL printed in the terminal.

### Demo admin account
- Email: `admin@acxiomcrm.local`
- Password: `Admin@12345!`

The application seeds this local demo account when it starts. Change the password and remove/replace the demo credentials before deployment. Do not publish real secrets or production database credentials.

## API quick test
Sign in and use the authenticated cookie in a browser/API client:
- `GET /api/customers`
- `GET /api/customers/{id}`
- `POST /api/customers`
- `PUT /api/customers/{id}`
- `DELETE /api/customers/{id}` (Admin/Manager; deactivates)

Example POST JSON:
```json
{
  "customerCode": "CUS-1001",
  "customerName": "Ananya Das",
  "email": "ananya@example.com",
  "phone": "9876543210",
  "companyName": "Example Pvt Ltd",
  "address": "Bhubaneswar",
  "city": "Bhubaneswar",
  "state": "Odisha",
  "status": "Active"
}
```

## Before submitting
- Run the project and verify it builds in your environment.
- Set a valid SQL Server connection string.
- Test login, CRUD, invalid form submissions, API status codes and role restrictions.
- Add screenshots and a short test report if requested.
- The assignment asks for assigned-record/team-level authorization, lead conversion, full activity management, monthly sales reporting and more. Treat this as a starter implementation and verify/complete those items rather than claiming every requirement is fully implemented.

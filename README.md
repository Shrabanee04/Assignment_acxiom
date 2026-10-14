# AcxiomCRM — Customer Relationship Management System

AcxiomCRM is a web-based Customer Relationship Management (CRM) application developed using ASP.NET Core MVC. It helps organizations manage customers, leads, sales opportunities, follow-ups, and business activities through a centralized platform.

The application includes role-based access control, data validation, audit logging, dashboard analytics, and REST API functionality to support efficient and secure CRM operations.

## 🚀 Features

### 1. Authentication & Authorization
- User registration, login, and logout.
- Secure password hashing using ASP.NET Core Identity.
- Configurable password policy and account lockout.
- Role-based access for Admin, Manager, and Sales Executive.
- Authorization for protected application pages and API endpoints.

### 2. Dashboard
- Total customer count.
- Total lead count.
- Open opportunities.
- Won and lost opportunities.
- Sales pipeline value.
- Interactive charts using Chart.js.

### 3. Customer Management
- Create, view, update, and delete customer records.
- Search customers by name, email, phone, and company.
- Validate email addresses and phone numbers.
- Prevent duplicate email addresses and phone numbers.
- Track customer status and creation information.

### 4. Lead Management
- Create, view, update, and delete leads.
- Track lead source, status, priority, and expected value.
- Search leads by name, company, and status.
- Support lead statuses such as New, Contacted, Qualified, Converted, and Lost.

### 5. Opportunity Management
- Manage sales opportunities and pipeline stages.
- Track opportunity amount and probability.
- Calculate weighted pipeline value.
- Validate opportunity amounts and probability ranges.
- Prevent past expected close dates for open opportunities.

### 6. Follow-Up Management
- Schedule customer follow-ups.
- Track follow-up types, dates, and status.
- Record follow-up remarks.
- Mark planned follow-ups as completed.
- Validate follow-up dates.

### 7. Audit Logging
- Record important CRM operations.
- Track actions involving customers, leads, opportunities, and follow-ups.
- Store timestamps, user identifiers, and record details.
- Restrict the audit log page to administrators.

### 8. REST API
- Retrieve customer records.
- Retrieve individual customers.
- Create customer records.
- Update customer records.
- Deactivate customers through the API.
- Validate incoming requests and return appropriate HTTP status codes.

### 9. User Interface
- Responsive layout using Bootstrap 5.
- Dashboard cards and interactive charts.
- Searchable data tables.
- Forms with validation messages.
- Consistent navigation across CRM modules.

## 🛠️ Technology Stack

### Backend
- C#
- ASP.NET Core MVC
- ASP.NET Core Identity
- Entity Framework Core

### Frontend
- Razor Views
- HTML5
- CSS3
- Bootstrap 5
- JavaScript
- Chart.js

### Database
- Microsoft SQL Server

### Development Tools
- Visual Studio / Visual Studio Code
- .NET SDK
- Git
- GitHub

## 🏗️ Application Architecture

The application follows a layered MVC architecture.

```text
AcxiomCRM
│
├── Controllers
│   ├── DashboardController
│   ├── CustomersController
│   ├── LeadsController
│   ├── OpportunitiesController
│   ├── FollowUpsController
│   ├── AuditController
│   └── Api
│
├── Models
│   ├── ApplicationUser
│   ├── Customer
│   ├── Lead
│   ├── Opportunity
│   ├── FollowUp
│   ├── Activity
│   └── AuditLog
│
├── Data
│   ├── ApplicationDbContext
│   └── DbInitializer
│
├── Services
│   ├── IAuditService
│   └── AuditService
│
├── Views
│   ├── Dashboard
│   ├── Customers
│   ├── Leads
│   ├── Opportunities
│   ├── FollowUps
│   ├── Audit
│   └── Shared
│
├── Program.cs
├── appsettings.json
└── AcxiomCRM.csproj

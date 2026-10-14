# Submission checklist (read before pushing)
- [ ] Install .NET 8 SDK and SQL Server LocalDB or update connection string.
- [ ] Run `dotnet restore`.
- [ ] Run `dotnet ef migrations add InitialCreate`.
- [ ] Run `dotnet ef database update`.
- [ ] Run `dotnet build` and fix any environment-specific errors.
- [ ] Run the app and verify Register/Login/Logout.
- [ ] Test customer duplicate email/phone and invalid phone.
- [ ] Test opportunity amount <= 0, probability > 100 and past close date.
- [ ] Test past follow-up date.
- [ ] Verify Admin can see Audit Log and non-admin cannot.
- [ ] Test API while authenticated.
- [ ] Replace demo credentials and database configuration before any real deployment.
- [ ] Do not claim features you have not tested.

## Known scope gaps
This is a deadline-oriented starter, not a certified production implementation. Review and complete: per-user/team data scoping for Manager/SalesExecutive, lead conversion workflow, user administration screens, activity CRUD, report exports/monthly sales report, API controllers for leads/opportunities, audit events for Identity login failures/role changes, and full automated tests.

# Employee & Task Management API

**Tech:** C#, ASP.NET Core 10, EF Core, SQL Server, JWT

## Features
- RESTful Web API with CRUD, search/filter and pagination (Employees + Tasks)
- JWT authentication with role-based access (Admin / User), documented in Swagger
- Layered architecture: Controller -> Service -> Repository, Dependency Injection, SQL Server

## Run
1. Install .NET 10 SDK + SQL Server (or LocalDB).
2. Edit `ConnectionStrings:DefaultConnection` and `Jwt:Key` in `appsettings.json`.
3. Install EF tool (once): `dotnet tool install --global dotnet-ef`
4. Create migration: `dotnet ef migrations add InitialCreate`
5. `dotnet run` (DB is migrated automatically at startup) and open `/swagger`.

## Default admin
`admin` / `Admin@123` (seeded on first run - change it!)

## Roles
| Action | Admin | User |
|---|---|---|
| View employees/tasks | yes | yes |
| Create/Update/Delete employees | yes | no |
| Create/Update/Delete tasks | yes | no |
| Update task status (PATCH) | yes | yes |

## Endpoints
- POST /api/auth/register, POST /api/auth/login
- GET /api/employees?search=&department=&page=1&pageSize=10, GET/POST/PUT/DELETE /api/employees/{id}
- GET /api/tasks?search=&status=&priority=&employeeId=&page=1&pageSize=10, GET/POST/PUT/DELETE /api/tasks/{id}
- PATCH /api/tasks/{id}/status

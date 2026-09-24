# SmartSupport API

ASP.NET Core 10 Web API backend for the SmartSupport project.

## Stack
- C# / ASP.NET Core 10
- MongoDB
- JWT authentication
- BCrypt password hashing
- FastAPI ML service (added later)
- Swagger

## Run

1. Start MongoDB locally.
2. Update `appsettings.json` if your MongoDB connection differs.
3. From this folder:

```bash
dotnet restore
dotnet run
```

Swagger is available in Development at `/swagger`.

## Endpoints

### Auth
- `POST /api/auth/register`
- `POST /api/auth/login`
- `GET /api/auth/me`

### Tickets (JWT required)
- `GET /api/tickets`
- `GET /api/tickets/{id}`
- `POST /api/tickets`
- `PATCH /api/tickets/{id}/status`

# Helpdesk System

A full-stack helpdesk and ticket management system built with ASP.NET Core and PostgreSQL.

> 🚧 **Status: In Development**
> The REST API backend is currently under development. The next phase is building the web frontend and integrating it with the API.

---

## Overview

Helpdesk System is a web-based application designed to manage internal support requests through a ticket-based workflow.

Users can create and manage their own support tickets, communicate through comments, and view their ticket history. Administrators can manage users and update ticket status and priority.

The project is being developed as a full-stack application, with the backend API being built first before moving to the frontend.

The current backend provides authentication, authorization, ticket management, comments, user management, data integrity features, and activity logging.

---

## Current Features

### Authentication & Authorization

* JWT-based authentication
* Role-based authorization
* Admin and User roles
* Password hashing with BCrypt
* Authenticated user context
* Protected API endpoints
* Custom `401 Unauthorized` and `403 Forbidden` responses

### Ticket Management

Users can:

* Create tickets
* View their tickets
* View ticket details
* Update their tickets
* Delete their tickets
* Add comments to tickets
* View comments associated with a ticket

Administrators can:

* Update ticket status
* Update ticket priority
* Manage tickets through admin-specific endpoints

Tickets currently support:

* Unique ticket numbers
* Title
* Description
* Status
* Priority
* Ticket owner
* Creation and update timestamps
* Optimistic concurrency versioning

### Comments

* Create comments on tickets
* View comments belonging to a ticket
* Update comments
* Delete comments
* Comment ownership validation
* Optimistic concurrency versioning

### User Management

Administrators can manage users through the API.

User data includes:

* Name
* Email
* Password hash
* Phone number
* Role
* Account status
* Audit information

User listing supports:

* Pagination
* Filtering
* Sorting
* Search

### Pagination, Filtering & Sorting

List endpoints use a reusable pagination response structure.

Example response structure:

```json
{
  "items": [],
  "page": 1,
  "pageSize": 10,
  "totalItems": 25,
  "totalPages": 3
}
```

Ticket queries currently support:

* Search
* Status filtering
* Priority filtering
* Sorting
* Ascending / descending order
* Pagination

---

## Data Integrity

The backend includes several features intended to make the application more reliable as it grows.

### Audit Fields

Entities track information such as:

* `CreatedAt`
* `CreatedBy`
* `UpdatedAt`
* `UpdatedBy`

### Soft Delete

Users, tickets, and comments support soft deletion instead of immediately removing records from the database.

This allows deleted records to remain available for data integrity and auditing purposes.

### Optimistic Concurrency

Tickets, comments, and users use a version value to detect conflicting updates.

For example, if two requests attempt to modify the same record using an outdated version, the application can detect the conflict instead of silently overwriting the newer data.

### Activity Logging

Important actions are recorded through an activity log.

Examples include actions such as:

* Create
* Update
* Delete

Activity logs store information about the affected entity and the user performing the action.

---

## Architecture

The backend currently follows a simple layered architecture:

```text
┌──────────────────────────┐
│       Controllers        │
│   HTTP / API Endpoints   │
└────────────┬─────────────┘
             │
             ▼
┌──────────────────────────┐
│         Services         │
│   Application Logic      │
└────────────┬─────────────┘
             │
             ▼
┌──────────────────────────┐
│      Entity Framework    │
│          Core            │
└────────────┬─────────────┘
             │
             ▼
┌──────────────────────────┐
│        PostgreSQL        │
│         Database         │
└──────────────────────────┘
```

DTOs are used to define API request and response models, while mapper classes handle entity-to-response projections.

The architecture is intentionally kept straightforward. The goal is to maintain clear separation of responsibilities without introducing unnecessary abstractions for the current size and scope of the project.

---

## Tech Stack

### Backend

* **C#**
* **ASP.NET Core 10**
* **Entity Framework Core 10**
* **PostgreSQL**
* **Npgsql**
* **JWT Bearer Authentication**
* **BCrypt**

### API Documentation

* **OpenAPI / Swagger**
* **Scalar**

### Development

* **Git**
* **GitHub**
* **.NET CLI**

The project currently targets `.NET 10` and uses Entity Framework Core with PostgreSQL through Npgsql.

---

## Project Structure

```text
Helpdesk/
│
├── Controllers/
│   ├── AdminTicketsController.cs
│   ├── AuthController.cs
│   ├── CommentsController.cs
│   ├── TicketCommentController.cs
│   ├── TicketsController.cs
│   └── UsersController.cs
│
├── Data/
│   └── AppDbContext.cs
│
├── Dtos/
│   ├── Auth/
│   ├── Comment/
│   ├── Common/
│   ├── Ticket/
│   └── User/
│
├── Exceptions/
│
├── Extensions/
│
├── Helpers/
│
├── Mappers/
│
├── Middleware/
│
├── Models/
│   ├── Base/
│   ├── Enums/
│   ├── ActivityLog.cs
│   ├── Comment.cs
│   ├── Ticket.cs
│   └── User.cs
│
├── Services/
│   ├── ActivityLogService.cs
│   ├── AuthService.cs
│   ├── CommentService.cs
│   ├── CurrentUserAccessor.cs
│   ├── CurrentUserService.cs
│   ├── JwtService.cs
│   ├── TicketService.cs
│   └── UserService.cs
│
├── Migrations/
│
├── Program.cs
├── appsettings.json
└── Helpdesk.Api.csproj
```

---

## API

The API follows REST-style endpoints.

### Authentication

```text
POST /api/auth/login
POST /api/auth/logout
```

### Tickets

```text
GET    /api/tickets
GET    /api/tickets/{id}
POST   /api/tickets
PUT    /api/tickets/{id}
DELETE /api/tickets/{id}
```

### Ticket Comments

```text
GET  /api/tickets/{ticketId}/comments
POST /api/tickets/{ticketId}/comments
```

### Comments

```text
PUT    /api/comments/{id}
DELETE /api/comments/{id}
```

### Admin Tickets

```text
PUT /api/admin/tickets/{id}
```

### Users

User management endpoints are available for administrative operations, including listing, filtering, sorting, and managing users.

All protected endpoints require authentication, with administrative operations additionally restricted by role.

---

## API Documentation

The API uses OpenAPI/Swagger for API specification and Scalar as the interactive API documentation interface during development.

When running the application in development mode, Scalar is configured with the title:

```text
Helpdesk API
```

The API also exposes its Swagger/OpenAPI document for the Scalar interface.

---

## Database

The application uses:

```text
PostgreSQL
     │
     ▼
Entity Framework Core
     │
     ▼
AppDbContext
```

Database schema changes are managed using Entity Framework Core migrations.

The project currently contains migrations covering features such as:

* Initial database structure
* Soft delete
* Created timestamps
* Base entity fields
* Audit fields
* Optimistic concurrency
* Database-side `CreatedAt` defaults
* Enum conversion
* Ticket numbers
* Activity logs
* Admin seeding

---

## Getting Started

### Prerequisites

Make sure the following are installed:

* [.NET 10 SDK](https://dotnet.microsoft.com/)
* PostgreSQL
* Git

### 1. Clone the repository

```bash
git clone <repository-url>
cd <repository-folder>
```

### 2. Configure the database

Configure the PostgreSQL connection string in your application configuration.

Example:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=helpdesk_db;Username=your_username;Password=your_password"
  }
}
```

### 3. Configure JWT

The application requires JWT configuration:

```json
{
  "Jwt": {
    "Key": "your-secret-key",
    "Issuer": "your-issuer",
    "Audience": "your-audience",
    "ExpireMinutes": 60
  }
}
```

> **Important:** Never commit real database credentials or JWT secrets to the repository.

For local development, use environment variables, user secrets, or another secure configuration method.

### 4. Apply database migrations

```bash
dotnet ef database update
```

### 5. Run the application

```bash
dotnet run
```

The development configuration currently uses:

```text
http://localhost:5109
https://localhost:7173
```

### 6. Open API documentation

Run the application and open the Scalar API documentation from the development environment.

---

## Development Roadmap

### Backend

* [x] Project setup
* [x] PostgreSQL integration
* [x] Entity Framework Core
* [x] Database migrations
* [x] User management
* [x] JWT authentication
* [x] Role-based authorization
* [x] Ticket management
* [x] Ticket comments
* [x] Pagination
* [x] Filtering
* [x] Sorting
* [x] DTOs
* [x] Entity-to-DTO mapping
* [x] `AsNoTracking` for read-only queries
* [x] CancellationToken support
* [x] Audit fields
* [x] Soft delete
* [x] Optimistic concurrency
* [x] Database-side `CreatedAt` defaults
* [x] Enum handling
* [x] Ticket numbers
* [x] Activity logging
* [x] API documentation

### Frontend

* [ ] Frontend project setup
* [ ] Authentication flow
* [ ] Login page
* [ ] User dashboard
* [ ] Ticket list
* [ ] Ticket creation
* [ ] Ticket detail page
* [ ] Ticket comments
* [ ] Ticket status and priority display
* [ ] User management
* [ ] Admin ticket management
* [ ] Activity log interface
* [ ] Responsive UI
* [ ] Backend API integration

### Deployment

* [ ] Production configuration
* [ ] Production database
* [ ] Deploy backend API
* [ ] Deploy frontend
* [ ] Configure reverse proxy
* [ ] HTTPS
* [ ] Production API documentation

---

## Future Improvements

After the initial frontend and deployment are completed, possible future improvements include:

* Ticket categories
* Ticket assignment
* File attachments
* Email notifications
* SLA tracking
* Dashboard analytics
* Advanced search
* Reporting
* Monitoring and logging improvements

These features are not part of the current implementation and may be added as the project evolves.

---

## Project Goals

This project is being developed not only as a CRUD application, but as a practical full-stack system for learning and applying software engineering concepts.

The main goals are:

* Building a real-world REST API
* Understanding ASP.NET Core and Entity Framework Core
* Implementing authentication and authorization
* Designing maintainable application structure
* Handling data integrity and concurrent updates
* Building a frontend that consumes the API
* Deploying the application to a production environment

The project prioritizes a practical architecture and incremental development rather than introducing unnecessary complexity.

---

## License

This project is currently intended as a personal learning and portfolio project.

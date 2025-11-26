<<<<<<< HEAD
# MoveOn - Fitness Mobile Application Backend

A comprehensive backend API for a fitness mobile application built with ASP.NET Core 8, following Clean Architecture principles.

## Project Structure

The solution follows Clean Architecture with the following projects:

- **MoveOn.Api**: ASP.NET Core Web API project (entry point)
- **MoveOn.Core**: Domain entities, enums, and service interfaces
- **MoveOn.Infrastructure**: Data access layer with Entity Framework Core
- **MoveOn.Services**: Application/business logic layer
- **MoveOn.Workers**: Background job processing with Hangfire

## Features

### 1. Authentication & Authorization
- JWT-based authentication
- Password hashing with BCrypt.Net-Next
- User registration and login endpoints
- Protected API endpoints

### 2. User Body Records
- Track weight, height, fat percentage, and muscle mass
- CRUD operations for body records
- User-specific data access

### 3. Social Feed
- Create posts with optional image upload
- Pagination support
- Comments and likes system
- User authorization checks

### 4. Real-time Chat
- SignalR integration for real-time messaging
- Conversation management
- Message history and read status

### 5. Background Notifications
- Hangfire for background job processing
- Daily workout reminders
- Weekly progress summaries
- Monthly fitness challenges

## Technology Stack

- **.NET 8** with ASP.NET Core
- **Entity Framework Core** with SQL Server
- **SignalR** for real-time communication
- **Hangfire** for background jobs
- **JWT** for authentication
- **Clean Architecture** principles

## Database Configuration

The application uses SQL Server with Entity Framework Core. All database configurations are done using Fluent API in `EntityConfiguration.cs`.

### Key Entities:
- Users
- BodyRecords
- Posts
- Comments
- Likes
- Conversations
- Messages

## Database Setup

### SQL Server Setup
1. **Install SQL Server** (SQL Server Express, Developer, or Standard)
2. **Create database** named "MoveOn"
3. **Update connection string** in appsettings.json

### Connection String Examples:

**Windows Authentication:**
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=MoveOn;Trusted_Connection=true;MultipleActiveResultSets=true"
  }
}
```

**SQL Server Authentication:**
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=MoveOn;User Id=your_username;Password=your_password;MultipleActiveResultSets=true"
  }
}
```

**Azure SQL Database:**
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=your_server.database.windows.net;Database=MoveOn;User Id=your_username;Password=your_password;MultipleActiveResultSets=true"
  }
}
```

## API Endpoints

### Authentication
- `POST /api/auth/register` - User registration
- `POST /api/auth/login` - User login
- `GET /api/auth/me` - Get current user info

### Body Records
- `POST /api/body-records` - Create body record
- `GET /api/body-records` - Get user's body records
- `DELETE /api/body-records/{id}` - Delete body record

### Posts
- `POST /api/posts` - Create post
- `POST /api/posts/with-image` - Create post with image
- `GET /api/posts` - Get posts (with pagination)
- `GET /api/posts/{id}` - Get specific post
- `DELETE /api/posts/{id}` - Delete post

### Comments
- `POST /api/comments` - Create comment
- `GET /api/comments/post/{postId}` - Get post comments

### Likes
- `POST /api/likes/{postId}` - Like/unlike post
- `GET /api/likes/post/{postId}/count` - Get like count
- `GET /api/likes/post/{postId}/status` - Get like status

### Conversations
- `POST /api/conversations` - Create conversation
- `GET /api/conversations` - Get user conversations
- `GET /api/conversations/{id}` - Get specific conversation
- `POST /api/conversations/{id}/messages` - Send message
- `GET /api/conversations/{id}/messages` - Get conversation messages

## SignalR Hub

The `ChatHub` provides real-time messaging capabilities:
- `SendMessageToUser` - Send private message
- `JoinGroup` - Join group chat
- `SendGroupMessage` - Send group message

## Background Jobs

Hangfire manages several recurring jobs:
- Daily workout reminders (9:00 AM)
- Weekly progress summaries (Sunday 6:00 PM)
- Monthly fitness challenges (1st of month 10:00 AM)

## Configuration

### Database Connection String
Update `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=MoveOn;Trusted_Connection=true;MultipleActiveResultSets=true"
  }
}
```

### JWT Configuration
Update `appsettings.json`:
```json
{
  "Jwt": {
    "Secret": "YourSecretKeyHere",
    "Issuer": "MoveOn",
    "Audience": "MoveOnUsers"
  }
}
```

## Getting Started

1. **Setup Database**:
   - Install SQL Server (Express, Developer, or Standard)
   - Create database named "MoveOn"
   - Update connection string in appsettings.json

2. **Run API**:
   ```bash
   cd MoveOn.Api
   dotnet run
   ```

3. **Run Worker Service**:
   ```bash
   cd MoveOn.Workers
   dotnet run
   ```

4. **Access Swagger UI**:
   - Navigate to `https://localhost:5001` (or your configured port)
   - API documentation available at `/swagger`

5. **Access Hangfire Dashboard**:
   - Navigate to `/hangfire` for job monitoring

## Security Considerations

- JWT tokens expire after 7 days
- Passwords are hashed using BCrypt
- All endpoints (except auth and public posts) require authentication
- User authorization checks ensure users can only access their own data
- CORS is configured for cross-origin requests

## Production Considerations

- Use environment variables for sensitive configuration
- Configure proper database connection pooling
- Set up proper logging and monitoring
- Use persistent storage for Hangfire in production
- Configure proper HTTPS and security headers
- Set up database migrations for schema management
- Consider using SQL Server Management Studio (SSMS) for database administration

## Development Notes

- The project uses top-level statements and modern C# features
- All database configurations use Fluent API (no Data Annotations)
- Clean Architecture principles are strictly followed
- Comprehensive error handling and logging
- Swagger documentation included for API testing
=======
# MoveOn-Backend
>>>>>>> ce844745d0814083a88f71136ba5a43777df307f

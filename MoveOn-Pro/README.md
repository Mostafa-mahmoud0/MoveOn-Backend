# MoveOn - Professional Fitness Backend API

A comprehensive, production-ready backend API for a fitness mobile application built with **Clean Architecture** principles and **professional folder structure**.

## 🏗️ Project Architecture

This solution follows **Clean Architecture** with clear separation of concerns:

```
MoveOn/
├── MoveOn.Core/                    # Domain Layer
│   ├── Models/
│   │   ├── Entities/            # Domain Entities
│   │   ├── Enums/              # Domain Enums
│   │   ├── Requests/           # Request DTOs
│   │   ├── Responses/          # Response DTOs
│   │   └── Common/             # Common Models
│   └── Interfaces/
│       ├── Services/            # Service Interfaces
│       ├── Repositories/       # Repository Interfaces
│       └── Infrastructure/     # Infrastructure Interfaces
├── MoveOn.Infrastructure/          # Data Access Layer
│   ├── Data/
│   │   ├── Contexts/          # Database Contexts
│   │   ├── Configurations/    # Entity Configurations
│   │   └── Repositories/       # Repository Implementations
│   └── Services/
│       ├── External/           # External Services
│       └── Internal/           # Internal Services
├── MoveOn.Services/               # Business Logic Layer
│   └── Services/
│       ├── Authentication/     # Auth Services
│       ├── BodyRecords/       # Body Tracking Services
│       ├── Social/            # Social Media Services
│       ├── Chat/             # Chat Services
│       ├── Notifications/     # Notification Services
│       └── Common/            # Common Services
├── MoveOn.Api/                   # Presentation Layer
│   ├── Controllers/
│   │   ├── Authentication/  # Auth Controllers
│   │   ├── BodyRecords/     # Body Record Controllers
│   │   ├── Social/          # Social Controllers
│   │   ├── Chat/            # Chat Controllers
│   │   └── Users/           # User Controllers
│   ├── Hubs/               # SignalR Hubs
│   ├── Middleware/          # Custom Middleware
│   ├── Filters/             # Action Filters
│   ├── Extensions/          # Service Extensions
│   ├── Configuration/       # Startup Configuration
│   └── wwwroot/             # Static Files
└── MoveOn.Workers/               # Background Jobs
    └── Services/
        ├── Notifications/     # Notification Jobs
        └── BackgroundJobs/    # Scheduled Jobs
```

## 🚀 Key Features

### 🔐 Authentication & Authorization
- **JWT-based authentication** with refresh tokens
- **Role-based authorization** (User, Admin, Trainer)
- **Password policies** with BCrypt hashing
- **Email verification** and password reset
- **Profile management** with image upload
- **Account activation/deactivation**

### 💪 Body Records & Tracking
- **Comprehensive body metrics** (weight, height, fat %, muscle mass)
- **BMI calculation** and trend analysis
- **Progress tracking** with visualizations
- **Goal setting** and achievement tracking
- **Historical data** with date range filtering
- **Data export** (CSV, PDF reports)

### 📱 Social Features
- **Posts** with text and image support
- **Comments** with nested threading
- **Likes** with real-time updates
- **User profiles** with customization
- **Follow/unfollow** system
- **Content reporting** and moderation
- **Search functionality** with filters

### 💬 Real-time Chat
- **Private messaging** with read receipts
- **Group conversations** with multiple participants
- **Online status** indicators
- **Typing notifications**
- **Message search** and filtering
- **Media sharing** in conversations
- **Message reactions** and replies

### 🔔 Notifications System
- **Multi-channel notifications** (Email, Push, SMS, In-App)
- **Workout reminders** with scheduling
- **Social notifications** (likes, comments, follows)
- **Achievement notifications** and badges
- **Notification preferences** per user
- **Bulk notification** handling

## 🛠️ Technology Stack

### Core Framework
- **.NET 6.0** with ASP.NET Core
- **C# 10** with modern language features
- **Clean Architecture** with SOLID principles

### Database & ORM
- **SQL Server** with Entity Framework Core 6.0
- **Code-first migrations** with Fluent API
- **Repository pattern** with Unit of Work
- **Database seeding** and migrations

### Authentication & Security
- **JWT authentication** with bearer tokens
- **Refresh token** mechanism
- **Password hashing** with BCrypt.Net-Next
- **Role-based authorization** with policies
- **CORS configuration** for cross-origin

### Real-time Communication
- **SignalR** for real-time messaging
- **WebSocket connections** with scaling
- **Hub-based architecture** for different features
- **Connection management** and reconnection

### Background Processing
- **Hangfire** for scheduled jobs
- **Recurring jobs** with cron expressions
- **Job monitoring** with dashboard
- **Retry mechanisms** and error handling

### File Storage
- **Local file storage** with organization
- **Image optimization** and thumbnail generation
- **File validation** and security
- **Multiple file types** support

## 📊 API Endpoints

### Authentication (`/api/auth`)
- `POST /register` - User registration
- `POST /login` - User login
- `POST /refresh` - Refresh token
- `POST /forgot-password` - Password reset
- `POST /verify-email` - Email verification
- `GET /profile` - Get user profile
- `PUT /profile` - Update profile
- `POST /change-password` - Change password
- `DELETE /account` - Delete account

### Body Records (`/api/body-records`)
- `GET /` - Get user's body records (paginated)
- `POST /` - Create new body record
- `GET /{id}` - Get specific body record
- `PUT /{id}` - Update body record
- `DELETE /{id}` - Delete body record
- `GET /summary` - Get body record summary
- `GET /progress` - Get progress data
- `GET /export` - Export body records

### Social Features (`/api/posts`, `/api/comments`, `/api/likes`)
- `GET /posts` - Get posts feed (paginated)
- `POST /posts` - Create new post
- `GET /posts/{id}` - Get specific post
- `PUT /posts/{id}` - Update post
- `DELETE /posts/{id}` - Delete post
- `POST /posts/{id}/comments` - Add comment
- `GET /posts/{id}/comments` - Get post comments
- `POST /posts/{id}/like` - Like post
- `DELETE /posts/{id}/like` - Unlike post
- `GET /users/{id}/posts` - Get user's posts
- `POST /users/{id}/follow` - Follow user
- `DELETE /users/{id}/follow` - Unfollow user

### Chat (`/api/conversations`, `/api/messages`)
- `GET /conversations` - Get user conversations
- `POST /conversations` - Create conversation
- `GET /conversations/{id}` - Get conversation details
- `POST /conversations/{id}/messages` - Send message
- `GET /conversations/{id}/messages` - Get conversation messages
- `PUT /messages/{id}/read` - Mark message as read
- `DELETE /messages/{id}` - Delete message

### Users (`/api/users`)
- `GET /search` - Search users
- `GET /{id}` - Get user profile
- `GET /{id}/followers` - Get user's followers
- `GET /{id}/following` - Get users following
- `GET /{id}/stats` - Get user statistics

## 🔧 Configuration

### Database Connection
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=MoveOn;Trusted_Connection=true;MultipleActiveResultSets=true"
  }
}
```

### JWT Configuration
```json
{
  "Jwt": {
    "Secret": "Your-256-bit-secret-key-here",
    "Issuer": "MoveOn",
    "Audience": "MoveOnUsers",
    "ExpirationInMinutes": 1440,
    "RefreshTokenExpirationInDays": 7
  }
}
```

### File Upload Settings
```json
{
  "FileUpload": {
    "MaxFileSize": 10485760,
    "AllowedExtensions": [".jpg", ".jpeg", ".png", ".gif", ".webp"],
    "UploadPath": "wwwroot/uploads",
    "EnableImageOptimization": true
  }
}
```

### Notification Settings
```json
{
  "Notifications": {
    "EnableEmailNotifications": true,
    "EnablePushNotifications": true,
    "EnableSmsNotifications": false,
    "DefaultEmailProvider": "SendGrid",
    "DefaultPushProvider": "Firebase"
  }
}
```

## 🎯 Advanced Features

### Performance Optimization
- **Response caching** with Redis integration
- **Database query optimization** with indexes
- **Lazy loading** for related entities
- **Pagination** for large datasets
- **Image compression** and CDN support

### Security Features
- **Rate limiting** per endpoint
- **Input validation** with Data Annotations
- **SQL injection prevention** with parameterized queries
- **XSS protection** with output encoding
- **CORS configuration** for frontend integration

### Monitoring & Logging
- **Structured logging** with Serilog
- **Performance monitoring** with Application Insights
- **Error tracking** with Sentry integration
- **Health checks** for system monitoring
- **Audit logging** for sensitive operations

### API Documentation
- **OpenAPI/Swagger** documentation
- **API versioning** with versioning strategy
- **Request/Response examples** in documentation
- **Interactive API testing** with Swagger UI
- **API key authentication** for external access

## 🚀 Getting Started

### Prerequisites
- **.NET 6.0 SDK** or later
- **SQL Server 2019** or later
- **Visual Studio 2022** or VS Code

### Setup Instructions

1. **Clone the repository**
   ```bash
   git clone https://github.com/your-repo/MoveOn.git
   cd MoveOn
   ```

2. **Configure the database**
   ```bash
   # Update connection string in appsettings.json
   # Run database migrations
   dotnet ef database update
   ```

3. **Run the application**
   ```bash
   # API
   cd MoveOn.Api
   dotnet run

   # Background Workers
   cd MoveOn.Workers
   dotnet run
   ```

4. **Access the API**
   - **Swagger UI**: `https://localhost:5001/swagger`
   - **Hangfire Dashboard**: `https://localhost:5001/hangfire`
   - **Health Check**: `https://localhost:5001/health`

## 📝 Development Guidelines

### Code Quality
- **Follow Clean Architecture** principles
- **Use dependency injection** throughout
- **Implement proper error handling**
- **Write unit tests** for business logic
- **Use async/await** for I/O operations

### Database Best Practices
- **Use migrations** for schema changes
- **Implement proper indexing**
- **Use transactions** for complex operations
- **Optimize queries** with profiling
- **Implement soft deletes** where appropriate

### API Design
- **Use RESTful conventions**
- **Implement proper HTTP status codes**
- **Provide consistent response format**
- **Use DTOs** for data transfer
- **Validate all inputs** thoroughly

## 🔒 Security Considerations

### Authentication
- **Strong password policies**
- **JWT token expiration**
- **Refresh token rotation**
- **Multi-factor authentication** support
- **Account lockout** after failed attempts

### Data Protection
- **Encryption at rest** for sensitive data
- **HTTPS enforcement** in production
- **Input sanitization** and validation
- **SQL injection prevention**
- **XSS and CSRF protection**

### API Security
- **Rate limiting** per user/IP
- **CORS configuration** for specific origins
- **API versioning** for backward compatibility
- **Request size limits**
- **IP whitelisting** for admin endpoints

## 📈 Monitoring & Analytics

### Application Metrics
- **Response time tracking**
- **Error rate monitoring**
- **User activity logging**
- **Database performance metrics**
- **Memory and CPU usage**

### Business Analytics
- **User registration trends**
- **Feature usage statistics**
- **Content engagement metrics**
- **API usage analytics**
- **Performance benchmarks**

## 🚀 Production Deployment

### Environment Setup
- **Environment variables** for sensitive data
- **Managed database services**
- **Load balancer configuration**
- **SSL/TLS certificates**
- **Backup and recovery** procedures

### Scaling Considerations
- **Horizontal scaling** for API instances
- **Database read replicas** for performance
- **CDN integration** for static assets
- **Message queue** for background processing
- **Caching layer** with Redis

### CI/CD Pipeline
- **Automated testing** on pull requests
- **Database migrations** in deployment pipeline
- **Rollback strategies** for failed deployments
- **Blue-green deployment** for zero downtime
- **Health checks** in load balancer

## 📚 API Documentation

### Interactive Documentation
- **Swagger UI**: `/swagger` - Interactive API explorer
- **ReDoc**: `/redoc` - Alternative documentation view
- **Postman Collection**: Exportable API collection
- **OpenAPI Spec**: `/swagger/v1/swagger.json`

### Code Examples
- **cURL commands** for each endpoint
- **JavaScript/TypeScript** client examples
- **Python** integration examples
- **Mobile app** integration patterns
- **Webhook** implementation guides

## 🤝 Contributing Guidelines

### Development Workflow
1. **Fork** the repository
2. **Create feature branch** from `develop`
3. **Write tests** for new functionality
4. **Ensure all tests pass**
5. **Submit pull request** with description

### Code Standards
- **Follow C# coding conventions**
- **Add XML documentation** for public APIs
- **Write meaningful commit messages**
- **Keep PRs focused** and small
- **Address all review comments**

## 📄 License

This project is licensed under the **MIT License** - see the [LICENSE](LICENSE) file for details.

## 📞 Support

For support and questions:
- **Documentation**: [Wiki](https://github.com/your-repo/MoveOn/wiki)
- **Issues**: [GitHub Issues](https://github.com/your-repo/MoveOn/issues)
- **Discussions**: [GitHub Discussions](https://github.com/your-repo/MoveOn/discussions)
- **Email**: support@moveon.com

---

**Built with ❤️ for the fitness community**
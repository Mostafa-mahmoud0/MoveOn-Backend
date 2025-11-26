using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MoveOn.Api.Hubs;
using MoveOn.Infrastructure.Data;
using MoveOn.Services.Services;
using MoveOn.Core.Interfaces;
using MoveOn.Infrastructure.Services;
using Hangfire;
using Hangfire.MemoryStorage;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Configure services
ConfigureServices(builder.Services, builder.Configuration);

var app = builder.Build();

// Configure middleware pipeline
ConfigureMiddleware(app);

app.Run();

static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
{
    // Add controllers
    services.AddControllers();

    // Add Swagger
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "MoveOn API", Version = "v1" });
        
        // Add JWT Authentication to Swagger
        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer"
        });

        c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                Array.Empty<string>()
            }
        });
    });

    // Configure Database
    var connectionString = configuration.GetConnectionString("DefaultConnection") 
        ?? "Host=localhost;Database=MoveOn;Username=postgres;Password=password";
    
    services.AddDbContext<MoveOnDbContext>(options =>
        options.UseNpgsql(connectionString));

    // Configure JWT Authentication
    var jwtSecret = configuration["Jwt:Secret"] ?? throw new InvalidOperationException("JWT Secret not configured");
    var jwtKey = Encoding.ASCII.GetBytes(jwtSecret);

    services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(jwtKey),
            ValidateIssuer = true,
            ValidIssuer = configuration["Jwt:Issuer"] ?? "MoveOn",
            ValidateAudience = true,
            ValidAudience = configuration["Jwt:Audience"] ?? "MoveOnUsers",
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    });

    services.AddAuthorization();

    // Configure SignalR
    services.AddSignalR();

    // Configure Hangfire
    services.AddHangfire(config =>
        config.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UseMemoryStorage());

    services.AddHangfireServer();

    // Register application services
    services.AddScoped<IAuthService, AuthService>();
    services.AddScoped<IBodyRecordService, BodyRecordService>();
    services.AddScoped<IPostService, PostService>();
    services.AddScoped<ICommentService, CommentService>();
    services.AddScoped<ILikeService, LikeService>();
    services.AddScoped<IConversationService, ConversationService>();
    services.AddScoped<INotificationService, NotificationService>();
    services.AddScoped<IImageUploadService, ImageUploadService>();

    // Add CORS
    services.AddCors(options =>
    {
        options.AddPolicy("AllowAll", builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
    });
}

static void ConfigureMiddleware(WebApplication app)
{
    // Configure the HTTP request pipeline
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "MoveOn API V1");
            c.RoutePrefix = string.Empty; // Set Swagger UI at apps root
        });
    }

    app.UseHttpsRedirection();

    // Use CORS
    app.UseCors("AllowAll");

    // Enable static files for image uploads
    app.UseStaticFiles();

    // Use authentication and authorization
    app.UseAuthentication();
    app.UseAuthorization();

    // Map SignalR hub
    app.MapHub<ChatHub>("/chatHub");

    // Map controllers
    app.MapControllers();

    // Configure Hangfire Dashboard
    app.UseHangfireDashboard("/hangfire", new DashboardOptions
    {
        Authorization = new[] { new HangfireAuthorizationFilter() }
    });

    // Create database if it doesn't exist
    using (var scope = app.Services.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<MoveOnDbContext>();
        context.Database.EnsureCreated();
    }
}

// Custom Hangfire Authorization Filter (for development, allow all access)
public class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
{
    public bool Authorize(DashboardContext context)
    {
        // In production, you should implement proper authorization
        return true;
    }
}
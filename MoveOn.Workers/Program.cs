using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MoveOn.Infrastructure.Data;
using MoveOn.Services.Services;
using MoveOn.Core.Interfaces;
using MoveOn.Infrastructure.Services;
using Hangfire;
using Hangfire.MemoryStorage;

namespace MoveOn.Workers;

public class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                // Configure logging
                services.AddLogging(builder =>
                {
                    builder.AddConsole();
                    builder.AddDebug();
                });

                // Get configuration
                var configuration = hostContext.Configuration;

                // Configure Database
                var connectionString = configuration.GetConnectionString("DefaultConnection") 
                    ?? "Host=localhost;Database=MoveOn;Username=postgres;Password=password";
                
                services.AddDbContext<MoveOnDbContext>(options =>
                    options.UseNpgsql(connectionString));

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

                // Register Hangfire notification service
                services.AddScoped<Services.HangfireNotificationService>();

                // Register the background worker
                services.AddHostedService<Worker>();
            });
}
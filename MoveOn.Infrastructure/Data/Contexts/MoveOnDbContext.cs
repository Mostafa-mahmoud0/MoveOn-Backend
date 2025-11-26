using Microsoft.EntityFrameworkCore;
using MoveOn.Core.Models.Entities;
using MoveOn.Infrastructure.Data.Configurations;

namespace MoveOn.Infrastructure.Data.Contexts;

public class MoveOnDbContext : DbContext
{
    public MoveOnDbContext(DbContextOptions<MoveOnDbContext> options) : base(options)
    {
    }

    // DbSets for all entities
    public DbSet<User> Users { get; set; }
    public DbSet<BodyRecord> BodyRecords { get; set; }
    public DbSet<Post> Posts { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Like> Likes { get; set; }
    public DbSet<Conversation> Conversations { get; set; }
    public DbSet<Message> Messages { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // Apply all configurations
        modelBuilder.ApplyConfiguration(new EntityConfiguration());
    }
}
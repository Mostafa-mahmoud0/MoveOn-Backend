using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MoveOn.Core.Entities;
using MoveOn.Core.Enums;

namespace MoveOn.Infrastructure.Configurations;

public class EntityConfiguration : IEntityTypeConfiguration<User>,
                                    IEntityTypeConfiguration<BodyRecord>,
                                    IEntityTypeConfiguration<Post>,
                                    IEntityTypeConfiguration<Comment>,
                                    IEntityTypeConfiguration<Like>,
                                    IEntityTypeConfiguration<Conversation>,
                                    IEntityTypeConfiguration<Message>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);

        builder.Property(u => u.Id)
            .HasDefaultValueSql("newid()");

        builder.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(u => u.PasswordHash)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(u => u.FirstName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(u => u.LastName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(u => u.ProfileImageUrl)
            .HasMaxLength(500);

        builder.Property(u => u.Role)
            .HasDefaultValue(Role.User)
            .HasConversion<int>();

        builder.Property(u => u.CreatedAt)
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(u => u.UpdatedAt)
            .HasDefaultValueSql("GETUTCDATE()");

        // Create unique index on email
        builder.HasIndex(u => u.Email)
            .IsUnique()
            .HasDatabaseName("IX_Users_Email_Unique");

        // Configure relationships
        builder.HasMany(u => u.BodyRecords)
            .WithOne(br => br.User)
            .HasForeignKey(br => br.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(u => u.Posts)
            .WithOne(p => p.User)
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(u => u.Comments)
            .WithOne(c => c.User)
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(u => u.Likes)
            .WithOne(l => l.User)
            .HasForeignKey(l => l.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(u => u.Conversations1)
            .WithOne(c => c.User1)
            .HasForeignKey(c => c.User1Id)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(u => u.Conversations2)
            .WithOne(c => c.User2)
            .HasForeignKey(c => c.User2Id)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(u => u.SentMessages)
            .WithOne(m => m.Sender)
            .HasForeignKey(m => m.SenderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(u => u.ReceivedMessages)
            .WithOne(m => m.Receiver)
            .HasForeignKey(m => m.ReceiverId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    public void Configure(EntityTypeBuilder<BodyRecord> builder)
    {
        builder.HasKey(br => br.Id);

        builder.Property(br => br.Id)
            .HasDefaultValueSql("newid()");

        builder.Property(br => br.UserId)
            .IsRequired();

        builder.Property(br => br.Weight)
            .IsRequired()
            .HasPrecision(5, 2);

        builder.Property(br => br.Height)
            .IsRequired()
            .HasPrecision(5, 2);

        builder.Property(br => br.FatPercentage)
            .HasPrecision(5, 2);

        builder.Property(br => br.MuscleMass)
            .HasPrecision(5, 2);

        builder.Property(br => br.RecordedAt)
            .IsRequired();

        builder.Property(br => br.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        // Create index for user's body records ordered by recorded date
        builder.HasIndex(br => new { br.UserId, br.RecordedAt })
            .HasDatabaseName("IX_BodyRecords_UserId_RecordedAt");
    }

    public void Configure(EntityTypeBuilder<Post> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .HasDefaultValueSql("newid()");

        builder.Property(p => p.UserId)
            .IsRequired();

        builder.Property(p => p.Content)
            .IsRequired()
            .HasMaxLength(2000);

        builder.Property(p => p.ImageUrl)
            .HasMaxLength(500);

        builder.Property(p => p.CreatedAt)
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(p => p.UpdatedAt)
            .HasDefaultValueSql("GETUTCDATE()");

        // Create index for posts ordered by creation date
        builder.HasIndex(p => p.CreatedAt)
            .HasDatabaseName("IX_Posts_CreatedAt");

        // Configure relationships
        builder.HasOne(p => p.User)
            .WithMany(u => u.Posts)
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(p => p.Comments)
            .WithOne(c => c.Post)
            .HasForeignKey(c => c.PostId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(p => p.Likes)
            .WithOne(l => l.Post)
            .HasForeignKey(l => l.PostId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    public void Configure(EntityTypeBuilder<Comment> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
            .HasDefaultValueSql("gen_random_uuid()");

        builder.Property(c => c.PostId)
            .IsRequired();

        builder.Property(c => c.UserId)
            .IsRequired();

        builder.Property(c => c.Content)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(c => c.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.Property(c => c.UpdatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        // Create index for post comments ordered by creation date
        builder.HasIndex(c => new { c.PostId, c.CreatedAt })
            .HasDatabaseName("IX_Comments_PostId_CreatedAt");

        // Configure relationships
        builder.HasOne(c => c.Post)
            .WithMany(p => p.Comments)
            .HasForeignKey(c => c.PostId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(c => c.User)
            .WithMany(u => u.Comments)
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    public void Configure(EntityTypeBuilder<Like> builder)
    {
        builder.HasKey(l => l.Id);

        builder.Property(l => l.Id)
            .HasDefaultValueSql("gen_random_uuid()");

        builder.Property(l => l.PostId)
            .IsRequired();

        builder.Property(l => l.UserId)
            .IsRequired();

        builder.Property(l => l.CreatedAt)
            .HasDefaultValueSql("GETUTCDATE()");

        // Create unique composite index to prevent duplicate likes
        builder.HasIndex(l => new { l.PostId, l.UserId })
            .IsUnique()
            .HasDatabaseName("IX_Likes_PostId_UserId_Unique");

        // Configure relationships
        builder.HasOne(l => l.Post)
            .WithMany(p => p.Likes)
            .HasForeignKey(l => l.PostId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(l => l.User)
            .WithMany(u => u.Likes)
            .HasForeignKey(l => l.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    public void Configure(EntityTypeBuilder<Conversation> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
            .HasDefaultValueSql("gen_random_uuid()");

        builder.Property(c => c.User1Id)
            .IsRequired();

        builder.Property(c => c.User2Id)
            .IsRequired();

        builder.Property(c => c.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.Property(c => c.UpdatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        // Create unique composite index to prevent duplicate conversations
        builder.HasIndex(c => new { c.User1Id, c.User2Id })
            .IsUnique()
            .HasDatabaseName("IX_Conversations_User1Id_User2Id_Unique");

        // Configure relationships
        builder.HasOne(c => c.User1)
            .WithMany(u => u.Conversations1)
            .HasForeignKey(c => c.User1Id)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(c => c.User2)
            .WithMany(u => u.Conversations2)
            .HasForeignKey(c => c.User2Id)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(c => c.Messages)
            .WithOne(m => m.Conversation)
            .HasForeignKey(m => m.ConversationId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    public void Configure(EntityTypeBuilder<Message> builder)
    {
        builder.HasKey(m => m.Id);

        builder.Property(m => m.Id)
            .HasDefaultValueSql("newid()");

        builder.Property(m => m.ConversationId)
            .IsRequired();

        builder.Property(m => m.SenderId)
            .IsRequired();

        builder.Property(m => m.ReceiverId)
            .IsRequired();

        builder.Property(m => m.Content)
            .IsRequired()
            .HasMaxLength(2000);

        builder.Property(m => m.IsRead)
            .HasDefaultValue(false);

        builder.Property(m => m.CreatedAt)
            .HasDefaultValueSql("GETUTCDATE()");

        // Create index for conversation messages ordered by creation date
        builder.HasIndex(m => new { m.ConversationId, m.CreatedAt })
            .HasDatabaseName("IX_Messages_ConversationId_CreatedAt");

        // Configure relationships
        builder.HasOne(m => m.Conversation)
            .WithMany(c => c.Messages)
            .HasForeignKey(m => m.ConversationId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(m => m.Sender)
            .WithMany(u => u.SentMessages)
            .HasForeignKey(m => m.SenderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(m => m.Receiver)
            .WithMany(u => u.ReceivedMessages)
            .HasForeignKey(m => m.ReceiverId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WeeControl.Core.Domain.User;

[Table(nameof(UserSessionLogDbo), Schema = nameof(User))]
public class UserSessionLogDbo
{
    [Key]
    public Guid LogId { get; }

    public Guid SessionId { get; set; }
    public UserSessionDbo UserSession { get; set; }

    public DateTime LogTs { get; set; }

    public string Context { get; set; }

    public string Details { get; set; }

    internal UserSessionLogDbo()
    {
    }
}

public class UserSessionLogEntityTypeConfig : IEntityTypeConfiguration<UserSessionLogDbo>
{
    public void Configure(EntityTypeBuilder<UserSessionLogDbo> builder)
    {
        builder.Property(p => p.LogId).ValueGeneratedOnAdd().HasDefaultValue(Guid.NewGuid());
        builder.Property(p => p.LogTs).HasDefaultValue(DateTime.UtcNow);

        builder.HasOne(x => x.UserSession)
            .WithMany()
            .HasForeignKey(x => x.SessionId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
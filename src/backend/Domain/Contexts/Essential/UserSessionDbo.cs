using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WeeControl.ApiApp.Domain.Contexts.Essential;

[Table(nameof(UserSessionDbo), Schema = nameof(Essential))]
public class UserSessionDbo
{
    public static UserSessionDbo Create(Guid userid, string deviceid, string otp)
    {
        return new UserSessionDbo() { UserId = userid, DeviceId = deviceid, CreatedTs = DateTime.UtcNow, OneTimePassword = otp};
    }
    
    [Key]
    public Guid SessionId { get; }
    public Guid UserId { get; set; }
    public UserDbo User { get; set; }
    
    [Required]
    [StringLength(128)]
    public string DeviceId { get; set; }
    
    [StringLength(4, MinimumLength = 4)]
    public string OneTimePassword { get; set; }
    
    public DateTime CreatedTs { get; set; }
    public DateTime? TerminationTs { get; set; }

    public virtual IEnumerable<UserSessionLogDbo> Logs { get; set; }

    public UserSessionLogDbo CreateLog(string context, string details)
    {
        return new UserSessionLogDbo() { SessionId = this.SessionId, LogTs = DateTime.UtcNow, Context = context, Details = details };
    }

    private UserSessionDbo()
    {
    }

    public UserSessionDbo(Guid userid, string deviceid, string otp)
    {
        UserId = userid;
        DeviceId = deviceid;
        CreatedTs = DateTime.UtcNow;
        OneTimePassword = otp;
    }
}

public class UserSessionEntityTypeConfig : IEntityTypeConfiguration<UserSessionDbo>
{
    public void Configure(EntityTypeBuilder<UserSessionDbo> builder)
    {
        builder.Property(p => p.SessionId).ValueGeneratedOnAdd().HasDefaultValue(Guid.NewGuid());

        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(x => x.DeviceId).HasMaxLength(255).IsRequired();
            
        builder.Property(p => p.CreatedTs).HasDefaultValue(DateTime.UtcNow);
            
        builder.HasMany(x => x.Logs)
            .WithOne().HasForeignKey(x => x.SessionId);
    }
}
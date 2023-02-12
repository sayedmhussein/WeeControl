using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WeeControl.Core.Domain.Exceptions;

namespace WeeControl.Core.Domain.Contexts.User;

[Table(nameof(UserSessionDbo), Schema = nameof(User))]
public class UserSessionDbo
{
    public static UserSessionDbo Create(Guid userid, string deviceid, string otp)
    {
        if (userid == Guid.Empty)
            throw new DomainOutOfRangeException("User ID can't but empty GUID.");
        
        if (string.IsNullOrWhiteSpace(deviceid))
            throw new DomainOutOfRangeException("Device ID must be supplied.");
        
        if (string.IsNullOrWhiteSpace(otp))
            throw new ArgumentException("OTP must be supplied by application logic when creating new session.");

        var dbo = new UserSessionDbo()
        {
            UserId = userid, DeviceId = deviceid.Trim(), CreatedTs = DateTime.UtcNow, OneTimePassword = otp.Trim()
        };
        
        DomainValidationException.ValidateEntity(dbo);

        return dbo;
    }

    [Key]
    public Guid SessionId { get; init; }
    
    public Guid UserId { get; set; }
    public UserDbo User { get; set; }

    [Required]
    [StringLength(128)]
    public string DeviceId { get; set; }

    [AllowNull]
    [StringLength(4, MinimumLength = 4)]
    public string OneTimePassword { get; set; }

    public DateTime CreatedTs { get; set; }
    public DateTime? TerminationTs { get; set; }

    public virtual IEnumerable<UserSessionLogDbo> Logs { get; set; }

    public UserSessionLogDbo CreateLog(string context, string details)
    {
        return new UserSessionLogDbo() { SessionId = SessionId, LogTs = DateTime.UtcNow, Context = context, Details = details };
    }

    private UserSessionDbo()
    {
    }

    public void DisableOtpRequirement()
    {
        OneTimePassword = null;
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

        builder.Property(x => x.DeviceId);

        builder.Property(p => p.CreatedTs).HasDefaultValue(DateTime.UtcNow);

        builder.HasMany(x => x.Logs)
            .WithOne().HasForeignKey(x => x.SessionId);
    }
}
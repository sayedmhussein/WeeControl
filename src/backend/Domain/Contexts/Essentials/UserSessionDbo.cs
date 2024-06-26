﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WeeControl.Core.DomainModel.Essentials;
using WeeControl.Core.SharedKernel.Exceptions;
using WeeControl.Core.SharedKernel.ExtensionHelpers;

namespace WeeControl.Core.Domain.Contexts.Essentials;

[Table(nameof(UserSessionDbo), Schema = nameof(Essentials))]
public class UserSessionDbo : SessionModel
{
    private UserSessionDbo()
    {
    }

    [Key] public Guid SessionId { get; init; }

    public Guid UserId { get; set; }
    public PersonDbo User { get; set; }

    [Required] [StringLength(128)] public string DeviceId { get; set; }

    [AllowNull]
    [StringLength(4, MinimumLength = 4)]
    public string OneTimePassword { get; set; }

    public virtual IEnumerable<UserSessionLogDbo> Logs { get; set; }

    public static UserSessionDbo Create(Guid userid, string deviceid, string otp)
    {
        if (userid == Guid.Empty)
            throw new EntityDomainValidationException("User ID can't but empty GUID.");

        if (string.IsNullOrWhiteSpace(deviceid))
            throw new EntityDomainValidationException("Device ID must be supplied.");

        if (string.IsNullOrWhiteSpace(otp))
            throw new ArgumentException("OTP must be supplied by application logic when creating new session.");

        var dbo = new UserSessionDbo
        {
            UserId = userid, DeviceId = deviceid.Trim(), CreatedTs = DateTime.UtcNow, OneTimePassword = otp.Trim()
        };


        dbo.ThrowExceptionIfEntityModelNotValid();

        return dbo;
    }

    public UserSessionLogDbo CreateLog(string context, string details)
    {
        return new UserSessionLogDbo
            {SessionId = SessionId, LogTs = DateTime.UtcNow, Context = context, Details = details};
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
        builder.Property(p => p.SessionId).ValueGeneratedOnAdd();

        builder.HasOne(x => x.User)
            .WithMany(x => x.Sessions)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(x => x.DeviceId);

        builder.Property(p => p.CreatedTs).ValueGeneratedOnAdd();

        builder.HasMany(x => x.Logs)
            .WithOne(x => x.UserSession).HasForeignKey(x => x.SessionId);
    }
}
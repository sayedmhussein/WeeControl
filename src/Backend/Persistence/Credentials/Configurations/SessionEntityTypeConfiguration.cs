﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WeeControl.Backend.Domain.Credentials.DatabaseObjects;

namespace WeeControl.Backend.Persistence.Credentials.Configurations
{
    public class SessionEntityTypeConfiguration : IEntityTypeConfiguration<SessionDbo>
    {
        public void Configure(EntityTypeBuilder<SessionDbo> builder)
        {
            builder.ToTable("session", "credentials");
            builder.Property(p => p.SessionId).ValueGeneratedOnAdd();
            builder.Property(p => p.CreatedTs).HasDefaultValue(DateTime.UtcNow);
        }
    }
}

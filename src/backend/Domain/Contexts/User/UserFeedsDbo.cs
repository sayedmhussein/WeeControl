using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WeeControl.Core.SharedKernel.Contexts.User;

namespace WeeControl.Core.Domain.Contexts.User;

[Table(nameof(UserFeedsDbo), Schema = nameof(User))]
public class UserFeedsDbo : HomeFeedModel
{
    [Key]
    public Guid FeedId { get; private set; }
    
    public DateTime? HideTs { get; set; }
}

public class UserFeedEntityTypeConfig : IEntityTypeConfiguration<UserFeedsDbo>
{
    public void Configure(EntityTypeBuilder<UserFeedsDbo> builder)
    {
        builder.Property(p => p.FeedId).ValueGeneratedOnAdd();
        builder.Property(p => p.FeedTs).HasDefaultValue(DateTime.UtcNow);
    }
}
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WeeControl.Core.DomainModel.Essentials;
using WeeControl.Core.SharedKernel.ExtensionMethods;

namespace WeeControl.Core.Domain.Contexts.Essentials;

[Table(nameof(UserFeedsDbo), Schema = nameof(Essentials))]
public class UserFeedsDbo : HomeFeedModel
{
    private UserFeedsDbo()
    {
    }

    [Key] public Guid FeedId { get; private set; }

    public DateTime? HideTs { get; set; }

    public static UserFeedsDbo Create(string subject, string body, string url)
    {
        var model = new UserFeedsDbo
        {
            FeedSubject = subject, FeedBody = body, FeedUrl = url, FeedTs = DateTime.UtcNow
        };

        model.ThrowExceptionIfEntityModelNotValid();

        return model;
    }
}

public class UserFeedEntityTypeConfig : IEntityTypeConfiguration<UserFeedsDbo>
{
    public void Configure(EntityTypeBuilder<UserFeedsDbo> builder)
    {
        builder.Property(p => p.FeedId).ValueGeneratedOnAdd();
        builder.Property(p => p.FeedTs).ValueGeneratedOnAdd();
    }
}
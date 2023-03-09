using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WeeControl.Core.SharedKernel.Contexts.Essentials;
using WeeControl.Core.SharedKernel.ExtensionMethods;

namespace WeeControl.Core.Domain.Contexts.Essentials;

[Table(nameof(UserFeedsDbo), Schema = nameof(Essentials))]
public class UserFeedsDbo : HomeFeedModel
{
    public static UserFeedsDbo Create(string subject, string body, string url)
    {
        
        var model = new UserFeedsDbo()
        {
            FeedSubject = subject, FeedBody = body, FeedUrl = url, FeedTs = DateTime.UtcNow
        };
        
        model.ThrowExceptionIfEntityModelNotValid();

        return model;
    }
    
    [Key]
    public Guid FeedId { get; private set; }
    
    public DateTime? HideTs { get; set; }

    private UserFeedsDbo()
    {
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
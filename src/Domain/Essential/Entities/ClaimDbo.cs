using System;
using System.ComponentModel.DataAnnotations;

namespace WeeControl.Domain.Essential.Entities;

public class ClaimDbo
{
    public static ClaimDbo Create(Guid userId, string type, string tag, Guid grantedById)
    {
        return new ClaimDbo()
        {
            UserId = userId, ClaimType = type, ClaimValue = tag, GrantedById = grantedById
        };
    }
    
    public static ClaimDbo Create(Guid userId, string type, string tag, UserDbo grantedBy)
    {
        return new ClaimDbo()
        {
            UserId = userId, ClaimType = type, ClaimValue = tag, GrantedBy = grantedBy
        };
    }

    [Key]
    public Guid ClaimId { get; }

    public Guid? UserId { get; private set;}
    //public UserDbo User { get; set; }

    [Required]
    [StringLength(5)]
    public string ClaimType { get; private set;}

    public string ClaimValue { get; private set;}

    public DateTime GrantedTs { get; }
    
    public Guid GrantedById { get; private set;}
    public UserDbo GrantedBy { get; private set;}
    
    public DateTime? RevokedTs { get; private set;}
    
    public Guid? RevokedById { get; private set;}
    public UserDbo RevokedBy { get; private set;}

    public void Revoke(Guid revokedById)
    {
        RevokedById = revokedById;
        RevokedTs = DateTime.UtcNow;
    }
    
    public void Revoke(UserDbo revokedBy)
    {
        RevokedBy = revokedBy;
        RevokedTs = DateTime.UtcNow;
    }
    
    private ClaimDbo()
    {
    }
}
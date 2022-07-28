using System;

namespace WeeControl.Domain.Contexts.Essential;

public class IdentityDbo
{
    public Guid IdentityId { get; set; }
    public Guid UserId { get; set; }
    public string Type { get; set; }
    public string Number { get; set; }

    public DateTime IssueDate { get; set; }

    public DateTime? ExpireDate { get; set; }

    public string CountryId { get; set; }
}
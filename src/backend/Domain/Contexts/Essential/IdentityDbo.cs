using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WeeControl.Domain.Contexts.Essential;

[Table(nameof(IdentityDbo), Schema = nameof(Essential))]
public class IdentityDbo
{
    [Key]   
    public Guid IdentityId { get; set; }

    public string Type { get; set; }

    public string Number { get; set; }

    public DateTime IssueDate { get; set; }

    public DateTime ExpireDate { get; set; }

    public string CountryId { get; set; }
}
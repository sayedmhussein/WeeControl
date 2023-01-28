using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WeeControl.Core.Domain.User;

[Table(nameof(EmployeeDbo), Schema = nameof(User))]
public class EmployeeDbo
{
    [Key]
    public Guid EmployeeId { get; }

    public Guid UserId { get; set; }
    public UserDbo User { get; set; }

    public Guid TerritoryId { get; set; }

    [Required]
    [StringLength(45)]
    public string EmployeeNo { get; set; } = string.Empty;

    private EmployeeDbo()
    {
    }

    public EmployeeDbo(Guid userId, Guid territoryId, EmployeeEntity employee)
    {
        UserId = userId;
        TerritoryId = territoryId;
    }
}

public class EmployeeEntityTypeConfig : IEntityTypeConfiguration<EmployeeDbo>
{
    public void Configure(EntityTypeBuilder<EmployeeDbo> builder)
    {
        builder.Property(x => x.EmployeeId).ValueGeneratedOnAdd().HasDefaultValue(Guid.NewGuid());
    }
}
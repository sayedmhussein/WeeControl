using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WeeControl.Common.SharedKernel.Contexts.Essential.Entities;

namespace WeeControl.ApiApp.Domain.Contexts.Essential;

[Table(nameof(EmployeeDbo), Schema = nameof(Essential))]
public class EmployeeDbo : EmployeeEntity
{
    [Key]
    public Guid EmployeeId { get; }
    
    public Guid UserId { get; set; }
    public UserDbo User { get; set; }

    public Guid TerritoryId { get; set; }

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
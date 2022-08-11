using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WeeControl.SharedKernel.Essential.Entities;

namespace WeeControl.Domain.Contexts.Essential;

[Table(nameof(EmployeeDbo), Schema = nameof(Essential))]
public class EmployeeDbo : EmployeeEntity
{
    [Key]
    public Guid EmployeeId { get; }
    
    public Guid UserId { get; set; }
    public UserDbo User { get; set; }

    private EmployeeDbo()
    {
    }

    public EmployeeDbo(Guid userId, EmployeeEntity employee)
    {
        UserId = userId;
        TerritoryName = employee.TerritoryName;
    }
}

public class EmployeeEntityTypeConfig : IEntityTypeConfiguration<EmployeeDbo>
{
    public void Configure(EntityTypeBuilder<EmployeeDbo> builder)
    {
        builder.Property(x => x.EmployeeId).ValueGeneratedOnAdd().HasDefaultValue(Guid.NewGuid());
    }
}
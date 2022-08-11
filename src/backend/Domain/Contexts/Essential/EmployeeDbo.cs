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
    public Guid EmployeeId { get; set; }
    
    public Guid UserId { get; set; }
    public UserDbo User { get; set; }
}

public class EmployeeEntityTypeConfig : IEntityTypeConfiguration<EmployeeDbo>
{
    public void Configure(EntityTypeBuilder<EmployeeDbo> builder)
    {
    }
}
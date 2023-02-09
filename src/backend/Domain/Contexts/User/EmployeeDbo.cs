using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WeeControl.Core.Domain.Contexts.User;

[Table("Employee", Schema = nameof(User))]
public class EmployeeDbo
{
    [Key]
    public Guid EmployeeId { get; }
    
    public Guid? SupervisorId { get; set; }
    public EmployeeDbo Supervisor { get; }
    public IEnumerable<EmployeeDbo> Supervise { get; }

    public Guid PersonId { get; set; }
    public PersonDbo Person { get; set; }

    public Guid TerritoryId { get; set; }

    [Required]
    [StringLength(45)]
    public string EmployeeNo { get; set; } = string.Empty;

    private EmployeeDbo()
    {
    }

    public EmployeeDbo(Guid userId, Guid territoryId, string employeeNo)
    {
        PersonId = userId;
        TerritoryId = territoryId;
        EmployeeNo = employeeNo;
    }
}

public class EmployeeEntityTypeConfig : IEntityTypeConfiguration<EmployeeDbo>
{
    public void Configure(EntityTypeBuilder<EmployeeDbo> builder)
    {
        builder.Property(x => x.EmployeeId).ValueGeneratedOnAdd().HasDefaultValue(Guid.NewGuid());
        
        builder.HasOne(e => e.Supervisor).WithMany();
        builder.HasMany(x => x.Supervise)
            .WithOne(x => x.Supervisor)
            .HasForeignKey(x => x.SupervisorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Person)
            .WithOne()
            .HasForeignKey<EmployeeDbo>(x => x.PersonId)
            .IsRequired();
    }
}
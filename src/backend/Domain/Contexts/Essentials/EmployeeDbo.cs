using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WeeControl.Core.SharedKernel.Contexts.Essentials;

namespace WeeControl.Core.Domain.Contexts.Essentials;

[Table("Employee", Schema = nameof(Essentials))]
public class EmployeeDbo : EmployeeModel
{
    public static EmployeeDbo Create(Guid personId, Guid? supervisorId, EmployeeModel model)
    {
        return new EmployeeDbo()
        {
            PersonId = personId, EmployeeNo = model.EmployeeNo
        };
    }
    
    [Key]
    public Guid EmployeeId { get; }
    
    public Guid? SupervisorId { get; set; }
    public EmployeeDbo Supervisor { get; }
    public IEnumerable<EmployeeDbo> Supervise { get; }

    public Guid PersonId { get; set; }
    public PersonDbo Person { get; set; }

    private EmployeeDbo()
    {
    }
}

public class EmployeeEntityTypeConfig : IEntityTypeConfiguration<EmployeeDbo>
{
    public void Configure(EntityTypeBuilder<EmployeeDbo> builder)
    {
        builder.Property(x => x.EmployeeId).ValueGeneratedOnAdd();//.HasDefaultValue(Guid.NewGuid());
        
        builder.HasOne(e => e.Supervisor).WithMany();
        builder.HasMany(x => x.Supervise)
            .WithOne(x => x.Supervisor)
            .HasForeignKey(x => x.SupervisorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Person)
            .WithOne()
            .HasForeignKey<EmployeeDbo>(x => x.PersonId)
            .IsRequired();

        builder.HasIndex(x => x.EmployeeId).IsUnique();

        builder.HasOne(x => x.Person)
            .WithOne()
            .HasForeignKey<EmployeeDbo>(x => x.PersonId);
    }
}
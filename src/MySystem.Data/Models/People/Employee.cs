﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using MySystem.Data.Models.Basic;

namespace MySystem.Data.Models.People
{
    [Table(nameof(Employee), Schema = nameof(People))]
    [Index(nameof(Username), IsUnique = true)]
    [Index(nameof(OfficeId), IsUnique = false)]
    [Comment("This table inherts from Person table.")]
    public class Employee : Person
    {
        #region ef_functions
        static internal List<Person> GetPersonList(Guid officeid)
        {
            var sayed = new Employee() { PersonId = Guid.NewGuid(), Title = "Mr.", FirstName = "Sayed", LastName = "Hussein", Gender = "m", OfficeId = officeid };
            var اhatem = new Employee() { PersonId = Guid.NewGuid(), Title = "Mr.", FirstName = "Hatem", LastName = "Nagaty", Gender = "m", OfficeId = officeid };
            return new()
            {
                sayed,
                new Employee("Yasser", "Gamal", officeid) { Supervisor = sayed },
                new Employee("Mohamed", "Abdelaal", officeid) { SupervisorId = sayed.PersonId }
            };
        }

        static internal void CreateEmployeeModel(DbContext dbContext, ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>()
                .Property(p => p.IsProductive)
                .HasDefaultValue(false);
        }
        #endregion

        public Guid? SupervisorId { get; set; }
        public virtual Employee Supervisor { get; set; }

        public Guid OfficeId { get; set; }
        public virtual Office Office { get; set; }

        [StringLength(45)]
        public string Username { get; set; }

        [StringLength(45)]
        public string Password { get; set; }

        public int Position { get; set; }

        public int Department { get; set; }

        public bool IsProductive { get; set; }

        #region Constructor
        public Employee() : base()
        {
        }

        public Employee(string firstname, string lastname, Guid officeid) : base(firstname, lastname)
        {
            OfficeId = officeid;
        }
    #endregion
}
}

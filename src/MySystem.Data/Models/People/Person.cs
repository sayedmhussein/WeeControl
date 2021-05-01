using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MySystem.ServerData.Models.People
{
    [Table(nameof(Person), Schema = nameof(People))]
    [Index(nameof(LastName), nameof(FirstName), IsUnique = false)]
    [Comment("Table contains any data related to a person.")]
    public abstract class Person
    {
        #region ef_functions
        static internal void CreatePersonModel(DbContext dbContext, ModelBuilder modelBuilder)
        {
            if (dbContext.Database.IsNpgsql())
            {
                modelBuilder.HasPostgresExtension("uuid-ossp")
                .Entity<Person>()
                .Property(p => p.Id)
                .HasDefaultValueSql("uuid_generate_v4()");
            }
            else
            {
                modelBuilder.Entity<Person>().Property(p => p.Id).ValueGeneratedOnAdd();
            }
        }
        #endregion

        [Key]
        public Guid Id { get; set; }

        [StringLength(10, ErrorMessage = "Always use english common titles not exceeding 10 characters.")]
        public string Title { get; set; }

        [Required]
        [StringLength(45, ErrorMessage = "First name cannot be longer than 50 characters.")]
        [Column("FirstName")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [StringLength(45)]
        public string SecondName { get; set; }

        [StringLength(45)]
        public string ThirdName { get; set; }

        [Required]
        [StringLength(45, ErrorMessage = "Last name cannot be longer than 50 characters.")]
        [Column("LastName")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [StringLength(1, ErrorMessage = "Use either m or f or keep it null.")]
        public string Gender { get; set; }

        public DateTime? DateOfBirth { get; set; }

        [StringLength(3)]
        public string Nationality { get; set; }

        [StringLength(3)]
        public string Language { get; set; }

        public string Email { get; set; }

        public string Mobile { get; set; }

        public string PhotoUrl { get; set; }

        #region Constructors
        public Person()
        {
        }

        public Person(string firstName, string lastName) : this()
        {
            FirstName = firstName;
            LastName = lastName;
        }
        #endregion
    }
}

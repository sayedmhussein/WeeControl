using System;
using MySystem.Data;
using MySystem.Data.Models.People;

namespace MySystem.Api.Dtos.V1
{
    public class EmployeeDto : RepositoryV1<EmployeeDto, Employee>
    {
        public Guid? Id { get; set; }

        public Guid? OfficeId { get; set; }

        public string Title { get; set; }

        public string FirstName { get; set; }

        public string SecondName { get; set; }

        public string ThirdName { get; set; }

        public string LastName { get; set; }

        public string Gender { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string Nationality { get; set; }

        public string Language { get; set; }

        public string Email { get; set; }

        public string Mobile { get; set; }

        public string PhotoUrl { get; set; }

        public Guid? SupervisorId { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public int Position { get; set; }

        public int Department { get; set; }

        public bool IsProductive { get; set; }

        public EmployeeDto()
        {
        }

        public EmployeeDto(DataContext context)
        {
            this.context = context;
        }
    }
}

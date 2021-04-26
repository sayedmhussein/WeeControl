using System;
using MySystem.Data.Data;
using MySystem.Data.Models.People;

namespace MySystem.Data.V1.Dtos
{
    public class EmployeeV1Dto : RepositoryV1<EmployeeV1Dto, Employee>
    {
        public Guid? Id { get; set; }

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

        public Guid OfficeId { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public int Position { get; set; }

        public int Department { get; set; }

        public bool IsProductive { get; set; }

        public EmployeeV1Dto()
        {
        }

        public EmployeeV1Dto(DataContext context)
        {
            this.context = context;
        }
    }
}

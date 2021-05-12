using System;

namespace Sayed.MySystem.SharedDto.V1.Entities
{
    public class EmployeeDto
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
    }
}

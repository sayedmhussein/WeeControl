using System;
using System.ComponentModel.DataAnnotations;
using MySystem.Data;
using MySystem.Data.Models.Basic;

namespace MySystem.Api.Dtos.V1
{
    public class OfficeDto : RepositoryV1<OfficeDto, Office>
    {
        public Guid? Id { get; set; }

        [Required]
        public Guid? ParentId { get; set; }

        [StringLength(3, ErrorMessage = "Only ISO 3166-1 alpha-3 Country Codes.")]
        public string CountryId { get; set; }

        [StringLength(45, ErrorMessage = "Office name must not exceed 45 character.")]
        public string OfficeName { get; set; }

        public OfficeDto()
        {
        }

        public OfficeDto(DataContext context)
        {
            this.context = context;
        }
    }
}

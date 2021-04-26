using System;
using MySystem.Data.Data;
using MySystem.Data.Models.Basic;

namespace MySystem.Data.V1.Dtos
{
    public class OfficeV1Dto : RepositoryV1<OfficeV1Dto, Office>
    {
        public Guid? Id { get; set; }

        public Guid? ParentId { get; set; }

        public string CountryId { get; set; }

        public string OfficeName { get; set; }

        public OfficeV1Dto()
        {
        }

        public OfficeV1Dto(DataContext context)
        {
            this.context = context;
        }
    }
}

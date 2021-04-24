using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MySystem.Data.Data;
using MySystem.Data.Models.Basic;
using MySystem.Data.Models.Component;

namespace MySystem.Data.V1.Dtos
{
    public class BuildingV1Dto : Repository<Building, BuildingV1Dto>, IEntity
    {
        public Guid? Id { get; set; }

        public int? BuildingType { get; set; }

        public string BuildingName { get; set; }

        public string CountryId { get; set; }

        public string AddressLine1 { get; set; }

        public string AddressLine2 { get; set; }

        public double? Latitude { get; set; }

        public double? Longitude { get; set; }

        public BuildingV1Dto()
        {
        }

        public BuildingV1Dto(DataContext context)
        {
            this.context = context;
        }        
    }
}

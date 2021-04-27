using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MySystem.Data;
using MySystem.Data.Models.Basic;

namespace MySystem.Api.Dtos.V1
{
    public class BuildingDto : RepositoryV1<BuildingDto, Building>
    {
        public Guid? Id { get; set; }

        public int? BuildingType { get; set; }

        public string BuildingName { get; set; }

        public string CountryId { get; set; }

        public string AddressLine1 { get; set; }

        public string AddressLine2 { get; set; }

        public double? Latitude { get; set; }

        public double? Longitude { get; set; }

        public BuildingDto()
        {
        }

        public BuildingDto(DataContext context)
        {
            this.context = context;
        }        
    }
}

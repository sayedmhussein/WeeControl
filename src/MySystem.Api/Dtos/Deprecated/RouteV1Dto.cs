using System;
namespace MySystem.Api.Dtos
{
    public class RouteV1Dto : PortfolioBaseV1Dto
    {
        public double? Latitude { get; set; }

        public double? Longitude { get; set; }
    }
}

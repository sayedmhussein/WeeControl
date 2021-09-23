namespace WeeControl.Common.SharedKernel.Abstract.ValueObjects
{
    public abstract record AddressBase
    {
        public string BuildingNo { get; set; }

        public string StreetName { get; set; }

        public string DistrictName { get; set; }

        public string CityName { get; set; }

        public string ZipCode { get; set; }

        public string AdditionalNo { get; set; }

        public double? Latitude { get; set; }

        public double? Longitude { get; set; }
    }
    
    
}
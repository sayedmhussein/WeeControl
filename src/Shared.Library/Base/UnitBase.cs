using System;
using System.ComponentModel.DataAnnotations;

namespace MySystem.Shared.Library.Base
{
    public abstract class UnitBase
    {
       

        [StringLength(10, ErrorMessage = "Always follow conventions ie. 88NB1234.")]
        public string UnitNo { get; set; }

        public Types UnitType { get; set; }

        public Guid BuildingId { get; set; }
        

        public enum Types { Elevator, Escalator, Travellator, Dumbwaiter, Platform, Other }
    }
}

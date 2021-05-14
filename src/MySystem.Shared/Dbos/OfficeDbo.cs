using System;
using System.ComponentModel.DataAnnotations;
using Sayed.MySystem.Shared.Entities;

namespace Sayed.MySystem.Shared.Dbos
{
    public class OfficeDbo : OfficeBase
    {
        [Key]
        [Display(Name = "ID")]
        public Guid Id { get; set; }

        public Guid? ParentId { get; set; }
        public virtual OfficeDbo Parent { get; set; }

        public OfficeDbo()
        {
        }

        public OfficeDbo(string name, string country) : this()
        {
            OfficeName = name;
            CountryId = country;
        }

        public OfficeDbo(string name, string country, Guid parentid) : this(name, country)
        {
            ParentId = parentid;
        }
    }
}

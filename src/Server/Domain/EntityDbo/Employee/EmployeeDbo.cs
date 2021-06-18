using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WeeControl.Server.Domain.EntityDbo.Territory;
using WeeControl.SharedKernel.CommonSchemas.Common.Interfaces;
using WeeControl.SharedKernel.CommonSchemas.Employee.Bases;

namespace WeeControl.Server.Domain.EntityDbo.EmployeeSchema
{
    public class EmployeeDbo : BaseEmployee, IEntityDbo
    {
        [Key]
        public Guid Id { get; set; }

        public virtual EmployeeDbo ReportTo { get; set; }
        public virtual TerritoryDbo Territory { get; set; }

        public virtual ICollection<EmployeeClaimDbo> Claims { get; set; }
        public virtual ICollection<EmployeeIdentityDbo> Identities { get; set; }
        public virtual ICollection<EmployeeSessionDbo> Sessions { get; set; }
    }
   
}

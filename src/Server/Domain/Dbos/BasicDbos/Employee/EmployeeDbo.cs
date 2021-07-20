using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WeeControl.Server.Domain.BasicDbos.Territory;
using WeeControl.SharedKernel.BasicSchemas.Common.Interfaces;
using WeeControl.SharedKernel.BasicSchemas.Employee.Entities;

namespace WeeControl.Server.Domain.BasicDbos.EmployeeSchema
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

﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MySystem.Domain.EntityDbo.PublicSchema;
using MySystem.SharedKernel.Entities.Public.Constants;
using MySystem.SharedKernel.Entities.Employee.Base;
using MySystem.SharedKernel.Interfaces;

namespace MySystem.Domain.EntityDbo.EmployeeSchema
{
    public class EmployeeDbo : EmployeeBase, IEntityDbo
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

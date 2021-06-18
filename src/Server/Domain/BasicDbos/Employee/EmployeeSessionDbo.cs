using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WeeControl.SharedKernel.BasicSchemas.Common.Interfaces;
using WeeControl.SharedKernel.BasicSchemas.Employee.Bases;

namespace WeeControl.Server.Domain.BasicDbos.EmployeeSchema
{
    public class EmployeeSessionDbo : BaseEmployeeSession, IEntityDbo
    {
        public static IEnumerable<EmployeeSessionDbo> InitializeList(Guid employeeid)
        {
            return new List<EmployeeSessionDbo>();
        }

        [Key]
        public Guid Id { get; set; }

        public virtual EmployeeDbo Employee { get; set; }

        public virtual ICollection<EmployeeSessionLogDbo> Logs { get; set; }
    }
}

﻿using System.Collections.Generic;
using MediatR;
using WeeControl.Common.SharedKernel.DataTransferObjectV1.Employee;

namespace WeeControl.Backend.Application.BoundContexts.HumanResources.Commands.AddEmployeeV1
{
    public class AddEmployeeCommand : IRequest<IEnumerable<EmployeeDto>>
    {
        public ICollection<EmployeeDto> List { get; set; }
    }
}
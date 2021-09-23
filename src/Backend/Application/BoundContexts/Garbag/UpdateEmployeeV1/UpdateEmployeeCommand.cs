using System.Collections.Generic;
using MediatR;
using WeeControl.Common.SharedKernel.DataTransferObjectV1.Employee;

namespace WeeControl.Backend.Application.BoundContexts.Garbag.UpdateEmployeeV1
{
    public class UpdateEmployeeCommand : IRequest<IEnumerable<EmployeeDto>>
    {
    }
}

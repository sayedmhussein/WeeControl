using Microsoft.EntityFrameworkCore;
using WeeControl.Common.SharedKernel.BoundedContexts.Shared.Abstract.ValueObjects;

namespace WeeControl.Backend.Domain.BoundedContexts.HumanResources.EmployeeModule.ValueObjects
{
    [Owned]
    public record Address : AddressBase
    {
    }
}
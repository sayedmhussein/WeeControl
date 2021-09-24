using Microsoft.EntityFrameworkCore;
using WeeControl.Common.SharedKernel.Abstract.ValueObjects;

namespace WeeControl.Backend.Domain.BoundedContexts.HumanResources.EmployeeModule.ValueObjects
{
    [Owned]
    public record Identity : IdentityBase
    {
    }
}
using Microsoft.EntityFrameworkCore;
using WeeControl.SharedKernel.Common.Bases;

namespace WeeControl.Server.Domain.HumanResources.ValueObjects
{
    [Owned]
    public record Address : AddressBase
    {
        public string AddressType { get; set; }
    }
}
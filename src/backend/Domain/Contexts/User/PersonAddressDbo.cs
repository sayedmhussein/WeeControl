using System;
using System.ComponentModel.DataAnnotations;
using WeeControl.Core.SharedKernel.Contexts.User;

namespace WeeControl.Core.Domain.Contexts.User;

public class PersonAddressDbo : AddressModel
{
    [Key] 
    public Guid AddressId { get; set; }

    public Guid PersonId { get; set; }
    public PersonDbo Person { get; set; }
}
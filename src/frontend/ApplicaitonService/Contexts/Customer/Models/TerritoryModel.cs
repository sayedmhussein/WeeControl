using WeeControl.SharedKernel.Contexts.Essential.Entities;

namespace WeeControl.Frontend.ApplicationService.Contexts.Customer.Models;

public class TerritoryModel : TerritoryEntity
{
    public TerritoryModel()
    {
    }

    public TerritoryModel(TerritoryEntity territoryModel)
    {
        CountryCode = territoryModel.CountryCode;
    }
}
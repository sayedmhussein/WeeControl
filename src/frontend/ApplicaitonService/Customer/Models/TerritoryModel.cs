using WeeControl.SharedKernel.Essential.Entities;

namespace WeeControl.Frontend.ApplicationService.Customer.Models;

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
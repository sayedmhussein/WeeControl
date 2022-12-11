using WeeControl.Common.SharedKernel.Contexts.Essential.Entities;

namespace WeeControl.Frontend.AppService.Contexts.Essential.Models;

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
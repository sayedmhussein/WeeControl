using WeeControl.Common.SharedKernel.Contexts.Temporary.Entities;

namespace WeeControl.Frontend.AppService.Contexts.Temporary.Models;

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
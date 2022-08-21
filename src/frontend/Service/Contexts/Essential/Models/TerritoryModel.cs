using WeeControl.Common.SharedKernel.Contexts.Essential.Entities;

namespace WeeControl.Frontend.Service.Contexts.Essential.Models;

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
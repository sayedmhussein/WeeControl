using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Refit;
using WeeControl.SharedKernel.EntityGroup.Territory.DtosV1;

namespace CommonLib.DataAccess
{
    public interface ITerritoryData
    {
        [Get("/Territory")]
        Task<List<TerritoryDto>> GetTerritories();
        
        
    }
}
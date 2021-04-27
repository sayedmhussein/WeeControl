using System;
using System.Collections.Generic;

namespace MySystem.Api.Dtos
{
    public class PortfolioV1Dto : PortfolioBaseV1Dto
    {
        public string ContractName { get; set; }

        public List<int> UnitStatues { get; set; }
    }
}

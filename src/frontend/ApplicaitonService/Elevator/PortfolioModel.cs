using WeeControl.SharedKernel.Elevator.Enums;

namespace WeeControl.Frontend.ApplicationService.Elevator;

public class PortfolioModel
{
    public ContractTypeEnum ContractType { get; set; }
    public UnitStateTypeEnum UnitState { get; set; }
    
    public string ContractNo { get; set; }

    public string UnitNo { get; set; }

    public string BuildingName { get; set; }
}
using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using WeeControl.SharedKernel.Essential.DataTransferObjects;
using WeeControl.SharedKernel.Interfaces;
using WeeControl.SharedKernel.RequestsResponses;

namespace WeeControl.Application.Contexts.Essential.Commands;

public class AddOrEditTerritoryCommand : IRequest<IResponseDto<TerritoryModelDto>>
{
    private readonly RequestDto<TerritoryModelDto> dto;

    public AddOrEditTerritoryCommand(RequestDto<TerritoryModelDto> dto)
    {
        this.dto = dto;
    }
    
    public class AddOrEditTerritoryHandler : IRequestHandler<AddOrEditTerritoryCommand, IResponseDto<TerritoryModelDto>>
    {
        public Task<IResponseDto<TerritoryModelDto>> Handle(AddOrEditTerritoryCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
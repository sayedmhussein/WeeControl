using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using WeeControl.SharedKernel.Essential.DataTransferObjects;
using WeeControl.SharedKernel.Interfaces;
using WeeControl.SharedKernel.RequestsResponses;

namespace WeeControl.Application.Contexts.Essential.Commands;

public class AddOrEditTerritoryCommand : IRequest<IResponseDto<TerritoryDto>>
{
    private readonly RequestDto<TerritoryDto> dto;

    public AddOrEditTerritoryCommand(RequestDto<TerritoryDto> dto)
    {
        this.dto = dto;
    }
    
    public class AddOrEditTerritoryHandler : IRequestHandler<AddOrEditTerritoryCommand, IResponseDto<TerritoryDto>>
    {
        public Task<IResponseDto<TerritoryDto>> Handle(AddOrEditTerritoryCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
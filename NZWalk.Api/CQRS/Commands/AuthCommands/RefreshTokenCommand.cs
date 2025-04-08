using MediatR;
using NZWalk.Api.Models.DTO;

namespace NZWalk.Api.CQRS.Commands.AuthCommands
{
    public class RefreshTokenCommand:IRequest<LoginResponseDto>
    {
        

        public RefreshTokenCommand(RefreshTokenRequestDto RequestDto)
        {
            this.RequestDto = RequestDto;
        }

        public RefreshTokenRequestDto RequestDto { get; }
    }
}

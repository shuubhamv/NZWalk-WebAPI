using MediatR;
using NZWalk.Api.Models.DTO;

namespace NZWalk.Api.CQRS.Commands.AuthCommands
{
    public class RegisterCommand:IRequest<string>
    {
        public RegisterRequestDto RequestDto { get; set; }
        public RegisterCommand(RegisterRequestDto requestDto)
        {
            RequestDto = requestDto;
        }
    }
}

using MediatR;
using NZWalk.Api.Models.DTO;

namespace NZWalk.Api.CQRS.Commands.AuthCommands
{
    public class LoginCommand:IRequest<LoginResponseDto>
    {
        public LoginRequestDto RequestDto { get; set; }

        public LoginCommand(LoginRequestDto requestDto)
        {
            RequestDto = requestDto ?? throw new ArgumentNullException(nameof(requestDto));
        }
    }
}

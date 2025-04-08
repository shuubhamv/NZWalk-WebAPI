namespace NZWalk.Api.Models.DTO
{
    public class LoginResponseDto
    {
        public string JwtToken { get; set; }
        public string RefreshToken { get; set; }
    }
}

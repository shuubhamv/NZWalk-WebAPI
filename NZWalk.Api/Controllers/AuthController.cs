using Azure.Core;
using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NZWalk.Api.CQRS.Commands.AuthCommands;
using NZWalk.Api.Models.DTO;
using NZWalk.Api.Repositories;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Text.Json;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace NZWalk.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAngularApp")] // Apply CORS to this controller only
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;  //– Manages user creation, authentication, and roles.
        private readonly ITokenRepository tokenRepository;  //Handles JWT token generation.
        private readonly IMediator mediator;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly ILogger<AuthController> logger;

        public AuthController(UserManager<IdentityUser> userManager,
            ITokenRepository tokenRepository, IMediator mediator, 
            RoleManager<IdentityRole> roleManager, ILogger<AuthController> logger)
        {
            this.userManager = userManager;
            this.tokenRepository = tokenRepository;
            this.mediator = mediator;
            this.roleManager = roleManager;
            this.logger = logger;
        }
        //post: api/auth/Register
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerRequestDto) //Accepts: RegisterRequestDto (username, password, roles).
                                                                                                    //Model Binding (FromBody) – Parses JSON request body.
        { //Creates a new user instance. 
          //var identityUser = new IdentityUser
          //{
          //    UserName = registerRequestDto.Username,
          //    Email = registerRequestDto.Username
          //};
          //var identityResult = await userManager.CreateAsync(identityUser, registerRequestDto.Password);   //Calls CreateAsync() to store user in ASP.NET Identity database.  

            //if (identityResult.Succeeded)
            //{
            //    //add role to user

            //    //  Checks if user registration was successful.
            //    //If roles exist, assigns them using AddToRolesAsync().
            //    if (registerRequestDto.Roles != null && registerRequestDto.Roles.Any())
            //    {
            //        identityResult = await userManager.AddToRolesAsync(identityUser, registerRequestDto.Roles);

            //        if (identityResult.Succeeded)
            //        {
            //            return Ok("User was registered! please login.");  //Returns success message if user & roles are created.
            //        }
            //    }

            //}
            //return BadRequest("Somthing went Wrong");
            logger.LogInformation("[AuthController] Received Register request.");

            try
            {
                var command = new RegisterCommand(registerRequestDto);

                logger.LogInformation($"Finishe Register request with data:{JsonSerializer.Serialize(command)}");

                var result = await mediator.Send(command);

                if (result == "User was registered! Please login.")
                {
                    logger.LogInformation($"[AuthController] User registered successfully: {registerRequestDto.Username}");
                    return Ok(result);
                }
                logger.LogWarning($"[AuthController] Registration failed: {result}");
                return BadRequest(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "[AuthController] Error occurred during registration.");
                return StatusCode(500, "An error occurred while processing  request.");


            }


        }


        //post: api/auth/Login
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
        {
            //var user = await userManager.FindByEmailAsync(loginRequestDto.Username);
            //if (user != null)
            //{
            //    var checkPasswordResult = await userManager.CheckPasswordAsync(user, loginRequestDto.Password);
            //    if (checkPasswordResult)
            //    {
            //        //get roles for this user

            //        var roles = await userManager.GetRolesAsync(user);  //Retrieves assigned roles

            //        if (roles != null) 
            //        {

            //            // create Token
            //            //Calls tokenRepository.CreateJwtToken() to generate a JWT token.
            //            var jwtToken = tokenRepository.CreateJwtToken(user, roles.ToList());

            //            //Creates LoginResponseDto containing the token.
            //            var response = new LoginResponseDto
            //            {
            //                JwtToken = jwtToken
            //            };

            //            return Ok(response);

            //        }               

            //    }

            //}
            //return BadRequest("Somthing went Wrong! Wrong Password or username");
            logger.LogInformation("[AuthController] Received login request.");
            try
            {

                var command = new LoginCommand(loginRequestDto);

                logger.LogInformation($"Finishe Login request with data:{JsonSerializer.Serialize(command)}");

                var response = await mediator.Send(command);

                if (response != null)
                {
                    logger.LogInformation($"[AuthController] User Loged in successfully: {loginRequestDto.Username}");
                    return Ok(response);
                }
                logger.LogWarning($"[AuthController] Login failed for user: {loginRequestDto.Username}");
                return BadRequest("Something went wrong! Wrong password or username.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"[AuthController] Error occurred during login for user: {loginRequestDto.Username}");
                return StatusCode(500, "An error occurred while processing your request.");
            }
            }

        
        [HttpGet("roles")]
        public IActionResult GetRoles()
        {
            logger.LogInformation("[AuthController] Fetching all roles.");

            try
            {
                var roles = roleManager.Roles.Select(r => r.Name).ToList();
                logger.LogInformation($"[AuthController] Retrieved {roles.Count} roles.");
                return Ok(roles);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "[AuthController] Error fetching roles.");
                return StatusCode(500, "An error occurred while fetching roles.");
            }
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequestDto request)
        {
            logger.LogInformation("[AuthController] Received RefreshToken request.");
            try
            {
                var response = await mediator.Send(new RefreshTokenCommand(request));

                logger.LogInformation($"Finishe RefreshToken request with data:{JsonSerializer.Serialize(response)}");
                if (response == null)
                {
                    logger.LogWarning("[AuthController] Invalid refresh token.");
                    return Unauthorized("Invalid refresh token.");
                }
                logger.LogInformation("[AuthController] Token refreshed successfully.");
                return Ok(response);
            }
            catch (Exception ex)
            {

                logger.LogError(ex, "[AuthController] Error occurred during token refresh.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
    }
}

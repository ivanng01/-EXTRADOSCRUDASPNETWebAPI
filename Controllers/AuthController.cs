using EjApi.AccessData.Dto;
using EjApi.AccessData.Interface;
using EjApi.AccessData.Repository;
using EjApi.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace EjApi.Controllers
{
    
    [ApiController]
    [Route("[controller]")]
    public class AuthController: Controller
    {
        private IHashService _hashService;
        
        private IAuthenticationService _authorizationService;

        public AuthController(IHashService hashService, IAuthenticationService authorizationService)

        {
            _hashService = hashService;
            _authorizationService = authorizationService;
        }

        
        [HttpPost("auth")]
        public async Task<IActionResult>Auth([FromBody] LoginRequestDto authorizationRequest)
        {
            try
            {

                LoginDto userData = await _authorizationService.ReturnToken(authorizationRequest);
                if (userData == null) 
                {
                    return BadRequest("User Not Found or Incorrect password");
                }
                return Ok(userData);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error authenticating user {ex.Message}");
                return StatusCode(500, "Error authenticating user" + ex.Message);
            }

        }
    }
}

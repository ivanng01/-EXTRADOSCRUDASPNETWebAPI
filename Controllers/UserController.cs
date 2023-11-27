using EjApi.AccessData.Dto;
using EjApi.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace EjApi.Controllers
{
    
    [ApiController]
    [Route("[controller]")]

    public class UserController : ControllerBase
    {
        private IHashService _hashService;
        private IUserService _userService;

        public UserController(IHashService hashService, IUserService userService)

        {
            _hashService = hashService;
            _userService = userService;
        }
        //Crear nuevo usuario 
        [Authorize]
        [HttpPost("create")]

        public async Task<IActionResult> CreateUser(CreateUserRequestDto createUserRequest)
        {
            if (string.IsNullOrEmpty(createUserRequest.name_user) || string.IsNullOrEmpty(createUserRequest.mail_user) || string.IsNullOrEmpty(createUserRequest.password_user))
            {
                //return BadRequest(new CreateUserDto({ msg = "Name, mail, and password are required", result = false });
                return BadRequest("Name, mail and password are requited");
            }
            if (!_userService.IsValidEmail(createUserRequest.mail_user)) return BadRequest(new CreateUserDto {check = false });

            try
            {
                CreateUserDto user = await _userService.CreateUserServ(createUserRequest);
                Console.WriteLine("Create User OK");
                return Ok(user);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error creating new user {e.Message }");
                return StatusCode(500, "Server error:"+e.Message);
            }
        }

        //Obtener usuario por su id
        [Authorize]
        [HttpPost("getuser")]
        public async Task<IActionResult> GetUserById([FromBody] GetUserByIdRequestDto getUserByIdRequestDTO)
        {
            if (getUserByIdRequestDTO.id_user == 0 || string.IsNullOrEmpty(getUserByIdRequestDTO.password_user))
            {
                //return BadRequest(new GetUserByIdDto { msg = "id and password are required", result = false });
                return BadRequest("Password and Id are required");
            }
            try
            {
                GetUserByIdDto user = await _userService.GetUserByIdSecServ(getUserByIdRequestDTO);
                Console.WriteLine("Filtered user");
                return Ok(user);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error getting user {e.Message}");
                return StatusCode(500, "Error:" + e.Message);
            }
        }

        //Modificar usuario por su id
        [Authorize]
        [HttpPut("update")]
        public async Task<IActionResult> UpdateUserById([FromBody] UpdateUserRequestDto updateUserRequestDTO)
        {
            if (updateUserRequestDTO.id_user == 0 || string.IsNullOrEmpty(updateUserRequestDTO.name_user))
            {
                //return StatusCode(400, new GetUserByIdDto { msg = "Name and id are required", result = false });
                return BadRequest("Name, Id are requited");

            }
            try
            {
                
                var userModifycated = await _userService.UpdateUserByIdServ(updateUserRequestDTO);
                if (userModifycated == 0) 
                { 
                    return StatusCode(404, $"User not found id: {updateUserRequestDTO.id_user}"); 
                }
                GetUserByIdDto user = await _userService.GetUserByIdServ(updateUserRequestDTO.id_user);
                Console.WriteLine("Modified User OK");
                return Ok(user);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error editing user {e.Message}");
                return StatusCode(500, "Error" + e.Message);
            }
        }


        //Eliminar usuario por su id
        [Authorize]
        [HttpDelete("delete/{id_user}")]
        public async Task<IActionResult> DeleteUserById(int id_user)
        {
            if (id_user == 0) 
            { 
            return BadRequest("Id is required");
            }
            try
            {
                var user = await _userService.DeleteUserByIdServ(id_user);

                if (user == 0) 
                { 
                    return StatusCode(404, $"User not found by id: {id_user}"); 
                }
                Console.WriteLine("Delete User OK");
                return Ok("User deleted");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error deleting user {e.Message}");
                return StatusCode(500, "Error" + e.Message);
            }
        }
    }
}
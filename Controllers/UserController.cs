﻿using AccessData.Dto;
using AccessData.InputsRequest;
using EjApi.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EjApi.Controllers
{

    [ApiController]
    [Route("api/[controller]")]


    public class UserController : ControllerBase
    {

        private IUserService _userService;

        private IHashService _hashService;

        public UserController(IHashService hashService, IUserService userService)
        {
            _hashService = hashService;
            _userService = userService;
        }

        //ver este punto
        //[Authorize(Roles = "Admin")]
        [HttpPost("createuser")]
        //registrar un usuario con roles
        public async Task<IActionResult> CreateUserWithRole(CreateUserWithRoleRequest createUserRequest)
        {
            if (string.IsNullOrEmpty(createUserRequest.name_user) || string.IsNullOrEmpty(createUserRequest.mail_user) || string.IsNullOrEmpty(createUserRequest.password_user) ||
               string.IsNullOrEmpty(createUserRequest.role_user)) return BadRequest("Name, mail,role and password are required");

            if (!_userService.IsValidEmail(createUserRequest.mail_user)) return BadRequest("Invalid email format");
            try
            {

                CreateUserWithRoleDto user = await _userService.CreateUserWithRoleService(createUserRequest);
                if (user.msg == "The role does not exist") return Conflict("The role does not exist");
                if (user.msg == "The email is already in use") return Conflict("The email is already in use");
                if (user.msg == "server error") return StatusCode(500, user.msg);
                return Ok(user);
            }
            catch (Exception Ex)
            {
                Console.WriteLine($"Error creating a new user {Ex.Message}");
                return StatusCode(500, "server error:");
            }
        }

        // obtener usuario por id
        //analist
        //[Authorize(Roles = "User")]
        [HttpPost("getuser")]
        public async Task<IActionResult> GetUserById([FromBody] GetUserByIdRequest getUserByIdRequestDTO)
        {
            if (getUserByIdRequestDTO.id_user == 0 || string.IsNullOrEmpty(getUserByIdRequestDTO.password_user))
                return BadRequest("id and password are required");

            try
            {
                GetUserByIdDto user = await _userService.GetUserByIdProtectedService(getUserByIdRequestDTO);
                if (user.msg == "User not found") return NotFound("User not found");
                if (user.msg == "Incorrect password") return BadRequest("Incorrect password");
                if (user.msg == "server error") return StatusCode(500, "server error");
                return Ok(user);
            }
            catch (Exception Ex)
            {
                Console.WriteLine($"Error getting user  errir{Ex.Message}");
                return StatusCode(500, "Server error:");
            }
        }

        // actualizar usuario por id
        //[Authorize(Roles = "User")]
        [HttpPut("update")]
        public async Task<IActionResult> UpdateUserById([FromBody] UpdateUserRequest updateUserRequestDTO)
        {
            if (updateUserRequestDTO.id_user == 0 || string.IsNullOrEmpty(updateUserRequestDTO.name_user))
                return BadRequest("Name and id are required");
            try
            {

                var userModifycated = await _userService.UpdateUserByIdService(updateUserRequestDTO);
                if (userModifycated == 0) { return NotFound($"User not found id: {updateUserRequestDTO.id_user}"); }
                GetUserByIdDto user = await _userService.GetUserByIdService(updateUserRequestDTO.id_user);

                return Ok(user);
            }
            catch (Exception Ex)
            {
                Console.WriteLine($"Error editing user {Ex.Message}");
                return StatusCode(500, "Server Error");
            }
        }

        // borrar usuario por id
        //[Authorize(Roles = "User")]
        [HttpDelete("delete/{id_user}")]
        public async Task<IActionResult> DeleteUserById(int id_user)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            int userIdInt = int.Parse(userId);
            if (userIdInt != id_user) return Unauthorized("Invalid user ID");
            if (id_user == 0) return BadRequest("id is required");
            try
            {

                var user = await _userService.DeleteUserByIdService(id_user);

                if (user == 0) { return NotFound($"User not found id: {id_user}"); }

                return Ok("user deleted");
            }
            catch (Exception Ex)
            {
                Console.WriteLine($"Error deleting user {Ex.Message}");
                return StatusCode(500, "Server Error");
            }
        }
    }
}
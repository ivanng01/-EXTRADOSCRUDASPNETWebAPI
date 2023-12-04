using AccessData.Dto.RoleDto;
using AccessData.InputsRequest;
using AccessData.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Interface;

namespace EjApi.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }


        //ver este punto
        //[Authorize(Roles = "Admin")]
        [HttpPost("create")]

        public async Task<IActionResult> CreateRole([FromBody] CreateRoleRequest roleRequest)
        {
            if (string.IsNullOrEmpty(roleRequest.name_role)) return BadRequest("Name is required");
            if (string.IsNullOrEmpty(roleRequest.description_role)) return BadRequest("Description is required");

            try
            {


                CreateRoleDto roleCreated = await _roleService.CreateRoleService(roleRequest);
                if (roleCreated.msg == "The role is already in use") return Conflict("The role is already in use");
                if (roleCreated.msg == "server error") return StatusCode(500, "server error");
                return Ok(roleCreated);
            }
            catch (Exception Ex)
            {
                Console.WriteLine($"Error creating a new role {Ex.Message}");
                return StatusCode(500, "server error:");
            }


        }




    }


}

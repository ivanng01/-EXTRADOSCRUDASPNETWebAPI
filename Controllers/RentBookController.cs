using AccessData.Dto.RentBookDto;
using AccessData.InputsRequest;
using AccessData.Models;
using EjApi.AccessData.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Interface;
using System.Security.Claims;

namespace EjApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RentBookController : ControllerBase
    {

        private IRentBookService _rentBookService;

        public RentBookController(IRentBookService rentBookService)
        {
            _rentBookService = rentBookService;

        }

        //[Authorize(Roles = "User")]
        [HttpPost("create")]

        public async Task<IActionResult> CreateRentBook([FromBody] CreatRentBookControllerRequest RentBookRequest)
        {
            try
            {
                // la fecha no debe ser anterior a la actual(en utc)
                DateTime utcNow = DateTime.UtcNow;
                if (RentBookRequest.rentDate < utcNow) return BadRequest("Date is in the past");
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                int userIdInt = int.Parse(userId);
                if (userIdInt != RentBookRequest.id_user) return Unauthorized("Invalid user ID");
                CreateRentBookDto bookCreated = await _rentBookService.CreateRentBookService(RentBookRequest);
                if (bookCreated.msg == "server error") return StatusCode(500, "server error");
                if (bookCreated.msg == "book not found") return NotFound("book not found");
                return Ok(bookCreated);
            }
            catch (Exception Ex)
            {
                Console.WriteLine($"Error creating a new book {Ex.Message}");
                return StatusCode(500, "server error:");
            }
        }
    }
}

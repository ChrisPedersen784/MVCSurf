using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SurfBoardProject.Data;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace API.Controllers
{
    [Route("api/shop")]
    [ApiController]
    public class WebShop : ControllerBase
    {

        private readonly SurfBoardProjectContext _context;


        public WebShop(SurfBoardProjectContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var rentals = await _context.BoardModel.ToListAsync();
                var options = new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.Preserve,
                };

                var jsonSerialized = System.Text.Json.JsonSerializer.Serialize(rentals, options);


                // Return JSON response
                return Ok(jsonSerialized);
            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately, e.g., log the error
                return BadRequest(ex.Message);
            }
        }
    }
}

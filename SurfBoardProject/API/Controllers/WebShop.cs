using Lib.Product.Models;
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
        public async Task<ActionResult<IEnumerable<Surfboard>>> Get()
        {
            if (_context.BoardModel == null)
            {
                return NotFound();
            }

            var validSurfboards = await _context.BoardModel
                .Where(surfboard => surfboard.Price <= 800)
                .Select(board => new Surfboard
                {
                    Id = board.Id,
                    Name = board.Name,
                    Length = board.Length,
                    Width = board.Width,
                    Volume = board.Volume,
                    ImgUrl = board.ImgUrl,
                    BoardDescription = board.BoardDescription,
                    Price = board.Price,
                    Equipment = board.Equipment,
                    IsAvailable = board.IsAvailable,
                    RowVersion = board.RowVersion
                })
                .ToListAsync();

            return Ok(validSurfboards); // Use Ok() to return 200 OK status along with the data
        }



    }
}







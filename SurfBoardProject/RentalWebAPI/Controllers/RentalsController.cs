using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SurfBoardProject.Data;
using SurfBoardProject.Models;

namespace RentalWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RentalsController : ControllerBase
    {
        private readonly SurfBoardProjectContext _context;

        public RentalsController(SurfBoardProjectContext context)
        {
            _context = context;
        }

        // GET: api/Rentals
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Rental>>> GetRentals()
        {
            try
            {
                var rentals = await _context.Rental.ToArrayAsync();
                return Ok(rentals);
            }
            catch (Exception ex)
            {

                return BadRequest(ex);
            }

        }

        // GET: api/Rentals/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Rental>> GetRental(int id)
        {
            var rental = await _context.Rental.FindAsync(id);

            if (rental == null)
            {
                return NotFound();
            }

            return Ok(rental);
        }

        // POST: api/Rentals
        [HttpPost]
        public async Task<IActionResult> CreateRental(Rental rental)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var rentals = new Rental
            {
                Start = rental.Start,
                End = rental.End,
                Price = rental.Price
            };

             _context.Rental.Add(rentals);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRental", new { id = rental.RentalId }, rental);
        }

        // Implement other actions (Edit, Delete, etc.) as needed

        private bool RentalExists(int id)
        {
            return _context.Rental.Any(e => e.RentalId == id);
        }
    }
}

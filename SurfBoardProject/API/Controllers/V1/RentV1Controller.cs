﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SurfBoardProject.Data;
using SurfBoardProject.Models;
using System.Linq;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace API.Controllers.V1
{
    [Route("api/rent")]
    [ApiVersion("1.0")]
    [ApiController]
    public class RentV1Controller : ControllerBase
    {
        private readonly SurfBoardProjectContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public RentV1Controller(SurfBoardProjectContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Book()
        {
            try
            {
                string[] lightVersion = { "The Minilog", "The Bomb", "Six Tourer", "Naish One" };
                var boardsLight = await _context.BoardModel
                    .Where(x => lightVersion.Contains(x.Name))
                    .ToListAsync();

                // Serialize the list of boards to JSON
                var jsonSerialized = JsonSerializer.Serialize(boardsLight);

                // Return JSON response
                return Ok(jsonSerialized);
            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately, e.g., log the error
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("GetRental")]
        public async Task<IActionResult> GetRental([FromQuery] string userId)
        {
            try
            {
                // Use the userId parameter to filter rentals
                var rentals = await _context.Rental
                .Where(r => r.Customers.Any(c => c.UserId == userId) && r.Boards.Any(ren => ren.IsAvailable != 0))
                .Include(r => r.Boards)
                .Include(r => r.Customers)
                .ToListAsync();

                var options = new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.Preserve,
                };

                var jsonSerialized = System.Text.Json.JsonSerializer.Serialize(rentals, options);


                return Ok(jsonSerialized);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }



        [HttpPost("{id}/rent")]
        public async Task<IActionResult> CreateRental(int id, [FromBody] RentalCustomer rentalCustomer, [FromQuery] string userId)
        {
            int maxRental = 2;

            var rentals = _context.Customer
                .Include(c => c.Rentals)
                .Where(c => c.UserId == userId &&
                c.Name.Contains(rentalCustomer.Customer.Name) &&
                c.LastName.Contains(rentalCustomer.Customer.LastName) &&
                c.Email.Contains(rentalCustomer.Customer.Email))
                .ToList();
            var totalRentals = rentals.Sum(c => c.Rentals.Count);

            if (totalRentals < maxRental)
            {
                var boardToUpdate = await _context.BoardModel.FirstOrDefaultAsync(m => m.Id == id);

                if (boardToUpdate == null)
                {
                    ModelState.AddModelError(string.Empty, "Board not found.");
                    return BadRequest(ModelState);
                }

                if (boardToUpdate.IsAvailable == 0)
                {
                    ModelState.Remove("Rental.RowVersion");

                    if (ModelState.IsValid)
                    {
                        // Proceed with booking
                        boardToUpdate.IsAvailable = 1;

                        var newRental = new Rental
                        {
                            Start = rentalCustomer.Rental.Start,
                            End = rentalCustomer.Rental.End,
                            Price = rentalCustomer.Rental.Price,
                            // RowVersion will be generated by the database
                            Boards = new List<BoardModel> { boardToUpdate }
                        };

                        var newCustomer = new Customer
                        {
                            UserId = userId,
                            Name = rentalCustomer.Customer.Name,
                            LastName = rentalCustomer.Customer.LastName,
                            Email = rentalCustomer.Customer.Email,
                            PhoneNumber = rentalCustomer.Customer.PhoneNumber,
                            Rentals = new List<Rental> { newRental }
                        };

                        _context.Customer.Add(newCustomer);

                        try
                        {
                            // SaveChangesAsync will handle concurrency conflicts
                            await _context.SaveChangesAsync();

                            return CreatedAtAction("Book", new { id = newRental.RentalId }, rentalCustomer);
                        }
                        catch (DbUpdateConcurrencyException)
                        {
                            ModelState.AddModelError(string.Empty, "Concurrency conflict occurred.");
                            return Conflict(ModelState);
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "The chosen board has already been booked by another user, please choose another board");
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "You have reach your rental limit");
                return BadRequest(ModelState);
            }
                return BadRequest(ModelState);

        }


    }
}
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging;
using SurfBoardProject.Data;
using SurfBoardProject.Models;
using SurfBoardProject.Utility;


namespace SurfBoardProject.Controllers
{
    public class RentalsController : Controller
    {
        private readonly SurfBoardProjectContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public RentalsController(SurfBoardProjectContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Rentals
        public async Task<IActionResult> Index()
        {

            //var user =  _context.Rental
            //    .Include(r => r.Boards)
            //    .Include(r => r.Customers);
            //return View(await user.ToListAsync());  

            return _context.Rental != null ?
                        View(await _context.Rental.ToListAsync()) :
                        Problem("Entity set 'SurfBoardProjectContext.Rental'  is null.");
        }


        // GET: Customers
        public async Task<IActionResult> ShowBookedSurfBoards()
        {
            var userId = _userManager.GetUserId(User);

            // Retrieve rentals for the user and include related BoardModel and Customer
            var rentals = _context.Rental
                .Where(r => r.Customers.Any(c => c.UserId == userId))
                .Include(r => r.Boards)
                .Include(r => r.Customers)
                .ToList();

            return View(rentals);
        }



        // GET: Rentals/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Rental == null)
            {
                return NotFound();
            }

            var rental = await _context.Rental
                .FirstOrDefaultAsync(m => m.RentalId == id);
            if (rental == null)
            {
                return NotFound();
            }

            return View(rental);
        }


        // GET: Rentals/Create
        public async Task<IActionResult> Create()
        {
            //if (id == null || !User.Identity.IsAuthenticated)
            //{
            //    return NotFound();
            //}

            //var board = await _context.Customer.FirstOrDefaultAsync(m => m.UserId == Ident);
            //if (board == null)
            //{
            //    return NotFound();
            //}




            return View();
        }

        // POST: Rentals/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int id, RentalCustomer rentalCustomer)
        {
            var userId = _userManager.GetUserId(User);
            rentalCustomer.Customer.UserId = userId;

            var boardToUpdate = await _context.BoardModel.FirstOrDefaultAsync(m => m.Id == id);

            if (boardToUpdate != null)
            {
                if (boardToUpdate.IsAvailable == 0)
                {
                    ModelState.Remove("Rental.RowVersion");
                    if (ModelState.IsValid)
                    {

                        // Proceed with booking
                        boardToUpdate.IsAvailable = 1;

                        var newCustomer = new Customer
                        {
                            UserId = userId,
                            Name = rentalCustomer.Customer.Name,
                            LastName = rentalCustomer.Customer.LastName,
                            Email = rentalCustomer.Customer.Email,
                            PhoneNumber = rentalCustomer.Customer.PhoneNumber,
                            Rentals = new List<Rental>
                    {
                        new Rental
                        {
                            Start = rentalCustomer.Rental.Start,
                            End = rentalCustomer.Rental.End,
                            Price = rentalCustomer.Rental.Price,
                            // RowVersion will be generated by the database
                            Boards = new List<BoardModel> { boardToUpdate }
                        }
                    }
                        };

                        _context.Customer.Add(newCustomer);

                        // SaveChangesAsync will handle concurrency conflicts
                        await _context.SaveChangesAsync();

                        return RedirectToAction("Book", "BoardModels");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "The chosen board has already been booked by another user, please chose another board");
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Board not found.");
            }

            return View(rentalCustomer);
        }






        // GET: Rentals/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Rental == null)
            {
                return NotFound();
            }

            var rental = await _context.Rental.FindAsync(id);
            if (rental == null)
            {
                return NotFound();
            }
            return View(rental);
        }

        // POST: Rentals/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RentalId,Start,End,Price,TotalPrice")] Rental rental)
        {
            if (id != rental.RentalId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(rental);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RentalExists(rental.RentalId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(rental);
        }

        // GET: Rentals/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Rental == null)
            {
                return NotFound();
            }

            var rental = await _context.Rental
                .FirstOrDefaultAsync(m => m.RentalId == id);
            if (rental == null)
            {
                return NotFound();
            }

            return View(rental);
        }

        // POST: Rentals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Rental == null)
            {
                return Problem("Entity set 'SurfBoardProjectContext.Rental'  is null.");
            }
            var rental = await _context.Rental.FindAsync(id);
            if (rental != null)
            {
                _context.Rental.Remove(rental);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RentalExists(int id)
        {
            return (_context.Rental?.Any(e => e.RentalId == id)).GetValueOrDefault();
        }
    }
}
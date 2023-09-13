using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SurfBoardProject.Data;
using SurfBoardProject.Models;
using SurfBoardProject.Models.Enum;
using SurfBoardProject.Utility;

namespace SurfBoardProject.Controllers
{
    public class BoardModelsController : Controller
    {
        private readonly SurfBoardProjectContext _context;
        private readonly BoardService _boardService;

        public BoardModelsController(SurfBoardProjectContext context, BoardService boardService)
        {
            _context = context;
            _boardService = boardService;
        }

        // GET: BoardModels
        //Responds to a HTTP Get Request
        // This action method handles requests to the Index view with sorting, filtering, and pagination parameters
       // [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            // Setting up sorting parameters for the view
            //ViewData[] is a dictionary
            ViewData["CurrentSort"] = sortOrder;
            ViewData["LengthSortParm"] = String.IsNullOrEmpty(sortOrder) ? "length_desc" : "";
            ViewData["BoardDescriptionSortParm"] = String.IsNullOrEmpty(sortOrder) ? "boardDescription_desc" : "";
            ViewData["ShowBoardsWithEquip"] = String.IsNullOrEmpty(sortOrder) ? "boardsWithEquip" : "";
            ViewData["CurrentFilter"] = searchString;

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            // Querying the database for BoardModels
            var boards = from b in _context.BoardModel select b;

            if (!String.IsNullOrEmpty(searchString))
            {
                boards = boards.Where(b => b.Equipment.Contains(searchString));
            }


            // Sorting the BoardModels based on sortOrder parameter
            switch (sortOrder)
            {
                case "length_desc":
                    boards = boards.OrderByDescending(b => b.Length);
                    break;
                case "boardDescription_desc":
                    boards = boards.OrderByDescending(b => b.BoardDescription);
                    break;
                case "boardsWithEquip":
                    boards = boards.OrderByDescending(b => b.Equipment);
                    break;
                default:
                    boards = boards.OrderBy(b => b.Name);
                    break;
            }

            int pageSize = 5; // Number of items per page

            // Creating a paginated list of BoardModel items based on sorting and pagination parameters
            return View(await PaginatedList<BoardModel>.
                CreateAsync(boards.AsNoTracking(), pageNumber ?? 1, pageSize));
        }
       // [Authorize(Roles = "Customer")]
        //GET: BoardModel/Book
        public async Task<IActionResult> Book(BoardModel board)
        {
          
            //If any of the IsAvailable properties are 1 then disable the board in the list
            IEnumerable<BoardModel> obj = _context.BoardModel.ToList();
                return View(obj);
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateBooking(int id, BoardModel boardModel)
        {
            if (id != boardModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    boardModel.IsAvailable = 1;
                
                    _context.Update(boardModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BoardModelExists(boardModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Create", "Rentals");
            }
            return Redirect("Book");
        }



        // GET: BoardModels/Details/5
        public async Task<IActionResult> ShowBookingDetails(int? id)
        {
            if (id == null || _context.BoardModel == null)
            {
                return NotFound();
            }

            var boardModel = await _context.BoardModel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (boardModel == null)
            {
                return NotFound();
            }

            return View(boardModel);
        }


        // GET: BoardModels/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: BoardModels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Length,Width,Volume,BoardType,Price,Equipment")] BoardModel boardModel)
        {

            if (ModelState.IsValid)
            {
                _boardService.ImageAndBoardSelector(boardModel);
                _context.Add(boardModel);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Surfboard created successfully";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["Error"] = "An Error has occured";

            }

            return View(boardModel);
        }

        // GET: BoardModels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.BoardModel == null)
            {
                return NotFound();
            }

            var boardModel = await _context.BoardModel.FindAsync(id);
            if (boardModel == null)
            {
                return NotFound();
            }
            return View(boardModel);
        }

        // POST: BoardModels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Length,Width,Volume,BoardDescription,Price,Equipment")] BoardModel boardModel)
        {
            if (id != boardModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(boardModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BoardModelExists(boardModel.Id))
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
            return View(boardModel);
        }

        // GET: BoardModels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.BoardModel == null)
            {
                return NotFound();
            }

            var boardModel = await _context.BoardModel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (boardModel == null)
            {
                return NotFound();
            }

            return View(boardModel);
        }

        // POST: BoardModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.BoardModel == null)
            {
                return Problem("Entity set 'SurfBoardProjectContext.BoardModel'  is null.");
            }
            var boardModel = await _context.BoardModel.FindAsync(id);
            if (boardModel != null)
            {
                _context.BoardModel.Remove(boardModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BoardModelExists(int id)
        {
            return (_context.BoardModel?.Any(e => e.Id == id)).GetValueOrDefault();
        }


    }



}

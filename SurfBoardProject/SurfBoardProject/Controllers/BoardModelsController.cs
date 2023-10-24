using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using Newtonsoft.Json;
using SurfBoardProject.Data;
using SurfBoardProject.Models;
using SurfBoardProject.Models.Enum;
using SurfBoardProject.Utility;

namespace SurfBoardProject.Controllers
{
    public class BoardModelsController : Controller
    {
        private readonly SurfBoardProjectContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly BoardService _boardService;
        private readonly IHttpClientFactory _httpClientFactory;

        public BoardModelsController(SurfBoardProjectContext context, BoardService boardService, IHttpClientFactory httpClientFactory, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _boardService = boardService;
            _httpClientFactory = httpClientFactory;
            _userManager = userManager;
        }

        // GET: BoardModels
        //Responds to a HTTP Get Request
        // This action method handles requests to the Index view with sorting, filtering, and pagination parameters

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


        // GET: Customers

        public async Task<IActionResult> ShowRentedBoard()
        {
            // Retrieve rentals for the user and include related BoardModel and Customer
            // Retrieve rentals for the user where the associated board is available (IsAvailable == 1)
            // Declare a variable to store the result of the query, which will be a list of rentals with available boards.
            var boardRented = _context.Rental
                // Filter the Rental entities.
                .Where(r => r.Boards.Any(board => board.IsAvailable == 1))
                // Eagerly load the Boards navigation property of Rental entities.
                .Include(r => r.Boards)
                // Eagerly load the Customers navigation property of Rental entities.
                .Include(r => r.Customers)
                // Execute the query and materialize the result into a list.
                .ToList();


            return View(boardRented);
        }



        public IActionResult ToggleAvailability(int itemId, int IsAvailable)
        {
            var item = _context.BoardModel.Find(itemId);

            if (item != null)
            {
                item.IsAvailable = IsAvailable == 1 ? 0 : 1;

                _context.SaveChanges();

                // Delete bookings if board's IsAvailable is set to 0
                if (item.IsAvailable == 0)
                {
                    var bookingsToDelete = _context.Rental
                        .Where(rental => rental.Boards.Any(board => board.IsAvailable == 0))
                        .ToList();

                    foreach (var booking in bookingsToDelete)
                    {
                        _context.Remove(booking);
                    }

                    _context.SaveChanges();
                }

                return RedirectToAction("Index");
            }

            return NotFound();
        }

        // [Authorize(Roles = "Customer")]
        //GET: BoardModel/Book
        public async Task<IActionResult> Book()
        {
            var userId = _userManager.GetUserId(User);
            string userAccessPackage = "1.0";

            if (!string.IsNullOrEmpty(userId))
            {
                userAccessPackage = "2.0";
            }

            using (var client = new HttpClient())
            {
                string baseUrl = $"https://localhost:7161/api/rent?api-version={userAccessPackage}";

                HttpResponseMessage response = await client.GetAsync(baseUrl);

                if (response.IsSuccessStatusCode)
                {
                    var jsonContent = await response.Content.ReadAsStringAsync();
                    var boards = JsonConvert.DeserializeObject<List<BoardModel>>(jsonContent);

                    return View(boards);
                }
                else
                {
                    return View("Error");
                }
            }
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
            ModelState.Remove("RowVersion");

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
                TempData["Error"] = "An Error has occured. Try again later";

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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, byte[] rowVersion)
        {
            if (id == null)
            {
                return NotFound();
            }

            var boardToUpdate = await _context.BoardModel.FirstOrDefaultAsync(m => m.Id == id);

            if (boardToUpdate == null)
            {
                BoardModel deletedBoard = new BoardModel();
                await TryUpdateModelAsync(deletedBoard);
                ModelState.AddModelError(string.Empty, "Unable to save changes. The record was deleted by another user.");
                return RedirectToAction(nameof(Index));
            }
            if (await TryUpdateModelAsync<BoardModel>(boardToUpdate, "",
                s => s.Name,
                s => s.Length,
                s => s.Width,
                s => s.Volume,
                s => s.BoardType,
                s => s.Price,
                s => s.Equipment,
                s => s.RowVersion))
            {
                try
                {
                    _context.Entry(boardToUpdate).OriginalValues["RowVersion"] = rowVersion;
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));

                }
                catch (DbUpdateConcurrencyException ex)
                {
                    var exceptionEntry = ex.Entries.Single();
                    var clientValues = (BoardModel)exceptionEntry.Entity;
                    var databaseEntry = exceptionEntry.GetDatabaseValues();
                    if (databaseEntry == null)
                    {
                        ModelState.AddModelError(string.Empty,
                            "Unable to save changes. The department was deleted by another user.");
                    }
                    else
                    {
                        var databaseValues = (BoardModel)databaseEntry.ToObject();

                        if (databaseValues.Name != clientValues.Name)
                        {
                            ModelState.AddModelError("Name", $"Current value: {databaseValues.Name}");
                        }
                        if (databaseValues.Length != clientValues.Length)
                        {
                            ModelState.AddModelError("Lenght", $"Current value: {databaseValues.Length}");
                        }
                        if (databaseValues.Width != clientValues.Width)
                        {
                            ModelState.AddModelError("Width", $"Current value: {databaseValues.Width}");
                        }
                        if (databaseValues.Volume != clientValues.Volume)
                        {
                            ModelState.AddModelError("Volume", $"Current value: {databaseValues.Volume}");
                        }
                        if (databaseValues.BoardType != clientValues.BoardType)
                        {
                            ModelState.AddModelError("BoardType", $"Current value: {databaseValues.BoardType}");
                        }
                        if (databaseValues.Price != clientValues.Price)
                        {
                            ModelState.AddModelError("Price", $"Current value: {databaseValues.Price}");
                        }
                        if (databaseValues.Equipment != clientValues.Equipment)
                        {
                            ModelState.AddModelError("Equipment", $"Current value: {databaseValues.Equipment}");
                        }
                        if (databaseValues.RowVersion != clientValues.RowVersion)
                        {
                            ModelState.AddModelError("RowVersion", $"Current value: {databaseValues.RowVersion}");
                        }

                        ModelState.AddModelError(string.Empty, "The record you attempted to edit "
                              + "was modified by another user after you got the original value. The "
                              + "edit operation was canceled and the current values in the database "
                              + "have been displayed. If you still want to edit this record, click "
                              + "the Save button again. Otherwise click the Back to List hyperlink.");
                        boardToUpdate.RowVersion = (byte[])databaseValues.RowVersion;
                        ModelState.Remove("RowVersion");

                    }
                }
            }
            return View(boardToUpdate);
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

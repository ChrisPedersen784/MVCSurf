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
using Newtonsoft.Json;
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
        private readonly IHttpClientFactory _httpClientFactory;
        private string _unAuthorizedUser = "44516742-ebe2-4454-9c14-b80d325c961e";
        //private string _unAuthorizedUser = "44516742-ebe2-4454-9c14-b80d325c961";

        public RentalsController(SurfBoardProjectContext context, UserManager<IdentityUser> userManager, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _userManager = userManager;
            _httpClientFactory = httpClientFactory;
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

            string userAccessPackage = "1.0";

            if (!string.IsNullOrEmpty(userId))
            {
                userAccessPackage = "2.0";
            }
            else
            {
                userId = _unAuthorizedUser;
            }

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7161/");

                string baseUrl = $"https://localhost:7161/api/RentAPI/GetRental?userId={userId}";
                // Append the userId as a query parameter
                //HttpResponseMessage response = await client.GetAsync($"api/RentAPI/GetRental?userId={userId}");
                HttpResponseMessage response = await client.GetAsync(baseUrl);

                if (response.IsSuccessStatusCode)
                {
                    var jsonContent = await response.Content.ReadAsStringAsync();
                    var rentals = JsonConvert.DeserializeObject<List<Rental>>(jsonContent);

                    return View(rentals);
                }
                else
                {
                    // Handle the error appropriately
                    return View("Error");
                }
            }
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

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int id, RentalCustomer rentalCustomer)
        {
            var userId = _userManager.GetUserId(User);

            string userAccessPackage = "1.0";

            if (!string.IsNullOrEmpty(userId))
            {
                userAccessPackage = "2.0";
            }
            else
            {
                userId = _unAuthorizedUser;
                rentalCustomer.Customer.UserId = userId;
            }

            using (var client = new HttpClient())
            {
                string baseUrl = $"https://localhost:7161/api/rent/{id}/rent?userId={userId}&api-version={userAccessPackage}";

                // Serialize the rentalCustomer object to JSON
                var jsonContent = JsonConvert.SerializeObject(rentalCustomer);

                // Create a StringContent with JSON data and set the Content-Type header
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                // Send the POST request with the content
                var response = await client.PostAsync(baseUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    // Log or debug the response status code and content
                    var statusCode = response.StatusCode;
                    TempData["Success"] = "Surfboard successfully booked";
                    // Redirect to the "Book" action
                    return RedirectToAction("Book", "BoardModels");
                }
                else
                {
                    // Handle the error response
                    var errorContent = await response.Content.ReadAsStringAsync();


                    // Parse the errorContent as JSON if it's in JSON format
                   
                    ModelState.AddModelError(string.Empty, errorContent);
                    // Handle the error appropriately
                    TempData["Error"] = errorContent.ToString();
                    return RedirectToAction("Book", "BoardModels");
                }
            }

        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create(int id, RentalCustomer rentalCustomer)
        //{
        //    var userId = _userManager.GetUserId(User);

        //    using (var client = new HttpClient())
        //    {
        //        string baseUrl = $"https://localhost:7161/api/RentAPI/{id}/rent?userId={userId}";

        //        // Serialize the rentalCustomer object to JSON
        //        var jsonContent = JsonConvert.SerializeObject(rentalCustomer);

        //        // Create a StringContent with JSON data and set the Content-Type header
        //        var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        //        // Send the POST request with the content
        //        var response = await client.PostAsync(baseUrl, content);

        //        /*var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");:
        //         * jsonContent is the JSON data that you want to send to the server. It represents the data you want to include in the request body. In your case, it's the serialized RentalCustomer object.
        //         * Encoding.UTF8 specifies the character encoding to use when converting the JSON data to bytes. UTF-8 is a common encoding for JSON data.
        //         * "application/json" is the media type (or content type) of the request body. It tells the server that the request body contains JSON data. This is important because the server needs to know how to interpret the data you're sending.
        //         * So, you're creating a StringContent object with your JSON data and specifying that it's in JSON format.
        //         * var response = await client.PostAsync(baseUrl, content);:
        //         * client.PostAsync(baseUrl, content) sends an HTTP POST request to the specified baseUrl with the JSON data included in the content variable as the request body.*/

        //        if (response.IsSuccessStatusCode)
        //        {
        //            // Log or debug the response status code and content
        //            var statusCode = response.StatusCode;
        //            // Redirect to the "Book" action
        //            return RedirectToAction("Book", "BoardModels");
        //        }

        //        else
        //        {
        //            // Handle the error appropriately
        //            return View("Error");
        //        }
        //    }
        //}


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


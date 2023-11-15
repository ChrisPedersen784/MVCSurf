using Lib.Product.Models;
using Lib.Storage;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Lib.Product
{
    public class ProductService : IProductService
    {
        private readonly IStorageService _storageService;
        private readonly HttpClient _httpClient;

        public ProductService(IStorageService storageService, HttpClient httpClient)
        {
            _storageService = storageService;
            _httpClient = httpClient;
        }

        public Surfboard? Get(int id)
        {
            return _storageService.Surfboards.FirstOrDefault(p => p.Id == id);
        }

        public Surfboard? GetById(int id)
        {
            return _storageService.Surfboards.FirstOrDefault(p => p.Id == id);
        }

        // Updated GetAll() method to fetch surfboards from the API
        public async Task<IList<Surfboard>> GetAllAsync()
        {
            List<Surfboard> surfboards = new List<Surfboard>();


            string baseUrl = "https://localhost:7161/api/shop";


            HttpResponseMessage response = await _httpClient.GetAsync(baseUrl);

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                surfboards = JsonConvert.DeserializeObject<List<Surfboard>>(jsonResponse);

                return surfboards;  // Return the surfboards obtained from the API
            }
            else
            {
                Console.WriteLine($"Status Code: {response.StatusCode}");
                Console.WriteLine($"Content: {await response.Content.ReadAsStringAsync()}");
            }
            return surfboards;
        }

        public IList<Surfboard> GetAll()
        {
            return _storageService.Surfboards.ToList();
        }
    }
}


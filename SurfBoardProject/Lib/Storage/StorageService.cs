using Lib.Product.Models;
using Lib.ShoppingCart.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Lib.Storage
{
    public class StorageService : IStorageService
    {
        public IList<Surfboard> Surfboards { get; private set; }
        public ShoppingCartModel ShoppingCart { get; private set; }

        public StorageService()
        {
            Surfboards = new List<Surfboard>();
            ShoppingCart = new ShoppingCartModel();

            // InitializeAsync method will be called during construction
            InitializeAsync();
        }

        public async Task InitializeAsync()
        {
            await GetProductsAsync();
        }

        private async Task GetProductsAsync()
        {
            using (var client = new HttpClient())
            {
                string baseUrl = "https://localhost:7161/api/shop";

                try
                {
                    HttpResponseMessage response = await client.GetAsync(baseUrl);
                    if (response.IsSuccessStatusCode)
                    {
                        var jsonContent = await response.Content.ReadAsStringAsync();
                        var surfboards = JsonConvert.DeserializeObject<List<Surfboard>>(jsonContent);
                        AddProducts(surfboards);
                    }
                }
                catch (Exception ex)
                {
                    // Log or inspect the exception
                    Console.WriteLine($"Exception: {ex.Message}");
                }

            }
        }

        private void AddProducts(List<Surfboard> surfboards)
        {
            foreach (var surfboard in surfboards)
            {
                if (!Surfboards.Any(p => p.Sku == surfboard.Sku))
                {
                    Surfboards.Add(surfboard);
                }
            }
        }
    }
}

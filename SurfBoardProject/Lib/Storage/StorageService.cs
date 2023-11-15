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

            string hexValue = "20";
            int decimalValue = Convert.ToInt32(hexValue, 16);

            byte myByte = 10;
            byte[] byteArray = new byte[] { myByte };
            AddProducts(new Surfboard(2, "Name", 2.0, 4.0, 1.0, 2.5, "Longboard", "g", 2.0 , "img.jpg", 44, byteArray));
        

    }

      

        private void AddProducts(Surfboard surfboards)
        {
           
                if (!Surfboards.Any(p => p.Id == surfboards.Id))
                {
                    Surfboards.Add(surfboards);
                }
            
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Product.Models
{
    public interface IProductAddToCart
    {
        // An instance of the product
        Surfboard? Product { get; set; }

        // The quantity wishing to be added to the cart
        int? Quantity { get; set; }

        /// <summary>
        /// The method to add a product to cart
        /// </summary>
        void AddToCart();
    }
}

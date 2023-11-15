using Lib.Product.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.ShoppingCart.Models
{
    public class ShoppingCartItemModel
    {
        /// <summary>
        /// Product type.
        /// </summary>
        public Surfboard Product { get; }

        /// <summary>
        /// Price of the product.
        /// </summary>
        public decimal Price { get; protected set; }

        /// <summary>
        /// Quantity of the product.
        /// </summary>
        public int Quantity { get; protected set; }

        /// <summary>
        /// Get the total price of the product.
        /// </summary>
        public decimal TotalPrice
        {
            get
            {
                return Price * Quantity;
            }
        }

        /// <summary>
        /// Constructs a new shopping cart item.
        /// </summary>
        /// <param name="product">Product type.</param>
        /// <param name="quantity">Quantity of the product.</param>
        public ShoppingCartItemModel(Surfboard product, int quantity)
        {
            Product = product;
            Price = Convert.ToDecimal(product.Price);
            Quantity = quantity;
        }

        /// <summary>
        /// Updates the quantity and the price.
        /// </summary>
        /// <param name="product">Product type.</param>
        /// <param name="quantity">Quantity of the product.</param>
        public void UpdateQuantity(Surfboard product, int quantity)
        {
            Price = Convert.ToDecimal(product.Price);
            Quantity += quantity;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.ShoppingCart.Models
{
    /// </summary>
    public class ShoppingCartModel
    {
        /// <summary>
        /// A list of all the items stored in the shopping cart.
        /// </summary>
        public IList<ShoppingCartItemModel> Items { get; }

        /// <summary>
        /// Constructs a new shopping cart.
        /// </summary>
        public ShoppingCartModel()
        {
            Items = new List<ShoppingCartItemModel>();
        }
    }
}

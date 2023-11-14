using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Product.Models
{
    public class Surfboard
    {

        public string Sku { get; }

        /// <summary>
        /// Name of the product.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Price of the product.
        /// </summary>
        public decimal Price { get; }

        /// <summary>
        /// The image path of the product.
        /// </summary>
        public string Image { get; }

        /// <summary>
        /// The route slug of the product.
        /// </summary>
        public string Slug
        {
            get
            {
                return Sku.ToLower();
            }
        }

        /// <summary>
        /// The full URL of the product
        /// </summary>
        public string FullUrl
        {
            get
            {
                return string.Format("/product/{0}", Slug);
            }
        }

        /// <summary>
        /// Constructs a new product.
        /// </summary>
        /// <param name="sku">Unique identifier of the product.</param>
        /// <param name="name">Name of the product.</param>
        /// <param name="price">Price of the product.</param>
        /// <param name="image">Image path of the product.</param>
        public Surfboard(string sku, string name, decimal price, string image)
        {
            Sku = sku;
            Name = name;
            Price = price;
            Image = image;
        }
    }
}


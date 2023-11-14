using Lib.Product.Models;
using Lib.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Product
{
    /// Used for product methods.
    /// </summary>
    public class ProductService : IProductService
    {
        /// <summary>
        /// A private reference to the storage service from the DI container.
        /// </summary>
        private readonly IStorageService _storageService;

        /// <summary>
        /// Constructs a product service.
        /// </summary>
        /// <param name="storageService">A reference to the storage service from the DI container.</param>
        public ProductService(IStorageService storageService)
        {
            _storageService = storageService;
        }

        /// <summary>
        /// Gets a product.
        /// </summary>
        /// <param name="sku">The unique sku reference.</param>
        /// <returns>A <see cref="ProductModel"/> type.</returns>
        public Surfboard? Get(string sku)
        {
            return _storageService.Surfboards.FirstOrDefault(p => p.Sku == sku);
        }

        /// <summary>
        /// Get a product by slug.
        /// </summary>
        /// <param name="slug">The slug of the product</param>
        /// <returns></returns>
        public Surfboard? GetBySlug(string slug)
        {
            return _storageService.Surfboards.FirstOrDefault(p => p.Slug == slug);
        }

        /// <summary>
        /// Gets all products
        /// </summary>
        /// <returns>A <see cref="IList<ProductModel>"/> type.</returns>
        public IList<Surfboard> GetAll()
        {
            return _storageService.Surfboards.ToList();
        }
    }

}

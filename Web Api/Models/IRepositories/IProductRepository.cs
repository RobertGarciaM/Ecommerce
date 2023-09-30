using Models.Products;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Models.IRepositories
{
    public interface IProductRepository
    {
        Task<Product> GetProductAsync(Guid id);
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task AddProductAsync(Product product);
        Task UpdateProductAsync(Product product);
        Task DeleteProductAsync(Guid id);
    }

}

using Model.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<ProductDto>> GetAllProductsAsync();
        Task<ProductDto> AddProductAsync(ProductDto product);
        Task<bool> UpdateProductAsync(Guid id, ProductDto product);
        Task<bool> DeleteProductAsync(Guid id);
    }

}

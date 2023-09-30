using Models.Dto;
using Models.IRepositories;
using Models.Sales;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class SaleRepository : ISaleRepository
    {
        private ApplicationDbContext dbContext;

        public SaleRepository(ApplicationDbContext context)
        {
            dbContext = context;
        }

        public async Task<Sale> GetSaleAsync(Guid id)
        {
            return await dbContext.Sales.FindAsync(id);
        }

        public async Task<IEnumerable<SaleDto>> GetSalesByDateAsync(DateTime startDate, DateTime endDate)
        {
            return await dbContext.Sales
              .Where(s => s.Date >= startDate && s.Date <= endDate)
              .Join(dbContext.Products,
                  sale => sale.IdProduct,
                  product => product.IdProduct,
                  (sale, product) => new { Sale = sale, Product = product })
              .Join(dbContext.Customers,
                  saleProduct => saleProduct.Sale.IdCustomer,
                  customer => customer.IdCustomer,
                  (saleProduct, customer) => new SaleDto
                  {
                      Customer = customer.Name,
                      Product = saleProduct.Product.Description,
                      Quantity = saleProduct.Sale.Quantity,
                      Date = saleProduct.Sale.Date
                  })
              .ToListAsync();
        }

        public async Task AddSaleAsync(Sale sale)
        {
            dbContext.Sales.Add(sale);
            await dbContext.SaveChangesAsync();
        }
    }
}

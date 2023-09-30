using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Models.Dto;
using Models.Sales;


namespace Models.IRepositories
{
    public interface ISaleRepository
    {
        Task<Sale> GetSaleAsync(Guid id);
        Task<IEnumerable<SaleDto>> GetSalesByDateAsync(DateTime startDate, DateTime endDate);
        Task AddSaleAsync(Sale sale);
    }
}

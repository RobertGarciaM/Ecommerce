using Models.Customer;
using Models.IRepositories;
using Models.Sales;
using System;
using System.Net.Http;
using System.Net;
using System.Web.Http;
using System.Threading.Tasks;

namespace ECOMMERCE.Controllers
{
    [Authorize]
    [RoutePrefix("api/Sale")]
    public class SaleController : ApiController
    {
        private ISaleRepository saleRepository;

        public SaleController(ISaleRepository repository)
        {
            saleRepository = repository;
        }

        /// <summary>
        /// Retrieves a specific sale by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the sale to retrieve.</param>
        /// <returns>The sale if found, or NotFound if not found.</returns>
        [HttpGet]
        public async Task<HttpResponseMessage> GetSale(Guid id)
        {
            var sale = await saleRepository.GetSaleAsync(id);
            if (sale == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
            return Request.CreateResponse(HttpStatusCode.OK, sale);
        }

        /// <summary>
        /// Retrieves sales that match the specified date.
        /// </summary>
        /// <param name="startDate">The start date to filter sales by.</param>
        /// /// <param name="endDate">The end date to filter sales by.</param>
        /// <returns>A list of sales matching the provided date.</returns>
        [HttpGet]
        [Route("GetSalesByDate")]
        public async Task<HttpResponseMessage> GetSalesByDate(DateTime startDate, DateTime endDate)
        {
            var sales = await saleRepository.GetSalesByDateAsync(startDate, endDate);
            if (sales == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
            return Request.CreateResponse(HttpStatusCode.OK, sales);
        }

        /// <summary>
        /// Adds a new sale to the system.
        /// </summary>
        /// <param name="sale">The sale information to add.</param>
        /// <returns>The created sale if successful, or BadRequest if the model state is invalid.</returns>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("Create")]
        public async Task<HttpResponseMessage> Create(Sale sale)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            try
            {
                await saleRepository.AddSaleAsync(sale);
                return Request.CreateResponse(HttpStatusCode.Created, sale);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, $"Error: {ex.Message}");
            }
        }
    }
}
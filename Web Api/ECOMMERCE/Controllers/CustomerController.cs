using ECOMMERCE.Interfaces;
using Models.Customer;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace ECOMMERCE.Controllers
{
    [Authorize]
    [RoutePrefix("api/Customer")]
    public class CustomerController : ApiController
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerController()
        {
        }

        public CustomerController(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }


        /// <summary>
        /// Get a list of all customers.
        /// </summary>
        /// <remarks>
        /// Use this endpoint to retrieve a list of all customers.
        /// </remarks>
        /// <returns>Returns an HTTP response containing a list of customers.</returns>
        [HttpGet]
        [Route("All")]
        public async Task<HttpResponseMessage> GetAll()
        {
            try
            {
                var customers = await _customerRepository.GetAllAsync();
                return Request.CreateResponse(HttpStatusCode.OK, customers);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, $"Error: {ex.Message}");
            }
        }

        /// <summary>
        /// Get a customer by ID.
        /// </summary>
        /// <remarks>
        /// Use this endpoint to retrieve a customer by their unique ID.
        /// </remarks>
        /// <param name="id">The ID of the customer to retrieve.</param>
        /// <returns>Returns an HTTP response containing the customer details.</returns>
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<HttpResponseMessage> GetById(Guid id)
        {
            var customer = await _customerRepository.GetByIdAsync(id);
            if (customer == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
            return Request.CreateResponse(HttpStatusCode.OK, customer);
        }

        /// <summary>
        /// Create a new customer.
        /// </summary>
        /// <remarks>
        /// Use this endpoint to create a new customer.
        /// </remarks>
        /// <param name="customer">The customer object to create.</param>
        /// <returns>Returns an HTTP response indicating the creation status.</returns>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("Create")]
        public async Task<HttpResponseMessage> Create(Customer customer)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            try
            {
                await _customerRepository.AddAsync(customer);
                return Request.CreateResponse(HttpStatusCode.Created, customer);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, $"Error: {ex.Message}");
            }
        }

        /// <summary>
        /// Update a customer.
        /// </summary>
        /// <remarks>
        /// Use this endpoint to update an existing customer.
        /// </remarks>
        /// <param name="id">The ID of the customer to update.</param>
        /// <param name="customer">The updated customer object.</param>
        /// <returns>Returns an HTTP response indicating the update status.</returns>
        [HttpPut]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<HttpResponseMessage> Update(Guid id, Customer customer)
        {
            if (customer.IdCustomer != id)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Customer ID does not match customer object");
            }

            try
            {
                await _customerRepository.UpdateAsync(customer);
                return Request.CreateResponse(HttpStatusCode.NoContent);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, $"Error: {ex.Message}");
            }
        }

        /// <summary>
        /// Delete a customer.
        /// </summary>
        /// <remarks>
        /// Use this endpoint to delete an existing customer.
        /// </remarks>
        /// <param name="id">The ID of the customer to delete.</param>
        /// <returns>Returns an HTTP response indicating the delete status.</returns>
        [HttpDelete]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<HttpResponseMessage> Delete(Guid id)
        {
            var existingCustomer = await _customerRepository.GetByIdAsync(id);
            if (existingCustomer == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            try
            {
                await _customerRepository.DeleteAsync(id);
                return Request.CreateResponse(HttpStatusCode.NoContent);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, $"Error: {ex.Message}");
            }
        }
    }

}
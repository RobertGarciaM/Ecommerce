using Models.Customer;
using Models.IRepositories;
using Models.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace ECOMMERCE.Controllers
{
    [Authorize]
    [RoutePrefix("api/Product")]
    public class ProductController : ApiController
    {
        private IProductRepository productRepository;

        public ProductController(IProductRepository repository)
        {
            productRepository = repository;
        }

        /// <summary>
        /// Retrieve a product by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the product.</param>
        /// <returns>Returns an HTTP response with the product details if found; otherwise, returns Not Found.</returns>
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<HttpResponseMessage> GetProduct(Guid id)
        {
            var product = await productRepository.GetProductAsync(id);
            if (product == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
            return Request.CreateResponse(HttpStatusCode.OK, product);
        }


        /// <summary>
        /// Retrieve a list of all products.
        /// </summary>
        /// <returns>Returns an HTTP response with a list of all products.</returns>
        [HttpGet]
        [Route("All")]
        public async Task<HttpResponseMessage> GetAllProducts()
        {
            try
            {
                var products = await productRepository.GetAllProductsAsync();
                return Request.CreateResponse(HttpStatusCode.Created, products);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, $"Error: {ex.Message}");
            }
        }

        /// <summary>
        /// Add a new product.
        /// </summary>
        /// <param name="product">The product to add.</param>
        /// <returns>Returns an HTTP response with the added product details and a Location header.</returns>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("Create")]
        public async Task<HttpResponseMessage> AddProduct(Product product)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            try
            {
                await productRepository.AddProductAsync(product);
                return Request.CreateResponse(HttpStatusCode.Created, product);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, $"Error: {ex.Message}");
            }
        }

        /// <summary>
        /// Update an existing product.
        /// </summary>
        /// <param name="id">The unique identifier of the product to update.</param>
        /// <param name="product">The updated product details.</param>
        /// <returns>Returns an HTTP response with No Content if the update is successful; otherwise, returns Bad Request.</returns>
        [HttpPut]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<HttpResponseMessage> UpdateProduct(Guid id, Product product)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            if (id != product.IdProduct)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Product ID does not match product object");
            }

            try
            {
                await productRepository.UpdateProductAsync(product);
                return Request.CreateResponse(HttpStatusCode.NoContent);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, $"Error: {ex.Message}");
            }
        }

        /// <summary>
        /// Delete a product by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the product to delete.</param>
        /// <returns>Returns an HTTP response with the deleted product details if found; otherwise, returns Not Found.</returns>
        [HttpDelete]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<HttpResponseMessage> DeleteProduct(Guid id)
        {
            var product = await productRepository.GetProductAsync(id);
            if (product == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            try
            {
                await productRepository.DeleteProductAsync(id);
                return Request.CreateResponse(HttpStatusCode.NoContent);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, $"Error: {ex.Message}");
            }
        }
    }
}
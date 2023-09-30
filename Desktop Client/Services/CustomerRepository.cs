using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Model.Interfaces;
using Model.Dto;
using Newtonsoft.Json;

namespace Services.CustomerService
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly HttpClient _httpClient;

        public CustomerRepository(string apiBaseUrl, string jwtToken)
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(apiBaseUrl);
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
        }

        public async Task<IEnumerable<CustomerDto>> GetAllCustomersAsync()
        {
            HttpResponseMessage response = await _httpClient.GetAsync("api/Customer/All");

            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                var customers = JsonConvert.DeserializeObject<IEnumerable<CustomerDto>>(responseBody);
                return customers;
            }
            else
            {
                throw new Exception($"Error: {response.StatusCode}");
            }
        }

        public async Task<CustomerDto> GetCustomerByIdAsync(int id)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"api/Customer/GetById/{id}");

            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                var customer = JsonConvert.DeserializeObject<CustomerDto>(responseBody);
                return customer;
            }
            else
            {
                throw new Exception($"Error: {response.StatusCode}");
            }
        }

        public async Task<CustomerDto> CreateCustomerAsync(CustomerDto customer)
        {
            var json = JsonConvert.SerializeObject(customer);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync("api/Customer/Create", content);

            if (response.IsSuccessStatusCode)
            {
                return customer;
            }
            else
            {
                throw new Exception($"Error: {response.StatusCode}");
            }
        }

        public async Task<bool> UpdateCustomerAsync(Guid id, CustomerDto customer)
        {
            var json = JsonConvert.SerializeObject(customer);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PutAsync($"api/Customer/{id}", content);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                throw new Exception($"Error: {response.StatusCode}");
            }
        }

        public async Task<bool> DeleteCustomerAsync(Guid id)
        {
            HttpResponseMessage response = await _httpClient.DeleteAsync($"api/Customer/{id}");

            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                throw new Exception($"Error: {response.StatusCode}");
            }
        }
    }
}

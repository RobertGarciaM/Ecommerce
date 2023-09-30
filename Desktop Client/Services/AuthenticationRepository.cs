using Model.Dto;
using Model.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Services.CustomerService
{
    public class AuthenticationRepository : IAuthenticationRepository
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiBaseUrl;

        public AuthenticationRepository(string apiBaseUrl)
        {
            _httpClient = new HttpClient();
            _apiBaseUrl = apiBaseUrl;
        }

        public async Task<string> AuthenticateAsync(LoginModelDto loginModel)
        {
            try
            {
                var json = JsonConvert.SerializeObject(loginModel);

                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync($"{_apiBaseUrl}/api/Account/Login", content);

                if (response.IsSuccessStatusCode)
                {
                    var tokenString = await response.Content.ReadAsStringAsync();
                    var tokenObject = JObject.Parse(tokenString);
                    var token = (string)tokenObject["Token"];

                    return token;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al realizar la operación: {ex.Message}", ex);
            }
        }
    }
}

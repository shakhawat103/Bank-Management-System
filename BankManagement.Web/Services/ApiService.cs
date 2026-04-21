// 🎯 Implementation using HttpClient to call Web API
// Dependencies: BankManagement.Application (DTOs), Microsoft.Extensions.Configuration
using BankManagement.Application.DTOs;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json; // For easier JSON handling
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BankManagement.Web.Services
{
    public class ApiService : IApiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;

        // 🔗 HttpClient + Configuration injected via DI
        public ApiService(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _baseUrl = config["ApiSettings:BaseUrl"] ?? "https://localhost:7001/api";
        }

        public async Task<IEnumerable<AccountDto>> GetAllAccountsAsync()
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/accounts");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<IEnumerable<AccountDto>>()
                   ?? new List<AccountDto>();
        }

        public async Task<AccountDto?> GetAccountByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/accounts/{id}");
            if (!response.IsSuccessStatusCode) return null;
            return await response.Content.ReadFromJsonAsync<AccountDto>();
        }

        public async Task<AccountDto> CreateAccountAsync(CreateAccountDto createDto)
        {
            var json = JsonConvert.SerializeObject(createDto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{_baseUrl}/accounts", content);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<AccountDto>()
                   ?? throw new Exception("Failed to create account");
        }

        public async Task<bool> UpdateAccountAsync(int id, UpdateAccountDto updateDto)
        {
            var json = JsonConvert.SerializeObject(updateDto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"{_baseUrl}/accounts/{id}", content);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteAccountAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{_baseUrl}/accounts/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}
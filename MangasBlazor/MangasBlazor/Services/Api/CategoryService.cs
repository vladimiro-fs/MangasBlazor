namespace MangasBlazor.Services.Api
{
    using System.Net.Http.Json;
    using MangasBlazor.Models.DTOs;

    public class CategoryService : ICategoryService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public ILogger<CategoryService> _logger;

        private const string apiEndpoint = "/api/categories";
        private CategoryDTO? category;

        public CategoryService(IHttpClientFactory httpClientFactory, 
            ILogger<CategoryService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public async Task<List<CategoryDTO>> GetCategories()
        {
            try
            {
                var httpClient = _httpClientFactory.CreateClient("MangasAPI");
                var result = await httpClient.GetFromJsonAsync<List<CategoryDTO>>(apiEndpoint);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error accessing categories: {apiEndpoint}" + ex.Message);
                throw new UnauthorizedAccessException();
            }
        }

        public async Task<CategoryDTO> GetCategory(int id)
        {
            try
            {
                var httpClient = _httpClientFactory.CreateClient("MangasAPI");
                var response = await httpClient.GetAsync(apiEndpoint + id);

                if (response.IsSuccessStatusCode)
                {
                    category = await response.Content.ReadFromJsonAsync<CategoryDTO>();
                    return category;
                }
                else
                {
                    var message = await response.Content.ReadAsStringAsync();
                    _logger.LogError($"Error getting category id= {id} - {message}");
                    throw new Exception($"Status code: {response.StatusCode} - {message}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting category id= {id} \n\n {ex.Message}");
                throw new UnauthorizedAccessException();
            }
        }
    }
}

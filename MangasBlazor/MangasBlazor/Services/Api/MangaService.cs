namespace MangasBlazor.Services.Api
{
    using System.Net;
    using System.Net.Http.Json;
    using System.Text;
    using System.Text.Json;
    using MangasBlazor.Models.DTOs;

    public class MangaService : IMangaService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public ILogger _logger;
        private const string apiEndpoint = "/api/mangas";
        private readonly JsonSerializerOptions _options;

        private MangaDTO? manga;
        private List<MangaDTO>? mangas;

        public MangaService(IHttpClientFactory httpClientFactory, ILogger logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
            _options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }

        public async Task<MangaDTO> CreateMangas(MangaDTO mangaDto)
        {
            var httpClient = _httpClientFactory.CreateClient("MangasAPI");

            StringContent content = new StringContent(JsonSerializer.Serialize(mangaDto), Encoding.UTF8, "application/json");

            using (var response = await httpClient.PostAsync(apiEndpoint, content))
            {
                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadAsStreamAsync();

                    manga = await JsonSerializer.DeserializeAsync<MangaDTO>(apiResponse, _options);
                }
                else if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    throw new UnauthorizedAccessException();
                }
                else
                {
                    return null;
                }
            }

            return manga;
        }

        public Task<bool> DeleteManga(int id)
        {
            throw new NotImplementedException();
        }

        public Task<MangaDTO> GetManga(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<MangaDTO>> GetMangaByCategory(int categoryId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<MangaDTO>> GetMangas()
        {
            try
            {
                var httpClient = _httpClientFactory.CreateClient("MangasAPI");
                var result = await httpClient.GetFromJsonAsync<List<MangaDTO>>(apiEndpoint);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error accessing mangas: {apiEndpoint}" + ex.Message);
                throw new UnauthorizedAccessException();
            }
        }

        public Task<MangaDTO> UpdateManga(int id, MangaDTO mangaDto)
        {
            throw new NotImplementedException();
        }
    }
}

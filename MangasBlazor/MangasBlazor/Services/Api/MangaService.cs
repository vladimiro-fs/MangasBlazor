﻿namespace MangasBlazor.Services.Api
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

        private int totalPageAmount;
        private MangaPaginationResponseDTO? responsePaginationDto;

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

        public async Task<bool> DeleteManga(int id)
        {
            var httpClient = _httpClientFactory.CreateClient("MangasAPI");
            
            using (var response = await httpClient.DeleteAsync(apiEndpoint + id))
            {
                if (response.IsSuccessStatusCode) 
                {
                    return true;
                }
                else if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    throw new UnauthorizedAccessException();
                }
            }

            return false;
        }

        public async Task<MangaDTO> GetManga(int id)
        {
            try
            {
                var httpClient = _httpClientFactory.CreateClient("MangasAPI");
                var response = await httpClient.GetAsync(apiEndpoint + id);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<MangaDTO>();
                }
                else
                {
                    var message = await response.Content.ReadAsStringAsync();
                    _logger.LogError($"Error getting manga id={id} - {message}");
                    throw new Exception($"Status Code: {response.StatusCode} - {message}");
                }
            }
            catch (UnauthorizedAccessException)
            {
                throw new UnauthorizedAccessException();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting manga id={id} \n\n {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<MangaDTO>> GetMangasByCategory(int categoryId)
        {
            try
            {
                var httpClient = _httpClientFactory.CreateClient("MangasAPI");
                var response = await httpClient.GetAsync(apiEndpoint + "getmangasbycategory/" + categoryId);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<IEnumerable<MangaDTO>>();
                }
                else
                {
                    var message = await response.Content.ReadAsStringAsync();
                    _logger.LogError($"Error getting manga from category id={categoryId} - {message}");
                    throw new Exception($"Status code: {response.StatusCode} - {message}");
                }
            }
            catch (UnauthorizedAccessException)
            {
                throw new UnauthorizedAccessException();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting mangas from category id ={categoryId} \n\n {ex.Message}");
                throw;
            }
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

        public async Task<MangaDTO> UpdateManga(int id, MangaDTO mangaDto)
        {
            var httpClient = _httpClientFactory.CreateClient("MangasAPI");

            MangaDTO mangaUpdated = new MangaDTO();

            using (var response = await httpClient.PutAsJsonAsync(apiEndpoint + id, mangaDto))
            {
                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadAsStreamAsync();
                    mangaUpdated = await JsonSerializer
                        .DeserializeAsync<MangaDTO>(apiResponse, _options);
                }
                else if (response.StatusCode == HttpStatusCode.Unauthorized) 
                {
                    throw new UnauthorizedAccessException();
                }
            }

            return mangaUpdated;
        }

        public async Task<MangaPaginationResponseDTO> GetMangaPagination(int page, int amountPerPage)
        {
            var path = $"pagination?page={page}&amountPerPage={amountPerPage}";
            var apiUrl = apiEndpoint + path;

            try
            {
                var httpClient = _httpClientFactory.CreateClient("MangasAPI");
                var httpResponse = await httpClient.GetAsync(apiUrl);

                if (httpResponse.IsSuccessStatusCode)
                {
                    var responseString = await httpResponse.Content.ReadAsStringAsync();
                    responsePaginationDto = JsonSerializer
                        .Deserialize<MangaPaginationResponseDTO>(responseString, _options);
                    totalPageAmount = responsePaginationDto.TotalPages;
                    mangas = responsePaginationDto.Mangas;
                }
                else
                {
                    _logger.LogWarning("The API request failed with status: " + httpResponse.StatusCode);
                }

                return responsePaginationDto;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error accessing mangas: {apiEndpoint}/pagination" + ex.Message);
                throw new UnauthorizedAccessException();
            }
        }
    }
}

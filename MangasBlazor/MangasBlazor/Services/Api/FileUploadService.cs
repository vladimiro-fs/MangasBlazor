﻿namespace MangasBlazor.Services.Api
{
    public class FileUploadService : IFileUploadService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public ILogger<FileUploadService> _logger;

        public FileUploadService(IHttpClientFactory httpClientFactory, 
                                 ILogger<FileUploadService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public async Task<HttpResponseMessage> UploadFileAsync(
            string endpoint, MultipartFormDataContent content)
        {
            HttpResponseMessage responseMessage = null;

            try
            {
                var httpClient = _httpClientFactory.CreateClient("MangasAPI");

                responseMessage = await httpClient.PostAsync(endpoint, content);
                return responseMessage;
            }
            catch (Exception)
            {
                _logger.LogInformation($"Error sending file: {endpoint}");
                throw;
            }
        }
    }
}

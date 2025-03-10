using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Logging;
using QuizManagementMicroservice.Core.Caching;
using QuizManagementMicroservice.Core.Dtos;
using QuizManagementMicroservice.Core.Dtos.ExternalDto;
using System.Net;
using System.Net.Http.Json;

namespace QuizManagementMicroservice.Core.HttpClients
{
    public class LectureMicroserviceClient : ILectureMicroserviceClient
    {
        private readonly HttpClient _httpClient;
        private readonly ICacheService _cacheService;
        private readonly ILogger<LectureMicroserviceClient> _logger;

        public LectureMicroserviceClient(HttpClient httpClient,
            ICacheService cacheService,
            ILogger<LectureMicroserviceClient> logger)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _cacheService = cacheService ?? throw new ArgumentNullException(nameof(cacheService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _logger.LogInformation("LectureMicroserviceClient initialized");
        }

        public async Task<LectureDto?> GetLectureAsync(Guid lectureId)
        {
            _logger.LogInformation("Attempting to get lecture with ID: {LectureId}", lectureId);

            try
            {
                string cachedKey = $"Lecture_{lectureId.ToString()}";
                _logger.LogDebug("Checking cache for lecture with key: {CacheKey}", cachedKey);

                var cachedLecture = await _cacheService.GetByAsync<LectureDto>(cachedKey);
                if (cachedLecture != null)
                {
                    _logger.LogInformation("Lecture found in cache with ID: {LectureId}", lectureId);
                    return cachedLecture;
                }

                _logger.LogDebug("Lecture not found in cache, making HTTP request for ID: {LectureId}", lectureId);
                var result = await _httpClient.GetAsync($"/api/Lecture/getLecture/{lectureId}");

                if (!result.IsSuccessStatusCode)
                {
                    if (result.StatusCode == HttpStatusCode.NotFound)
                    {
                        _logger.LogWarning("Lecture not found for ID: {LectureId}", lectureId);
                        return null;
                    }
                    else if (result.StatusCode == HttpStatusCode.BadRequest)
                    {
                        _logger.LogError("Bad request when getting lecture with ID: {LectureId}", lectureId);
                        throw new HttpRequestException("Invalid request.", null, HttpStatusCode.BadRequest);
                    }
                    else
                    {
                        _logger.LogError("Internal server error when getting lecture with ID: {LectureId}, StatusCode: {StatusCode}",
                            lectureId, result.StatusCode);
                        throw new HttpRequestException("Internal server error.", null, HttpStatusCode.InternalServerError);
                    }
                }

                _logger.LogDebug("Deserializing response for lecture ID: {LectureId}", lectureId);
                var response = await result.Content.ReadFromJsonAsync<ApiResponse<LectureDto>>();

                if (response == null || !response.IsSuccess || response.Result == null)
                {
                    _logger.LogWarning("Invalid or unsuccessful response received for lecture ID: {LectureId}", lectureId);
                    return null;
                }

                var lecture = response.Result;
                _logger.LogInformation("Lecture retrieved successfully for ID: {LectureId}", lectureId);

                _logger.LogDebug("Caching lecture with key: {CacheKey}", cachedKey);
                await _cacheService.SetAsync(cachedKey, lecture);

                return lecture;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting lecture with ID: {LectureId}", lectureId);
                return null;
            }
        }
    }
}
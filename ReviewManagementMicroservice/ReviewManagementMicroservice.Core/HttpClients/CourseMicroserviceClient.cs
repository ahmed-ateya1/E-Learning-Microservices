using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using ReviewManagementMicroservice.Core.Dtos.ExternalDto;
using System.Net.Http.Json;
using System.Net;
using System.Text.Json;

namespace ReviewManagementMicroservice.Core.HttpClients
{
    public class CourseMicroserviceClient : ICourseMicroserviceClient
    {
        private readonly ILogger<CourseMicroserviceClient> _logger;
        private readonly IDistributedCache _distributedCache;
        private readonly HttpClient _httpClient;

        public CourseMicroserviceClient(ILogger<CourseMicroserviceClient> logger,
            IDistributedCache distributedCache,
            HttpClient httpClient)
        {
            _logger = logger;
            _distributedCache = distributedCache;
            _httpClient = httpClient;
        }

        public async Task<CourseDto> GetCourseInfo(Guid courseId)
        {
            var cacheKey = $"Course_{courseId}";
            var result = await _distributedCache.GetStringAsync(cacheKey);
            if (result != null)
            {
                return JsonSerializer.Deserialize<CourseDto>(result);
            }
            var response = await _httpClient.GetAsync($"/api/Course/getCourse/{courseId}");
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("An error occurred while getting course with ID {CourseId}.", courseId);
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return null;
                }
                else if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    throw new HttpRequestException("Bad Request", null, HttpStatusCode.BadRequest);
                }
            }
            var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<CourseDto>>();
            if (apiResponse == null || !apiResponse.IsSuccess || apiResponse.Result == null)
            {
                return null;
            }
            await _distributedCache.SetStringAsync(cacheKey, JsonSerializer.Serialize(apiResponse.Result), new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5),
                SlidingExpiration = TimeSpan.FromMinutes(1)
            });
            return apiResponse.Result;
        }
    }
}

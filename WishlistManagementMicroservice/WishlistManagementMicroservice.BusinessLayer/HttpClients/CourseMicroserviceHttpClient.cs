using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using WishlistManagementMicroservice.BusinessLayer.Dtos.ExternalDto;

namespace WishlistManagementMicroservice.BusinessLayer.HttpClients
{
    public class CourseMicroserviceHttpClient : ICourseMicroserviceHttpClient
    {
        private readonly ILogger<CourseMicroserviceHttpClient> _logger;
        private readonly IDistributedCache _distributedCache;
        private readonly HttpClient _httpClient;

        public CourseMicroserviceHttpClient(
            ILogger<CourseMicroserviceHttpClient> logger,
            IDistributedCache distributedCache,
            HttpClient httpClient)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _distributedCache = distributedCache ?? throw new ArgumentNullException(nameof(distributedCache));
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _logger.LogDebug("CourseMicroserviceHttpClient initialized with base address: {BaseAddress}", _httpClient.BaseAddress);
        }

        public async Task<CourseDto> GetCourseInfoAsync(Guid courseID)
        {
            if (courseID == Guid.Empty)
            {
                _logger.LogWarning("GetCourseInfoAsync called with empty CourseID.");
                throw new ArgumentException("Course ID cannot be empty", nameof(courseID));
            }

            var cacheKey = $"Course:{courseID}";
            _logger.LogDebug("Checking cache for CourseID: {CourseID} with key: {CacheKey}", courseID, cacheKey);

            try
            {
                // Check cache first
                var cachedResult = await _distributedCache.GetStringAsync(cacheKey);
                if (cachedResult != null)
                {
                    _logger.LogInformation("Cache hit for CourseID: {CourseID}", courseID);
                    try
                    {
                        var cachedCourse = JsonSerializer.Deserialize<CourseDto>(cachedResult);
                        _logger.LogDebug("Successfully deserialized cached CourseDto for CourseID: {CourseID}", courseID);
                        return cachedCourse;
                    }
                    catch (JsonException ex)
                    {
                        _logger.LogError(ex, "Failed to deserialize cached data for CourseID: {CourseID}", courseID);
                        // Proceed to fetch from API if deserialization fails
                    }
                }
                else
                {
                    _logger.LogDebug("Cache miss for CourseID: {CourseID}", courseID);
                }

                // Fetch from API
                _logger.LogInformation("Fetching course info from API for CourseID: {CourseID}", courseID);
                var response = await _httpClient.GetAsync($"/api/Course/getCourse/{courseID}");
                _logger.LogDebug("API request completed for CourseID: {CourseID} with StatusCode: {StatusCode}", courseID, response.StatusCode);

                if (!response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == HttpStatusCode.NotFound)
                    {
                        _logger.LogWarning("Course not found for CourseID: {CourseID} (StatusCode: 404)", courseID);
                        return null;
                    }
                    else if (response.StatusCode == HttpStatusCode.BadRequest)
                    {
                        _logger.LogWarning("Bad request for CourseID: {CourseID} (StatusCode: 400)", courseID);
                        throw new HttpRequestException("Bad request", null, HttpStatusCode.BadRequest);
                    }
                    else
                    {
                        _logger.LogError("API request failed for CourseID: {CourseID} with StatusCode: {StatusCode}", courseID, response.StatusCode);
                        throw new HttpRequestException($"API request failed with status code: {response.StatusCode}", null, response.StatusCode);
                    }
                }

                // Deserialize response
                var content = await response.Content.ReadFromJsonAsync<ApiResponse<CourseDto>>();
                if (content == null || !content.IsSuccess || content.Result == null)
                {
                    _logger.LogWarning("Invalid or unsuccessful API response for CourseID: {CourseID}", courseID);
                    return null;
                }

                // Cache the result
                _logger.LogDebug("Caching course info for CourseID: {CourseID} with expiration settings", courseID);
                await _distributedCache.SetStringAsync(
                    cacheKey,
                    JsonSerializer.Serialize(content.Result),
                    new DistributedCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5),
                        SlidingExpiration = TimeSpan.FromMinutes(1)
                    });

                _logger.LogInformation("Successfully retrieved and cached course info for CourseID: {CourseID}, Title: {Title}", courseID, content.Result.Title);
                return content.Result;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "HTTP error occurred while fetching course info for CourseID: {CourseID}, StatusCode: {StatusCode}", courseID, ex.StatusCode);
                throw;
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "JSON serialization/deserialization error for CourseID: {CourseID}", courseID);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while fetching course info for CourseID: {CourseID}", courseID);
                throw;
            }
        }
    }
}
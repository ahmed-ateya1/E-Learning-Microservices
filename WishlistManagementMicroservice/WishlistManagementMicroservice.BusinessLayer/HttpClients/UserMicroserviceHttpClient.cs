using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using WishlistManagementMicroservice.BusinessLayer.Dtos.ExternalDto;

namespace WishlistManagementMicroservice.BusinessLayer.HttpClients
{
    public class UserMicroserviceHttpClient : IUserMicroserviceHttpClient
    {
        private readonly HttpClient _httpClient;
        private readonly IDistributedCache _distributedCache;
        private readonly ILogger<UserMicroserviceHttpClient> _logger;

        public UserMicroserviceHttpClient(
            HttpClient httpClient,
            IDistributedCache distributedCache,
            ILogger<UserMicroserviceHttpClient> logger)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _distributedCache = distributedCache ?? throw new ArgumentNullException(nameof(distributedCache));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _logger.LogDebug("UserMicroserviceHttpClient initialized with base address: {BaseAddress}", _httpClient.BaseAddress);
        }

        public async Task<UserDto?> GetUserInfoAsync(Guid userID)
        {
            if (userID == Guid.Empty)
            {
                _logger.LogWarning("GetUserInfoAsync called with empty UserID.");
                throw new ArgumentException("User ID cannot be empty", nameof(userID));
            }

            var cacheKey = $"User:{userID}";
            _logger.LogDebug("Checking cache for UserID: {UserID} with key: {CacheKey}", userID, cacheKey);

            try
            {
                // Check cache first
                var cachedResult = await _distributedCache.GetStringAsync(cacheKey);
                if (cachedResult != null)
                {
                    _logger.LogInformation("Cache hit for UserID: {UserID}", userID);
                    try
                    {
                        var cachedUser = JsonSerializer.Deserialize<UserDto>(cachedResult);
                        _logger.LogDebug("Successfully deserialized cached UserDto for UserID: {UserID}", userID);
                        return cachedUser;
                    }
                    catch (JsonException ex)
                    {
                        _logger.LogError(ex, "Failed to deserialize cached data for UserID: {UserID}", userID);
                        // Proceed to fetch from API if deserialization fails
                    }
                }
                else
                {
                    _logger.LogDebug("Cache miss for UserID: {UserID}", userID);
                }

                // Fetch from API
                _logger.LogInformation("Fetching user info from API for UserID: {UserID}", userID);
                var response = await _httpClient.GetAsync($"/api/Account/getUserInfo/{userID}");
                _logger.LogDebug("API request completed for UserID: {UserID} with StatusCode: {StatusCode}", userID, response.StatusCode);

                if (!response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == HttpStatusCode.NotFound)
                    {
                        _logger.LogWarning("User not found for UserID: {UserID} (StatusCode: 404)", userID);
                        return null;
                    }
                    else if (response.StatusCode == HttpStatusCode.BadRequest)
                    {
                        _logger.LogWarning("Bad request for UserID: {UserID} (StatusCode: 400)", userID);
                        throw new HttpRequestException("Bad request", null, HttpStatusCode.BadRequest);
                    }
                    else
                    {
                        _logger.LogError("API request failed for UserID: {UserID} with StatusCode: {StatusCode}", userID, response.StatusCode);
                        throw new HttpRequestException($"API request failed with status code: {response.StatusCode}", null, response.StatusCode);
                    }
                }

                // Deserialize response
                var content = await response.Content.ReadFromJsonAsync<ApiResponse<UserDto>>();
                if (content == null || !content.IsSuccess || content.Result == null)
                {
                    _logger.LogWarning("Invalid or unsuccessful API response for UserID: {UserID}", userID);
                    return null;
                }

                // Cache the result
                _logger.LogDebug("Caching user info for UserID: {UserID} with expiration settings", userID);
                await _distributedCache.SetStringAsync(
                    cacheKey,
                    JsonSerializer.Serialize(content.Result),
                    new DistributedCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5),
                        SlidingExpiration = TimeSpan.FromMinutes(1)
                    });

                _logger.LogInformation("Successfully retrieved and cached user info for UserID: {UserID}, FullName: {FullName}", userID, content.Result.FullName);
                return content.Result;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "HTTP error occurred while fetching user info for UserID: {UserID}, StatusCode: {StatusCode}", userID, ex.StatusCode);
                throw;
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "JSON serialization/deserialization error for UserID: {UserID}", userID);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while fetching user info for UserID: {UserID}", userID);
                throw;
            }
        }
    }
}
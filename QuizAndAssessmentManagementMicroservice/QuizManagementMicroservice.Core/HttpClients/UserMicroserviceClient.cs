using Microsoft.Extensions.Logging;
using QuizManagementMicroservice.Core.Dtos.ExternalDto;
using System.Net.Http.Json;
using System.Net;
using QuizManagementMicroservice.Core.Dtos;
using QuizManagementMicroservice.Core.Caching;

namespace QuizManagementMicroservice.Core.HttpClients
{
    public class UserMicroserviceClient : IUserMicroserviceClient
    {
        private readonly ILogger<UserMicroserviceClient> _logger;
        private readonly HttpClient _httpClient;
        private readonly ICacheService _cacheService;

        public UserMicroserviceClient(
            ILogger<UserMicroserviceClient> logger,
            HttpClient httpClient,
            ICacheService cacheService)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _cacheService = cacheService ?? throw new ArgumentNullException(nameof(cacheService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _logger.LogInformation("UserMicroserviceClient initialized");
        }

        public async Task<UserDto?> GetUserAsync(Guid userId)
        {
            _logger.LogInformation("Attempting to get user with ID: {UserId}", userId);

            try
            {
                var cacheKey = $"User_{userId}";
                _logger.LogDebug("Checking cache for user with key: {CacheKey}", cacheKey);

                var cachedUser = await _cacheService.GetByAsync<UserDto>(cacheKey);
                if (cachedUser != null)
                {
                    _logger.LogInformation("User found in cache with ID: {UserId}", userId);
                    return cachedUser;
                }

                _logger.LogDebug("User not found in cache, making HTTP request for ID: {UserId}", userId);
                var response = await _httpClient.GetAsync($"/api/Account/getUserInfo/{userId}");

                if (!response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == HttpStatusCode.NotFound)
                    {
                        _logger.LogWarning("User not found for ID: {UserId}", userId);
                        return null;
                    }
                    else if (response.StatusCode == HttpStatusCode.BadRequest)
                    {
                        _logger.LogError("Bad request when getting user with ID: {UserId}", userId);
                        throw new HttpRequestException("Bad Request", null, HttpStatusCode.BadRequest);
                    }
                    else
                    {
                        _logger.LogError("Unexpected error when getting user with ID: {UserId}, StatusCode: {StatusCode}",
                            userId, response.StatusCode);
                        throw new HttpRequestException($"Unexpected error with status code: {response.StatusCode}",
                            null, response.StatusCode);
                    }
                }

                _logger.LogDebug("Deserializing response for user ID: {UserId}", userId);
                var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<UserDto>>();

                if (apiResponse == null || !apiResponse.IsSuccess || apiResponse.Result == null)
                {
                    _logger.LogWarning("Invalid or unsuccessful response received for user ID: {UserId}", userId);
                    return null;
                }

                var user = apiResponse.Result;
                _logger.LogInformation("User retrieved successfully for ID: {UserId}", userId);

                _logger.LogDebug("Caching user with key: {CacheKey}", cacheKey);
                await _cacheService.SetAsync(cacheKey, user);

                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting user with ID: {UserId}", userId);
                return null;
            }
        }
    }
}
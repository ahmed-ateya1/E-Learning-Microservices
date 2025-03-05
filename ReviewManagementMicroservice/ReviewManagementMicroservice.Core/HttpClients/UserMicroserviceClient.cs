using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using ReviewManagementMicroservice.Core.Dtos.ExternalDto;
using System.Net.Http.Json;
using System.Net;
using System.Text.Json;

namespace ReviewManagementMicroservice.Core.HttpClients
{
    public class UserMicroserviceClient : IUserMicroserviceClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<UserMicroserviceClient> _logger;
        private readonly IDistributedCache _distributedCache;

        public UserMicroserviceClient(HttpClient httpClient,
            ILogger<UserMicroserviceClient> logger,
            IDistributedCache distributedCache)
        {
            _httpClient = httpClient;
            _logger = logger;
            _distributedCache = distributedCache;
        }

        public async Task<UserDto> GetUserInfo(Guid userId)
        {
            var cacheKey = $"User_{userId}";
            var result = await _distributedCache.GetStringAsync(cacheKey);
            if (result != null)
            {
                return JsonSerializer.Deserialize<UserDto>(result);
            }
            var response = await _httpClient.GetAsync($"/api/Account/getUserInfo/{userId}");
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("An error occurred while getting user with ID {UserId}.", userId);

                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return null;
                }
                else if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    throw new HttpRequestException("Bad Request", null, HttpStatusCode.BadRequest);
                }
            }
            var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<UserDto>>();
            if (apiResponse == null || !apiResponse.IsSuccess || apiResponse.Result == null)
            {
                return null;
            }

            var user = apiResponse.Result;
            await _distributedCache.SetStringAsync(cacheKey, JsonSerializer.Serialize(user), new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5),
                SlidingExpiration = TimeSpan.FromMinutes(1)
            });
            return user;
        }
    }
}

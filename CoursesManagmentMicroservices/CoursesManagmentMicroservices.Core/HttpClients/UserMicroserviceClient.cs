using CoursesManagmentMicroservices.Core.Caching;
using CoursesManagmentMicroservices.Core.Dtos.ExternalDto;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Http.Json;

namespace CoursesManagmentMicroservices.Core.HttpClients
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
            _logger = logger;
            _httpClient = httpClient;
            _cacheService = cacheService;
        }

        public async Task<UserDto?> GetUserAsync(Guid userId)
        {
            try
            {
                var cacheKey = $"User_{userId}";
                var cachedUser = await _cacheService.GetByAsync<UserDto>(cacheKey);
                if (cachedUser != null)
                {
                    return cachedUser;
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
                await _cacheService.SetAsync(cacheKey, user);
                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting user with ID {UserId}.", userId);
                return null;
            }
        }
        public class ApiResponse<T>
        {
            public string Message { get; set; }
            public HttpStatusCode StatusCode { get; set; }
            public bool IsSuccess { get; set; }
            public T Result { get; set; }
        }

    }
}

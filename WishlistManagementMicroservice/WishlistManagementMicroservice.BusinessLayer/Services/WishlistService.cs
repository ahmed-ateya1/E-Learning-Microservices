using AutoMapper;
using FluentValidation;
using Microsoft.Extensions.Logging;
using WishlistManagementMicroservice.BusinessLayer.Dtos;
using WishlistManagementMicroservice.BusinessLayer.HttpClients;
using WishlistManagementMicroservice.BusinessLayer.ServiceContract;
using WishlistManagementMicroservice.DataAccessLayer.Entities;
using WishlistManagementMicroservice.DataAccessLayer.RepoistoryContract;

namespace WishlistManagementMicroservice.BusinessLayer.Services
{
    public class WishlistService : IWishlistService
    {
        private readonly IWishlistRepository _wishlistRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<WishlistService> _logger;
        private readonly IUserMicroserviceHttpClient _userMicroserviceHttpClient;
        private readonly ICourseMicroserviceHttpClient _courseMicroserviceHttpClient;

        public WishlistService(
            IWishlistRepository wishlistRepository,
            IMapper mapper,
            IValidator<WishlistAddRequest> validatorAdd,
            ILogger<WishlistService> logger,
            IUserMicroserviceHttpClient userMicroserviceHttpClient,
            ICourseMicroserviceHttpClient courseMicroserviceHttpClient)
        {
            _wishlistRepository = wishlistRepository ?? throw new ArgumentNullException(nameof(wishlistRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _userMicroserviceHttpClient = userMicroserviceHttpClient ?? throw new ArgumentNullException(nameof(userMicroserviceHttpClient));
            _courseMicroserviceHttpClient = courseMicroserviceHttpClient ?? throw new ArgumentNullException(nameof(courseMicroserviceHttpClient));
            _logger.LogDebug("WishlistService initialized with all dependencies.");
        }

        public async Task<WishlistResponse> CreateAsync(WishlistAddRequest? request)
        {
            if (request == null)
            {
                _logger.LogWarning("CreateAsync called with null request.");
                throw new ArgumentNullException(nameof(request));
            }

            _logger.LogInformation("Creating wishlist for UserID: {UserID}, CourseID: {CourseID}", request.UserID, request.CourseID);

            try
            {

                _logger.LogDebug("Fetching user info for UserID: {UserID}", request.UserID);
                var user = await _userMicroserviceHttpClient.GetUserInfoAsync(request.UserID);
                if (user == null)
                {
                    _logger.LogWarning("User not found for UserID: {UserID}", request.UserID);
                    throw new ArgumentException("User not found");
                }
                _logger.LogInformation("User found: {FullName} (UserID: {UserID})", user.FullName, user.UserID);

                _logger.LogDebug("Fetching course info for CourseID: {CourseID}", request.CourseID);
                var course = await _courseMicroserviceHttpClient.GetCourseInfoAsync(request.CourseID);
                if (course == null)
                {
                    _logger.LogWarning("Course not found for CourseID: {CourseID}", request.CourseID);
                    throw new ArgumentException("Course not found");
                }
                _logger.LogInformation("Course found: {Title} (CourseID: {CourseID})", course.Title, course.CourseID);

                var wishlist = _mapper.Map<Wishlist>(request);
                wishlist.UserID = user.UserID;
                wishlist.CourseID = request.CourseID;

                _logger.LogDebug("Persisting wishlist to repository for UserID: {UserID}", wishlist.UserID);
                await _wishlistRepository.CreateAsync(wishlist);

                var response = _mapper.Map<WishlistResponse>(wishlist);
                response.FullName = user.FullName;
                response.PictureUrl = user.PictureUrl;
                response.CourseName = course.Title;
                response.PosterUrl = course.PosterUrl;

                _logger.LogInformation("Wishlist created successfully with WishlistID: {WishlistID} for UserID: {UserID}", wishlist.WishlistID, wishlist.UserID);
                return response;
            }
            catch (ValidationException ex)
            {
                _logger.LogError(ex, "Validation failed for WishlistAddRequest with UserID: {UserID}, CourseID: {CourseID}", request.UserID, request.CourseID);
                throw;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "HTTP error occurred while communicating with microservices for UserID: {UserID}, CourseID: {CourseID}", request.UserID, request.CourseID);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while creating wishlist for UserID: {UserID}, CourseID: {CourseID}", request.UserID, request.CourseID);
                throw;
            }
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                _logger.LogWarning("DeleteAsync called with empty WishlistID.");
                throw new ArgumentException("Wishlist ID cannot be empty");
            }
            _logger.LogInformation("Deleting wishlist with WishlistID: {WishlistID}", id);
            var result = await _wishlistRepository.DeleteAsync(id);
            if (result)
            {
                _logger.LogInformation("Wishlist with WishlistID: {WishlistID} deleted successfully", id);
            }
            else
            {
                _logger.LogWarning("Wishlist with WishlistID: {WishlistID} not found for deletion", id);
            }
            return result;
        }

        public async Task<IEnumerable<WishlistResponse>> GetAllAsync()
        {
            _logger.LogInformation("Fetching all wishlists.");
            var wishlists = await _wishlistRepository.GetAllWishlistsAsync();
            var responses = new List<WishlistResponse>();

            foreach (var wishlist in wishlists)
            {
                var user = await _userMicroserviceHttpClient.GetUserInfoAsync(wishlist.UserID);
                var course = await _courseMicroserviceHttpClient.GetCourseInfoAsync(wishlist.CourseID);

                var response = _mapper.Map<WishlistResponse>(wishlist);
                if (user != null)
                {
                    response.FullName = user.FullName;
                    response.PictureUrl = user.PictureUrl;
                }
                if (course != null)
                {
                    response.CourseName = course.Title;
                    response.PosterUrl = course.PosterUrl;
                }
                responses.Add(response);
            }

            _logger.LogInformation("Retrieved {Count} wishlists successfully.", responses.Count);
            return responses;
        }

        public async Task<IEnumerable<WishlistResponse>> GetWishlistByCourseIdAsync(Guid courseId)
        {
            if (courseId == Guid.Empty)
            {
                _logger.LogWarning("GetWishlistByCourseIdAsync called with empty CourseID.");
                throw new ArgumentException("Course ID cannot be empty");
            }
            _logger.LogInformation("Fetching wishlists for CourseID: {CourseID}", courseId);
            var wishlists = await _wishlistRepository.GetWishlistByCourseAsync(courseId);
            var responses = await MapWishlistsToResponses(wishlists);
            _logger.LogInformation("Retrieved {Count} wishlists for CourseID: {CourseID}", responses.Count(), courseId);
            return responses;
        }

        public async Task<WishlistResponse> GetWishlistByIdAsync(Guid wishlistId)
        {
            if (wishlistId == Guid.Empty)
            {
                _logger.LogWarning("GetWishlistByIdAsync called with empty WishlistID.");
                throw new ArgumentException("Wishlist ID cannot be empty");
            }

            _logger.LogInformation("Fetching wishlist with WishlistID: {WishlistID}", wishlistId);
            var wishlist = await _wishlistRepository.GetWishlistByIdAsync(wishlistId);
            if (wishlist == null)
            {
                _logger.LogWarning("Wishlist with WishlistID: {WishlistID} not found", wishlistId);
                return null;
            }

            var response = await MapWishlistToResponse(wishlist);
            _logger.LogInformation("Wishlist with WishlistID: {WishlistID} retrieved successfully", wishlistId);
            return response;
        }

        public async Task<IEnumerable<WishlistResponse>> GetWishlistByUserIdAsync(Guid userId)
        {
            if (userId == Guid.Empty)
            {
                _logger.LogWarning("GetWishlistByUserIdAsync called with empty UserID.");
                throw new ArgumentException("User ID cannot be empty");
            }


            _logger.LogInformation("Fetching wishlists for UserID: {UserID}", userId);
            var wishlists = await _wishlistRepository.GetWishlistByUserAsync(userId);
            var responses = await MapWishlistsToResponses(wishlists);
            _logger.LogInformation("Retrieved {Count} wishlists for UserID: {UserID}", responses.Count(), userId);
            return responses;

        }

        private async Task<WishlistResponse> MapWishlistToResponse(Wishlist wishlist)
        {
            var user = await _userMicroserviceHttpClient.GetUserInfoAsync(wishlist.UserID);
            var course = await _courseMicroserviceHttpClient.GetCourseInfoAsync(wishlist.CourseID);

            var response = _mapper.Map<WishlistResponse>(wishlist);
            if (user != null)
            {
                response.FullName = user.FullName;
                response.PictureUrl = user.PictureUrl;
            }
            if (course != null)
            {
                response.CourseName = course.Title;
                response.PosterUrl = course.PosterUrl;
            }
            return response;
        }

        private async Task<IEnumerable<WishlistResponse>> MapWishlistsToResponses(IEnumerable<Wishlist> wishlists)
        {
            var responses = new List<WishlistResponse>();
            foreach (var wishlist in wishlists)
            {
                responses.Add(await MapWishlistToResponse(wishlist));
            }
            return responses;
        }
    }
}
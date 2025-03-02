using Dapper;
using Microsoft.Extensions.Logging;
using Npgsql;
using WishlistManagementMicroservice.DataAccessLayer.Data;
using WishlistManagementMicroservice.DataAccessLayer.Entities;
using WishlistManagementMicroservice.DataAccessLayer.RepoistoryContract;

namespace WishlistManagementMicroservice.DataAccessLayer.Repositories
{
    public class WishlistRepository : IWishlistRepository
    {
        private readonly DapperDbContext _dbContext;
        private readonly ILogger<WishlistRepository> _logger;

        public WishlistRepository(DapperDbContext dbContext, ILogger<WishlistRepository> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Wishlist> CreateAsync(Wishlist wishlist)
        {
            if (wishlist == null || wishlist.UserID == Guid.Empty || wishlist.CourseID == Guid.Empty)
            {
                _logger.LogWarning("Invalid wishlist data: UserID or CourseID is empty");
                throw new ArgumentException("Wishlist data is invalid");
            }

            try
            {
                wishlist.WishlistID = Guid.NewGuid();
                wishlist.CreatedAt = DateTime.UtcNow;
                await _dbContext.DbConnection.ExecuteAsync(
                    "INSERT INTO wishlists (wishlist_id, user_id, course_id, created_at) VALUES (@WishlistID::uuid, @UserID::uuid, @CourseID::uuid, @CreatedAt)",
                    new
                    {
                        wishlist.WishlistID,
                        wishlist.UserID,
                        wishlist.CourseID,
                        wishlist.CreatedAt
                    });
                _logger.LogInformation("Wishlist created successfully with ID: {WishlistID} for UserID: {UserID}", wishlist.WishlistID, wishlist.UserID);
                return wishlist;
            }
            catch (NpgsqlException ex)
            {
                _logger.LogError(ex, "Database error occurred while creating wishlist for UserID: {UserID}, CourseID: {CourseID}", wishlist.UserID, wishlist.CourseID);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while creating wishlist for UserID: {UserID}", wishlist.UserID);
                throw;
            }
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                _logger.LogWarning("Attempted to delete wishlist with empty ID");
                throw new ArgumentException("Wishlist ID cannot be empty");
            }

            try
            {
                var rowsAffected = await _dbContext.DbConnection.ExecuteAsync(
                    "DELETE FROM wishlists WHERE wishlist_id = @WishlistID::uuid",
                    new { WishlistID = id });
                if (rowsAffected > 0)
                {
                    _logger.LogInformation("Wishlist with ID: {WishlistID} deleted successfully", id);
                    return true;
                }
                _logger.LogWarning("No wishlist found with ID: {WishlistID} to delete", id);
                return false;
            }
            catch (NpgsqlException ex)
            {
                _logger.LogError(ex, "Database error occurred while deleting wishlist with ID: {WishlistID}", id);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while deleting wishlist with ID: {WishlistID}", id);
                throw;
            }
        }

        public async Task<Wishlist> GetWishlistByIdAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                _logger.LogWarning("Attempted to fetch wishlist with empty ID");
                throw new ArgumentException("Wishlist ID cannot be empty");
            }

            try
            {
                var wishlist = await _dbContext.DbConnection.QueryFirstOrDefaultAsync<Wishlist>(
                    "SELECT * FROM wishlists WHERE wishlist_id = @WishlistID::uuid",
                    new { WishlistID = id });
                if (wishlist == null)
                {
                    _logger.LogWarning("Wishlist with ID: {WishlistID} not found", id);
                    return null; 
                }
                _logger.LogInformation("Wishlist with ID: {WishlistID} retrieved successfully", id);
                return wishlist;
            }
            catch (NpgsqlException ex)
            {
                _logger.LogError(ex, "Database error occurred while fetching wishlist with ID: {WishlistID}", id);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while fetching wishlist with ID: {WishlistID}", id);
                throw;
            }
        }

        public async Task<IEnumerable<Wishlist>> GetWishlistByUserAsync(Guid userId)
        {
            if (userId == Guid.Empty)
            {
                _logger.LogWarning("Attempted to fetch wishlists with empty UserID");
                throw new ArgumentException("User ID cannot be empty");
            }

            try
            {
                var wishlists = await _dbContext.DbConnection.QueryAsync<Wishlist>(
                    "SELECT * FROM wishlists WHERE user_id = @UserID::uuid",
                    new { UserID = userId });
                _logger.LogInformation("Retrieved {Count} wishlists for UserID: {UserID}", wishlists.Count(), userId);
                return wishlists;
            }
            catch (NpgsqlException ex)
            {
                _logger.LogError(ex, "Database error occurred while fetching wishlists for UserID: {UserID}", userId);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while fetching wishlists for UserID: {UserID}", userId);
                throw;
            }
        }

        public async Task<IEnumerable<Wishlist>> GetWishlistByCourseAsync(Guid courseId)
        {
            if (courseId == Guid.Empty)
            {
                _logger.LogWarning("Attempted to fetch wishlists with empty CourseID");
                throw new ArgumentException("Course ID cannot be empty");
            }

            try
            {
                var wishlists = await _dbContext.DbConnection.QueryAsync<Wishlist>(
                    "SELECT * FROM wishlists WHERE course_id = @CourseID::uuid",
                    new { CourseID = courseId });
                _logger.LogInformation("Retrieved {Count} wishlists for CourseID: {CourseID}", wishlists.Count(), courseId);
                return wishlists;
            }
            catch (NpgsqlException ex)
            {
                _logger.LogError(ex, "Database error occurred while fetching wishlists for CourseID: {CourseID}", courseId);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while fetching wishlists for CourseID: {CourseID}", courseId);
                throw;
            }
        }

        public async Task<IEnumerable<Wishlist>> GetAllWishlistsAsync()
        {
            try
            {
                var wishlists = await _dbContext.DbConnection.QueryAsync<Wishlist>(
                    "SELECT * FROM wishlists");
                _logger.LogInformation("Retrieved {Count} wishlists from the database", wishlists.Count());
                return wishlists;
            }
            catch (NpgsqlException ex)
            {
                _logger.LogError(ex, "Database error occurred while fetching all wishlists");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while fetching all wishlists");
                throw;
            }
        }
    }
}
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ReviewManagementMicroservice.Core.Domian.Entities;
using ReviewManagementMicroservice.Core.Domian.RepositoryContract;
using ReviewManagementMicroservice.Infrastructure.Data;
using System.Linq;
using System.Linq.Expressions;

namespace ReviewManagementMicroservice.Infrastructure.Repositories
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly ILogger<ReviewRepository> _logger;

        public ReviewRepository(
            ApplicationDbContext db,
            ILogger<ReviewRepository> logger)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _logger.LogInformation("ReviewRepository initialized.");
        }

        public async Task<Review> CreateAsync(Review review)
        {
            if (review == null)
            {
                _logger.LogError("Attempted to create a null review.");
                throw new ArgumentNullException(nameof(review));
            }

            _logger.LogInformation("Starting to create a new review with ID: {ReviewID}", review.ReviewID);

            await _db.Reviews.AddAsync(review);
            await _db.SaveChangesAsync();

            _logger.LogInformation("Successfully created review with ID: {ReviewID}", review.ReviewID);
            return review;
        }

        public async Task<bool> DeleteAsync(Guid reviewID)
        {
            _logger.LogInformation("Attempting to delete review with ID: {ReviewId}", reviewID);

            var review = await _db.Reviews.FindAsync(reviewID);
            if (review == null)
            {
                _logger.LogWarning("Review with ID {ReviewId} not found for deletion", reviewID);
                return false;
            }

            _db.Reviews.Remove(review);
            await _db.SaveChangesAsync();

            _logger.LogInformation("Successfully deleted review with ID: {ReviewId}", reviewID);
            return true;
        }

        public async Task<IEnumerable<Review>> GetAllAsync(Expression<Func<Review, bool>>? expression = null)
        {
            _logger.LogInformation("Fetching all reviews{Expression}", expression != null ? " with filter" : "");

            IQueryable<Review> query = _db.Reviews;

            if (expression != null)
            {
                _logger.LogDebug("Applying filter to reviews query: {Expression}", expression.ToString());
                query = query.Where(expression);
            }

            var reviews = await query.ToListAsync();
            _logger.LogInformation("Retrieved {Count} reviews from the database", reviews.Count);

            return reviews;
        }

        public async Task<Review?> GetByAsync(Expression<Func<Review, bool>> expression)
        {
            if (expression == null)
            {
                _logger.LogError("Attempted to fetch review with null expression.");
                throw new ArgumentNullException(nameof(expression));
            }

            _logger.LogInformation("Fetching review with filter: {Expression}", expression.ToString());

            var review = await _db.Reviews
                                 .Where(expression)
                                 .FirstOrDefaultAsync();

            if (review == null)
            {
                _logger.LogWarning("No review found matching the filter: {Expression}", expression.ToString());
            }
            else
            {
                _logger.LogInformation("Successfully retrieved review with ID: {ReviewID}", review?.ReviewID);
            }

            return review;
        }

        public async Task<Review> UpdateAsync(Review review)
        {
            if (review == null)
            {
                _logger.LogError("Attempted to update a null review.");
                throw new ArgumentNullException(nameof(review));
            }

            _logger.LogInformation("Starting to update review with ID: {ReviewID}", review.ReviewID);

            _db.Reviews.Update(review);
            await _db.SaveChangesAsync();

            _logger.LogInformation("Successfully updated review with ID: {ReviewID}", review.ReviewID);
            return review;
        }
    }
}
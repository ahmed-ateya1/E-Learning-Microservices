using ReviewManagementMicroservice.Core.Domian.Entities;
using ReviewManagementMicroservice.Core.Dtos;
using System.Linq.Expressions;

namespace ReviewManagementMicroservice.Core.ServiceContract
{
    /// <summary>
    /// Interface for Review Service to manage reviews.
    /// </summary>
    public interface IReviewService
    {
        /// <summary>
        /// Creates a new review.
        /// </summary>
        /// <param name="request">The review add request containing review details.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the created review response.</returns>
        Task<ReviewResponse> CreateAsync(ReviewAddRequest? request);

        /// <summary>
        /// Updates an existing review.
        /// </summary>
        /// <param name="request">The review update request containing updated review details.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the updated review response.</returns>
        Task<ReviewResponse> UpdateAsync(ReviewUpdateRequest? request);

        /// <summary>
        /// Deletes a review by its identifier.
        /// </summary>
        /// <param name="id">The identifier of the review to delete.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a boolean indicating whether the deletion was successful.</returns>
        Task<bool> DeleteAsync(Guid id);

        /// <summary>
        /// Gets a review by a specified predicate.
        /// </summary>
        /// <param name="predicate">The predicate to filter the review.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the review response that matches the predicate.</returns>
        Task<ReviewResponse> GetByAsync(Expression<Func<Review, bool>> predicate);

        /// <summary>
        /// Gets all reviews optionally filtered by a specified expression.
        /// </summary>
        /// <param name="expression">The expression to filter the reviews. If null, all reviews are returned.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a collection of review responses.</returns>
        Task<IEnumerable<ReviewResponse>> GetAllAsync(Expression<Func<Review, bool>>? expression = null);
    }
}

using ReviewManagementMicroservice.Core.Domian.Entities;
using System.Linq.Expressions;

namespace ReviewManagementMicroservice.Core.Domian.RepositoryContract
{
    public interface IReviewRepository
    {
        Task<Review> CreateAsync(Review review);
        Task<Review> UpdateAsync(Review review);
        Task<bool> DeleteAsync(Guid reviewID);
        Task<Review?> GetByAsync(Expression<Func<Review, bool>> expression);
        Task<IEnumerable<Review>> GetAllAsync(Expression<Func<Review,bool>>? expression =null);

    }
}

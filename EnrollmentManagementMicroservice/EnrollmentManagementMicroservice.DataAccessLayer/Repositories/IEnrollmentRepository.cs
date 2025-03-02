using EnrollmentManagementMicroservice.DataAccessLayer.Entities;
using System.Linq.Expressions;

namespace EnrollmentManagementMicroservice.DataAccessLayer.Repositories
{
    public interface IEnrollmentRepository
    {
        Task<Enrollment> CreateAsync(Enrollment enrollment);
        Task<bool> DeleteAsync(Guid id);
        Task<Enrollment> UpdateAsync(Enrollment enrollment);
        Task<Enrollment?> GetByAsync(Expression<Func<Enrollment, bool>> expression);
        Task<IEnumerable<Enrollment>> GetAllAsync(Expression<Func<Enrollment,bool>>? expression = null);

    }
}

using EnrollmentManagementMicroservice.BusinessLayer.Dtos;
using EnrollmentManagementMicroservice.DataAccessLayer.Entities;
using System.Linq.Expressions;

namespace EnrollmentManagementMicroservice.BusinessLayer.Services
{
    public interface IEnrollmentService
    {
        Task<EnrollmentResponse> AddEnrollmentAsync(EnrollmentAddRequest request);
        Task<EnrollmentResponse> UpdateEnrollmentAsync(EnrollmentUpdateRequest request);
        Task<bool> DeleteEnrollmentAsync(Guid enrollmentId);
        Task<EnrollmentResponse?> GetByAsync(Expression<Func<Enrollment, bool>> predicate);
        Task<IEnumerable<EnrollmentResponse>> GetAllAsync(Expression<Func<Enrollment,bool>>? expression = null);

    }
}

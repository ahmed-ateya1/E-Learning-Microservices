using QuizManagementMicroservice.Core.Domain.Entities;
using QuizManagementMicroservice.Core.Dtos;
using QuizManagementMicroservice.Core.Dtos.QuizzDto;
using System.Linq.Expressions;

namespace QuizManagementMicroservice.Core.ServiceContracts
{
    public interface IQuizzService
    {
        Task<QuizzResponse> CreateAsync(QuizzAddRequest? request);
        Task<QuizzResponse> UpdateAsync(QuizzUpdateRequest? request);
        Task<bool> DeleteAsync(Guid id);
        Task<QuizzResponse> GetByAsync(Expression<Func<Quizz, bool>> predicate);
        Task<PaginatedResponse<QuizzResponse>> GetAllAsync
            (Expression<Func<Quizz, bool>>? predicate=null , PaginationDto? pagination= null);
    }
}

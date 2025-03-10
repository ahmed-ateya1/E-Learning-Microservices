using Microsoft.EntityFrameworkCore.Storage;

namespace QuizManagementMicroservice.Core.Domain.RepositoryContracts
{
    public interface IUnitOfWork
    {
        IGenericRepository<T> Repository<T>() where T : class;
        Task<IDbContextTransaction> BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
        Task<int> CompleteAsync();
    }
}

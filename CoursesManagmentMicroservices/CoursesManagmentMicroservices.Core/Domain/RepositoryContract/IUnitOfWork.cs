﻿using Microsoft.EntityFrameworkCore.Storage;

namespace CoursesManagmentMicroservices.Core.Domain.RepositoryContract
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<T> Repository<T>() where T : class;
        Task<IDbContextTransaction> BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
        Task<int> CompleteAsync();
    }
}

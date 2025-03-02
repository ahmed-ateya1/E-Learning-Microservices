using EnrollmentManagementMicroservice.DataAccessLayer.Data;
using EnrollmentManagementMicroservice.DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EnrollmentManagementMicroservice.DataAccessLayer.Repositories
{
    public class EnrollmentRepository : IEnrollmentRepository
    {
        private readonly ApplicationDbContext _db;

        public EnrollmentRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<Enrollment> CreateAsync(Enrollment enrollment)
        {
            await _db.Enrollments.AddAsync(enrollment);
            await _db.SaveChangesAsync();
            return enrollment;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var enrollment = await _db.Enrollments.FindAsync(id);
            if (enrollment == null)
            {
                return false;
            }
            _db.Enrollments.Remove(enrollment);
            await _db.SaveChangesAsync();
            return true;
        }

     
        public Task<IEnumerable<Enrollment>> GetAllAsync
            (Expression<Func<Enrollment, bool>>? expression = null)
        {
            var enrollements = expression == null ?
                _db.Enrollments :
                _db.Enrollments.Where(expression);
            return Task.FromResult(enrollements.AsEnumerable());
        }

        public Task<Enrollment?> GetByAsync(Expression<Func<Enrollment, bool>> expression)
        {
            var enrollment = _db.Enrollments.FirstOrDefault(expression);
            return Task.FromResult(enrollment);
        }

       
        public Task<Enrollment> UpdateAsync(Enrollment enrollment)
        {
            var entry = _db.Enrollments.Update(enrollment);
            entry.State = EntityState.Modified;
            _db.SaveChanges();
            return Task.FromResult(enrollment);
        }
    }
}

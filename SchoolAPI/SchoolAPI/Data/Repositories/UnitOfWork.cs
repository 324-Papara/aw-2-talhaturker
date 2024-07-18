using System;
using System.Threading.Tasks;
using SchoolAPI.Data.Repositories;
using SchoolAPI.Models;

namespace SchoolAPI.Data.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly SchoolContext _context;

        public UnitOfWork(SchoolContext context)
        {
            _context = context;
            Students = new GenericRepository<Student>(_context);
            Courses = new GenericRepository<Course>(_context);
        }

        public IGenericRepository<Student> Students { get; }
        public IGenericRepository<Course> Courses { get; }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}

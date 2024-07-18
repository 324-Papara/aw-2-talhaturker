using System;
using System.Threading.Tasks;
using SchoolAPI.Models;

namespace SchoolAPI.Data.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<Student> Students { get; }
        IGenericRepository<Course> Courses { get; }
        Task<int> CompleteAsync();
    }
}

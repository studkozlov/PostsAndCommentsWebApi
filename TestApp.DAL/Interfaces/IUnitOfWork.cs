using System;
using System.Threading.Tasks;
using TestApp.DAL.Models;

namespace TestApp.DAL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Post> Posts { get; }
        IRepository<Comment> Comments { get; }

        void Save();
        Task SaveAsync();
    }
}

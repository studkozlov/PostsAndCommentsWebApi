using System;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Threading.Tasks;
using TestApp.DAL.Infrastructure;
using TestApp.DAL.Interfaces;
using TestApp.DAL.Models;

namespace TestApp.DAL.DataAccess
{
    public class EntityFrameworkUnitOfWork : IUnitOfWork
    {
        private readonly TestAppEntityFrameworkContext _context;
        private readonly IRepository<Post> _posts;
        private readonly IRepository<Comment> _comments;
        private bool _disposed;

        public IRepository<Post> Posts
        {
            get
            {
                return _posts;
            }
        }
        public IRepository<Comment> Comments
        {
            get
            {
                return _comments;
            }
        }
        
        public EntityFrameworkUnitOfWork(string connectionName)
        {
            _context = new TestAppEntityFrameworkContext(connectionName);
            _posts = new EntityFrameworkPostRepository(_context);
            _comments = new EntityFrameworkCommentRepository(_context);
        }
        public EntityFrameworkUnitOfWork(TestAppEntityFrameworkContext context)
        {
            _context = context;
            _posts = new EntityFrameworkPostRepository(_context);
            _comments = new EntityFrameworkCommentRepository(_context);
        }
        public void Save()
        {
            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                throw new DatabaseException("An error occurred sending updates to the database.", ex);
            }
            catch (DbEntityValidationException ex)
            {
                throw new DatabaseException("", ex);
            }
            catch (Exception ex)
            {
                throw new DatabaseException("An error occured during saving changes into database.", ex);
            }
        }
        public async Task SaveAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new DatabaseException("An error occurred sending updates to the database.", ex);
            }
            catch (DbEntityValidationException ex)
            {
                throw new DatabaseException("", ex);
            }
            catch (Exception ex)
            {
                throw new DatabaseException("An error occured during saving changes into database.", ex);
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
                _disposed = true;
            }
        }
    }
}

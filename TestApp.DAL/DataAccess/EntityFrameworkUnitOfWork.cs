using System;
using System.Threading.Tasks;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using TestApp.DAL.Interfaces;
using TestApp.DAL.Models;
using TestApp.DAL.Infrastructure;

namespace TestApp.DAL.DataAccess
{
    public class EntityFrameworkUnitOfWork : IUnitOfWork
    {
        private TestAppEntityFrameworkContext _context;
        private IRepository<Post> _posts;
        private IRepository<Comment> _comments;
        private bool _disposed = false;

        public IRepository<Post> Posts
        {
            get
            {
                return this._posts;
            }
        }
        public IRepository<Comment> Comments
        {
            get
            {
                return this._comments;
            }
        }
        
        public EntityFrameworkUnitOfWork(string connectionName)
        {
            this._context = new TestAppEntityFrameworkContext(connectionName);
            this._posts = new EntityFrameworkPostRepository(this._context);
            this._comments = new EntityFrameworkCommentRepository(this._context);
        }
        public EntityFrameworkUnitOfWork(TestAppEntityFrameworkContext context)
        {
            this._context = context;
            this._posts = new EntityFrameworkPostRepository(this._context);
            this._comments = new EntityFrameworkCommentRepository(this._context);
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
            if (!this._disposed)
            {
                if (disposing)
                {
                    this._context.Dispose();
                }
                this._disposed = true;
            }
        }
    }
}

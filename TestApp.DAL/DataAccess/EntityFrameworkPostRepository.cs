using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TestApp.DAL.Interfaces;
using TestApp.DAL.Models;

namespace TestApp.DAL.DataAccess
{
    public class EntityFrameworkPostRepository : IRepository<Post>
    {
        private TestAppEntityFrameworkContext _context;

        public EntityFrameworkPostRepository(TestAppEntityFrameworkContext context)
        {
            this._context = context;
        }

        public void Add(Post post)
        {
            _context.Posts.Add(post);
        }

        public void Delete(int id)
        {
            var post = _context.Posts.Find(id);
            if (post != null)
            {
                _context.Posts.Remove(post);
            }
        }

        public async Task<IEnumerable<Post>> FindAsync(Expression<Func<Post, bool>> pred)
        {
            return await _context.Posts.Where(pred).ToListAsync();
        }

        public Post Get(int id)
        {
            return _context.Posts.FirstOrDefault(p => p.Id == id);
        }

        public async Task<IEnumerable<Post>> GetAllAsync()
        {
            return await _context.Posts.ToListAsync();
        }

        public void Update(Post post)
        {
            _context.SetModified(post);
        }
    }
}

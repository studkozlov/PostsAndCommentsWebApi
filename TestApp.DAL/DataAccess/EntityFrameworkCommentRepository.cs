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
    public class EntityFrameworkCommentRepository : IRepository<Comment>
    {
        private readonly TestAppEntityFrameworkContext _context;

        public EntityFrameworkCommentRepository(TestAppEntityFrameworkContext context)
        {
            _context = context;
        }

        public void Add(Comment comment)
        {
            _context.Comments.Add(comment);
        }

        public void Delete(int id)
        {
            var comment = _context.Comments.Find(id);
            if (comment != null)
            {
                _context.Comments.Remove(comment);
            }
        }

        public async Task<IEnumerable<Comment>> FindAsync(Expression<Func<Comment, bool>> pred)
        {
            return await _context.Comments.Where(pred).ToListAsync();
        }

        public Comment Get(int id)
        {
            return _context.Comments.FirstOrDefault(c => c.Id == id);
        }

        public async Task<IEnumerable<Comment>> GetAllAsync()
        {
            return await _context.Comments.ToListAsync();
        }

        public void Update(Comment comment)
        {
            _context.SetModified(comment);
        }
    }
}

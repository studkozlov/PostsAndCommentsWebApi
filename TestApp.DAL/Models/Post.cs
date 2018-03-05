using System;
using System.Collections.Generic;

namespace TestApp.DAL.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string Author { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreationDate { get; set; }

        public ICollection<Comment> Comments { get; set; }

        public Post()
        {
            this.CreationDate = DateTime.Now;
        }
    }
}

using System;

namespace TestApp.DAL.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string User { get; set; }
        public string Text { get; set; }
        public DateTime CreationDate { get; set; }

        public int? PostId { get; set; }
        public Post Post { get; set; }

        public Comment()
        {
            CreationDate = DateTime.Now;
        }
    }
}

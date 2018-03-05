using System;

namespace TestApp.BLL.DTOModels
{
    public class CommentDTO
    {
        public int Id { get; set; }
        
        public string User { get; set; }
        
        public string Text { get; set; }

        public DateTime CreationDate { get; set; }

        public int PostId { get; set; }

        public CommentDTO()
        {
            this.CreationDate = DateTime.Now;
        }
    }
}

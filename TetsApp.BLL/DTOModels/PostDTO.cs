using System;

namespace TestApp.BLL.DTOModels
{
    public class PostDTO
    {
        public int Id { get; set; }

        public string Author { get; set; }
        
        public string Title { get; set; }
        
        public string Content { get; set; }

        public DateTime CreationDate { get; set; }

        public PostDTO()
        {
            this.CreationDate = DateTime.Now;
        }
    }
}

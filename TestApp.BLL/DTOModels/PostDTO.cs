using System;

namespace TestApp.BLL.DTOModels
{
    public class PostDto
    {
        public int Id { get; set; }

        public string Author { get; set; }
        
        public string Title { get; set; }
        
        public string Content { get; set; }

        public DateTime CreationDate { get; set; }

        public PostDto()
        {
            CreationDate = DateTime.Now;
        }
    }
}

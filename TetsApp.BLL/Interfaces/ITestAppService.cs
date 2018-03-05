using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TestApp.BLL.DTOModels;

namespace TestApp.BLL.Interfaces
{
    public interface ITestAppService : IDisposable
    {
        Task<IEnumerable<PostDTO>> GetAllPostsAsync();
        Task<IEnumerable<CommentDTO>> GetCommentsOfPostAsync(int postId);
        PostDTO GetPostById(int id);
        void AddPost(PostDTO post);
        Task UpdatePostAsync(PostDTO post);
        void DeletePostById(int id);
        CommentDTO GetCommentById(int id);
        void AddComment(CommentDTO comment);
        Task UpdateCommentAsync(CommentDTO comment);
        void DeleteCommentById(int id);
    }
}

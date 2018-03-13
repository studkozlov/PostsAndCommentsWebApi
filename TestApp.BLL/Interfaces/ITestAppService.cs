using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TestApp.BLL.DTOModels;

namespace TestApp.BLL.Interfaces
{
    public interface ITestAppService : IDisposable
    {
        Task<IEnumerable<PostDto>> GetAllPostsAsync();
        Task<IEnumerable<CommentDto>> GetCommentsOfPostAsync(int postId);
        PostDto GetPostById(int id);
        void AddPost(PostDto postDto);
        Task UpdatePostAsync(PostDto postDto);
        void DeletePostById(int id);
        CommentDto GetCommentById(int id);
        void AddComment(CommentDto commentDto);
        Task UpdateCommentAsync(CommentDto commentDto);
        void DeleteCommentById(int id);
    }
}

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using TestApp.DAL.Models;
using TestApp.DAL.Interfaces;
using TestApp.DAL.Infrastructure;
using TestApp.BLL.DTOModels;
using TestApp.BLL.Interfaces;
using TestApp.BLL.Infrastructure;

namespace TestApp.BLL.Services
{
    public class PostsAndCommentsManager : ITestAppService
    {
        private IUnitOfWork _db;
        private IDTOModelsValidator _validator;

        public PostsAndCommentsManager(IUnitOfWork uow, IDTOModelsValidator validator)
        {
            this._db = uow;
            this._validator = validator;
        }

        static PostsAndCommentsManager()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<PostDTO, Post>()
                .ForMember(d => d.Id, m => m.MapFrom(p => p.Id))
                .ForMember(d => d.Title, m => m.MapFrom(p => p.Title))
                .ForMember(d => d.Content, m => m.MapFrom(p => p.Content))
                .ForMember(d => d.Author, m => m.MapFrom(p => p.Author))
                .ForMember(d => d.CreationDate, m => m.MapFrom(p => p.CreationDate));

                cfg.CreateMap<CommentDTO, Comment>()
                .ForMember(d => d.Id, m => m.MapFrom(c => c.Id))
                .ForMember(d => d.User, m => m.MapFrom(c => c.User))
                .ForMember(d => d.CreationDate, m => m.MapFrom(c => c.CreationDate))
                .ForMember(d => d.Text, m => m.MapFrom(c => c.Text))
                .ForMember(d => d.PostId, m => m.MapFrom(c => c.PostId))
                .ForMember(d => d.Post, m => m.AllowNull());
            });
        }

        public void AddComment(CommentDTO commentDTO)
        {
            var validationErrors = _validator.GetCommentDTOValidationErrors(commentDTO);
            if (commentDTO != null && _db.Posts.Get(commentDTO.PostId) == null)
            {
                validationErrors.Add("", "There isn't post with such PostId");
            }
            if (validationErrors.Count > 0)
            {
                throw new ValidationException(validationErrors);
            }

            var comment = Mapper.Map<Comment>(commentDTO);
            comment.CreationDate = DateTime.Now;
            _db.Comments.Add(comment);

            try
            {
                _db.Save();
            }
            catch (DatabaseException ex)
            {
                throw new DataAccessException("An error occured during database interaction.", ex);
            }
        }

        public void AddPost(PostDTO postDTO)
        {
            var validationErrors = _validator.GetPostDTOValidationErrors(postDTO);
            if (validationErrors.Count > 0)
            {
                throw new ValidationException(validationErrors);
            }

            var post = Mapper.Map<Post>(postDTO);
            post.CreationDate = DateTime.Now;
            _db.Posts.Add(post);

            try
            {
                _db.Save();
            }
            catch (DatabaseException ex)
            {
                throw new DataAccessException("An error occured during database interaction.", ex);
            }
        }

        public void DeleteCommentById(int id)
        {
            _db.Comments.Delete(id);

            try
            {
                _db.Save();
            }
            catch (DatabaseException ex)
            {
                throw new DataAccessException("An error occured during database interaction.", ex);
            }
        }

        public void DeletePostById(int id)
        {
            _db.Posts.Delete(id);

            try
            {
                _db.Save();
            }
            catch (DatabaseException ex)
            {
                throw new DataAccessException("An error occured during database interaction.", ex);
            }
        }

        public async Task<IEnumerable<PostDTO>> GetAllPostsAsync()
        {
            var posts = await _db.Posts.GetAllAsync();
            return Mapper.Map<IEnumerable<Post>, IEnumerable<PostDTO>>(posts);
        }

        public async Task<IEnumerable<CommentDTO>> GetCommentsOfPostAsync(int postId)
        {
            var comments = await _db.Comments.FindAsync(c => c.PostId == postId);
            return Mapper.Map<IEnumerable<Comment>, IEnumerable<CommentDTO>>(comments);
        }

        public CommentDTO GetCommentById(int id)
        {
            var comment = _db.Comments.Get(id);
            return Mapper.Map<CommentDTO>(comment);
        }

        public PostDTO GetPostById(int id)
        {
            var post = _db.Posts.Get(id);
            return Mapper.Map<PostDTO>(post);
        }

        public async Task UpdateCommentAsync(CommentDTO commentDTO)
        {
            var validationErrors = _validator.GetCommentDTOValidationErrors(commentDTO);
            if (commentDTO != null && _db.Posts.Get(commentDTO.PostId) == null)
            {
                validationErrors.Add("", "There isn't post with such PostId");
            }
            if (validationErrors.Count > 0)
            {
                throw new ValidationException(validationErrors);
            }

            var comment = Mapper.Map<Comment>(commentDTO);
            _db.Comments.Update(comment);

            try
            {
                await _db.SaveAsync();
            }
            catch (DatabaseException ex)
            {
                throw new DataAccessException("An error occured during database interaction.", ex);
            }
        }

        public async Task UpdatePostAsync(PostDTO postDTO)
        {
            var validationErrors = _validator.GetPostDTOValidationErrors(postDTO);
            if (validationErrors.Count > 0)
            {
                throw new ValidationException(validationErrors);
            }

            var post = Mapper.Map<Post>(postDTO);
            _db.Posts.Update(post);
            
            try
            {
                await _db.SaveAsync();
            }
            catch (DatabaseException ex)
            {
                throw new DataAccessException("An error occured during database interaction.", ex);
            }
        }

        public void Dispose()
        {
            this._db.Dispose();
        }
    }
}

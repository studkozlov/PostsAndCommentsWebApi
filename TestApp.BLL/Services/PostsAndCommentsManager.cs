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
        private readonly IUnitOfWork _db;
        private readonly IDtoModelsValidator _validator;

        public PostsAndCommentsManager(IUnitOfWork uow, IDtoModelsValidator validator)
        {
            _db = uow;
            _validator = validator;
        }

        static PostsAndCommentsManager()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<PostDto, Post>()
                .ForMember(d => d.Id, m => m.MapFrom(p => p.Id))
                .ForMember(d => d.Title, m => m.MapFrom(p => p.Title))
                .ForMember(d => d.Content, m => m.MapFrom(p => p.Content))
                .ForMember(d => d.Author, m => m.MapFrom(p => p.Author))
                .ForMember(d => d.CreationDate, m => m.MapFrom(p => p.CreationDate));

                cfg.CreateMap<CommentDto, Comment>()
                .ForMember(d => d.Id, m => m.MapFrom(c => c.Id))
                .ForMember(d => d.User, m => m.MapFrom(c => c.User))
                .ForMember(d => d.CreationDate, m => m.MapFrom(c => c.CreationDate))
                .ForMember(d => d.Text, m => m.MapFrom(c => c.Text))
                .ForMember(d => d.PostId, m => m.MapFrom(c => c.PostId))
                .ForMember(d => d.Post, m => m.AllowNull());
            });
        }

        /// <summary>
        /// Adding new comment
        /// </summary>
        /// <param name="commentDto">comment to be added</param>
        /// <exception cref="ValidationException">Thrown when comment validation don't pass</exception>
        /// <exception cref="DatabaseException">Thrown when problems on data source level appear</exception>
        public void AddComment(CommentDto commentDto)
        {
            var validationErrors = _validator.GetCommentDtoValidationErrors(commentDto);
            if (commentDto != null && _db.Posts.Get(commentDto.PostId) == null)
            {
                validationErrors.Add(("", "There isn't post with such PostId"));
            }
            if (validationErrors.Count > 0)
            {
                throw new ValidationException(validationErrors);
            }

            var comment = Mapper.Map<Comment>(commentDto);
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

        /// <summary>
        /// Adding new post
        /// </summary>
        /// <param name="postDto">post to be added</param>
        /// <exception cref="ValidationException">Thrown when post validation don't pass</exception>
        /// <exception cref="DataAccessException">Thrown when problems on data source level appear</exception>
        public void AddPost(PostDto postDto)
        {
            var validationErrors = _validator.GetPostDtoValidationErrors(postDto);
            if (validationErrors.Count > 0)
            {
                throw new ValidationException(validationErrors);
            }

            var post = Mapper.Map<Post>(postDto);
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

        /// <summary>
        /// Deleting comment by its ID
        /// </summary>
        /// <param name="id">ID of comment to be deleted</param>
        /// <exception cref="DataAccessException">Thrown when problems on data source level appear</exception>
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

        /// <summary>
        /// Deleting post by its ID
        /// </summary>
        /// <param name="id">ID of post to be deleted</param>
        /// <exception cref="DataAccessException">Thrown when problems on data source level appear</exception>
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

        /// <summary>
        /// Getting all existing posts asynchronously
        /// </summary>
        /// <returns>posts as IEnumerable object</returns>
        public async Task<IEnumerable<PostDto>> GetAllPostsAsync()
        {
            var posts = await _db.Posts.GetAllAsync();
            return Mapper.Map<IEnumerable<Post>, IEnumerable<PostDto>>(posts);
        }

        /// <summary>
        /// Getting all comments related to specific post
        /// </summary>
        /// <param name="postId">ID of post</param>
        /// <returns>comments as IEnumerable object</returns>
        public async Task<IEnumerable<CommentDto>> GetCommentsOfPostAsync(int postId)
        {
            var comments = await _db.Comments.FindAsync(c => c.PostId == postId);
            return Mapper.Map<IEnumerable<Comment>, IEnumerable<CommentDto>>(comments);
        }

        /// <summary>
        /// Getting comment by its ID
        /// </summary>
        /// <param name="id">ID of comment</param>
        /// <returns>comment with requested ID, null if doesn't exist</returns>
        public CommentDto GetCommentById(int id)
        {
            var comment = _db.Comments.Get(id);
            return Mapper.Map<CommentDto>(comment);
        }

        /// <summary>
        /// Getting post by its ID
        /// </summary>
        /// <param name="id">ID of post</param>
        /// <returns>post with requested ID, null if doesn't exist</returns>
        public PostDto GetPostById(int id)
        {
            var post = _db.Posts.Get(id);
            return Mapper.Map<PostDto>(post);
        }

        /// <summary>
        /// Updating comment asynchronously
        /// </summary>
        /// <param name="commentDto">comment to be updated</param>
        /// <exception cref="ValidationException">Thrown when comment validation don't pass</exception>
        /// <exception cref="DataAccessException">Thrown when problems on data source level appear</exception>
        public async Task UpdateCommentAsync(CommentDto commentDto)
        {
            var validationErrors = _validator.GetCommentDtoValidationErrors(commentDto);
            if (commentDto != null && _db.Posts.Get(commentDto.PostId) == null)
            {
                validationErrors.Add(("", "There isn't post with such PostId"));
            }
            if (validationErrors.Count > 0)
            {
                throw new ValidationException(validationErrors);
            }

            var comment = Mapper.Map<Comment>(commentDto);
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

        /// <summary>
        /// Updating post asynchronously
        /// </summary>
        /// <param name="postDto">post to be updated</param>
        /// <exception cref="ValidationException">Thrown when post validation don't pass</exception>
        /// <exception cref="DataAccessException">Thrown when problems on data source level appear</exception>
        public async Task UpdatePostAsync(PostDto postDto)
        {
            var validationErrors = _validator.GetPostDtoValidationErrors(postDto);
            if (validationErrors.Count > 0)
            {
                throw new ValidationException(validationErrors);
            }

            var post = Mapper.Map<Post>(postDto);
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
            _db.Dispose();
        }
    }
}

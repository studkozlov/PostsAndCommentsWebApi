using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TestApp.BLL.Interfaces;
using TestApp.BLL.Services;
using TestApp.BLL.DTOModels;
using TestApp.DAL.Interfaces;
using TestApp.DAL.Models;

namespace TestApp.BLL.Tests
{
    [TestClass]
    public class PostsAndCommentsManagerUnitTest
    {
        private Mock<IRepository<Post>> _mockPosts;
        private Mock<IRepository<Comment>> _mockComments;
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private Mock<IDTOModelsValidator> _mockValidator;
        private ITestAppService _manager;

        [TestInitialize]
        public void SetupContext()
        {
            _mockPosts = new Mock<IRepository<Post>>();
            _mockComments = new Mock<IRepository<Comment>>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockValidator = new Mock<IDTOModelsValidator>();
            _mockValidator.Setup(m => m.GetCommentDTOValidationErrors(It.IsAny<CommentDTO>()))
                .Returns(new Dictionary<string, string>(0));
            _mockValidator.Setup(m => m.GetPostDTOValidationErrors(It.IsAny<PostDTO>()))
                .Returns(new Dictionary<string, string>(0));
            _mockUnitOfWork.Setup(m => m.Posts).Returns(_mockPosts.Object);
            _mockUnitOfWork.Setup(m => m.Comments).Returns(_mockComments.Object);

            _manager = new PostsAndCommentsManager(_mockUnitOfWork.Object, _mockValidator.Object);
        }

        [TestMethod]
        public void AddCommentByService()
        {
            var comment = new CommentDTO()
            {
                User = "User",
                Text = "Text",
                CreationDate = DateTime.Now,
                PostId = 1
            };
            _mockPosts.Setup(m => m.Get(It.Is<int>(arg => arg == 1))).Returns(new Post());

            _manager.AddComment(comment);

            _mockValidator.Verify(m => m.GetCommentDTOValidationErrors(comment));
            _mockComments.Verify(m => m.Add(It.IsAny<Comment>()), Times.Once);
            _mockUnitOfWork.Verify(m => m.Save(), Times.Once);
        }

        [TestMethod]
        public void AddPostByService()
        {
            var post = new PostDTO()
            {
                Title = "Title",
                Author = "Author",
                CreationDate = DateTime.Now
            };

            _manager.AddPost(post);

            _mockPosts.Verify(m => m.Add(It.IsAny<Post>()), Times.Once);
            _mockUnitOfWork.Verify(m => m.Save(), Times.Once);
        }

        [TestMethod]
        public void DeleteCommentByService()
        {
            _manager.DeleteCommentById(1);

            _mockComments.Verify(m => m.Delete(It.Is<int>(arg => arg == 1)), Times.Once);
            _mockUnitOfWork.Verify(m => m.Save(), Times.Once);
        }

        [TestMethod]
        public void DeletePostByService()
        {
            _manager.DeletePostById(1);

            _mockPosts.Verify(m => m.Delete(It.Is<int>(ArgIterator => ArgIterator == 1)), Times.Once);
            _mockUnitOfWork.Verify(m => m.Save(), Times.Once);
        }

        [TestMethod]
        public void GetCommentByIdByService()
        {
            _manager.GetCommentById(1);

            _mockComments.Verify(m => m.Get(It.Is<int>(arg => arg == 1)), Times.Once);
        }

        [TestMethod]
        public void GetPostByIdByService()
        {
            _manager.GetPostById(1);

            _mockPosts.Verify(m => m.Get(It.Is<int>(arg => arg == 1)), Times.Once);
        }

        [TestMethod]
        public async Task GetAllPostsByManager()
        {
            await _manager.GetAllPostsAsync();

            _mockPosts.Verify(m => m.GetAllAsync(), Times.Once);
        }

        [TestMethod]
        public async Task UpdateCommentAsyncByManager()
        {
            var comment = new CommentDTO()
            {
                User = "User",
                Text = "Text",
                CreationDate = DateTime.Now,
                PostId = 1
            };
            _mockPosts.Setup(m => m.Get(It.Is<int>(arg => arg == 1))).Returns(new Post());

            await _manager.UpdateCommentAsync(comment);

            _mockValidator.Verify(m => m.GetCommentDTOValidationErrors(comment));
            _mockComments.Verify(m => m.Update(It.IsAny<Comment>()), Times.Once);
            _mockUnitOfWork.Verify(m => m.SaveAsync(), Times.Once);
        }

        [TestMethod]
        public async Task UpdatePostAsyncByManager()
        {
            var post = new PostDTO()
            {
                Title = "Title",
                Author = "Author",
                CreationDate = DateTime.Now
            };

            await _manager.UpdatePostAsync(post);

            _mockPosts.Verify(m => m.Update(It.IsAny<Post>()), Times.Once);
            _mockUnitOfWork.Verify(m => m.SaveAsync(), Times.Once);
        }

        [TestMethod]
        public async Task GetAllCommentsOfPost()
        {
            var comments = new List<CommentDTO>()
            {
                new CommentDTO()
                {
                    PostId = 1
                },
                new CommentDTO()
                {
                    PostId = 2
                },
                new CommentDTO()
                {
                    PostId = 1
                }
            };
            _mockComments.Setup(m => m.FindAsync(It.IsAny<Expression<Func<Comment, bool>>>()))
                .Returns((Expression<Func<Comment, bool>> predicate) => Task.FromResult(new List<Comment>()
                {
                    new Comment(),
                    new Comment()
                }.AsEnumerable()));

            var commentsOfPost = await _manager.GetCommentsOfPostAsync(1);

            Assert.AreEqual(commentsOfPost.Count(), 2);
        }
    }
}

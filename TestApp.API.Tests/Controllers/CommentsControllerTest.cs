using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Results;
using System.Threading.Tasks;
using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TestApp.API.Controllers;
using TestApp.BLL.Interfaces;
using TestApp.BLL.DTOModels;

namespace TestApp.API.Tests.Controllers
{
    [TestClass]
    public class CommentsControllerTest
    {
        private CommentsController _controller;
        private Mock<ITestAppService> _mockManager;

        [TestInitialize]
        public void SetupContext()
        {
            _mockManager = new Mock<ITestAppService>();
            _controller = new CommentsController(_mockManager.Object);
        }

        [TestMethod]
        public async Task GetCommentsOfPost()
        {
            _mockManager.Setup(m => m.GetCommentsOfPostAsync(1)).Returns(Task.FromResult(new List<CommentDTO>()
            {
                new CommentDTO()
            }.AsEnumerable()));

            var actionResult = await _controller.GetCommentsOfPost(1);
            var contentResult = actionResult as OkNegotiatedContentResult<IEnumerable<CommentDTO>>;

            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);
            Assert.AreEqual(1, contentResult.Content.Count());
        }

        [TestMethod]
        public void GetCommentById()
        {
            _mockManager.Setup(m => m.GetCommentById(1)).Returns(new CommentDTO() { Id = 1 });

            var actionResult = _controller.Get(1);
            var contentResult = actionResult as OkNegotiatedContentResult<CommentDTO>;

            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);
            Assert.AreEqual(1, contentResult.Content.Id);
        }

        [TestMethod]
        public void GetCommentByIdReturnsNotFound()
        {

            var actionResult = _controller.Get(1);
            var contentResult = actionResult as OkNegotiatedContentResult<CommentDTO>;

            Assert.IsInstanceOfType(actionResult, typeof(NotFoundResult));
        }

        [TestMethod]
        public void PostPost()
        {
            var actionResult = _controller.Post(new CommentDTO() { Id = 1 });

            _mockManager.Verify(m => m.AddComment(It.IsAny<CommentDTO>()), Times.Once);
            Assert.IsInstanceOfType(actionResult, typeof(OkResult));
        }

        [TestMethod]
        public async Task PutComment()
        {
            var actionResult = await _controller.Put(2, new CommentDTO() { Id = 2 });
            var contentResult = actionResult as NegotiatedContentResult<CommentDTO>;

            _mockManager.Verify(m => m.UpdateCommentAsync(It.IsAny<CommentDTO>()), Times.Once);
            Assert.IsNotNull(contentResult);
            Assert.AreEqual(HttpStatusCode.Accepted, contentResult.StatusCode);
            Assert.IsNotNull(contentResult.Content);
            Assert.AreEqual(contentResult.Content.Id, 2);
        }

        [TestMethod]
        public void DeleteComment()
        {

            var actionResult = _controller.Delete(1);

            Assert.IsInstanceOfType(actionResult, typeof(OkResult));
        }
    }
}

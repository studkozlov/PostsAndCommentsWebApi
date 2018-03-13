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
    public class PostsControllerTest
    {
        private PostsController _controller;
        private Mock<ITestAppService> _mockManager;

        [TestInitialize]
        public void SetupContext()
        {
            _mockManager = new Mock<ITestAppService>();
            _controller = new PostsController(_mockManager.Object);
        }

        [TestMethod]
        public async Task GetAllPosts()
        {
            _mockManager.Setup(m => m.GetAllPostsAsync()).Returns(Task.FromResult(new List<PostDto>()
            {
                new PostDto()
            }.AsEnumerable()));

            var actionResult = await _controller.Get();
            var contentResult = actionResult as OkNegotiatedContentResult<IEnumerable<PostDto>>;

            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);
            Assert.AreEqual(1, contentResult.Content.Count());
        }

        [TestMethod]
        public void GetPostById()
        {
            _mockManager.Setup(m => m.GetPostById(1)).Returns(new PostDto() { Id = 1 });
            
            var actionResult = _controller.Get(1);
            var contentResult = actionResult as OkNegotiatedContentResult<PostDto>;
            
            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);
            Assert.AreEqual(1, contentResult.Content.Id);
        }

        [TestMethod]
        public void GetPostByIdReturnsNotFound()
        {

            var actionResult = _controller.Get(1);

            Assert.IsInstanceOfType(actionResult, typeof(NotFoundResult));
        }

        [TestMethod]
        public void PostPost()
        {
            var actionResult = _controller.Post(new PostDto() { Id = 1 });

            _mockManager.Verify(m => m.AddPost(It.IsAny<PostDto>()), Times.Once);
            Assert.IsInstanceOfType(actionResult, typeof(OkResult));
        }

        [TestMethod]
        public async Task PutPost()
        {
            var actionResult = await _controller.Put(2, new PostDto() { Id = 2 });
            var contentResult = actionResult as NegotiatedContentResult<PostDto>;

            _mockManager.Verify(m => m.UpdatePostAsync(It.IsAny<PostDto>()), Times.Once);
            Assert.IsNotNull(contentResult);
            Assert.AreEqual(HttpStatusCode.Accepted, contentResult.StatusCode);
            Assert.IsNotNull(contentResult.Content);
            Assert.AreEqual(contentResult.Content.Id, 2);
        }

        [TestMethod]
        public void DeletePost()
        {

            var actionResult = _controller.Delete(1);

            Assert.IsInstanceOfType(actionResult, typeof(OkResult));
        }

    }
}

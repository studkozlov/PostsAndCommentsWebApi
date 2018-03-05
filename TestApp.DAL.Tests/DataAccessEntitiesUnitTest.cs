using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TestApp.DAL.DataAccess;
using TestApp.DAL.Models;
using TestApp.DAL.Tests.Infrastructure;

namespace TestApp.DAL.Tests
{
    [TestClass]
    public class DataAccessEntitiesUnitTest
    {
        private Mock<DbSet<Post>> _mockPostsSet;
        private Mock<DbSet<Comment>> _mockCommentsSet;
        private Mock<TestAppEntityFrameworkContext> _mockContext;

        [TestInitialize]
        public void SetupContext()
        {
            _mockPostsSet = new Mock<DbSet<Post>>();
            _mockCommentsSet = new Mock<DbSet<Comment>>();
            _mockContext = new Mock<TestAppEntityFrameworkContext>();
            _mockContext.Setup(m => m.Posts).Returns(_mockPostsSet.Object);
            _mockContext.Setup(m => m.Comments).Returns(_mockCommentsSet.Object);
        }

        [TestMethod]
        public void CreatePostViaContext()
        {
            var postRepo = new EntityFrameworkPostRepository(_mockContext.Object);
            postRepo.Add(new Post());

            _mockPostsSet.Verify(m => m.Add(It.IsAny<Post>()), Times.Once);
        }

        [TestMethod]
        public void DeletePostViaContext()
        {
            _mockPostsSet.Setup(m => m.Find(It.Is<int>(arg => arg == 0))).Returns(new Post());

            var postRepo = new EntityFrameworkPostRepository(_mockContext.Object);
            postRepo.Delete(0);

            _mockPostsSet.Verify(m => m.Remove(It.IsAny<Post>()), Times.Once);
        }

        [TestMethod]
        public void UpdatePostViaContext()
        {
            var postRepo = new EntityFrameworkPostRepository(_mockContext.Object);
            postRepo.Update(new Post() { Id = 0 });

            _mockContext.Verify(m => m.SetModified(It.IsAny<Post>()), Times.Once);
        }

        [TestMethod]
        public async Task GetAllPosts()
        {
            _mockContext.Setup(m => m.Posts).Returns(GetQueryableAsyncMockDbSet<Post>(
                new Post()
                {
                    Id = 1,
                    Title = "First"
                },
                new Post()
                {
                    Id = 2,
                    Title = "Second"
                },
                new Post()
                {
                    Id = 3,
                    Title = "Third"
                }));
            
            var postRepo = new EntityFrameworkPostRepository(_mockContext.Object);
            var allPosts = await postRepo.GetAllAsync();

            Assert.AreEqual(allPosts.Count() ,3);
        }

        [TestMethod]
        public async Task FindPosts()
        {
            _mockContext.Setup(m => m.Posts).Returns(GetQueryableAsyncMockDbSet<Post>(
                new Post()
                {
                    Id = 1,
                    Title = "First"
                },
                new Post()
                {
                    Id = 2,
                    Title = "Second"
                },
                new Post()
                {
                    Id = 3,
                    Title = "Third"
                }));

            var postRepo = new EntityFrameworkPostRepository(_mockContext.Object);
            var foundPosts = await postRepo.FindAsync(p => p.Title == "Second");

            Assert.AreEqual(foundPosts.Count(), 1);
            Assert.AreEqual(foundPosts.First().Id, 2);
        }

        [TestMethod]
        public void GetPost()
        {
            _mockContext.Setup(m => m.Posts).Returns(GetQueryableMockDbSet(
                new Post()
                {
                    Id = 1,
                    Title = "First"
                },
                new Post()
                {
                    Id = 2,
                    Title = "Second"
                }));
            var postRepo = new EntityFrameworkPostRepository(_mockContext.Object);
            var post = postRepo.Get(2);
            Assert.AreEqual(post.Title, "Second");
        }

        [TestMethod]
        public void CreateCommentViaContext()
        {
            var commentRepo = new EntityFrameworkCommentRepository(_mockContext.Object);
            commentRepo.Add(new Comment());

            _mockCommentsSet.Verify(m => m.Add(It.IsAny<Comment>()), Times.Once);
        }

        [TestMethod]
        public void DeleteCommentViaContext()
        {
            _mockCommentsSet.Setup(m => m.Find(It.Is<int>(arg => arg == 0))).Returns(new Comment());

            var commentRepo = new EntityFrameworkCommentRepository(_mockContext.Object);
            commentRepo.Delete(0);

            _mockCommentsSet.Verify(m => m.Remove(It.IsAny<Comment>()), Times.Once);
        }

        [TestMethod]
        public void UpdateCommentViaContext()
        {
            var commentRepo = new EntityFrameworkCommentRepository(_mockContext.Object);
            commentRepo.Update(new Comment() { Id = 0 });

            _mockContext.Verify(m => m.SetModified(It.IsAny<Comment>()), Times.Once);
        }

        [TestMethod]
        public async Task GetAllComments()
        {
            _mockContext.Setup(m => m.Comments).Returns(GetQueryableAsyncMockDbSet<Comment>(
                new Comment()
                {
                    Id = 1,
                    User = "First"
                },
                new Comment()
                {
                    Id = 2,
                    User = "Second"
                },
                new Comment()
                {
                    Id = 3,
                    User = "Third"
                }));

            var commentRepo = new EntityFrameworkCommentRepository(_mockContext.Object);
            var allComments = await commentRepo.GetAllAsync();

            Assert.AreEqual(allComments.Count(), 3);
        }

        [TestMethod]
        public async Task FindComments()
        {
            _mockContext.Setup(m => m.Comments).Returns(GetQueryableAsyncMockDbSet<Comment>(
                new Comment()
                {
                    Id = 1,
                    User = "First"
                },
                new Comment()
                {
                    Id = 2,
                    User = "Second"
                },
                new Comment()
                {
                    Id = 3,
                    User = "Third"
                }));

            var commentRepo = new EntityFrameworkCommentRepository(_mockContext.Object);
            var foundComments = await commentRepo.FindAsync(c => c.User == "Second");
            var idLessThanThreeComments = await commentRepo.FindAsync(c => c.Id < 3);

            Assert.AreEqual(foundComments.Count(), 1);
            Assert.AreEqual(foundComments.First().Id, 2);
            Assert.AreEqual(idLessThanThreeComments.Count(), 2);
        }

        [TestMethod]
        public void GetComment()
        {
            _mockContext.Setup(m => m.Comments).Returns(GetQueryableMockDbSet(
                new Comment()
                {
                    Id = 1,
                    User = "First"
                },
                new Comment()
                {
                    Id = 2,
                    User = "Second"
                }));
            var commentRepo = new EntityFrameworkCommentRepository(_mockContext.Object);
            var comment = commentRepo.Get(2);
            Assert.AreEqual(comment.User, "Second");
        }

        [TestMethod]
        public void UnitOfWorkSaveViaContext()
        {
            var unitOfWork = new EntityFrameworkUnitOfWork(_mockContext.Object);

            unitOfWork.Save();

            _mockContext.Verify(m => m.SaveChanges(), Times.Once);
        }

        [TestMethod]
        public async Task UnitOfWorkSaveAsyncViaContext()
        {
            var unitOfWork = new EntityFrameworkUnitOfWork(_mockContext.Object);

            await unitOfWork.SaveAsync();

            _mockContext.Verify(m => m.SaveChangesAsync(), Times.Once);
        }


        private static DbSet<T> GetQueryableMockDbSet<T>(params T[] sourceList) where T : class
        {
            var queryable = sourceList.AsQueryable();

            var dbSet = new Mock<DbSet<T>>();
            dbSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
            dbSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
            dbSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            dbSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(() => queryable.GetEnumerator());

            return dbSet.Object;
        }

        private static DbSet<T> GetQueryableAsyncMockDbSet<T>(params T[] sourceList) where T : class
        {
            var queryable = sourceList.AsQueryable();

            var dbSet = new Mock<DbSet<T>>();
            dbSet.As<IDbAsyncEnumerable<T>>()
                .Setup(m => m.GetAsyncEnumerator())
                .Returns(new TestDbAsyncEnumerator<T>(queryable.GetEnumerator()));

            dbSet.As<IQueryable<T>>()
                .Setup(m => m.Provider)
                .Returns(new TestDbAsyncQueryProvider<T>(queryable.Provider));

            dbSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
            dbSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            dbSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(queryable.GetEnumerator());

            return dbSet.Object;
        }
    }
}

using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestApp.BLL.Infrastructure;
using TestApp.BLL.DTOModels;

namespace TestApp.BLL.Tests
{
    [TestClass]
    public class DtoModelsValidatorUnitTest
    {
        [TestMethod]
        public void PostDtoValidationTest()
        {
            var validator = new DtoModelsValidator();
            var emptyFieldsPost = new PostDto()
            {
                Title = null,
                Content = null
            };
            var shortContentPost = new PostDto()
            {
                Title = "Title",
                Author = "Author",
                Content = "123"
            };
            var validPost = new PostDto()
            {
                Title = "Title",
                Author = "Author",
                Content = "1234567890"
            };

            var nullPostErrors = validator.GetPostDtoValidationErrors(null);
            var emptyFieldsPostErrors = validator.GetPostDtoValidationErrors(emptyFieldsPost);
            var shortContentPostErrors = validator.GetPostDtoValidationErrors(shortContentPost);
            var validPostErrors = validator.GetPostDtoValidationErrors(validPost);

            Assert.AreEqual(nullPostErrors.Count, 3);
            Assert.AreEqual(emptyFieldsPostErrors.Count, 2);
            Assert.AreEqual(shortContentPostErrors.Count, 1);
            Assert.AreEqual(validPostErrors.Count, 0);
        }

        [TestMethod]
        public void CommentDtoValidationTest()
        {
            var validator = new DtoModelsValidator();
            var emptyFieldsComment = new CommentDto()
            {
                User = null,
                Text = null
            };
            var shortUsernameComment = new CommentDto()
            {
                User = "FU",
                Text = "Text"
            };
            var validComment = new CommentDto()
            {
                User = "User",
                Text = "Text"
            };

            var nullCommentErrors = validator.GetCommentDtoValidationErrors(null);
            var emptyFieldsCommentErrors = validator.GetCommentDtoValidationErrors(emptyFieldsComment);
            var shortUsernameCommentErrors = validator.GetCommentDtoValidationErrors(shortUsernameComment);
            var validCommentErrors = validator.GetCommentDtoValidationErrors(validComment);

            Assert.AreEqual(nullCommentErrors.Count, 3);
            Assert.AreEqual(emptyFieldsCommentErrors.Count, 2);
            Assert.AreEqual(shortUsernameCommentErrors.Count, 1);
            Assert.AreEqual(validCommentErrors.Count, 0);
        }
    }
}

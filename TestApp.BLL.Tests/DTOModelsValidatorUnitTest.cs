using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestApp.BLL.Infrastructure;
using TestApp.BLL.DTOModels;

namespace TestApp.BLL.Tests
{
    [TestClass]
    public class DTOModelsValidatorUnitTest
    {
        [TestMethod]
        public void PostDTOValidationTest()
        {
            var validator = new DTOModelsValidator();
            PostDTO nullPost = null;
            var emptyFieldsPost = new PostDTO()
            {
                Title = null,
                Content = null
            };
            var shortContentPost = new PostDTO()
            {
                Title = "Title",
                Author = "Author",
                Content = "123"
            };
            var validPost = new PostDTO()
            {
                Title = "Title",
                Author = "Author",
                Content = "1234567890"
            };

            var nullPostErrors = validator.GetPostDTOValidationErrors(nullPost);
            var emptyFieldsPostErrors = validator.GetPostDTOValidationErrors(emptyFieldsPost);
            var shortContentPostErrors = validator.GetPostDTOValidationErrors(shortContentPost);
            var validPostErrors = validator.GetPostDTOValidationErrors(validPost);

            Assert.AreEqual(nullPostErrors.Count, 3);
            Assert.AreEqual(emptyFieldsPostErrors.Count, 2);
            Assert.AreEqual(shortContentPostErrors.Count, 1);
            Assert.AreEqual(validPostErrors.Count, 0);
        }

        [TestMethod]
        public void CommentDTOValidationTest()
        {
            var validator = new DTOModelsValidator();
            CommentDTO nullComment = null;
            var emptyFieldsComment = new CommentDTO()
            {
                User = null,
                Text = null
            };
            var shortUsernameComment = new CommentDTO()
            {
                User = "FU",
                Text = "Text"
            };
            var validComment = new CommentDTO()
            {
                User = "User",
                Text = "Text"
            };

            var nullCommentErrors = validator.GetCommentDTOValidationErrors(nullComment);
            var emptyFieldsCommentErrors = validator.GetCommentDTOValidationErrors(emptyFieldsComment);
            var shortUsernameCommentErrors = validator.GetCommentDTOValidationErrors(shortUsernameComment);
            var validCommentErrors = validator.GetCommentDTOValidationErrors(validComment);

            Assert.AreEqual(nullCommentErrors.Count, 3);
            Assert.AreEqual(emptyFieldsCommentErrors.Count, 2);
            Assert.AreEqual(shortUsernameCommentErrors.Count, 1);
            Assert.AreEqual(validCommentErrors.Count, 0);
        }
    }
}

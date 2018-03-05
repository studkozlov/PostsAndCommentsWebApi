using System.Collections.Generic;
using TestApp.BLL.DTOModels;
using TestApp.BLL.Interfaces;

namespace TestApp.BLL.Infrastructure
{
    public class DTOModelsValidator : IDTOModelsValidator
    {
        public IDictionary<string, string> GetPostDTOValidationErrors(PostDTO post)
        {
            var validationErrors = new Dictionary<string, string>();
            if (post == null)
            {
                validationErrors.Add("", "There isn't object to add");
            }
            if (post?.Title == null)
            {
                validationErrors.Add("Title", "Title can't be empty");
            }
            if (post?.Content == null)
            {
                validationErrors.Add("Content", "Post content can't be empty");
            }
            if (post?.Content != null && (post.Content.Length < 10 || post.Content.Length > 2000))
            {
                validationErrors.Add("Content", "Post length must be between 10 and 2000");
            }
            return validationErrors;
        }
        public IDictionary<string, string> GetCommentDTOValidationErrors(CommentDTO comment)
        {
            var validationErrors = new Dictionary<string, string>();
            if (comment == null)
            {
                validationErrors.Add("", "There isn't object to add");
            }
            if (comment?.User == null)
            {
                validationErrors.Add("User", "User field can't be empty");
            }
            if (comment?.User != null && (comment.User.Length < 3 || comment.User.Length > 20))
            {
                validationErrors.Add("User", "Username length must be between 3 and 20 letters");
            }
            if (comment?.Text == null)
            {
                validationErrors.Add("Text", "Comment text can't be empty");
            }
            if (comment?.Text != null && (comment.Text.Length < 1 || comment.Text.Length > 500))
            {
                validationErrors.Add("Text", "Comment length must be between 1 and 500 letters");
            }

            return validationErrors;
        }
    }
}

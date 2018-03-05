using System.Collections.Generic;
using TestApp.BLL.DTOModels;

namespace TestApp.BLL.Interfaces
{
    public interface IDTOModelsValidator
    {
        IDictionary<string, string> GetPostDTOValidationErrors(PostDTO post);
        IDictionary<string, string> GetCommentDTOValidationErrors(CommentDTO comment);
    }
}

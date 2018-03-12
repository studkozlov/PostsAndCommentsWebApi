using System.Collections.Generic;
using TestApp.BLL.DTOModels;

namespace TestApp.BLL.Interfaces
{
    public interface IDTOModelsValidator
    {
        IList<(string, string)> GetPostDTOValidationErrors(PostDTO post);
        IList<(string, string)> GetCommentDTOValidationErrors(CommentDTO comment);
    }
}

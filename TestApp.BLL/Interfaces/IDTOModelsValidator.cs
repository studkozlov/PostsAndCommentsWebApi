using System.Collections.Generic;
using TestApp.BLL.DTOModels;

namespace TestApp.BLL.Interfaces
{
    public interface IDtoModelsValidator
    {
        IList<(string, string)> GetPostDtoValidationErrors(PostDto post);
        IList<(string, string)> GetCommentDtoValidationErrors(CommentDto comment);
    }
}

using System.Threading.Tasks;
using System.Web.Http;
using System.Net;
using TestApp.BLL.DTOModels;
using TestApp.BLL.Interfaces;
using TestApp.BLL.Infrastructure;

namespace TestApp.API.Controllers
{
    public class CommentsController : ApiController
    {
        private ITestAppService _appService;

        public CommentsController(ITestAppService service)
        {
            this._appService = service;
        }
        
        public IHttpActionResult Get(int id)
        {
            var comment = _appService.GetCommentById(id);
            if (comment == null)
            {
                return NotFound();
            }
            return Ok(comment);
        }

        [Route("api/posts/{postId}/comments")]
        [HttpGet]
        public async Task<IHttpActionResult> GetCommentsOfPost(int postId)
        {
            var comments = await _appService.GetCommentsOfPostAsync(postId);
            return Ok(comments);
        }
        
        public IHttpActionResult Post([FromBody]CommentDTO comment)
        {
            try
            {
                _appService.AddComment(comment);
            }
            catch (DataAccessException ex)
            {
                return InternalServerError(ex);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok();
        }
        
        public async Task<IHttpActionResult> Put(int id, [FromBody]CommentDTO comment)
        {
            try
            {
                await _appService.UpdateCommentAsync(comment);
            }
            catch (DataAccessException ex)
            {
                return InternalServerError(ex);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }

            return Content(HttpStatusCode.Accepted, comment);
        }
        
        public IHttpActionResult Delete(int id)
        {
            _appService.DeleteCommentById(id);
            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this._appService.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
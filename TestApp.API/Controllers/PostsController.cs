using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using TestApp.BLL.DTOModels;
using TestApp.BLL.Infrastructure;
using TestApp.BLL.Interfaces;

namespace TestApp.API.Controllers
{
    public class PostsController : ApiController
    {
        private readonly ITestAppService _appService;

        public PostsController(ITestAppService service)
        {
            _appService = service;
        }
        
        public async Task<IHttpActionResult> Get()
        {
            var posts = await _appService.GetAllPostsAsync();
            return Ok(posts);
        }
        
        public IHttpActionResult Get(int id)
        {
            var post = _appService.GetPostById(id);
            if (post == null)
            {
                return NotFound();
            }
            return Ok(post);
        }
        
        public IHttpActionResult Post([FromBody]PostDto post)
        {
            try
            {
                _appService.AddPost(post);
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
        
        public async Task<IHttpActionResult> Put(int id, [FromBody]PostDto post)
        {
            try
            {
                await _appService.UpdatePostAsync(post);
            }
            catch (DataAccessException ex)
            {
                return InternalServerError(ex);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }

            return Content(HttpStatusCode.Accepted, post);
        }
        
        public IHttpActionResult Delete(int id)
        {
            _appService.DeletePostById(id);
            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _appService.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
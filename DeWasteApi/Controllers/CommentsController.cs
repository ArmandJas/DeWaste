using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DeWasteApi.Data;
using DeWasteApi.Models;
using Autofac.Extras.DynamicProxy;

namespace DeWasteApi.Controllers
{
    
    [ApiController]
    [Route("[controller]")]
    public class CommentsController : Controller
    {
        private readonly DeWasteDbContext _context;

        
        public CommentsController(DeWasteDbContext context)
        {
            _context = context;
        }

        // GET: Comments
        [HttpGet] 
        public async virtual Task<IActionResult> Index()
        {
            using (_context)
            {
                return _context.comment != null ?
                            Json(await _context.comment.ToListAsync()) :
                            Problem("Entity set 'DeWasteDbContext.comment'  is null.");
            }
        }

        
        [HttpGet("{item_id}")]
        public async Task<IActionResult> Details(int? item_id)
        {
            if (item_id == null || _context.comment == null)
            {
                return NotFound();
            }

            var comment = await _context.comment.Where(m => m.item_id == item_id).ToListAsync();

            if (comment == null)
            {
                return NotFound();
            }

            return Json(comment);
        }

        // POST api/comments
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Comment comment)
        {
            if (comment == null)
            {
                return BadRequest();
            }

            _context.comment.Add(comment);
            await _context.SaveChangesAsync();

            return Ok(comment);
        }

        // DELETE api/comments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            Comment comment = await _context.comment.FirstOrDefaultAsync(x => x.id == id);
            if (comment == null)
            {
                return NotFound();
            }
            _context.comment.Remove(comment);
            await _context.SaveChangesAsync();
            return Ok(comment);
        }

        //update comment
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Comment comment)
        {
            if (comment == null)
            {
                return BadRequest();
            }
            if (!_context.comment.Any(x => x.id == comment.id))
            {
                return NotFound();
            }

            _context.Update(comment);
            await _context.SaveChangesAsync();
            return Ok(comment);
        }

        bool CommentExists(int id)
        {
            return _context.comment.Any(x => x.id == id);
        }
    }
}

using DeWasteApi.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DeWasteApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SimilarItemsController : Controller
    {

        private readonly DeWasteDbContext _context;

        public SimilarItemsController(DeWasteDbContext context)
        {
            _context = context;
        }

        // GET: SimilarItems
        [HttpGet]
        public async Task<IActionResult> SimilarItems()
        {
            return _context.item != null ?
                        Json(await _context.item.Select(x => new { x.id, x.name }).ToListAsync()) :
                        Problem("Entity set 'DeWasteDbContext.item'  is null.");
        }


        [HttpGet("{name}")]
        public async Task<IActionResult> SimilarItem(string? name)
        {
            if (name == null || _context.item == null)
            {
                return NotFound();
            }

            var items = await _context.item
                .Where(m => m.name.ToLower().Contains(name)).Select(x => new { x.id, x.name})
                .ToListAsync();
            if (items == null)
            {
                return NotFound();
            }

            return Json(items);
        }
    }
}

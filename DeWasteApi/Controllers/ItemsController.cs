using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DeWasteApi.Data;
using DeWasteApi.Models;

namespace DeWasteApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ItemsController : Controller
    {
        private readonly DeWasteDbContext _context;

        public ItemsController(DeWasteDbContext context)
        {
            _context = context;
        }

        // GET: Items
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return _context.item != null ?
                        Json(await _context.item.ToListAsync()) :
                        Problem("Entity set 'DeWasteDbContext.item'  is null.");
        }

        // GET: Items/Details/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null || _context.item == null)
            {
                return NotFound();
            }

            var item = await _context.item
                .FirstOrDefaultAsync(m => m.id == id);
            if (item == null)
            {
                return NotFound();
            }

            return Json(item);
        }
        
     
    }
}

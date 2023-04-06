using DeWasteApi.Data;
using DeWasteApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace DeWasteApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RatingsController : Controller
    {

        private readonly DeWasteDbContext _context;

        public RatingsController(DeWasteDbContext context)
        {
            _context = context;
        }


        [HttpGet]
        public IActionResult Index()
        {
            return _context.rating != null ?
                        Json(_context.rating) :
                        Problem("Entity set 'DeWasteDbContext.rating'  is null.");
        }

        //get rating by comment id and user id
        [HttpGet("{comment_id}/{user_id}")]
        public IActionResult Details(int? comment_id, string? user_id)
        {
            if (comment_id == null || user_id == null || _context.rating == null)
            {
                return NotFound();
            }

            var rating = _context.rating
                .FirstOrDefault(m => m.comment_id == comment_id && m.user_id.ToString() == user_id);

            return Json(rating);
        }

        //create new rating
        [HttpPost]
        public IActionResult Create([FromBody] Rating rating)
        {
            if (rating == null || _context.rating == null)
            {
                return NotFound();
            }

            _context.rating.Add(rating);
            _context.SaveChanges();

            return Json(rating);
        }

        //update rating
        [HttpPut]
        public IActionResult Update([FromBody] Rating rating)
        {
            if (rating == null || _context.rating == null)
            {
                return NotFound();
            }

            var ratingToUpdate = _context.rating.FirstOrDefault(x => x.comment_id == rating.comment_id && x.user_id == rating.user_id);
            if (ratingToUpdate == null)
            {
                return NotFound();
            }

            ratingToUpdate.is_liked = rating.is_liked;
            _context.SaveChanges();

            return Json(ratingToUpdate);
        }

        //delete rating
        [HttpDelete("{comment_id}/{user_id}")]
        public IActionResult Delete(int? comment_id, string? user_id)
        {
            if (comment_id == null || user_id == null || _context.rating == null)
            {
                return NotFound();
            }

            var rating = _context.rating.FirstOrDefault(x => x.comment_id == comment_id && x.user_id.ToString() == user_id);
            if (rating == null)
            {
                return NotFound();
            }

            _context.rating.Remove(rating);
            _context.SaveChanges();

            return Json(rating);
        }
    }
}

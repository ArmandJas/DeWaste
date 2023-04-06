using Microsoft.EntityFrameworkCore;

namespace DeWasteApi.Models
{
    public class Rating
    {
        public int id { get; set; }
        public int comment_id { get; set; }
        public Guid user_id { get; set; }
        public bool is_liked { get; set; }
    }
}

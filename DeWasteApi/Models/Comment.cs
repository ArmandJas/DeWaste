using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DeWasteApi.Models
{
    public class Comment
    {
        public int id { get; set; }
        public Guid user_id { get; set; }
        public int item_id { get; set; }
        public DateTime timestamp { get; set; }
        public string content { get; set; }
        public int likes { get; set; }
        public int dislikes { get; set; }
    }
}

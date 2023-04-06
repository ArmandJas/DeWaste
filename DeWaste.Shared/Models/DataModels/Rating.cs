using System;
using System.Collections.Generic;
using System.Text;

namespace DeWaste.Models.DataModels
{
    public class Rating
    {
        public int comment_id { get; set; }
        public Guid user_id { get; set; }
        public bool is_liked { get; set; }
    }
}

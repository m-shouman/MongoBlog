using System;
using System.Collections.Generic;

namespace WebApi.Models
{
    public class Post : BaseCollection
    {
        public string Title { get; set; }

        public DateTime CreatedOn { get; set; }

        public string AuthorId { get; set; }
        public User Author { get; set; }

        public string Body { get; set; }

        public List<Comment> Comments { get; set; }

        public Post()
        {
            Comments = new List<Comment>();
        }
    }
}

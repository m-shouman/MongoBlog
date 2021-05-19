using System;

namespace WebApi.Models
{
    public class Comment
    {
        public string Id { get; set; }
        public string AuthorId { get; set; }
        public User Author { get; set; }

        public DateTime CreatedOn { get; set; }
        public string Content { get; set; }
    }
}

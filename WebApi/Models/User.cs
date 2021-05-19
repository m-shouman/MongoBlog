using System;

namespace WebApi.Models
{
    public class User : BaseCollection
    {
        public string Username { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}

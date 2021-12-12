using System;
using System.ComponentModel.DataAnnotations;

namespace training_api.Domain
{
    public class Post
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}

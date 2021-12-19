using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace training_api.Domain
{
    public class Post
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string IdAuthor { get; set; }

        [ForeignKey(nameof(IdAuthor))]
        public IdentityUser Author { get; set; }
    }
}

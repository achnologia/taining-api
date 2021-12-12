using System;
using System.Collections.Generic;
using System.Linq;
using training_api.Domain;

namespace training_api.Services
{
    public class PostService : IPostService
    {
        private List<Post> _posts;
        public PostService()
        {
            _posts = new();

            for (var i = 0; i < 5; i++)
            {
                _posts.Add(new Post 
                { 
                    Id = Guid.NewGuid(), 
                    Name = $"Post-{i}" 
                });
            }
        }

        public IEnumerable<Post> GetAllAsync()
        {
            return _posts;
        }

        public Post GetByIdAsync(Guid id)
        {
            var post = _posts.SingleOrDefault(x => x.Id == id);

            return post;
        }

        public Guid CreateAsync(Post newPost)
        {
            _posts.Add(newPost);

            return newPost.Id;
        }

        public bool UpdateAsync(Post postToUpdate)
        {
            var exists = GetByIdAsync(postToUpdate.Id);

            if (exists == null)
                return false;

            var index = _posts.FindIndex(post => post.Id == postToUpdate.Id);
            _posts[index] = postToUpdate;

            return true;
        }

        public bool DeleteAsync(Guid id)
        {
            var exists = GetByIdAsync(id);

            if (exists == null)
                return false;

            _posts.Remove(exists);

            return true;
        }
    }
}

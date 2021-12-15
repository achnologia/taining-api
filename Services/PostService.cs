using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using training_api.Data;
using training_api.Domain;

namespace training_api.Services
{
    public class PostService : IPostService
    {
        private readonly DataContext _dataContext;

        public PostService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<IEnumerable<Post>> GetAllAsync()
        {
            return await _dataContext.Posts.ToListAsync();
        }

        public async Task<Post> GetByIdAsync(Guid idPost)
        {
            var post = await _dataContext.Posts.SingleOrDefaultAsync(x => x.Id == idPost);

            return post;
        }

        public async Task<Guid> CreateAsync(Post newPost)
        {
            await _dataContext.Posts.AddAsync(newPost);
            await _dataContext.SaveChangesAsync();

            return newPost.Id;
        }

        public async Task<bool> UpdateAsync(Post postToUpdate)
        {
            var post = await GetByIdAsync(postToUpdate.Id);

            if (post is null)
                return false;

            _dataContext.Posts.Update(postToUpdate);
            var updated = await _dataContext.SaveChangesAsync();

            return updated > 0;
        }

        public async Task<bool> DeleteAsync(Guid idPost)
        {
            var post = await GetByIdAsync(idPost);

            if (post is null)
                return false;

            _dataContext.Posts.Remove(post);
            var deleted = await _dataContext.SaveChangesAsync();

            return deleted > 0;
        }
    }
}

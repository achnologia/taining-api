using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using training_api.Contacts.Requests;
using training_api.Domain;

namespace training_api.Services
{
    public interface IPostService
    {
        Task<IEnumerable<Post>> GetAllAsync();
        Task<Post> GetByIdAsync(Guid idPost);
        Task<Guid> CreateAsync(Post newPost);
        Task<bool> UpdateAsync(Post postToUpdate);
        Task<bool> DeleteAsync(Guid idPost);
        Task<bool> IsUserPostAuthor(Guid idPost, string idUser);
    }
}

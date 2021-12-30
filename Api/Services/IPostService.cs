using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Contacts.Requests;
using Api.Domain;

namespace Api.Services
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

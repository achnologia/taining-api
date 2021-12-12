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
        IEnumerable<Post> GetAllAsync();
        Post GetByIdAsync(Guid id);
        Guid CreateAsync(Post newPost);
        bool UpdateAsync(Post postToUpdate);
        bool DeleteAsync(Guid id);
    }
}

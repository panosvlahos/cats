using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.Interfaces
{
    public interface ITagRepository
    {
        Task<List<Tag>> GetAllTagsAsync();
        Task AddTagAsync(Tag tag);
        Task SaveAsync();

    }
}

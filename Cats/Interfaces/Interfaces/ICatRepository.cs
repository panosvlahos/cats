using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.Interfaces
{
    public interface ICatRepository
    {
        Task<List<string>> GetExistingCatIdsAsync();
        Task AddCatAsync(Cat cat);
        Task SaveAsync();
    }
}

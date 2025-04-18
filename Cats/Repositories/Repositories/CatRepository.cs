using Entities.Models;
using Interfaces.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Repositories.Repositories
{
    public class CatRepository : ICatRepository
    {
        private readonly CatsContext _context;

        public CatRepository(CatsContext context)
        {
            _context = context;
        }

        public async Task<List<string>> GetExistingCatIdsAsync()
        {
            return await _context.Cat.Select(c => c.CatId).ToListAsync();
        }

        public async Task AddCatAsync(Cat cat)
        {
            await _context.Cat.AddAsync(cat);
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}

using Entities.Models;
using Interfaces.Interfaces;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Infrastructure.Repositories
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

        public async Task<Cat?> GetCatByIdAsync(string id)
        {
            return await _context.Cat
                .Include(c => c.Tags)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.CatId == id);
        }

        public async Task<List<Cat>> GetCatsByTagAsync(string? tag)
        {
            var query = _context.Cat.Include(c => c.Tags).AsQueryable();

            if (!string.IsNullOrEmpty(tag))
            {
                query = query.Where(c => c.Tags.Any(t => t.Name == tag));
            }

            return await query
                .OrderByDescending(c => c.Id)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<Cat>> GetCatsAsync(int page, int pageSize)
        {
            var query = _context.Cat.Include(c => c.Tags).AsQueryable();

            return await query
                .OrderByDescending(c => c.Created)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}

using Entities.Models;
using Interfaces.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Persistence.Context;
namespace Infrastructure.Repositories
{
    public class TagRepository : ITagRepository
    {
        private readonly CatsContext _context;

        public TagRepository(CatsContext context)
        {
            _context = context;
        }

        public async Task<List<Tag>> GetAllTagsAsync()
        {
            return await _context.Tag.ToListAsync();
        }

        public async Task AddTagAsync(Tag tag)
        {
            await _context.Tag.AddAsync(tag);
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}

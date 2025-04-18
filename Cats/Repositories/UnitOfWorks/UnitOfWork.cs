using Entities.Models;
using Interfaces.Interfaces;
using Persistence.Context;
using Repositories.Repositories;

namespace UnitOfWorks.UnitOfWorks
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CatsContext _context;

        public UnitOfWork(CatsContext context)
        {
            _context = context;
            CatRepository = new CatRepository(context);
            TagRepository = new TagRepository(context);
        }

        public ICatRepository CatRepository { get; }
        public ITagRepository TagRepository { get; }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}

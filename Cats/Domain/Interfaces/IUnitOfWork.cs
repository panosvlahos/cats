using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.Interfaces
{
    public interface IUnitOfWork
    {
        ICatRepository CatRepository { get; }
        ITagRepository TagRepository { get; }
        Task SaveChangesAsync();
    }
}

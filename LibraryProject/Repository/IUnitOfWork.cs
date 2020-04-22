using System.Threading.Tasks;

namespace LibraryProject.Repository
{
    public interface IUnitOfWork
    {
        Task CompleteAsync();
    }
}

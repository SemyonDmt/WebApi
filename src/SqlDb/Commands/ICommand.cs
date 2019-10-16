using System.Data.Common;
using System.Threading.Tasks;

namespace SqlDb.Commands
{
    public interface ICommand
    {
        Task ExecuteAsync(DbCommand command);
    }
}
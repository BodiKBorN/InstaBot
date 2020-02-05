using System.Threading.Tasks;

namespace InstaBot.BLL.Services.Abstract
{
    public interface ISubscribersService
    {
        Task<int> RemoveSubscribersAsync(int count);
    }
}
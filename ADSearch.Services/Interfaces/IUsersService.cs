using ADSearch.Domain.Items;
using ADSearch.Services.Interfaces.Base;
using System.Threading.Tasks;

namespace ADSearch.Services.Interfaces
{
    /// <summary>
    /// Author: Can DOĞU
    /// </summary>
    public interface IUsersService<T> : IServiceBase
    {
        Task<T> SearchAsync(RequestItem requestDto);
        Task<T> SearchGroupsAsync(RequestItem requestDto);
    }
}

using ADSearch.Domain.Dto;
using ADSearch.Services.Interfaces.Base;
using System.Threading.Tasks;

namespace ADSearch.Services.Interfaces
{
    public interface IUsersService<T> : IServiceBase
    {
        Task<T> SearchAsync(RequestDto requestDto);
    }
}

using ADSearch.Common.Helpers;
using ADSearch.Domain.Constants;
using ADSearch.Domain.Items;
using ADSearch.Services.Interfaces;
using System.Threading.Tasks;

namespace ADSearch.Services.Services
{
    /// <summary>
    /// Author: Can DOĞU
    /// </summary>
    public class UsersService : IUsersService<ResultItem>
    {
        public async Task<ResultItem> SearchAsync(RequestItem requestDto)
        {
            if (requestDto == null) return new ResultItem(false, null, Consts.SearchDtoNullMessage);

            return await ADHelper.Instance.GetUserList(requestDto);
        }
    }
}

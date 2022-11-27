using ADSearch.Common.Helpers;
using ADSearch.Domain.Constants;
using ADSearch.Domain.Dto;
using ADSearch.Domain.Items;
using ADSearch.Services.Interfaces;
using System.Threading.Tasks;

namespace ADSearch.Services.Services
{
    public class UsersService : IUsersService<ResultItem>
    {
        public async Task<ResultItem> SearchAsync(RequestDto requestDto)
        {
            if (requestDto == null) return new ResultItem(false, null, Consts.SearchDtoNullMessage);

            var dynamicList = await ADHelper.Instance.GetUserList(requestDto);

            if (dynamicList == null) return new ResultItem(false, null, Consts.ResultNullMessage);

            return new ResultItem(true, dynamicList);
        }
    }
}

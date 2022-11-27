using ADSearch.Domain.Dto;
using ADSearch.Domain.Items;
using ADSearch.Services.Interfaces;
using ADSearch.Services.Services;
using System.Threading.Tasks;
using System.Web.Http;

namespace ADSearch.Web.API.Controllers
{
    public class UsersController : ApiController
    {
        // variables
        private readonly IUsersService<ResultItem> usersService;

        public UsersController()
        {
            usersService = new UsersService();
        }

        [HttpPost]
        public async Task<ResultItem> SearchAsync(RequestDto request)
        {
            var result = await usersService.SearchAsync(request);

            return result;
        }
    }
}

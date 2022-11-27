using ADSearch.Common.Extensions;
using ADSearch.Common.Generic;
using ADSearch.Domain.Dto;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace ADSearch.Common.Helpers
{
    /// <summary>
    /// Author: Can DOĞU
    /// </summary>
    public class ADHelper : Singleton<ADHelper>
    {
        public async Task<List<dynamic>> GetUserList(RequestDto requestDto)
        {
            List<dynamic> userList = new List<dynamic>();

            try
            {
                if (requestDto.Properties.IsNullOrEmpty() || requestDto.LdapServer.IsNullOrEmpty()) return await Task.FromResult<dynamic>(null);

                using (var domainEntry = (requestDto.UserName.IsNotNullOrEmpty()) ? new DirectoryEntry($"LDAP://{requestDto.LdapServer}", requestDto.UserName, requestDto.Password) : new DirectoryEntry($"LDAP://{requestDto.LdapServer}"))
                using (var directorySearcher = domainEntry.GetSearcher(requestDto.Properties, requestDto.Filter))
                {
                    var results = directorySearcher.FindAll().OfType<SearchResult>().Skip(requestDto.Skip).Take(requestDto.Take);

                    if (results == null) return await Task.FromResult(userList);

                    foreach (SearchResult result in results)
                    {
                        var myObject = await CreateObject(requestDto.Properties, result);

                        if (myObject == null) continue;

                        userList.Add(myObject);
                    }
                }
            }
            catch
            { }

            return await Task.FromResult(userList);
        }

        private async Task<dynamic> CreateObject(string properties, SearchResult result)
        {
            try
            {
                dynamic obj = new ExpandoObject();

                foreach (var prop in properties.Split(';'))
                {
                    ((IDictionary<string, object>)obj)[prop] = (result.Properties.Contains(prop) && result.Properties[prop].Count > 0) ? result.Properties[prop][0].ToString() : string.Empty;
                }

                return await Task.FromResult(obj);
            }
            catch
            { }

            return await Task.FromResult<dynamic>(null);
        }
    }
}

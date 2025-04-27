using ADSearch.Common.Extensions;
using ADSearch.Common.Generic;
using ADSearch.Domain.Constants;
using ADSearch.Domain.Items;
using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Dynamic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace ADSearch.Common.Helpers
{
    /// <summary>
    /// Author: Can DOĞU
    /// </summary>
    public class ADHelper : Singleton<ADHelper>
    {
        public async Task<ResultItem> GetUserList(RequestItem requestDto)
        {
            try
            {
                if (requestDto.Properties.IsNullOrEmpty() || requestDto.LdapServer.IsNullOrEmpty()) return ResultItem.GenerateError(Consts.SearchDtoNullMessage);

                using (var domainEntry = (requestDto.UserName.IsNotNullOrEmpty())
                    ? new DirectoryEntry($"LDAP://{requestDto.LdapServer}", requestDto.UserName, requestDto.Password)
                    : new DirectoryEntry($"LDAP://{requestDto.LdapServer}"))
                using (var directorySearcher = domainEntry.GetSearcher(requestDto.Properties, requestDto.Filter))
                {
                    var results = await Task.Run(() => directorySearcher.FindAll().OfType<SearchResult>().Skip(requestDto.Skip).Take(requestDto.Take));

                    if (!results.Any()) return ResultItem.GenerateError(Consts.ResultNullMessage);

                    var userList = new List<dynamic>();

                    foreach (SearchResult result in results)
                    {
                        var myObject = CreateObject(requestDto.Properties, result);

                        if (myObject == null) continue;

                        userList.Add(myObject);
                    }

                    return ResultItem.GenerateSuccess(userList);
                }
            }
            catch (Exception ex)
            {
                return ResultItem.GenerateError($"{ex.Message}");
            }
        }

        public async Task<ResultItem> GetUserListWithGroups(RequestItem requestDto)
        {
            if (requestDto.Properties.IsNullOrEmpty() || requestDto.GroupProperties.IsNullOrEmpty() || requestDto.LdapServer.IsNullOrEmpty()) return ResultItem.GenerateError(Consts.SearchDtoNullMessage);

            try
            {
                using (var domainEntry = (requestDto.UserName.IsNotNullOrEmpty())
                    ? new DirectoryEntry($"LDAP://{requestDto.LdapServer}", requestDto.UserName, requestDto.Password)
                    : new DirectoryEntry($"LDAP://{requestDto.LdapServer}"))
                using (var directorySearcher = domainEntry.GetSearcher(requestDto.Properties, requestDto.Filter))
                {
                    var results = await Task.Run(() => directorySearcher.FindAll().OfType<SearchResult>().Skip(requestDto.Skip).Take(requestDto.Take));

                    if (!results.Any()) return ResultItem.GenerateError(Consts.ResultNullMessage);

                    var userList = new List<dynamic>();

                    foreach (SearchResult result in results)
                    {
                        var myObject = CreateObject(requestDto.Properties, result);

                        if (myObject == null) continue;

                        var groupList = new List<dynamic>();

                        var nGroupsEntry = result.GetDirectoryEntry();

                        nGroupsEntry.RefreshCache(new string[] { "tokenGroups" });

                        foreach (byte[] sid in nGroupsEntry.Properties["tokenGroups"])
                        {
                            var groupSID = new SecurityIdentifier(sid, 0).ToString();

                            var groupAdResult = await FindBySidAsync(domainEntry, groupSID, requestDto);

                            if (groupAdResult == null) continue;

                            groupList.Add(groupAdResult);
                        }

                        myObject.Groups = groupList;

                        userList.Add(myObject);
                    }

                    return ResultItem.GenerateSuccess(userList);
                }
            }
            catch (Exception ex)
            {
                return ResultItem.GenerateError($"{ex.Message}");
            }
        }

        private async Task<dynamic> FindBySidAsync(DirectoryEntry domainEntry, string sid, RequestItem requestDto)
        {
            using (var dSearcher = domainEntry.GetSearcher(requestDto.GroupProperties, $"(objectSid={BuildSidFilter(sid)})", true))
            {
                var gResult = await Task.Run(() => dSearcher.FindOne());

                if (gResult == null) return null;

                return CreateObject(requestDto.GroupProperties, gResult);
            }
        }

        private string BuildSidFilter(string sid)
        {
            var sidBytes = new SecurityIdentifier(sid).GetBinaryForm();
            var sb = new StringBuilder();

            foreach (byte b in sidBytes)
            {
                sb.AppendFormat("\\{0:X2}", b);
            }

            return sb.ToString();
        }

        private dynamic CreateObject(string properties, SearchResult result)
        {
            dynamic obj = new ExpandoObject();
            var dict = (IDictionary<string, object>)obj;

            foreach (var prop in properties.Split(';'))
            {
                if (prop.IsNullOrEmpty()) continue;

                if (result.Properties.Contains(prop) && result.Properties[prop].Count > 0)
                {
                    var rawValue = result.Properties[prop][0];
                    dict[prop] = ConvertAdValue(prop, rawValue);
                }
                else
                {
                    dict[prop] = string.Empty;
                }
            }

            return obj;
        }

        private dynamic ConvertAdValue(string propertyName, object value)
        {
            if (value == null)
                return null;

            if (value is byte[] bytes)
            {
                if (propertyName.Equals("objectSid", StringComparison.OrdinalIgnoreCase))
                    return new SecurityIdentifier(bytes, 0).Value;

                if (propertyName.Equals("objectGuid", StringComparison.OrdinalIgnoreCase))
                    return new Guid(bytes).ToString();

                return BitConverter.ToString(bytes).Replace("-", "");
            }

            if (value.GetType().Name == "__ComObject")
            {
                try
                {
                    var type = value.GetType();
                    var highPart = (int)type.InvokeMember("HighPart", System.Reflection.BindingFlags.GetProperty, null, value, null);
                    var lowPart = (int)type.InvokeMember("LowPart", System.Reflection.BindingFlags.GetProperty, null, value, null);

                    var fileTime = ((long)highPart << 32) + (uint)lowPart;
                    if (fileTime <= 0) return null;

                    return DateTime.FromFileTimeUtc(fileTime);
                }
                catch
                {
                    return value.ToString();
                }
            }

            return value;
        }
    }
}

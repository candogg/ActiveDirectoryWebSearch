using System.DirectoryServices;

namespace ADSearch.Common.Extensions
{
    /// <summary>
    /// Author: Can DOĞU
    /// </summary>
    public static class DirectoryEntryExtensions
    {
        public static DirectorySearcher GetSearcher(this DirectoryEntry entry, string properties, string filter, bool isGroup = false)
        {
            var searcher = new DirectorySearcher(entry)
            {
                SearchScope = SearchScope.Subtree,
                Filter = isGroup ? "(&(objectCategory=group)(objectClass=group)" : "(&(objectCategory=person)(objectClass=user)",
                PageSize = int.MaxValue,
                Sort = new SortOption("cn", SortDirection.Ascending)
            };

            try
            {
                foreach (var property in filter.Split(';'))
                {
                    if (property.Trim().IsNullOrEmpty()) continue;

                    searcher.Filter = $"{searcher.Filter}({property})";
                }
            }
            catch
            { }

            searcher.Filter = $"{searcher.Filter})";

            try
            {
                foreach (var property in properties.Split(';'))
                {
                    if (property.Trim().IsNullOrEmpty()) continue;

                    searcher.PropertiesToLoad.Add(property);
                }
            }
            catch
            { }

            return searcher;
        }
    }
}

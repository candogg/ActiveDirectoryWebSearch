using System.Security.Principal;

namespace ADSearch.Common.Extensions
{
    /// <summary>
    /// Author: Can DOĞU
    /// </summary>
    public static class SecurityIdentifierExtensions
    {
        public static byte[] GetBinaryForm(this SecurityIdentifier sid)
        {
            var bytes = new byte[sid.BinaryLength];
            sid.GetBinaryForm(bytes, 0);

            return bytes;
        }
    }
}

using ADSearch.Domain.Dto.Base;
using System.ComponentModel.DataAnnotations;

namespace ADSearch.Domain.Dto
{
    public class RequestDto : DtoBase
    {
        [Required]
        [MaxLength(500)]
        public string LdapServer { get; set; }

        [Required]
        [MaxLength(500)]
        public string Properties { get; set; }

        [MaxLength(500)]
        public string Filter { get; set; }

        public int Skip { get; set; }

        public int Take { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public RequestDto()
        {
            Filter = string.Empty;
            Skip = 0;
            Take = int.MaxValue;
            UserName = string.Empty;
            Password = string.Empty;
        }
    }
}

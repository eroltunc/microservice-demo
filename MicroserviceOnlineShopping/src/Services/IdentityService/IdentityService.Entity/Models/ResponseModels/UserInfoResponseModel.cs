using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityService.Entity.Models.ResponseModels
{
    public class UserInfoResponseModel
    {
        public required string Email { get; set; }
        public required bool IsEmailConfirmed { get; set; }
        public string? FullName { get; set; }
    }
}

using System.Runtime.Serialization;
namespace IdentityService.Entity.Models.ResponseModels
{
    public class TokenResponseModel
    {
        [DataMemberAttribute(Name = "access_token")]
        public string AccessToken { get; set; }
        [DataMemberAttribute(Name = "expires_in")]
        public DateTime ExpiresIn { get; set; }
        [DataMemberAttribute(Name = "refresh_token")]
        public string RefreshToken { get; set; }
        [DataMemberAttribute(Name = "token_type")]
        public string TokenType { get; set; }
    }
}

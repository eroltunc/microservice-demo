using Microsoft.AspNetCore.Identity;
namespace IdentityService.Entity.Concrete
{
    public class ApplicationUser:IdentityUser
    {
        public string? Description { get; set; }
        public string? ProfilePictureUrl { get; set; } 
        public string FullName { get; set; }
        public string? CompanyName { get; set; }
        public string Status { get; set; }
        public DateTime LastSeen { get; set; }
    }
   
}

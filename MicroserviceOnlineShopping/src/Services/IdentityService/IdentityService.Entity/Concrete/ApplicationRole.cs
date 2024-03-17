using Microsoft.AspNetCore.Identity;

namespace IdentityService.Entity.Concrete
{
    public class ApplicationRole : IdentityRole
    {
        public string Description { get; set; }
    }
}

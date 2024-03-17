using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasketService.Business.Services.Abstract
{
    public interface IIdentityService
    {
        string GetUserName();
        string GetUserId();
        string GetUserEmail();
    }
}

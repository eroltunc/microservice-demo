using EventBus.Base.Events;

namespace IdentityService.Business.IntegrationEvents.Events
{
    public class UserLoggedIntegrationEvent:IntegrationEvent
    {
        public UserLoggedIntegrationEvent(string email, string fullName)
        {
            Email = email;
            FullName = fullName;
        }

        public string Email { get; private set; }
        public string FullName { get; private set; }
    }
}

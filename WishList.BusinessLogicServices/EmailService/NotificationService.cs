using System.Threading.Tasks;
using WishList.BusinessLogic.Interfaces;

namespace WishList.BusinessLogicServices.EmailService
{
    public class NotificationService : INotificationService
    {
        private readonly IEmailClient emailClient;
        public NotificationService(IEmailClient emailClient)
        {
            this.emailClient = emailClient;
        }
        public async Task RegistrationSuccessful(string userEmail, string Name)
        {
            await emailClient.SendAsync(userEmail, $"Dear {Name}, Welcome to WishList Web API", "Registration");
        }
    }
}

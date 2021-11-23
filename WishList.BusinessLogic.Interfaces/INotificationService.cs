using System.Threading.Tasks;

namespace WishList.BusinessLogic.Interfaces
{
    public interface INotificationService
    {
        Task RegistrationSuccessful(string userEmail, string name);
    }
}

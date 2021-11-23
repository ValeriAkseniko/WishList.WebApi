using System.Threading.Tasks;

namespace WishList.BusinessLogic.Interfaces
{
    public interface IEmailClient
    {
        Task SendAsync(string email, string message, string subject);
    }
}

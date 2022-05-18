using Microsoft.AspNetCore.Identity.UI.Services;
using System.Threading.Tasks;

namespace ReelJunkies.Services.Interfaces
{
    public interface IAppEmailSender : IEmailSender
    {
        Task SendContactEmailAsync(string emailFrom,
                                   string name,
                                   string subject,
                                   string htmlMessageBody);
    }
}

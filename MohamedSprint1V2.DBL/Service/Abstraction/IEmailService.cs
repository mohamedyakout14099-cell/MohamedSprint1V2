using System.Threading.Tasks;

namespace MohamedSprint1V2.DLL.Service.Abstraction
{
    public interface IEmailService
    {
        /// <summary>
        /// Sends a plain or HTML email to the specified recipient.
        /// </summary>
        Task<bool> SendEmailAsync(string toEmail, string toName, string subject, string htmlBody);

        /// <summary>
        /// Sends an order confirmation email after a successful checkout.
        /// </summary>
        Task<bool> SendOrderConfirmationEmailAsync(string toEmail, string toName, int orderId, decimal total, string city, string address);

        /// <summary>
        /// Sends a shipping update email when order status changes to Shipped.
        /// </summary>
        Task<bool> SendOrderShippedEmailAsync(string toEmail, string toName, int orderId, string? carrier, string? trackingNumber);

        /// <summary>
        /// Sends a welcome email on new account registration.
        /// </summary>
        Task<bool> SendWelcomeEmailAsync(string toEmail, string toName);
    }
}

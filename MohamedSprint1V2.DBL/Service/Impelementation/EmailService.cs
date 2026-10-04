using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using MohamedSprint1V2.DLL.ModelVM.Email;
using MohamedSprint1V2.DLL.Service.Abstraction;
using System;
using System.Threading.Tasks;

namespace MohamedSprint1V2.DLL.Service.Impelementation
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _settings;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IOptions<EmailSettings> settings, ILogger<EmailService> logger)
        {
            _settings = settings.Value;
            _logger = logger;
        }

        // ─── Core Send Method ────────────────────────────────────────────────────
        public async Task<bool> SendEmailAsync(string toEmail, string toName, string subject, string htmlBody)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(_settings.DisplayName, _settings.Email));
                message.To.Add(new MailboxAddress(toName, toEmail));
                message.Subject = subject;

                message.Body = new TextPart(MimeKit.Text.TextFormat.Html)
                {
                    Text = htmlBody
                };

                using var client = new SmtpClient();
                await client.ConnectAsync(_settings.Host, _settings.Port, SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(_settings.Email, _settings.Password);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[EmailService] Failed to send email to {ToEmail} | Subject: {Subject}", toEmail, subject);
                return false;
            }
        }

        // ─── Order Confirmation Email ─────────────────────────────────────────────
        public async Task<bool> SendOrderConfirmationEmailAsync(
            string toEmail, string toName, int orderId, decimal total, string city, string address)
        {
            var subject = $"✅ Order Confirmed - #{orderId} | Mohamed Store";
            var body = $@"
<!DOCTYPE html>
<html>
<head>
  <meta charset='utf-8'>
  <style>
    body {{ font-family: 'Segoe UI', Arial, sans-serif; background: #f5f5f5; margin: 0; padding: 0; }}
    .container {{ max-width: 600px; margin: 30px auto; background: #ffffff; border-radius: 12px; overflow: hidden; box-shadow: 0 4px 20px rgba(0,0,0,0.08); }}
    .header {{ background: linear-gradient(135deg, #0d6efd, #0a58ca); padding: 40px 30px; text-align: center; }}
    .header h1 {{ color: #ffffff; margin: 0; font-size: 26px; }}
    .header p {{ color: rgba(255,255,255,0.85); margin: 8px 0 0; font-size: 14px; }}
    .success-icon {{ font-size: 48px; margin-bottom: 10px; }}
    .body {{ padding: 35px 40px; }}
    .greeting {{ font-size: 18px; font-weight: 600; color: #1a1a2e; margin-bottom: 12px; }}
    .message {{ color: #555; line-height: 1.7; margin-bottom: 25px; font-size: 15px; }}
    .order-card {{ background: #f8f9ff; border: 1px solid #dee3ff; border-radius: 10px; padding: 20px 25px; margin-bottom: 25px; }}
    .order-card h3 {{ margin: 0 0 15px; font-size: 15px; color: #0d6efd; text-transform: uppercase; letter-spacing: 0.5px; }}
    .detail-row {{ display: flex; justify-content: space-between; padding: 8px 0; border-bottom: 1px solid #eef0ff; font-size: 14px; }}
    .detail-row:last-child {{ border-bottom: none; font-weight: 700; font-size: 16px; color: #0d6efd; }}
    .detail-label {{ color: #777; }}
    .detail-value {{ color: #1a1a2e; font-weight: 500; }}
    .cta-btn {{ display: inline-block; background: linear-gradient(135deg, #0d6efd, #0a58ca); color: #ffffff !important; text-decoration: none; padding: 14px 35px; border-radius: 30px; font-size: 15px; font-weight: 600; margin: 5px 0; }}
    .footer {{ background: #f0f2ff; padding: 25px 40px; text-align: center; }}
    .footer p {{ color: #888; font-size: 13px; margin: 4px 0; }}
    .footer strong {{ color: #0d6efd; }}
  </style>
</head>
<body>
  <div class='container'>
    <div class='header'>
      <div class='success-icon'>🎉</div>
      <h1>Order Confirmed!</h1>
      <p>Your order has been placed and is being processed.</p>
    </div>
    <div class='body'>
      <div class='greeting'>Hi {toName},</div>
      <p class='message'>
        Thank you for shopping with us! We've successfully received your order and our team is already preparing it for you.
        You'll receive another update once your order is shipped.
      </p>

      <div class='order-card'>
        <h3>📦 Order Summary</h3>
        <div class='detail-row'>
          <span class='detail-label'>Order Number</span>
          <span class='detail-value'>#{orderId}</span>
        </div>
        <div class='detail-row'>
          <span class='detail-label'>Delivery City</span>
          <span class='detail-value'>{city}</span>
        </div>
        <div class='detail-row'>
          <span class='detail-label'>Delivery Address</span>
          <span class='detail-value'>{address}</span>
        </div>
        <div class='detail-row'>
          <span class='detail-label'>Payment Method</span>
          <span class='detail-value'>Cash On Delivery</span>
        </div>
        <div class='detail-row'>
          <span class='detail-label'>Total Amount</span>
          <span class='detail-value'>${total:N2}</span>
        </div>
      </div>

      <div style='text-align:center; margin: 20px 0;'>
        <a href='#' class='cta-btn'>View My Orders</a>
      </div>
    </div>
    <div class='footer'>
      <p>Thank you for choosing <strong>Mohamed Store</strong> 💙</p>
      <p>Need help? Reply to this email anytime.</p>
      <p style='margin-top:10px; font-size:11px; color:#aaa;'>© 2026 Mohamed Store. All rights reserved.</p>
    </div>
  </div>
</body>
</html>";

            return await SendEmailAsync(toEmail, toName, subject, body);
        }

        // ─── Order Shipped Email ──────────────────────────────────────────────────
        public async Task<bool> SendOrderShippedEmailAsync(
            string toEmail, string toName, int orderId, string? carrier, string? trackingNumber)
        {
            var subject = $"🚚 Your Order #{orderId} Has Been Shipped! | Mohamed Store";
            var carrierInfo = string.IsNullOrEmpty(carrier) ? "Our delivery partner" : carrier;
            var trackingInfo = string.IsNullOrEmpty(trackingNumber)
                ? "<p style='color:#777;font-size:14px;'>Tracking number will be sent shortly.</p>"
                : $"<div style='background:#e8f5e9;border:1px solid #a5d6a7;border-radius:8px;padding:12px 18px;margin:15px 0;font-size:15px;'><strong>Tracking Number:</strong> <code style='background:#fff;padding:3px 8px;border-radius:4px;color:#2e7d32;font-weight:700;'>{trackingNumber}</code></div>";

            var body = $@"
<!DOCTYPE html>
<html>
<head>
  <meta charset='utf-8'>
  <style>
    body {{ font-family: 'Segoe UI', Arial, sans-serif; background: #f5f5f5; margin: 0; padding: 0; }}
    .container {{ max-width: 600px; margin: 30px auto; background: #ffffff; border-radius: 12px; overflow: hidden; box-shadow: 0 4px 20px rgba(0,0,0,0.08); }}
    .header {{ background: linear-gradient(135deg, #198754, #146c43); padding: 40px 30px; text-align: center; }}
    .header h1 {{ color: #ffffff; margin: 0; font-size: 26px; }}
    .header p {{ color: rgba(255,255,255,0.85); margin: 8px 0 0; font-size: 14px; }}
    .body {{ padding: 35px 40px; }}
    .greeting {{ font-size: 18px; font-weight: 600; color: #1a1a2e; margin-bottom: 12px; }}
    .message {{ color: #555; line-height: 1.7; margin-bottom: 25px; font-size: 15px; }}
    .footer {{ background: #f0fff4; padding: 25px 40px; text-align: center; }}
    .footer p {{ color: #888; font-size: 13px; margin: 4px 0; }}
    .footer strong {{ color: #198754; }}
  </style>
</head>
<body>
  <div class='container'>
    <div class='header'>
      <div style='font-size:48px;margin-bottom:10px;'>🚚</div>
      <h1>Your Order is On the Way!</h1>
      <p>Order #{orderId} has been handed to the delivery partner.</p>
    </div>
    <div class='body'>
      <div class='greeting'>Hi {toName},</div>
      <p class='message'>
        Great news! Your order <strong>#{orderId}</strong> has been shipped by <strong>{carrierInfo}</strong> 
        and is on its way to you. Expect your delivery soon!
      </p>
      {trackingInfo}
      <p style='color:#555;font-size:14px;'>
        If you have any questions about your delivery, please don't hesitate to contact us.
      </p>
    </div>
    <div class='footer'>
      <p>Thank you for choosing <strong>Mohamed Store</strong> 💚</p>
      <p style='margin-top:10px; font-size:11px; color:#aaa;'>© 2026 Mohamed Store. All rights reserved.</p>
    </div>
  </div>
</body>
</html>";

            return await SendEmailAsync(toEmail, toName, subject, body);
        }

        // ─── Welcome Email ────────────────────────────────────────────────────────
        public async Task<bool> SendWelcomeEmailAsync(string toEmail, string toName)
        {
            var subject = "🎉 Welcome to Mohamed Store!";
            var body = $@"
<!DOCTYPE html>
<html>
<head>
  <meta charset='utf-8'>
  <style>
    body {{ font-family: 'Segoe UI', Arial, sans-serif; background: #f5f5f5; margin: 0; padding: 0; }}
    .container {{ max-width: 600px; margin: 30px auto; background: #ffffff; border-radius: 12px; overflow: hidden; box-shadow: 0 4px 20px rgba(0,0,0,0.08); }}
    .header {{ background: linear-gradient(135deg, #6f42c1, #59359a); padding: 40px 30px; text-align: center; }}
    .header h1 {{ color: #ffffff; margin: 0; font-size: 26px; }}
    .header p {{ color: rgba(255,255,255,0.85); margin: 8px 0 0; font-size: 14px; }}
    .body {{ padding: 35px 40px; }}
    .greeting {{ font-size: 18px; font-weight: 600; color: #1a1a2e; margin-bottom: 12px; }}
    .message {{ color: #555; line-height: 1.7; margin-bottom: 25px; font-size: 15px; }}
    .features {{ display: flex; gap: 10px; margin: 20px 0; }}
    .feature {{ flex: 1; background: #f8f4ff; border-radius: 10px; padding: 18px; text-align: center; }}
    .feature .icon {{ font-size: 28px; margin-bottom: 8px; }}
    .feature p {{ margin: 0; font-size: 13px; color: #555; }}
    .cta-btn {{ display: inline-block; background: linear-gradient(135deg, #6f42c1, #59359a); color: #ffffff !important; text-decoration: none; padding: 14px 35px; border-radius: 30px; font-size: 15px; font-weight: 600; }}
    .footer {{ background: #f9f4ff; padding: 25px 40px; text-align: center; }}
    .footer p {{ color: #888; font-size: 13px; margin: 4px 0; }}
    .footer strong {{ color: #6f42c1; }}
  </style>
</head>
<body>
  <div class='container'>
    <div class='header'>
      <div style='font-size:48px;margin-bottom:10px;'>👋</div>
      <h1>Welcome Aboard!</h1>
      <p>We're thrilled to have you with us.</p>
    </div>
    <div class='body'>
      <div class='greeting'>Hi {toName},</div>
      <p class='message'>
        Welcome to <strong>Mohamed Store</strong>! Your account has been successfully created.
        You can now browse our catalog, add items to your cart, and enjoy a seamless shopping experience.
      </p>

      <table width='100%' cellpadding='0' cellspacing='10'>
        <tr>
          <td width='33%' style='background:#f8f4ff;border-radius:10px;padding:18px;text-align:center;vertical-align:top;'>
            <div style='font-size:28px;margin-bottom:8px;'>🛍️</div>
            <p style='margin:0;font-size:13px;color:#555;'>Wide selection of quality products</p>
          </td>
          <td width='33%' style='background:#f8f4ff;border-radius:10px;padding:18px;text-align:center;vertical-align:top;'>
            <div style='font-size:28px;margin-bottom:8px;'>🚀</div>
            <p style='margin:0;font-size:13px;color:#555;'>Fast & secure checkout experience</p>
          </td>
          <td width='33%' style='background:#f8f4ff;border-radius:10px;padding:18px;text-align:center;vertical-align:top;'>
            <div style='font-size:28px;margin-bottom:8px;'>💬</div>
            <p style='margin:0;font-size:13px;color:#555;'>Friendly support at all times</p>
          </td>
        </tr>
      </table>

      <div style='text-align:center;margin-top:30px;'>
        <a href='#' class='cta-btn'>Start Shopping Now</a>
      </div>
    </div>
    <div class='footer'>
      <p>Thank you for joining <strong>Mohamed Store</strong> 💜</p>
      <p style='margin-top:10px;font-size:11px;color:#aaa;'>© 2026 Mohamed Store. All rights reserved.</p>
    </div>
  </div>
</body>
</html>";

            return await SendEmailAsync(toEmail, toName, subject, body);
        }
    }
}

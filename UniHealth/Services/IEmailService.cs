using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Text;
using UniHealth.Models;
using MailKit.Security;

namespace UniHealth.Services
{
    public interface IEmailService
    {
        Task SendPasswordResetCodeAsync(User user, string resetCode);
    }

    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendPasswordResetCodeAsync(User user, string resetCode)
        {
            var emailSettings = _configuration.GetSection("EmailSettings");

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(
                emailSettings["SenderName"],
                emailSettings["SenderEmail"]));
            message.To.Add(new MailboxAddress($"{user.FName} {user.LName}", user.Email));
            message.Subject = "كود التحقق لإعادة تعيين كلمة المرور - UniHealth";

            message.Body = new TextPart(TextFormat.Html)
            {
                Text = GetEmailBody(user, resetCode, emailSettings["SenderEmail"])
            };

            using var client = new SmtpClient();

            try
            {
                // الاتصال باستخدام STARTTLS (الطريقة الموصى بها)
                await client.ConnectAsync(
                    emailSettings["SmtpServer"],
                    int.Parse(emailSettings["SmtpPort"]),
                    SecureSocketOptions.StartTls);

                await client.AuthenticateAsync(
                    emailSettings["SenderEmail"],
                    emailSettings["SenderPassword"]);

                await client.SendAsync(message);
            }
            finally
            {
                await client.DisconnectAsync(true);
            }
        }

        private string GetEmailBody(User user, string resetCode, string supportEmail)
        {
            return $@"<!DOCTYPE html>
            <html dir='rtl' lang='ar'>
            <head>
                <meta charset='UTF-8'>
                <title>كود التحقق</title>
                <style>
                    body {{ font-family: Arial, sans-serif; background-color: #F9F9F9; }}
                    .container {{ max-width: 600px; margin: 0 auto; background: white; border-radius: 10px; }}
                    .header {{ padding: 20px; text-align: center; }}
                    .content {{ padding: 30px; }}
                    .code {{ 
                        background: #0057FF; 
                        color: white; 
                        font-size: 24px; 
                        font-weight: bold; 
                        padding: 15px; 
                        text-align: center; 
                        border-radius: 5px;
                        margin: 20px 0;
                    }}
                    .footer {{ 
                        background: #F2EFF3; 
                        padding: 15px; 
                        text-align: center; 
                        font-size: 12px; 
                        border-radius: 5px;
                    }}
                </style>
            </head>
            <body>
                <div class='container'>
                    <div class='header'>
                        <h1 style='color: #0057FF;'>UniHealth</h1>
                    </div>
                    <div class='content'>
                        <h2>ازيك! {user.FName} {user.LName},</h2>
                        <p>لقد تلقينا طلباً لإعادة تعيين كلمة المرور الخاصة بحسابك في نظام UniHealth.</p>
                        <p>رمز التحقق الخاص بك هو:</p>
                        <div class='code'>{resetCode}</div>
                        <p> الرمز صالح لمدة 15 دقيقة فقط.</p>
                        <p>إذا لم تطلب إعادة تعيين كلمة المرور، يرجى تجاهل هذه الرسالة.</p>
                    </div>
                    <div class='footer'>
                        <p>© {DateTime.Now.Year} UniHealth. جميع الحقوق محفوظة.</p>
                        <p>إذا كنت بحاجة إلى مساعدة، يرجى التواصل عبر {supportEmail}</p>
                    </div>
                </div>
            </body>
            </html>";
        }
    }
}
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Email {
    public class EmailSender {
        private readonly IConfiguration _config;




        public EmailSender(IConfiguration config) {
            _config = config;
        }





        public async Task SendEmailAsync(string toEmail, string subject, string body) {
            var smtpClient = new SmtpClient(_config["Smtp:Host"]) {
                Port = int.Parse(_config["Smtp:Port"]),
                Credentials = new NetworkCredential(
                    _config["Smtp:Username"],
                    _config["Smtp:Password"]
                ),
                EnableSsl = true
            };

            var mail = new MailMessage {
                From = new MailAddress(_config["Smtp:From"]),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            mail.To.Add(toEmail);

            await smtpClient.SendMailAsync(mail);
        }









        //public Task SendResetPasswordEmail(string email, string resetLink) {
        //    throw new NotImplementedException();
        //}


        public Task SendResetPasswordEmail(string email, string resetLink) {
            var subject = "Reset Your Password";
            var body = $@"
                    <a href='{resetLink}'>Reset Password</a>";

            return SendEmailAsync(email, subject, body);
        }







    }

}

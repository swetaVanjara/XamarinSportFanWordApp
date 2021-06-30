using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
namespace Fanword.Data.IdentityConfig.Services {
    public class EmailService  : IIdentityMessageService {
        public SmtpClient Client => new SmtpClient();

        public Task SendAsync(IdentityMessage message) {
            var mailMessage = new MailMessage(ConfigurationManager.AppSettings["SenderEmailAddress"],
                message.Destination) {
                    IsBodyHtml = true,
                    Subject = message.Subject,
                    Body = message.Body,
                };
            return Client.SendMailAsync(mailMessage);
        }

        public Task SendAsync(List<string> emails, IdentityMessage message) {
            var mailMessage = new MailMessage() {
                IsBodyHtml = true,
                Subject = message.Subject,
                Body = message.Body,
            };
            foreach (var email in emails) {
                mailMessage.To.Add(email);
            }
            return Client.SendMailAsync(mailMessage);
        }
    }
}

using FlightManager.EmailService;
using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Threading.Tasks;

namespace EmailService
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailConfiguration emailConfiguration;

        public EmailSender(EmailConfiguration emailConfiguration)
        {
            this.emailConfiguration = emailConfiguration;
        }

        public async Task SendEmailAsync(Message message)
        {
            await SendAsync(CreateEmailMessage(message));
        }

        private MimeMessage CreateEmailMessage(Message message)
        {
            MimeMessage emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(emailConfiguration.From));
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Text) { Text = message.Content };

            return emailMessage;
        }

        private async Task SendAsync(MimeMessage mailMessage)
        {
            SmtpClient client = new SmtpClient();

            try
            {
                client.CheckCertificateRevocation = false;
                await client.ConnectAsync(emailConfiguration.SmtpServer, emailConfiguration.Port, true);
                client.AuthenticationMechanisms.Remove("XOAUTH2");
                await client.AuthenticateAsync(emailConfiguration.Username, emailConfiguration.Password);

                await client.SendAsync(mailMessage);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                await client.DisconnectAsync(true);
                client.Dispose();
            }
        }
    }
}

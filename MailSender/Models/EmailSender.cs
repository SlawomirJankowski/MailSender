using Cipher;
using EmailSender.Extensions;
using MailSender.Models.Domains;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;

namespace MailSender
{
    public class EmailSender
    {
        private SmtpClient _smtp;
        private MailMessage _mail;

        private string _hostSmtp;
        private bool _enableSsl;
        private int _port;
        private string _senderEmail;
        private string _senderEmailPassword;
        private string _senderName;

        private static StringCipher _cipher = new StringCipher();

        public EmailSender(UserEmailAccountParams userEmailAccountParams)
        {
            _hostSmtp = userEmailAccountParams.HostSmtp;
            _enableSsl = userEmailAccountParams.EnableSsl;
            _port = Convert.ToInt32(userEmailAccountParams.Port);
            _senderEmail = userEmailAccountParams.SenderEmail;
            _senderEmailPassword = _cipher.Decrypt(userEmailAccountParams.SenderEmailPassword);
            _senderName = userEmailAccountParams.SenderName;
        }

        public async Task Send(EmailMessage emailMessage)
        {
            _mail = new MailMessage();
            _mail.From = new MailAddress(_senderEmail, _senderName);
            _mail.To.Add(new MailAddress(emailMessage.ToEmail));
            if (!string.IsNullOrEmpty(emailMessage.CCEmail))
            {
                _mail.CC.Add(new MailAddress(emailMessage.CCEmail));
            }
            _mail.IsBodyHtml = true;
            _mail.Subject = emailMessage.Subject;
            _mail.BodyEncoding = Encoding.UTF8;
            _mail.SubjectEncoding = Encoding.UTF8;

            _mail.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(emailMessage.Body.StripHTML(), null, MediaTypeNames.Text.Plain));

            _mail.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(emailMessage.Body, null, MediaTypeNames.Text.Html));

            if (emailMessage.AttachmentsFileNames != null && emailMessage.AttachmentsFileNames.Any())
            {
                foreach (var fileName in emailMessage.GetAttachmentFilesList())
                {
                    var filePath = emailMessage.GetAttachmentsDirectoryPath() + fileName;

                    _mail.Attachments.Add(new Attachment(filePath));
                }
            }

            _smtp = new SmtpClient
            {
                Host = _hostSmtp,
                EnableSsl = _enableSsl,
                Port = _port,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_senderEmail, _senderEmailPassword)
            };

            _smtp.SendCompleted += OnSendCompleted;

            await _smtp.SendMailAsync(_mail);
        }

        private void OnSendCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            //LOG + message for user
            _smtp.Dispose();
            _mail.Dispose();
        }
    }
}

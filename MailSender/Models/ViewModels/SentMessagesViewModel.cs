using MailSender.Models.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MailSender.Models.ViewModels
{
    public class SentMessagesViewModel
    {
        public IEnumerable<EmailMessage> EmailMessages { get; set; }
        public List<UserEmailAccountParams> UserEmailAccountParams { get; set; }
    }
}
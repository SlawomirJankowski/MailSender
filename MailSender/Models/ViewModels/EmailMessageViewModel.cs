using MailSender.Extensions;
using MailSender.Models.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MailSender.Models.ViewModels
{
    public class EmailMessageViewModel
    {
        public EmailMessage EmailMessage { get; set; }
        public List<UserEmailAccountParams> UserEmailAccountsParams { get; set; }
        public FileUploader FileUploader { get; set; }

    }
}
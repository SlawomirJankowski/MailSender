using MailSender.Models.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MailSender.Models.ViewModels
{
    public class EmailAccountViewModel
    {
        public UserEmailAccountParams UserEmailAccountParams { get; set; }
        public bool IsReadonly { get; set; }
        public string Heading { get; set; }
        //public List<EmailMessage> EmailMessages { get; set; }
    }
}
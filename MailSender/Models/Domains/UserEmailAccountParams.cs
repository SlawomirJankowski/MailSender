using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MailSender.Models.Domains
{
    public class UserEmailAccountParams
    {
        public UserEmailAccountParams()
        {
            EmailMessages = new Collection<EmailMessage>();
        }


        public int Id { get; set; }

        [Required]
        [DisplayName("Sender:")]
        public string SenderName { get; set; }

        [Required]
        [DisplayName("From:")]
        public string SenderEmail { get; set; }

        [Required]
        public string SenderEmailPassword { get; set; }

        [Required]
        public string Port { get; set; }

        [Required]
        public string HostSmtp { get; set; }

        [Required]
        public bool EnableSsl { get; set; }

        [Required]
        [ForeignKey("User")]
        public string UserId { get; set; }


        public ICollection<EmailMessage> EmailMessages { get; set; }
        public ApplicationUser User { get; set; }
            
    }
}
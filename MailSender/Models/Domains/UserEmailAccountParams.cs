using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MailSender.Models.Domains
{
    public class UserEmailAccountParams
    {
        
        public int Id { get; set; }

        [Required]
        [DisplayName("Sender Name:")]
        public string SenderName { get; set; }

        [Required]
        [DisplayName("E-mail:")]
        public string SenderEmail { get; set; }

        [Required]
        [DisplayName("Password:")]
        public string SenderEmailPassword { get; set; }

        [Required]
        [DisplayName("Port:")]
        public string Port { get; set; }

        [Required]
        [DisplayName("Host SMTP:")]
        public string HostSmtp { get; set; }

        [Required]
        [DisplayName("SSL:")]
        public bool EnableSsl { get; set; }

        [Required]
        [ForeignKey("User")]
        public string UserId { get; set; }


        public ApplicationUser User { get; set; }
            
    }
}
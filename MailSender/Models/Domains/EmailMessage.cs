using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MailSender.Models.Domains
{
    public class EmailMessage
    {

        public int Id { get; set; }

        public DateTime SentDate { get; set; }

        [Required]
        [DisplayName("To:")]
        public string ToEmail { get; set; }

        [DisplayName("Sender E-mail:")]
        public int UserEmailAccountParamsId { get; set; }

        [DisplayName("CC:")]
        public string CCEmail { get; set; }

        [Required]
        [DisplayName("Subject:")]
        public string Subject { get; set; }

        [DisplayName("Message content:")]
        public string Body { get; set; }

        // add attachment 


        [Required]
        [ForeignKey("User")]
        public string UserId { get; set; }

        public ApplicationUser User { get; set; }
        public UserEmailAccountParams UserEmailAccountParams { get; set; }



    }
}
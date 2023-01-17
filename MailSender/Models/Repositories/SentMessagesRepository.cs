using MailSender.Models.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;
using System.Data.Entity.Validation;

namespace MailSender.Models.Repositories
{
    public class SentMessagesRepository
    {
        public List<EmailMessage> GetSentMessages(string userId)
        {
            using (var context = new ApplicationDbContext())
            {
                return context.SentMessages
                    .Where(x => x.UserId == userId)
                    .ToList();
            }
        }

        public EmailMessage GetSentMessage(int id, string userId)
        {
            using (var context = new ApplicationDbContext())
            {
                return context.SentMessages
                    .Include(x => x.UserEmailAccountParams)
                    .Single(x => x.Id == id && x.UserId == userId);
            }
        }

        public void Add(EmailMessage message)
        {
            using (var context = new ApplicationDbContext())
            {
                try
                {
                    context.SentMessages.Add(message);
                    context.SaveChanges();
                }
                catch (Exception e)
                {

                    throw e;
                }

            }
        }

        public void Delete(int id, string userId)
        {
            throw new NotImplementedException();
        }
    }
}
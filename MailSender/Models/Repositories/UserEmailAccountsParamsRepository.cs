using MailSender.Models.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MailSender.Models.Repositories
{
    public class UserEmailAccountsParamsRepository
    {
        public List<UserEmailAccountParams> GetEmailAccounts(string userId)
        {
            using (var context = new ApplicationDbContext())
            {
                return context.UserEmailAccounts
                    .Where(a => a.UserId == userId)
                    .ToList();
            }
        }

        public UserEmailAccountParams GetAccountParams(int userEmailAccountParamsId, string userId)
        {
            using (var context = new ApplicationDbContext())
            {
                return context.UserEmailAccounts
                    .Single(x => x.Id == userEmailAccountParamsId && x.UserId == userId);
            }
        }

        public void Add(UserEmailAccountParams userEmailAccountParams)
        {
            using (var context = new ApplicationDbContext())
            {
                context.UserEmailAccounts.Add(userEmailAccountParams);
                context.SaveChanges();
            }
        }

        public void Update(UserEmailAccountParams userEmailAccountParams)
        {
            using (var context = new ApplicationDbContext())
            {
                var userEmailAccountParamsToEdit = context.UserEmailAccounts
                    .Single(x => x.Id == userEmailAccountParams.Id && x.UserId == userEmailAccountParams.UserId);

                userEmailAccountParamsToEdit.SenderName = userEmailAccountParams.SenderName;
                userEmailAccountParamsToEdit.SenderEmail = userEmailAccountParams.SenderEmail;
                userEmailAccountParamsToEdit.SenderEmailPassword = userEmailAccountParams.SenderEmailPassword;
                userEmailAccountParamsToEdit.Port = userEmailAccountParams.Port;
                userEmailAccountParamsToEdit.HostSmtp = userEmailAccountParams.HostSmtp;
                userEmailAccountParamsToEdit.EnableSsl = userEmailAccountParams.EnableSsl;

                context.SaveChanges();
            }
        }

        public void Delete(int id, string userId)
        {
            using (var context = new ApplicationDbContext())
            {
                var userEmailAccountParamsToDelete = context.UserEmailAccounts
                    .Single(x => x.Id == id && x.UserId == userId);

                context.UserEmailAccounts.Remove(userEmailAccountParamsToDelete);
                context.SaveChanges();
            }
        }
    }
}
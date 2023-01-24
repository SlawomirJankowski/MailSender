using MailSender.Models.Domains;
using MailSender.Models.Repositories;
using MailSender.Models.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MailSender.Controllers
{
    public class EmailAccountsController : Controller
    {
        private UserEmailAccountsParamsRepository _userEmailAccountsParamsRepository =
            new UserEmailAccountsParamsRepository();

        private string _key = "569CAE02-1A80-4C32-95B6-E9B8AC0CD3EF";

        [Authorize]
        // GET: EmailAccounts
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var userEmailAccounts = _userEmailAccountsParamsRepository.GetEmailAccounts(userId);

            return View(userEmailAccounts);
        }

        public ActionResult EmailAccount (int id = 0, bool isReadonly = false)
        {
            EmailAccountViewModel vm; 

            if (id != 0 && !isReadonly)
            {
                vm = new EmailAccountViewModel //edit
                {
                    UserEmailAccountParams = new UserEmailAccountParams
                    {
                        Id = id,
                        SenderEmail = "asd@wer.com",
                        SenderName = "Marian",
                        SenderEmailPassword = "EDIT Password",
                        Port = "324",
                        HostSmtp = "eee.fer.er",
                        EnableSsl = true

                    },
                    IsReadonly = isReadonly,
                    Heading = "Edit e-mail account"

                };
            }
            else if (id != 0 && isReadonly)
            {
                vm = new EmailAccountViewModel //view
                {
                    UserEmailAccountParams = new UserEmailAccountParams
                    {
                        Id = id,
                        SenderEmail = "asd@wer.com",
                        SenderName = "Marian",
                        SenderEmailPassword = "VIEW READONLY TRUE",
                        Port = "324",
                        HostSmtp = "eee.fer.er",
                        EnableSsl = true

                    },
                    IsReadonly = isReadonly,
                    Heading = "View e-mail account"

                };
            }
            else
            {
                vm = new EmailAccountViewModel //add
                {
                    UserEmailAccountParams = new UserEmailAccountParams
                    {
                        Id = id
                    },
                    IsReadonly = isReadonly,
                    Heading = "Add a new e-mail account"
                };
            }


            return View(vm);
        }


        [HttpPost]
        public ActionResult EmailAccount(UserEmailAccountParams userEmailAccountParams)
        {
            return View();
        }
    }
}
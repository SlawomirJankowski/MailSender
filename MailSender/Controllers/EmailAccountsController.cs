using MailSender.Models.Repositories;
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

        // GET: EmailAccounts
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var userEmailAccounts = _userEmailAccountsParamsRepository.GetEmailAccounts(userId);

            return View(userEmailAccounts);
        }
    }
}
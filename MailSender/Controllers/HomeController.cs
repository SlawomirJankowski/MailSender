using MailSender.Models;
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
    [Authorize]
    public class HomeController : Controller
    {
        private SentMessagesRepository _sentMessagesRepository = new SentMessagesRepository();
        private UserEmailAccountsParamsRepository _userEmailAccountsParamsRepository =
            new UserEmailAccountsParamsRepository();

        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();

            var sentMessages = _sentMessagesRepository.GetSentMessages(userId);

            return View(sentMessages);
        }

        public ActionResult ViewMessage(int id)
        {
            var userId = User.Identity.GetUserId();
            var message = _sentMessagesRepository.GetSentMessage(id, userId);

            var vm = PrepareEmailMessageVM(message, userId);

            return View(vm);
        }

        public ActionResult NewEmail()
        {
            var userId = User.Identity.GetUserId();
            var message = new EmailMessage { UserId = userId };
            
            var vm = PrepareEmailMessageVM(message, userId);

            return View(vm);
        }

        private EmailMessageViewModel PrepareEmailMessageVM(EmailMessage message, string userId)
        {
            return new EmailMessageViewModel
            {
                EmailMessage = message,
                UserEmailAccountsParams = _userEmailAccountsParamsRepository.GetEmailAccounts(userId)
            };
        }


        [HttpPost]
        public ActionResult NewEmail(EmailMessage message)
        {
            var userId = User.Identity.GetUserId();
            message.UserId = userId; 
            message.SentDate = DateTime.Now;
            //message.ToEmail = "xyz@com.com";
            // message.Subject = "Subject_xyz";
            //message.Body = "Email content";
            //message.UserEmailAccountId = 1;

            if (!ModelState.IsValid)
            {
                var vm = PrepareEmailMessageVM(message, userId);
                return View("NewEmail", vm);
            }

            _sentMessagesRepository.Add(message); 

            //send e-mail 

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult DeleteMessage(int id)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                _sentMessagesRepository.Delete(id, userId);

            }
            catch (Exception exc)
            {
                //logowanie do pliku
                return Json(new {Success = false, Message= exc.Message});
            }
            return Json(new { Success = true });
        }



        [AllowAnonymous]
        public ActionResult About()
        {

            return View();
        }

        [AllowAnonymous]
        public ActionResult Contact()
        {
            return View();
        }
    }
}
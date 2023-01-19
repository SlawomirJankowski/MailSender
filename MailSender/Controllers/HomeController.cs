using MailSender.Models;
using MailSender.Models.Domains;
using MailSender.Models.Repositories;
using MailSender.Models.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Ganss.Xss;
using System.Web.Services.Description;


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
            var emailMessage = _sentMessagesRepository.GetSentMessage(id, userId);


            var vm = PrepareEmailMessageVM(emailMessage, userId);

            return View(vm);
        }

        private string HtmlSanitizer(string dirtyHtml)
        {
            var sanitizer = new HtmlSanitizer();
            var cleanHtml = sanitizer.Sanitize(dirtyHtml);
            return cleanHtml;
        }


        public ActionResult NewEmail()
        {
            var userId = User.Identity.GetUserId();
            var emailMessage = new EmailMessage { UserId = userId };

            var vm = PrepareEmailMessageVM(emailMessage, userId);

            return View(vm);
        }

        private EmailMessageViewModel PrepareEmailMessageVM(EmailMessage emailMessage, string userId)
        {
            return new EmailMessageViewModel
            {
                EmailMessage = emailMessage,
                UserEmailAccountsParams = _userEmailAccountsParamsRepository.GetEmailAccounts(userId)
            };
        }


        [HttpPost]
        public ActionResult NewEmail(EmailMessage emailMessage)
        {
            var userId = User.Identity.GetUserId();
            emailMessage.UserId = userId;
            emailMessage.SentDate = DateTime.Now;

            //to prevent XXS attacks
            emailMessage.Body = WebUtility.HtmlDecode(emailMessage.Body);
            emailMessage.Body = HtmlSanitizer(emailMessage.Body);

            if (!ModelState.IsValid)
            {
                var vm = PrepareEmailMessageVM(emailMessage, userId);
                return View("NewEmail", vm);
            }



            //send e-mail 



            _sentMessagesRepository.Add(emailMessage);

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
                return Json(new { Success = false, Message = exc.Message });
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
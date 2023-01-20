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

            //to prevent XSS attacks
            emailMessage.Body = WebUtility.HtmlDecode(emailMessage.Body);
            emailMessage.Body = HtmlSanitizer(emailMessage.Body);

            if (!ModelState.IsValid)
            {
                var vm = PrepareEmailMessageVM(emailMessage, userId);
                return View("NewEmail", vm);
            }

            var userEmailAccountParams = _userEmailAccountsParamsRepository.GetAccountParams(emailMessage.UserEmailAccountParamsId, userId);

            try
            {
                var emailSender = new EmailSender(userEmailAccountParams);
                emailSender.Send(emailMessage);
                ViewBag.Result = "E-mail succesfully sent !";
            }
            catch (Exception ex)
            {
                //logowanie
                ViewBag.Result = ex.Message;
            }
                             

            _sentMessagesRepository.Add(emailMessage);

            return RedirectToAction("Index");
        }



        private string HtmlSanitizer(string dirtyHtml)
        {
            var sanitizer = new HtmlSanitizer();
            var cleanHtml = sanitizer.Sanitize(dirtyHtml);
            return cleanHtml;
        }


        [HttpPost] //from main window
        public ActionResult Delete(int id)
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
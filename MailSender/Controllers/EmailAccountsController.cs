using Cipher;
using MailSender.Models.Domains;
using MailSender.Models.Repositories;
using MailSender.Models.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Web.Mvc;

namespace MailSender.Controllers
{
    [Authorize]
    public class EmailAccountsController : Controller
    {
        private UserEmailAccountsParamsRepository _userEmailAccountsParamsRepository =
            new UserEmailAccountsParamsRepository();
        private SentMessagesRepository _sentMessagesRepository = new SentMessagesRepository();

        private static StringCipher _cipher = new StringCipher();


        // GET: EmailAccounts
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var userEmailAccounts = _userEmailAccountsParamsRepository.GetEmailAccounts(userId);

            return View(userEmailAccounts);
        }

        // GET: Edit, View, Add
        public ActionResult EmailAccount(int id = 0, bool isReadonly = false)
        {
            var userId = User.Identity.GetUserId();
            EmailAccountViewModel vm = id != 0 ? 
                PrepareEditViewEmailAccountVM(id, isReadonly, userId) 
                : PrepareBasicEmailAccountVM(new UserEmailAccountParams {UserId = userId}, isReadonly);

            return View(vm);
        }

        private EmailAccountViewModel PrepareEditViewEmailAccountVM (int id, bool isReadonly, string userId)
        {
            var userEmailAccountParams = _userEmailAccountsParamsRepository.GetAccountParams(id, userId);
            userEmailAccountParams.SenderEmailPassword = _cipher.Decrypt(userEmailAccountParams.SenderEmailPassword);

            var vm = PrepareBasicEmailAccountVM(userEmailAccountParams, isReadonly);
            return vm;
        }

        private EmailAccountViewModel PrepareBasicEmailAccountVM(UserEmailAccountParams userEmailAccountParams, bool isReadonly)
        {
            string heading;

            if (userEmailAccountParams.Id != 0)
            {
                if (!isReadonly)
                    heading = "Edit e-mail account details";
                else
                    heading = "View e-mail account details";
            }
            else
                heading = "Add a new e-mail account";

            return new EmailAccountViewModel
            {
                UserEmailAccountParams = userEmailAccountParams,
                IsReadonly = isReadonly,
                Heading = heading
            };
        }

        // POST: Add + Edit
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult EmailAccount(UserEmailAccountParams userEmailAccountParams)
        {
            var userId = User.Identity.GetUserId();
            userEmailAccountParams.UserId = userId;

            
            if (!ModelState.IsValid)
            {
                var vm = PrepareBasicEmailAccountVM(userEmailAccountParams, false);
                return View("EmailAccount", vm);
            }
           

            userEmailAccountParams.SenderEmailPassword = _cipher.Encrypt( userEmailAccountParams.SenderEmailPassword);
            
            if (userEmailAccountParams.Id == 0)
                _userEmailAccountsParamsRepository.Add(userEmailAccountParams);
            else
                _userEmailAccountsParamsRepository.Update(userEmailAccountParams);

            return RedirectToAction("Index", "EmailAccounts");
        }


        // POST: Delete
        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var emailMessage = _userEmailAccountsParamsRepository.GetAccountParams(id, userId);

                _userEmailAccountsParamsRepository.Delete(id, userId);

            }
            catch (Exception exc)
            {
                //log to file
                return Json(new { Success = false, Message = exc.Message });
            }
            return Json(new { Success = true });
        }

    }
}
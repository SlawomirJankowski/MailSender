using Cipher;
using Ganss.Xss;
using MailSender.Extensions;
using MailSender.Models.Domains;
using MailSender.Models.Repositories;
using MailSender.Models.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
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
            var vm = new SentMessagesViewModel
            {
                EmailMessages = _sentMessagesRepository.GetSentMessages(userId),
                UserEmailAccountParams = _userEmailAccountsParamsRepository.GetEmailAccounts(userId)
            };

            return View(vm);
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
        [ValidateAntiForgeryToken()]
        public async Task<ActionResult> NewEmail(EmailMessage emailMessage, FileUploader fileUploader)
        {
            var userId = User.Identity.GetUserId();
            emailMessage.UserId = userId;
            emailMessage.SentDate = DateTime.Now;

            //upload attachments
            if (fileUploader.PostedFiles[0] != null)
            {
                var uploadedFilesList = fileUploader.GetFilesNamesAndUpload();
                emailMessage.AttachmentsDirectoryPath = fileUploader.Path;
                emailMessage.AttachmentsFileNames = emailMessage.ConvertListOfAttachedFilesTostring(uploadedFilesList);
            }

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
                await emailSender.Send(emailMessage);
                ViewBag.Result = $"E-mail succesfully sent !";
            }
            catch (Exception ex)
            {
                //Log to file
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


        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var emailMessage = _sentMessagesRepository.GetSentMessage(id, userId);

                //delete folder with attachments
                DeleteAttachments(emailMessage);

                _sentMessagesRepository.Delete(id, userId);

            }
            catch (Exception exc)
            {
                //log to file
                return Json(new { Success = false, Message = exc.Message });
            }
            return Json(new { Success = true });
        }

        public void DeleteAttachments(EmailMessage emailMessage)
        {
            if (emailMessage.AttachmentsDirectoryPath != null)
            {
                var directoryPath = emailMessage.GetAttachmentsDirectoryPath();
                var listOfAttachments = emailMessage.GetAttachmentFilesList();

                foreach (var fileName in listOfAttachments)
                {
                    var fileToDelete = directoryPath + fileName;
                    if (System.IO.File.Exists(fileToDelete))
                        System.IO.File.Delete(fileToDelete);
                }
                if (Directory.Exists(directoryPath))
                    Directory.Delete(directoryPath);
            }
        }

        public FileResult Download(string fileName, string directoryPath)
        {
            var filePath = Path.Combine(directoryPath, fileName);
            var mimeType = MimeMapping.GetMimeMapping(fileName);

            return File(filePath, mimeType, fileName);
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
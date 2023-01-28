using MailSender.Models.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace MailSender.Extensions
{
    public static class EmailMessageExtensions
    {
        public static string GetAttachmentsDirectoryPath(this EmailMessage emailMessage)
        {
            return HostingEnvironment.MapPath(emailMessage.AttachmentsDirectoryPath);
        }

        public static string[] GetAttachmentFilesList(this EmailMessage emailMessage)
        {
            return emailMessage.AttachmentsFileNames.Split(new string[] { "|||" }, StringSplitOptions.None);
        }

        public static string ConvertListOfAttachedFilesTostring(this EmailMessage emailMessage, List<string> uploadedFilesList)
        {
            return emailMessage.AttachmentsFileNames = string.Join("|||", uploadedFilesList);
        }

    }
}
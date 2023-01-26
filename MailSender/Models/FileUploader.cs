using AngleSharp.Text;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;

namespace MailSender.Extensions
{
    public class FileUploader
    {
        private string _path = $"~/UploadedFiles/{Guid.NewGuid()}/";

        public string Path { get; set; }

        [DisplayName("Attachments:")]
        public List<HttpPostedFileBase> PostedFiles { get; set; }


        public FileUploader()
        {
            Path = _path;
        }


        public List<string> GetFilesNamesAndUpload()
        {

            List<string> fileNames = new List<string>();
            Directory.CreateDirectory(HostingEnvironment.MapPath(Path));

            foreach (var file in PostedFiles)
            {
                var fileName = System.IO.Path.GetFileName(file.FileName);
                fileNames.Add(fileName);

                var filePath = HostingEnvironment.MapPath(Path + file.FileName);
                file.SaveAs(filePath);
            }
            return fileNames;

        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Album.Entities;
using System.Web.Http.Results;

namespace Album.WebUI.Images
{
    public class ImageUploadController : ApiController
    {
        [HttpPost]
        public void UploadFile()
        {
            if (HttpContext.Current.Request.Files.AllKeys.Any())
            {
                // Get the uploaded image from the Files collection
                HttpFileCollection files = HttpContext.Current.Request.Files;
                if(files != null)
                {
                    for(int i = 0; i < files.Count; i++)
                    {
                        HttpPostedFile file = files[i];
                        byte[] imageContent = new byte[file.ContentLength];
                        Image img = new Image { Content = imageContent, ImageType = file.ContentType };
                    }
                }

            }
        }
    }
}
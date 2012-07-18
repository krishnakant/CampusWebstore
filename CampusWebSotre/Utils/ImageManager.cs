using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;

namespace CampusWebStore.Utils
{
    public class ImageManager
    {
        protected static int thumbWidth = 150;
        protected static int thumbHeight = 150;

        public static void CreateThumbnail(HttpPostedFileBase image, string folder) {
            var thumbnail = new WebImage(image.InputStream);
            System.Diagnostics.Debug.WriteLine(thumbnail.FileName);

            var fullPath = Path.Combine("~/" + folder + "/Thumbnails/", image.FileName);

            thumbnail.Resize(thumbWidth, thumbHeight, true);

            thumbnail.Save(fullPath, thumbnail.ImageFormat);
        }
    }
}
using System.IO;
using System.Net;

namespace CampusWebStore.Utils
{
    using System;
    using System.Drawing;
    using System.Web;

    public class DocumentThumbnailUtil
    {
        #region Public Methods

        public static void GenerateThumbnail(HttpPostedFileBase file, string filename, int targetWidth, int targetHeight)
        {
            Image originalImage = Image.FromStream(file.InputStream);
            Bitmap finalImage = null;
            Graphics graphic = null;
            int width = originalImage.Width;
            int height = originalImage.Height;
            float targetRatio = targetWidth / (float)targetHeight;
            float imageRatio = width / (float)height;
            if (originalImage.Width >= targetWidth || originalImage.Height >= targetHeight)
            {
                try
                {
                    int newWidth;
                    int newHeight;
                    if (targetRatio > imageRatio)
                    {
                        newHeight = targetHeight;
                        newWidth = (int)Math.Floor(imageRatio * targetHeight);
                    }
                    else
                    {
                        newHeight = (int)Math.Floor(targetWidth / imageRatio);
                        newWidth = targetWidth;
                    }

                    newWidth = newWidth > targetWidth ? targetWidth : newWidth;
                    newHeight = newHeight > targetHeight ? targetHeight : newHeight;
                    finalImage = new Bitmap(newWidth, newHeight);
                    graphic = Graphics.FromImage(finalImage);
                    graphic.FillRectangle(new SolidBrush(Color.Black), new Rectangle(0, 0, targetWidth, targetHeight));
                    graphic.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic; /* new way */
                    graphic.DrawImage(originalImage, 0, 0, newWidth, newHeight);
                    string path = filename;
                    finalImage.Save(path);
                }

                    // ReSharper disable EmptyGeneralCatchClause
                catch (Exception)
                {
                    // ReSharper restore EmptyGeneralCatchClause
                }
                finally
                {
                    // Clean up
                    if (finalImage != null)
                    {
                        finalImage.Dispose();
                    }

                    if (graphic != null)
                    {
                        graphic.Dispose();
                    }

                    originalImage.Dispose();
                }
            }
            else
            {
                string path = filename;
                originalImage.Save(path);
                originalImage.Dispose();
            }
        }

        public static bool CheckForImage(string image)
        {
            if (!File.Exists(image))
                        {
                            try
                            {
                                var client = new WebClient();
                                var strm = client.OpenRead(image);
                                if (strm != null)
                                {
                                    var myBitMap = new System.Drawing.Bitmap(strm);
                                    if (myBitMap.Height == 1 && myBitMap.Width == 1)
                                    {
                                        //there is a blank image
                                        return true;
                                    }
                                }
                            }
                            catch { }
                        }
            return false;
        }

        #endregion
    }
}
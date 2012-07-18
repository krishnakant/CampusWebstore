namespace CampusWebStore.Utils
{
   

    public static class ThumbnailHelper
    {
        #region Constants and Fields

        public const string ArchiveThumbnail = "~/Content/Modules/Programs/Images/iconarchive.png";

        public const string AudioThumbnail = "~/Content/Modules/Programs/Images/iconAUDIO.png";

        public const string DocThumbnail = "~/Content/Modules/Programs/Images/iconDOC.png";

        public const string ImageThumbnail = "~/Content/Modules/Programs/Images/iconIMAGE.png";

        public const string PdfThumbnail = "~/Content/Modules/Programs/Images/iconPDF.png";

        public const string PptThumbnail = "~/Content/Modules/Programs/Images/iconPPT.png";

        public const string RtfThumbnail = "~/Content/Modules/Programs/Images/iconRTF.png";

        public const string TxtThumbnail = "~/Content/Modules/Programs/Images/iconTXT.png";

        public const string VideoThumbnail = "~/Content/Modules/Programs/Images/iconVIDEO.png";

        public const string XlsThumbnail = "~/Content/Modules/Programs/Images/iconXLS.png";

        #endregion

        #region Public Methods

        //public static string GetThumbnail(Document document)
        //{
        //    var eDocumentType = EDocumentTypeHelper.Parse(document.Category);
        //    switch (eDocumentType)
        //    {
        //        case EDocumentType.Archive:
        //            if (document.MimeType == MimeTypes.TarGz) return ArchiveThumbnail;
        //            if (document.MimeType == MimeTypes.Zip) return ArchiveThumbnail;
        //            break;
        //        case EDocumentType.Audio:
        //            if (document.MimeType == MimeTypes.M4A) return AudioThumbnail;
        //            if (document.MimeType == MimeTypes.Mp3) return AudioThumbnail;
        //            break;
        //        case EDocumentType.Document:
        //            {
        //                if (document.MimeType == MimeTypes.Doc) return DocThumbnail;
        //                if (document.MimeType == MimeTypes.Docx) return DocThumbnail;
        //                if (document.MimeType == MimeTypes.Pdf) return PdfThumbnail;
        //                if (document.MimeType == MimeTypes.Ppt) return PptThumbnail;

        //                // if (document.StaticFile.MimeType == MimeTypes.Rtf) return TxtThumbnail;
        //                if (document.MimeType == MimeTypes.Txt) return TxtThumbnail;
        //                if (document.MimeType == MimeTypes.Xls) return XlsThumbnail;
        //            }

        //            break;
        //        case EDocumentType.Photo:
        //            if (document.MimeType == MimeTypes.Jpeg) return ImageThumbnail;
        //            if (document.MimeType == MimeTypes.Png) return ImageThumbnail;
        //            if (document.MimeType == MimeTypes.XPng) return ImageThumbnail;
        //            if (document.MimeType == MimeTypes.Ai) return ImageThumbnail;
        //            if (document.MimeType == MimeTypes.Gif) return ImageThumbnail;
        //            if (document.MimeType == MimeTypes.Ico) return ImageThumbnail;
        //            if (document.MimeType == MimeTypes.Pjpeg) return ImageThumbnail;
        //            if (document.MimeType == MimeTypes.OctetStream) return ImageThumbnail;
        //            break;
        //        case EDocumentType.Video:
        //            if (document.MimeType == MimeTypes.Swf) return VideoThumbnail;
        //            if (document.MimeType == MimeTypes.Flv) return VideoThumbnail;
        //            if (document.MimeType == MimeTypes.Mp4) return VideoThumbnail;
        //            if (document.MimeType == MimeTypes.Swf) return VideoThumbnail;
        //            if (document.MimeType == MimeTypes.OctetStream) return VideoThumbnail;
        //            break;
        //    }

        //    return DocThumbnail;
        //}

        #endregion
    }
}
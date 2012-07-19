//// ReSharper disable PossibleNullReferenceException
//namespace CampusWebSotre.Utils
//{
//    using System;
//    using System.IO;

   

//    public static class PathUtil
//    {
//        #region Public Methods

//        public static string GetStaticFilePath(GuidId<Document> staticFileId)
//        {
//            if (staticFileId == null)
//            {
//                throw new ArgumentNullException("staticFileId");
//            }

//            string dir = System.Configuration.ConfigurationManager.AppSettings["StaticFiles"];
//            return Path.Combine(dir, staticFileId.ToString());
//        }

//         public static string GetArchivedStaticFileDirectory()
//        {
//           return System.Configuration.ConfigurationManager.AppSettings["ArchivedStaticFiles"];
//        }

//        #endregion
//    }
//}


using System.Xml;
using System.Xml.Linq;
using CampusWebStore.Shared.Models;

namespace CampusWebStore.Data.Daos
{

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.Practices.Unity;



    public interface ICourseDaos
    {
        #region Public Methods
        /// <summary>
        /// Get the record for the store
        /// </summary>
        /// <param name="storeId"></param>
        /// <param name="callName"></param>
        /// <param name="myVars"></param>
        /// <param name="userName"></param>
        /// <param name="userPwd"></param>
        /// <param name="dbType"></param>
        /// <param name="uvAddress"></param>
        /// <param name="uvAccount"></param>
        /// <param name="cacheTIme"></param>
        /// <param name="dblCache"></param>
        /// <param name="strd3PortNumber"></param>
        /// <param name="useEncryption"></param>
        /// <param name="d3PortNumber"></param>
        /// <returns></returns>
        IEnumerable<CourseModel> GetCourses(string storeId, string callName, object myVars, string userName, string userPwd, string dbType,
                        string uvAddress, string uvAccount, string cacheTIme, string dblCache, string strd3PortNumber,
                        string useEncryption, string d3PortNumber);


        #endregion
    }

    public class CourseDaos : ICourseDaos
    {
        
        #region Properties
        [Dependency]
        public IDbAccess DbAccess { get; set; }
       
        #endregion
        
        #region Public Methods

        public IEnumerable<CourseModel> GetCourses(string storeId, string callName, object myVars, string userName, string userPwd, string dbType, string uvAddress, string uvAccount, string cacheTIme, string dblCache, string strd3PortNumber, string useEncryption, string d3PortNumber)
        {
            try
            {
                 var strPickDataReturn= DbAccess.GetStringResult(storeId, callName, myVars, userName, userPwd, dbType, uvAddress,
                                                uvAccount,cacheTIme, dblCache, strd3PortNumber, useEncryption, d3PortNumber);
                 var doc = new XmlDocument();

                doc.LoadXml(strPickDataReturn);

                var xmlStore = XElement.Parse(strPickDataReturn);

                var courseModel = (from store in xmlStore.Descendants("COURSE")
                                   let xElement = store.Element("COURSEID")
                                   where xElement != null
                                   let element = store.Element("COURSEDESC")
                                   where element != null
                                   select new CourseModel
                                              {

                                                  CourseId = xElement.Value,

                                                  Name = element.Value,

                                              }).ToList();

                return courseModel;
            }
            catch(Exception x)
            {
                throw;
            }
        }
        #endregion
    }
}

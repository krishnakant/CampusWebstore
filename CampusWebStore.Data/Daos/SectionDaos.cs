

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



    public interface ISectionDaos
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
        IEnumerable<SectionModel> GetSections(string storeId, string callName, object myVars, string userName, string userPwd, string dbType,
                        string uvAddress, string uvAccount, string cacheTIme, string dblCache, string strd3PortNumber,
                        string useEncryption, string d3PortNumber);
        /// <summary>
        /// Retrun book's  xml string for the sections
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
        IEnumerable<CourseSectionModel> GetSectioBooks(string storeId, string callName, object myVars, string userName, string userPwd, string dbType,
                        string uvAddress, string uvAccount, string cacheTIme, string dblCache, string strd3PortNumber,
                        string useEncryption, string d3PortNumber);

        #endregion
    }

    public class SectionDaos : ISectionDaos
    {
        
        #region Properties
        [Dependency]
        public IDbAccess DbAccess { get; set; }
       
        #endregion
        
        #region Public Methods

        public IEnumerable<SectionModel> GetSections(string storeId, string callName, object myVars, string userName, string userPwd, string dbType, string uvAddress, string uvAccount, string cacheTIme, string dblCache, string strd3PortNumber, string useEncryption, string d3PortNumber)
        {
            try
            {
                //Get the xml from the database
                var strPickDataReturn= DbAccess.GetStringResult(storeId, callName, myVars, userName, userPwd, dbType, uvAddress,
                                                uvAccount,cacheTIme, dblCache, strd3PortNumber, useEncryption, d3PortNumber);
                var doc = new XmlDocument();

                doc.LoadXml(strPickDataReturn);

                var xmlTerm = XElement.Parse(strPickDataReturn);

                var sectionModels = (from terms in xmlTerm.Descendants("SECTION")
                                     let xElement = terms.Element("ID")
                                     where xElement != null
                                     let element = terms.Element("DESC")
                                     where element != null
                                     select new SectionModel
                                     {
                                         SectionId = xElement.Value,

                                         Name = element.Value,

                                     }).ToList();
                return sectionModels;
            }
            catch(Exception x)
            {
                throw;
            }
        }
        #endregion


        public IEnumerable<CourseSectionModel> GetSectioBooks(string storeId, string callName, object myVars, string userName, string userPwd, string dbType, string uvAddress, string uvAccount, string cacheTIme, string dblCache, string strd3PortNumber, string useEncryption, string d3PortNumber)
        {
            try
            {
                var strPickDataReturn= DbAccess.GetStringResult(storeId, callName, myVars, userName, userPwd, dbType, uvAddress,
                                                uvAccount, cacheTIme, dblCache, strd3PortNumber, useEncryption,
                                                d3PortNumber);

                var doc = new XmlDocument();

                doc.LoadXml(strPickDataReturn);

                var xmlBooks = XElement.Parse(strPickDataReturn);

                var courseSectionModel = (from section in xmlBooks.Descendants("SECTION")

                                          select new CourseSectionModel
                                          {
                                              Comment = section.Element("COMMENTS").Value,
                                              Class = section.Element("CLASS").Value,
                                              ItemModels = (from item in section.Descendants("ITEMS")
                                                            select new ItemModel
                                                            {
                                                                StoreId = item.Element("STOREID").Value,
                                                                ItemId = item.Element("ITEMID").Value,
                                                                Isbn = item.Element("ISBN").Value,
                                                                ReqFlag = item.Element("REQFLAG").Value,
                                                                Author = item.Element("AUTHOR").Value,
                                                                Title = item.Element("TITLE").Value,
                                                                Type = item.Element("TYPE").Value,
                                                                Edition = item.Element("EDITION").Value,
                                                                Copyright = item.Element("COPYRIGHT").Value,
                                                                Binding = item.Element("BINDING").Value,
                                                                NewPrice = item.Element("NEWPRICE").Value,
                                                                UsedPrice = item.Element("USEDPRICE").Value,
                                                                Image = item.Element("IMAGE").Value,
                                                                EbookPrice = item.Element("EBOOKPRICE").Value,
                                                                RentalPrice = item.Element("RENTALPRICE").Value,
                                                                NewQoh = item.Element("NEWQOH").Value,
                                                                UsedQoh = item.Element("USEDQOH").Value,
                                                                NewQoo = item.Element("NEWQOO").Value,
                                                                UsedQoo = item.Element("USEDQOO").Value,
                                                                //RentalQoh = item.Element("RENTALQOH").Value,
                                                                EbookQoh = item.Element("EBOOKQOH").Value,
                                                                CanBuy = item.Element("CANBUY").Value,
                                                                EnableEbook = item.Element("ENABLEEBOOK").Value,
                                                                //EnableRental = item.Element("ENABLERENTAL").Value,
                                                            })

                                          }).ToList();

                return courseSectionModel;

            }
            catch (Exception x)
            {
                throw;
            }
         
        }
    }
}

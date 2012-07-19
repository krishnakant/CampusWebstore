using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PickDB;

namespace CampusWebStore.Data.Daos
{
    public interface IDbAccess
    {
        #region Properties
        #endregion

        #region Public Methods

        string GetStringResult(string storeNumbber, string programName, object myVars, string userName, string userPwd,
                               string dbType, string uvAddress, string uvAccount, string cacheTIme, string dblCache,
                               string strd3PortNumber, string useEncryption, string d3PortNumber);

        #endregion
    }
    public class DbAccess:IDbAccess
    {
      /// <summary>
      /// Private constructor
      /// </summary>
    
      /// <summary>
      /// Method wchich will fetches the data from the database and return the data into the form of the xml
      /// </summary>
      /// <param name="storeNumbber"></param>
      /// <param name="programName"></param>
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
      public string GetStringResult(string storeNumbber, string programName, object myVars, string userName, string userPwd, string dbType, string uvAddress, string uvAccount, string cacheTIme, string dblCache, string strd3PortNumber, string useEncryption, string d3PortNumber)
      {
          var gatherdata = "";
          var pickReturn = "";
         
          try
          {
            var pick = new JetKit();
              
              //pick.Timeout = 25;
                  if (dbType == "UV")
                  {
                     pick.initialize(uvAddress, userName, userPwd, uvAccount, JetKit.UNIVERSE, programName,
                                      "UO.INIT");
                  }
                  else
                  {
                      pick.initialize(uvAddress, uvAccount,Convert.ToInt32(d3PortNumber), programName);
                  }

                   System.Reflection.PropertyInfo[] propInfo = myVars.GetType().GetProperties();

                  try
                  {
                      foreach (System.Reflection.PropertyInfo info in propInfo)
                      {
                          pick.addVar(info.Name, info.GetValue(myVars, null).ToString());
                         
                      }
                      pick.execute();
                      pickReturn = "";
                      gatherdata += "pickReturn initialized to empty string";
                      var expTime = Convert.ToDouble(60);
                      gatherdata += "expTime was set";
                      pickReturn = pick.getResult();
                      gatherdata += "pickReturn is set via pick GET RESULT call";
                      var myObj = new object();
                      gatherdata += "new object created";
                  }
                  catch (Exception x)
                  {
                  }

                  //if (useCache == "TRUE")
                  //{

                  //    string strExceptionList =
                  //        System.Configuration.ConfigurationManager.AppSettings["CACHEDEXCEPTIONLIST"] as string;
                  //    //get the list of exceptions fromt he config file

                  //    string[] mySpliter1 = { ";" }; // our delim

                  //    string[] myEmailArray;

                  //    myEmailArray = strExceptionList.Split(mySpliter1, StringSplitOptions.None);

                  //    //add each exception to a list
                  //    foreach (string s in myEmailArray)
                  //    {
                  //        if (!String.IsNullOrEmpty(s))
                  //        {
                  //            exceptionList.Add(s);
                  //        }
                  //    }

                  //    //check the list to see if we can cache
                  //    if (!exceptionList.Contains(programName))
                  //    {
                  //        object cachedFile = new object();
                  //        try
                  //        {
                  //            System.Web.HttpContext.Current.Cache.Insert(key, pickReturn, null,
                  //                                                        DateTime.Now.AddSeconds(dblcacheTime),
                  //                                                        System.Web.Caching.Cache.NoSlidingExpiration);

                  //        }
                  //        catch (Exception eexxe)
                  //        {

                  //        }
                  //    }
                  //}
                  return pickReturn;
              
          }
          catch (Exception eexxee)
          {
              throw;
          }
          finally
          {

          }
          return pickReturn;
      }
    }
}

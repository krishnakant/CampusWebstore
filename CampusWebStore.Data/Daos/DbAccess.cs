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
        /// Method for fetching data from the pick database and returning the data as xml
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
            // Initialize vars
            var pick = new JetKit();
            string pickReturn = null;
            string error = null;
            System.Reflection.PropertyInfo[] propInfo = myVars.GetType().GetProperties();

            // Determine database type
            if (dbType == "UV")
            {
                pick.initialize(
                    uvAddress, 
                    userName, 
                    userPwd, 
                    uvAccount, 
                    JetKit.UNIVERSE, 
                    programName, 
                    "UO.INIT"
                );
            }
            else
            {
                pick.initialize(
                    uvAddress, 
                    uvAccount,
                    Convert.ToInt32(d3PortNumber), 
                    programName
                );
            }


            // Attempt db query
            try
            {
                foreach (System.Reflection.PropertyInfo info in propInfo)
                {
                    pick.addVar(info.Name, info.GetValue(myVars, null).ToString());
                }
                pick.execute();
                pickReturn = pick.getResult();
            }
            catch (Exception x)
            {
                error = x.Message;
            }

            // Error check
            if (pickReturn == null || pickReturn == "" || error != null) {
                if (error != null)
                {
                    throw new Exception(error);
                }
                else
                {
                    //throw new Exception("Pick Error");
                    pickReturn = "<?xml version=\"1.0\" encoding=\"ISO-8859-1\"?><root><response>ok</response></root>";
                }
            }

            // Return results
            return pickReturn;
        }
    }
}

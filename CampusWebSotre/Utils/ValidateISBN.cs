using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CampusWebStore.Utils
{
    public class ValidateIsbn
    {
        public ValidateIsbn()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        public static bool CheckIsbn(ref string isbn)
        {
            if (isbn.Length == 18) //'find what number has been scanned. if ean, change to isbn
                isbn = isbn.Substring(0, 13);
            isbn = isbn.Replace("-", "");
            if (isbn.Length == 13)
            {
                var strIsbn = isbn.Substring(3, 9);
                var intCheck = 0;
                for (int i = 0; i < strIsbn.Length; i++)
                {
                    intCheck += (10 - i) * int.Parse(strIsbn[i].ToString());
                }
                intCheck = (11 - (intCheck % 11)) % 11;
                if (intCheck == 10)
                    strIsbn += "X";
                else
                    strIsbn += intCheck.ToString();

                isbn = strIsbn;

            }
            if (isbn.Length != 10) //Make sure the isbn is the correct 10 digits
                return false;
            else
                return true;
        }
    }
}
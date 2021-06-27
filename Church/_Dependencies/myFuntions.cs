using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace Church._Dependencies
{
    public class myFuntions
    {
        public static Boolean AccessStringTest(string requiredaccess)
        {
            string user = (string)HttpContext.Current.Session["church_user"] ?? "";
            if (user != "")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
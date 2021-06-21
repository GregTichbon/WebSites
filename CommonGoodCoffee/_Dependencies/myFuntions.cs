using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace CommonGoodCoffee._Dependencies
{
    public class myFuntions
    {
        public static Boolean AccessStringTest(string requiredaccess)
        {
            string user = (string)HttpContext.Current.Session["cgc_user"] ?? "";
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
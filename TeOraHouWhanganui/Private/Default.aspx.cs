using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using localfunctions = TeOraHouWhanganui._Dependencies;

namespace TeOraHouWhanganui.Private
{
    public partial class Default : System.Web.UI.Page
    {
        public string username = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            //_Dependencies.functions xfunctions = new _Dependencies.functions();
            

            username = HttpContext.Current.User.Identity.Name.ToLower();

            if (username == "")
            {
                username = "toh\\gtichbon";   //localhost
            }
            /*
            if (localfunctions.functions.AccessStringTest(username, "111"))
            {
                string a = "1";
            }
            */
        }
    }
}
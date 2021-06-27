using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using localfunctions = Church._Dependencies.myFuntions;

namespace Church
{
    public partial class PersonSearch : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!localfunctions.AccessStringTest(""))
            {
                Response.Redirect("/login.aspx");
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CommonGoodCoffee
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Session["cgc_user"] = "";
        }

        protected void btn_submit_Click(object sender, EventArgs e)
        {
            if(fld_password.Text == "BadHagrid")
            {
                Session["cgc_user"] = "Logged in";
                Response.Redirect("Default.aspx");
            } 
        }
    }
}
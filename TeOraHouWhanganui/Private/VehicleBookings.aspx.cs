using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TeOraHouWhanganui.Private
{
    public partial class VehicleBookings : System.Web.UI.Page
    {
        public string username = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            username = HttpContext.Current.User.Identity.Name.ToLower();

            if (username == "")
            {
                username = "toh\\gtichbon";   //localhost
            }
        }
    }
}
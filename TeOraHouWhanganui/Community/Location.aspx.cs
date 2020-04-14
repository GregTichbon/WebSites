using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TeOraHouWhanganui.Community
{
    public partial class Location : System.Web.UI.Page
    {
        public static string location = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            location = "Greg was here";
        }
    }
}
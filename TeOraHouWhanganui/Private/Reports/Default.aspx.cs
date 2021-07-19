using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TeOraHouWhanganui.Private.Reports
{
    public partial class Default : System.Web.UI.Page
    {
        public string html = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            //this will be done from a database table at some stage See - CGC
            html += "<p><a href=\"report.aspx?id=Current Workers\">Current Workers</a></p>";
        }
    }
}
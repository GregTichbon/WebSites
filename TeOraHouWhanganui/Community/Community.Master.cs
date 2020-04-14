using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TeOraHouWhanganui.Community
{
    public partial class Community : System.Web.UI.MasterPage
    {
        public string headerimage = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            headerimage = ResolveUrl("~/Community/_Dependencies/Images/Covid19.jpg");
        }
    }
}
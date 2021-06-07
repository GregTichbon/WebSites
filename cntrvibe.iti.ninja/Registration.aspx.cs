using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace cntrvibe.iti.ninja
{
    public partial class Registration : System.Web.UI.Page
    {
        public string html = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            html = _Dependencies.functions.get_entry("e6e22471-80f1-49fd-b2ac-a46388d253cd","");
        }
    }
}
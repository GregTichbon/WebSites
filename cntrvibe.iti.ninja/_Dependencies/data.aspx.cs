using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace cntrvibe.iti.ninja._Dependencies
{
    public partial class data : System.Web.UI.Page
    {
        public string html = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            string mode = Request.Form["mode"];

            switch (mode)
            {
                case "get_entry":
                    get_entry();
                    break;

            }
        }

        protected string get_entry()
        {
            string reference = Request.Form["reference"];
            string entry_ctr = Request.Form["entry_ctr"];
            html = _Dependencies.functions.get_entry(reference, entry_ctr);
            return (html);
        }
    }
}
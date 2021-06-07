using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace cntrvibe.iti.ninja
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                string wdcscripts = "";
                if (Request.QueryString["populate"] != null || 1 == 2)
                {
                    wdcscripts += "$.getScript('/_dependencies/populate.js');";
                }
                if (Request.QueryString["showfields"] != null)
                {
                    wdcscripts += "$.getScript('/_dependencies/showfields.js');";
                }
                if (wdcscripts != "")
                {
                    wdcscripts = "<script type='text/javascript'>" + wdcscripts + "</script>";
                    ClientScript.RegisterStartupScript(this.GetType(), "ConfirmSubmit", wdcscripts);
                }
            }
        }
    }
}
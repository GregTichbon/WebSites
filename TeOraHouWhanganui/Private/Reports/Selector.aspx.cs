using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TeOraHouWhanganui.Private.Reports
{
    public partial class Selector : System.Web.UI.Page
    {
        public string html = "";
        public string username = "";
        public string reporttitle;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string r = Request.QueryString["id"] + "";
                if (r == "")
                {
                    Response.Redirect("../default.aspx");
                }

                string[] fields;

                switch (r)
                {
                    case "EncountersSummary":
                        reporttitle = "Encounters Summary Audit";
                        fields = new string[] { "From Date|Date", "To Date|Date" };
                        html = formatfields(fields);
                        break;
                    default:
                        break;
                }
            }

        }

        protected string formatfields(string[] fields)
        {
            string response = "";

            foreach (string field in fields)
            {
                string[] parts = field.Split('|');
                string label = parts[0];
                string fieldname = label.Replace(" ", "").ToLower();
                string type = parts[1];
                response += "<div class=\"form-group\">";
                response += "<label for=\"" + fieldname + "\" class=\"control-label col-sm-4\">" + label + "</label>";
                response += "<div class=\"col-sm-4\">";

                switch (type)
                {
                    case "Date":
                        response += "<div class=\"input-group date\">";
                        response += "<input name=\"" + fieldname + "\" type=\"text\" class=\"form-control date\" required=\"required\" />";
                        response += "<span class=\"input-group-addon\">";
                        response += "<span class=\"glyphicon glyphicon-calendar\"></span>";
                        response += "</span>";
                        response += "</div>";

                        break;
                }

                response += "</div>";
                response += "</div>";
            }

            return response;
        }

        protected void btn_submit_Click(object sender, EventArgs e)
        {
            string r = Request.QueryString["id"] + "";
            r = "?id=" + r;
            foreach (string key in Request.Form)
            {
                if (!(key.StartsWith("__") || key.StartsWith("ctl")))
                {
                    r += "&" + key + "=" + Request.Form[key];
                }
            }
            Response.Redirect("report.aspx" + r);
        }
    }
}
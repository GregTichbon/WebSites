using Generic;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace VehicleService
{
    public partial class MakeModelMaintenance : System.Web.UI.Page
    {
        public Dictionary<string, string> options = new Dictionary<string, string>();
        public string[] nooptions = { };

        public Dictionary<string, string> vehiclemakes = new Dictionary<string, string>();
        protected void Page_Load(object sender, EventArgs e)
        {
            string systemPrefix = WebConfigurationManager.AppSettings["systemPrefix"];
            String connectionString = ConfigurationManager.ConnectionStrings[systemPrefix + "ConnectionString"].ConnectionString;


            options.Clear();
            options.Add("storedprocedure", "");
            options.Add("storedprocedurename", "");
            //options.Add("parameters", "|countcustomers|");
            options.Add("usevalues", "");
            //options.Add("insertblank", "start");
            vehiclemakes = Functions.buildselectionlist(connectionString, "get_vehiclemakes_DD", options);
        }

        protected void btn_submit_Click(object sender, EventArgs e)
        {
             /*
            options.Clear();
            options.Add("style", "html");
            string xx = Functions.show_requestform(Request, options);

            Response.Write(xx);
            Response.End();
            */

            string vehiclemake_ctr = Request.Form["hf_vehiclemake_ctr"];

            string systemPrefix = WebConfigurationManager.AppSettings["systemPrefix"];
            String connectionString = ConfigurationManager.ConnectionStrings[systemPrefix + "ConnectionString"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("update_vehiclemake", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@vehiclemake_ctr", SqlDbType.VarChar).Value = vehiclemake_ctr;
                    cmd.Parameters.Add("@make", SqlDbType.VarChar).Value = Request.Form["fld_vehiclemake"];
                    
                    con.Open();
                    vehiclemake_ctr = cmd.ExecuteScalar().ToString();
                    con.Close();
                }

                foreach (string key in Request.Form)
                {
                    if (key.StartsWith("model_"))
                    {
                        string[] keyparts = key.Split('_');
                      
                        using (SqlCommand cmd = new SqlCommand("update_vehiclemodel", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add("@vehiclemodel_ctr", SqlDbType.VarChar).Value = keyparts[1];
                            cmd.Parameters.Add("@vehiclemake_ctr", SqlDbType.VarChar).Value = vehiclemake_ctr;
                            cmd.Parameters.Add("@model", SqlDbType.VarChar).Value = Request.Form[key];
                            con.Open();
                            cmd.ExecuteNonQuery();
                            con.Close();
                        }
                    }
                }
            }
            /*
            string returnto = ViewState["returnto"].ToString();

            if (Creating)
            {
                if (returnto == "")
                {
                    returnto = "eventmaintenance.aspx?id=" + event_ctr;
                }
                else
                {
                    returnto = "eventmaintenance.aspx?id=" + event_ctr + "&returnto=" + returnto + ".aspx";
                }
            }
            else
            {
                if (returnto == "")
                {
                    returnto = "eventsearch.aspx";
                }
            }

            Response.Redirect(returnto);
            */


        }
    }
}
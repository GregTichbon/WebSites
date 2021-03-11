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
    public partial class Update_WOF : System.Web.UI.Page
    {
        public int WOF_Cycle = 0;
        public string WOF_Due = "";
        public string odometer = "";
        public string service_date_due = "";
        public string service_km_due = "";
        public string filter_date_due = "";
        public string filter_km_due = "";
        public string nextwofdate = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            string id = Request.QueryString["id"];
            string systemPrefix = WebConfigurationManager.AppSettings["systemPrefix"];
            String connectionString = ConfigurationManager.ConnectionStrings[systemPrefix + "ConnectionString"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("get_vehicle", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@vehicle_ctr", SqlDbType.VarChar).Value = id;
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    dr.Read();
                    WOF_Cycle = (int)dr["WOF_Cycle"];
                    WOF_Due = Functions.formatdate(dr["Wof_Due"].ToString(), "d MMM yyyy");
                    odometer = dr["odometer"].ToString();
                    service_date_due = Functions.formatdate(dr["service_date_due"].ToString(), "d MMM yyyy");
                    service_km_due = dr["service_km_due"].ToString();
                    filter_date_due = Functions.formatdate(dr["filter_date_due"].ToString(), "d MMM yyyy");
                    filter_km_due = dr["filter_km_due"].ToString();

                    if(WOF_Cycle != 0 && WOF_Due != "")
                    {
                        DateTime WOF_Due_Date = DateTime.Parse(WOF_Due);
                        nextwofdate = WOF_Due_Date.AddMonths(WOF_Cycle).ToString("d MMM yyyy");
                    }

                }
                dr.Close();
                con.Close();
            }
        }
    }

}
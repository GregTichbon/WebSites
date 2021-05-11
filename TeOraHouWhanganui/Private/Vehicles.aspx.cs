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

namespace TeOraHouWhanganui.Private
{
    public partial class Vehicles : System.Web.UI.Page
    {
        public string username = "";
        public string html = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            string sql = "get_vehicles";

            string systemPrefix = WebConfigurationManager.AppSettings["systemPrefix"];
            String connectionString = ConfigurationManager.ConnectionStrings[systemPrefix + "ConnectionString"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;


                    con.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    html = "<table class=\"table\"><thead><tr><th>Registration</th><th>Name</th><th>Description</th><th>WOF Due</th><th>Registration Due</th><th>Start Date</th><th>End Date</th></tr></thead><tbody>";
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            string vehicle_ctr = dr["vehicle_ctr"].ToString();
                            string registration = dr["registration"].ToString();
                            string Name = dr["Name"].ToString();
                            string description = dr["description"].ToString();
                            string wofcycle = dr["wofcycle"].ToString();
                            string wofdue = Functions.formatdate(dr["wofdue"].ToString(), "d MMM yyyy");
                            string registrationdue = Functions.formatdate(dr["registrationdue"].ToString(), "d MMM yyyy");
                            string startdate = Functions.formatdate(dr["startdate"].ToString(), "d MMM yyyy");
                            string enddate = Functions.formatdate(dr["enddate"].ToString(), "d MMM yyyy");
                            string note = dr["note"].ToString();

                            html += "<tr><td>" + registration + "</td><td>" + Name + "</td><td>" + description + "</td><td>" + wofdue + "</td><td>" + registrationdue + "</td><td>" + startdate + "</td><td>" + enddate + "</td></tr>";

                        }
                    }
                    html += "</tbody></table>";
                }


            }

        }

        protected void btn_submit_Click(object sender, EventArgs e)
        {

        }
    }
}
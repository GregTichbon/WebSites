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
    public partial class Followup : System.Web.UI.Page
    {
        public string html = "";
        protected void Page_Load(object sender, EventArgs e)
        {


                string systemPrefix = WebConfigurationManager.AppSettings["systemPrefix"];
                String connectionString = ConfigurationManager.ConnectionStrings[systemPrefix + "ConnectionString"].ConnectionString;

                using (SqlConnection con = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand("Wof_and_Followup", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.Add("@id", SqlDbType.VarChar).Value = id;
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    string lastvehicle = "";
                    html += "<table class=\"table\"><thead><tr>";
                    html += "<th>Customer</th><th>Vehicle</th><th>WOF Due</th><th>Followup Date</th><th>Detail</th><th>";
                    html += "</tr></thead><tbody>";

                    while (dr.Read())
                    {
                        string Customer_CTR = dr["Customer_CTR"].ToString();
                        string Customer = dr["Customer"].ToString();
                        string vehicle_CTR = dr["vehicle_CTR"].ToString();
                        string Vehicle = dr["Vehicle"].ToString();
                        string Customer_Vehicle_CTR = dr["Customer_Vehicle_CTR"].ToString();
                        string WOF_Due = Functions.formatdate(dr["WOF_Due"].ToString(), "dd/MM/yy");
                        string FollowupDate = Functions.formatdate(dr["FollowupDate"].ToString(), "dd/MM/yy");
                        string Detail = dr["Detail"].ToString();
                        string vehicle_followup_ctr = dr["vehicle_followup_ctr"].ToString();

                        html += "<tr data-customer=\"" + Customer_CTR + "\" data-vehicle=\"" + vehicle_CTR + "\" data-customer_vehicle=\"" + Customer_Vehicle_CTR + "\" data-followup=\"" + vehicle_followup_ctr + "\">";
                        if (Vehicle != lastvehicle)
                        {
                            html += "<td><a class=\"customer\">" + Customer + "</a></td><td><a class=\"customer_vehicle\">" + Vehicle + " </a></td><td>" + WOF_Due + "</td>";
                            lastvehicle = Vehicle;
                        } else
                        {
                            html += "<td colspan=\"3\"></td>";
                        }
                        //html += "<td><a class=\"followup\">" + FollowupDate + " </a></td>;
                        html += "<td>" + FollowupDate + " </td>"; 
                        html += "<td>" + Detail + "</td><td>";
                        html += "</tr>";
                    }
                    html += "</tbody></table>";
                }
                dr.Close();
                con.Close();
            }
        }
    }
    }
 
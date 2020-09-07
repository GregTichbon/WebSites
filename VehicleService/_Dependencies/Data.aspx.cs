using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using Generic;
using System.Web.Configuration;
using System.Configuration;

namespace VehicleService._Dependencies
{
    public partial class Data : System.Web.UI.Page
    {
        public string html = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            string systemPrefix = WebConfigurationManager.AppSettings["systemPrefix"];
            String connectionString = ConfigurationManager.ConnectionStrings[systemPrefix + "ConnectionString"].ConnectionString;
            SqlDataReader dr;

            string mode = Request.Form["mode"];

            switch (mode)
            {
                case "get_customer_vehicle_activity":

                  
                    string customer_vehicle_ctr = Request.Form["customer_vehicle_ctr"];
                    html = "<table id=\"activitytable\" class=\"table\" style=\"width:100%\">";
                    html += "<thead>";
                    html += "<tr><th style=\"width:50px;text-align:center\"></th><th>Date</th><th>Detail</th><th style=\"width:100px\">Action / <a class=\"vehicleactivityedit\" link=\"new\" customer=\"" + customer_vehicle_ctr + "\">Add</a></th></tr>";
                    html += "</thead>";
                    html += "<tbody>";
                    /*
                  using (SqlConnection con = new SqlConnection(connectionString))
                  using (SqlCommand cmd = new SqlCommand("get_customer_vehicle_activity", con))
                  {
                      cmd.CommandType = CommandType.StoredProcedure;
                      cmd.Parameters.Add("@customer_ctr", SqlDbType.VarChar).Value = customer_vehicle_ctr;
                      con.Open();
                      dr = cmd.ExecuteReader();
                      while (dr.Read())
                      {
                          string customer_vehicle_CTR = dr["Customer_Vehicle_CTR"].ToString();
                          string vehicle_CTR = dr["Vehicle_CTR"].ToString();
                          string registration = dr["registration"].ToString();
                          string description = dr["description"].ToString();
                          string wofcycle = ""; // dr["wof_cycle"].ToString();
                          //string date = Functions.formatdate(dr["date"].ToString(), "dd MMM yyyy");
                          string note = dr["note"].ToString();

                          html += "<tr>"; // id=\"vehicle_" + customer_vehicle_CTR + "\">";
                          html += "<td style=\"text-align:center\"></td>";
                          //html += "<td programid=\"" + programid + "\">" + programname + "</td>";
                          html += "<td>" + registration + "</td>";
                          html += "<td>" + "To do" + "</td>";
                          html += "<td>" + description + "</td>";
                          //html += "<td worker=\"" + worker + "\">" + YesNoBit.FirstOrDefault(x => x.Value == worker).Key + "</td>";
                          html += "<td>" + wofcycle + "</td>";
                          html += "<td>" + "To do" + "</td>";
                          html += "<td>" + note + "</td>";
                          html += "<td><a link=\"" + customer_vehicle_CTR + "\" class=\"vehicleedit\">Edit</td>";
                          html += "</tr>";
                      }
                      dr.Close();
                      con.Close();
                  }
                  */

                    break;


                case "get_customer_vehicles":
                    string customer_ctr = Request.Form["customer_ctr"];
                    html = "<table id=\"vehicletable\" class=\"table\" style=\"width:100%\">";
                    html += "<thead>";
                    html += "<tr><th style=\"width:50px;text-align:center\"></th><th>Registration</th><th>Make/Model</th><th>Description</th><th>WOF Cycle</th><th>WOF Due</th><th>Note</th><th style=\"width:100px\">Action / <a class=\"vehicleedit\" link=\"new\" customer=\"" + customer_ctr + "\">Add</a></th></tr>";
                    html += "</thead>";
                    html += "<tbody>";

                    using (SqlConnection con = new SqlConnection(connectionString))
                    using (SqlCommand cmd = new SqlCommand("get_customer_vehicles", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@customer_ctr", SqlDbType.VarChar).Value = customer_ctr;
                        con.Open();
                        dr = cmd.ExecuteReader();
                        while (dr.Read())
                        {
                            string customer_vehicle_CTR = dr["Customer_Vehicle_CTR"].ToString();
                            string vehicle_CTR = dr["Vehicle_CTR"].ToString();
                            string registration = dr["registration"].ToString();
                            string description = dr["description"].ToString();
                            string wofcycle = ""; // dr["wof_cycle"].ToString();
                            //string date = Functions.formatdate(dr["date"].ToString(), "dd MMM yyyy");
                            string note = dr["note"].ToString();

                            html += "<tr>"; // id=\"vehicle_" + customer_vehicle_CTR + "\">";
                            html += "<td style=\"text-align:center\"></td>";
                            //html += "<td programid=\"" + programid + "\">" + programname + "</td>";
                            html += "<td>" + registration + "</td>";
                            html += "<td>" + "To do" + "</td>";
                            html += "<td>" + description + "</td>";
                            //html += "<td worker=\"" + worker + "\">" + YesNoBit.FirstOrDefault(x => x.Value == worker).Key + "</td>";
                            html += "<td>" + wofcycle + "</td>";
                            html += "<td>" + "To do" + "</td>";
                            html += "<td>" + note + "</td>";
                            html += "<td><a link=\"" + customer_vehicle_CTR + "\" class=\"vehicleedit\">Edit</td>";
                            html += "</tr>";
                        }
                        dr.Close();
                        con.Close();

                    }
                    html += "</tbody></table>";
                    break;
            }
        }
    }
}
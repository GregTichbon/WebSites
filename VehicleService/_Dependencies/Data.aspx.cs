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


            string mode = Request.Form["mode"];

            switch (mode)
            {
                case "get_vehicle_activities":
                    get_vehicle_activities();
                    break;
                case "get_customer_vehicles":
                    get_customer_vehicles();
                    break;
                case "get_vehiclemodels":
                    get_vehiclemodels();
                    break;
                case "get_vehicle_followups":
                    get_vehicle_followups();
                    break;
            }
        }

        
        protected string get_vehiclemodels()
        {

            string systemPrefix = WebConfigurationManager.AppSettings["systemPrefix"];
            String connectionString = ConfigurationManager.ConnectionStrings[systemPrefix + "ConnectionString"].ConnectionString;

            string vehiclemake_ctr = Request.Form["vehiclemake_ctr"];
            html = "<table id=\"modelstable\" class=\"table table-condensed\" style=\"width:100%\">";
            html += "<thead>";
            html += "<tr><th style=\"width:50px;text-align:center\"></th><th>Model</th><th>Used</th><th style=\"width:100px\">Action / <a class=\"vehiclemodeledit\" link=\"new\">Add</a></th></tr>";
            html += "</thead>";
            html += "<tbody>";

            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("get_vehiclemodels", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@vehiclemake_ctr", SqlDbType.VarChar).Value = vehiclemake_ctr;
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    string vehiclemodel_CTR = dr["vehiclemodel_CTR"].ToString();
                    string vehiclemodel = dr["model"].ToString();
                    int vehicles = (int)dr["vehicles"];
                    //string note = dr["note"].ToString();

                    html += "<tr link=\"" + vehiclemodel_CTR + "\">"; 
                    html += "<td style=\"text-align:center\"></td>";
                    //html += "<td programid=\"" + programid + "\">" + programname + "</td>";
                    html += "<td>" + vehiclemodel + "</td>";
                    html += "<td>" + vehicles.ToString() + "</td>";
                    //html += "<td>" + note + "</td>";
                    if(vehicles == 0) { 
                    html += "<td><a link=\"" + vehiclemodel_CTR + "\" class=\"vehiclemodeledit\">Remove</td>";
                    } else
                    {
                        html += "<td></td>";
                    }
                    html += "</tr>";
                }
                dr.Close();
                con.Close();
            }
            html += "</tbody></table>";
            return (html);
        }
        protected string get_customer_vehicles()
        {

            string systemPrefix = WebConfigurationManager.AppSettings["systemPrefix"];
            String connectionString = ConfigurationManager.ConnectionStrings[systemPrefix + "ConnectionString"].ConnectionString;

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
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    string customer_vehicle_CTR = dr["Customer_Vehicle_CTR"].ToString();
                    string vehicle_CTR = dr["Vehicle_CTR"].ToString();
                    string registration = dr["registration"].ToString();
                    string description = dr["description"].ToString();
                    string make = dr["make"].ToString();
                    string model = dr["model"].ToString();
                    string wofcycle = dr["wof_cycle"].ToString() + " to do";
                    string wof_due = Functions.formatdate(dr["wof_due"].ToString(), "dd MMM yyyy");
                    string note = dr["note"].ToString();

                    html += "<tr>"; // id=\"vehicle_" + customer_vehicle_CTR + "\">";
                    html += "<td style=\"text-align:center\"></td>";
                    //html += "<td programid=\"" + programid + "\">" + programname + "</td>";
                    html += "<td>" + registration + "</td>";
                    html += "<td>" + make + " - " + model + "</td>";
                    html += "<td>" + description + "</td>";
                    //html += "<td worker=\"" + worker + "\">" + YesNoBit.FirstOrDefault(x => x.Value == worker).Key + "</td>";
                    html += "<td>" + wofcycle + "</td>";
                    html += "<td>" + wof_due + "</td>";
                    html += "<td>" + note + "</td>";
                    html += "<td><a link=\"" + customer_vehicle_CTR + "\" class=\"vehicleedit\">Edit</td>";
                    html += "</tr>";
                }
                dr.Close();
                con.Close();
            }
            html += "</tbody></table>";
            return (html);
        }
        protected string get_vehicle_activities()
        {
            string systemPrefix = WebConfigurationManager.AppSettings["systemPrefix"];
            String connectionString = ConfigurationManager.ConnectionStrings[systemPrefix + "ConnectionString"].ConnectionString;

            string customer_ctr = Request.Form["customer_ctr"];
            string vehicle_ctr = Request.Form["vehicle_ctr"];
            html = "<table id=\"activitytable\" class=\"table\" style=\"width:100%\">";
            html += "<thead>";
            html += "<tr><th style=\"width:50px;text-align:center\"></th><th>Date</th><th>Detail</th><th style=\"width:100px\">Action / <a class=\"vehicle_activityedit\" link=\"new\" customer=\"" + customer_ctr + "\">Add</a></th></tr>";
            html += "</thead>";
            html += "<tbody>";

            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("get_vehicle_activities", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@customer_ctr", SqlDbType.VarChar).Value = customer_ctr;
                cmd.Parameters.Add("@vehicle_ctr", SqlDbType.VarChar).Value = vehicle_ctr;
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    string vehicle_activity_ctr = dr["vehicle_activity_ctr"].ToString();
                    string vehicle_customer_ctr = dr["customer_ctr"].ToString(); 
                    string date = Functions.formatdate(dr["date"].ToString(),"dd MMM yyyy");
                    string detail = dr["detail"].ToString();

                    html += "<tr>"; // id=\"vehicle_" + customer_vehicle_CTR + "\">";
                    html += "<td style=\"text-align:center\"></td>";
                    html += "<td>" + date + "</td>";
                    html += "<td>" + detail + "</td>";
                    html += "<td><a link=\"" + vehicle_activity_ctr + "\" class=\"vehicle_activityedit\">Edit</td>";
                    html += "</tr>";
                }
                dr.Close();
                con.Close();
            }
            html += "</tbody></table>";
            return (html);
        }

        protected string get_vehicle_followups()
        {
            string systemPrefix = WebConfigurationManager.AppSettings["systemPrefix"];
            String connectionString = ConfigurationManager.ConnectionStrings[systemPrefix + "ConnectionString"].ConnectionString;

            string customer_ctr = Request.Form["customer_ctr"];
            string vehicle_ctr = Request.Form["vehicle_ctr"];
            html = "<table id=\"followuptable\" class=\"table\" style=\"width:100%\">";
            html += "<thead>";
            html += "<tr><th style=\"width:50px;text-align:center\"></th><th>Entry Date</th><th>Followup Date</th><th>Detail</th><th>Actioned</th><th style=\"width:100px\">Action / <a class=\"vehicle_followupedit\" link=\"new\" customer=\"" + customer_ctr + "\">Add</a></th></tr>";
            html += "</thead>";
            html += "<tbody>";

            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("get_vehicle_followups", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@customer_ctr", SqlDbType.VarChar).Value = customer_ctr;
                cmd.Parameters.Add("@vehicle_ctr", SqlDbType.VarChar).Value = vehicle_ctr;
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    string vehicle_followup_ctr = dr["vehicle_followup_ctr"].ToString();
                    string vehicle_customer_ctr = dr["customer_ctr"].ToString();
                    string entrydate = Functions.formatdate(dr["entrydate"].ToString(), "dd MMM yyyy");
                    string followupdate = Functions.formatdate(dr["followupdate"].ToString(), "dd MMM yyyy");
                    string detail = dr["detail"].ToString();
                    string actioneddate = Functions.formatdate(dr["actioneddate"].ToString(), "dd MMM yyyy");
                    //string actioneddetail = dr["actioneddetail"].ToString();

                    html += "<tr>"; // id=\"vehicle_" + customer_vehicle_CTR + "\">";
                    html += "<td style=\"text-align:center\"></td>";
                    html += "<td>" + entrydate + "</td>";
                    html += "<td>" + followupdate + "</td>";
                    html += "<td>" + detail + "</td>";
                    html += "<td>" + actioneddate + "</td>";
                    html += "<td><a link=\"" + vehicle_followup_ctr + "\" class=\"vehicle_followupedit\">Edit</td>";
                    html += "</tr>";
                }
                dr.Close();
                con.Close();
            }
            html += "</tbody></table>";
            return (html);
        }
    }
}
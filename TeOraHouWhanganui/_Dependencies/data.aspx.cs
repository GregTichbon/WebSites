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

namespace TeOraHouWhanganui._Dependencies
{

    public partial class data : System.Web.UI.Page
    {
        public string html = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            string systemPrefix = WebConfigurationManager.AppSettings["systemPrefix"];
            String connectionString = ConfigurationManager.ConnectionStrings[systemPrefix + "ConnectionString"].ConnectionString;
            //SqlConnection con;
            //SqlCommand cmd;
            SqlDataReader dr;

            string mode = Request.Form["mode"];

            switch (mode)
            {
                case "get_vehicle":
                    get_vehicle();
                    break;
                case "pickups":
                    Pickups();
                    break;
                case "get_employee_periods":
                    get_employee_periods();
                    break;

                case "get_roster_workers":
                    get_roster_workers();
                    break;

                case "get_roster_persons":
                    get_roster_persons();
                    break;

                case "eventsearch":
                    using (SqlConnection con = new SqlConnection(connectionString))
                    using (SqlCommand cmd = new SqlCommand("Get_Events", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@program", SqlDbType.VarChar).Value = Request.Form["fld_program"];
                        cmd.Parameters.Add("@date", SqlDbType.VarChar).Value = Request.Form["fld_date"];
                        cmd.Parameters.Add("@description", SqlDbType.VarChar).Value = Request.Form["fld_description"];
                        con.Open();
                        dr = cmd.ExecuteReader();
                        html = "<table class=\"table\"><thead><tr><th nowrap>Program</th><th>Description</th><th>Attendance</th><th nowrap>Date</th><th class=\"event\" eventid=\"new\"><a>Add</a></th></tr></thead><tbody>";
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                string eventid = dr["ID"].ToString();
                                string program = dr["ProgramName"].ToString();
                                string description = dr["description"].ToString();
                                //string date = Functions.formatdate( dr["attendance"].ToString(),"D MMM yyyy");
                                string daterange = dr["daterange"].ToString();
                                string attendance = dr["attendance"].ToString();

                                html += "<tr><td nowrap>" + program + "</td><td>" + description + "</td><td>" + attendance + "</td><td nowrap>" + daterange + "</td><td class=\"event\" eventid=\"" + eventid + "\"><a href=\"javascript: void(0)\">View</a></td></tr>";
                            }
                        }
                        html += "</tbody></table>";
                        dr.Close();
                        con.Close();
                    }




                    /*
                    string event_id = Request.QueryString["event_id"];

                    strConnString = "Data Source=toh-app;Initial Catalog=UBC;Integrated Security=False;user id=OnlineServices;password=Whanganui497";

                    con = new SqlConnection(strConnString);
                    cmd = new SqlCommand("[get_event_person]", con);
                    cmd.Parameters.Add("@event_id", SqlDbType.VarChar).Value = event_id;
                    cmd.Parameters.Add("@mode", SqlDbType.VarChar).Value = "Recorded";

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = con;
                    try
                    {
                        con.Open();
                        dr = cmd.ExecuteReader();
                        if (dr.HasRows)
                        {

                            html = "<table><thead><tr><th>Name</th><th>Attendance</th><th>Note</th><th>Person's Note</th></tr></thead><tbody>";

                            while (dr.Read())
                            {
                                string name = dr["name"].ToString();
                                string attendance = dr["attendance"].ToString();
                                string note = dr["note"].ToString();
                                string personnote = dr["personnote"].ToString();

                                html += "<tr><td>" + name + "</td><td>" + attendance + "</td><td>" + note + "</td><td>" + personnote + "</td></tr>";

                            }
                            html += "</tbody></table>";
                        }
                        dr.Close();
                    }

                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        con.Close();
                        con.Dispose();
                    }
                    */

                    break;
                case "rostersearch":

                    using (SqlConnection con = new SqlConnection(connectionString))
                    using (SqlCommand cmd = new SqlCommand("Get_Rosters", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@date", SqlDbType.VarChar).Value = Request.Form["fld_date"];
                        cmd.Parameters.Add("@detail", SqlDbType.VarChar).Value = Request.Form["fld_detail"];
                        con.Open();
                        dr = cmd.ExecuteReader();
                        html = "<table class=\"table\"><thead><tr><th>Date/Time</th><th nowrap>Detail</th><th class=\"roster\" rosterid=\"new\"><a>Add</a></th></tr></thead><tbody>";
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                string rosterid = dr["roster_ctr"].ToString();
                                string detail = dr["detail"].ToString();
                                string daterange = dr["daterange"].ToString();

                                html += "<tr><td nowrap>" + daterange + "</td><td>" + detail + "</td><td class=\"roster\" rosterid=\"" + rosterid + "\"><a href=\"javascript: void(0)\">View</a></td></tr>";

                            }
                        }
                        html += "</tbody></table>";
                        dr.Close();
                        con.Close();
                    }

                    break;
            }
        }

        protected string get_vehicle()
        {

            string systemPrefix = WebConfigurationManager.AppSettings["systemPrefix"];
            String connectionString = ConfigurationManager.ConnectionStrings[systemPrefix + "ConnectionString"].ConnectionString;

            string vehicle_ctr = Request.Form["vehicle_ctr"];
            string options = Request.Form["options"];

            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("get_vehicle", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@vehicle_ctr", SqlDbType.VarChar).Value = vehicle_ctr;
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    string description = dr["description"].ToString();
                    string wofdue = Functions.formatdate(dr["wofdue"].ToString(), "d MMM yyyy");
                    string registrationdue = Functions.formatdate(dr["registrationdue"].ToString(), "d MMM yyyy");

                    html += description;
                    if(wofdue != "")
                    {
                        html += "\nWOF Due: " + wofdue;
                    }
                    if (registrationdue != "")
                    {
                        html += "\nRegistration Due: " + registrationdue;
                    }
                }
                dr.Close();
                con.Close();
            }
            
            return (html);
        }


        protected string get_employee_periods()
        {

            string systemPrefix = WebConfigurationManager.AppSettings["systemPrefix"];
            String connectionString = ConfigurationManager.ConnectionStrings[systemPrefix + "ConnectionString"].ConnectionString;

            string person_ctr = Request.Form["person_ctr"];
            html = "<table class=\"table table-condensed\" style=\"width:100%\">";
            html += "<thead>";
            html += "<tr><th>Start Date</th><th>End Date</th><th>Position</th><th style=\"width:100px\">Action / <a class=\"employeeperiodedit\" employee_ctr=\"new\">Add</a></th></tr>";
            html += "</thead>";
            html += "<tbody>";

            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("get_employee_periods", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@person_ctr", SqlDbType.VarChar).Value = person_ctr;
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    string employee_CTR = dr["employee_CTR"].ToString();
                    string startdate = Functions.formatdate(dr["startdate"].ToString(), "d MMM yyyy");
                    string enddate = Functions.formatdate(dr["enddate"].ToString(), "d MMM yyyy");
                    string position = dr["position"].ToString();

                    html += "<tr>";
                    html += "<td>" + startdate + "</td>";
                    html += "<td>" + enddate + "</td>";
                    html += "<td>" + position + "</td>";
                    html += "<td><a employee_ctr=\"" + employee_CTR + "\" class=\"employeeperiodedit\">Edit</td>";
                    html += "</tr>";
                }
                dr.Close();
                con.Close();
            }
            html += "</tbody></table>";
            return (html);
        }

        protected string get_roster_workers()
        {

            string systemPrefix = WebConfigurationManager.AppSettings["systemPrefix"];
            String connectionString = ConfigurationManager.ConnectionStrings[systemPrefix + "ConnectionString"].ConnectionString;

            string roster_ctr = Request.Form["id"];
            html = "<h3>Worker</h3><table class=\"table\"><thead>";
            html += "<tr><th>Name</th><th>Date/Time</th><th>Status</th><th>Notes</th><th><a class=\"workeredit\">Add</a></th></tr>";
            html += "</thead>";
            html += "<tbody>";

            using (SqlConnection con = new SqlConnection(connectionString))
            {

                using (SqlCommand cmd = new SqlCommand("get_roster_workers", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@roster_ctr", SqlDbType.VarChar).Value = roster_ctr;

                    con.Open();

                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            string roster_worker_ctr = dr["roster_worker_ctr"].ToString();
                            //string roster_ctr = dr["roster_ctr"].ToString();
                            string worker_ctr = dr["worker_ctr"].ToString();
                            string workername = dr["workername"].ToString();

                            string DateTimeStart = dr["DateTimeStart"].ToString();
                            string DateTimeEnd = dr["DateTimeEnd"].ToString();
                            string Notes = dr["Notes"].ToString();
                            string DateTimeStartActual = dr["DateTimeStartActual"].ToString();
                            string DateTimeEndActual = dr["DateTimeEndActual"].ToString();
                            string Status = dr["Status"].ToString();
                            string WorkNotes = dr["WorkNotes"].ToString();
                            string DateRange = dr["DateRange"].ToString();

                            //string firstevent = Functions.formatdate(dr["firstevent"].ToString(), "dd MMM yyyy");
                            //string lastevent = Functions.formatdate(dr["lastevent"].ToString(), "dd MMM yyyy");


                            html += "<tr data-id=\"" + roster_worker_ctr + "\" worker_ctr=\"" + worker_ctr + "\">";
                            //html += "<td style=\"text-align:center\"></td>";
                            html += "<td>" + workername + "</td>";
                            html += "<td>" + DateRange + "</td>";
                            html += "<td>" + Status + "</td>";
                            html += "<td>" + Notes + "</td>";

                            html += "<td><a class=\"workeredit\">Edit</a></td>";
                            html += "</tr>";
                        }
                    }
                    dr.Close();
                    con.Close();

                }
            }

            html += "</tbody></table>";
            return (html);
        }

        protected string get_roster_persons()
        {

            string systemPrefix = WebConfigurationManager.AppSettings["systemPrefix"];
            String connectionString = ConfigurationManager.ConnectionStrings[systemPrefix + "ConnectionString"].ConnectionString;

            string roster_ctr = Request.Form["id"];
            html = "<h3>Youth</h3><table class=\"table\"><thead>";
            html += "<tr><th>Name</th><th>Date/Time</th><th>Status</th><th>Notes</th><th><a class=\"personedit\">Add</a></th></tr>";
            html += "</thead>";
            html += "<tbody>";

            using (SqlConnection con = new SqlConnection(connectionString))
            {

                using (SqlCommand cmd = new SqlCommand("get_roster_persons", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@roster_ctr", SqlDbType.VarChar).Value = roster_ctr;

                    con.Open();

                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            string roster_person_ctr = dr["roster_person_ctr"].ToString();
                            //string roster_ctr = dr["roster_ctr"].ToString();
                            string person_ctr = dr["person_ctr"].ToString();
                            string personname = dr["personname"].ToString();

                            string DateTimeStart = dr["DateTimeStart"].ToString();
                            string DateTimeEnd = dr["DateTimeEnd"].ToString();
                            string Notes = dr["Notes"].ToString();
                            string DateTimeStartActual = dr["DateTimeStartActual"].ToString();
                            string DateTimeEndActual = dr["DateTimeEndActual"].ToString();
                            string Status = dr["Status"].ToString();
                            string WorkNotes = dr["WorkNotes"].ToString();
                            string DateRange = dr["DateRange"].ToString();

                            //string firstevent = Functions.formatdate(dr["firstevent"].ToString(), "dd MMM yyyy");
                            //string lastevent = Functions.formatdate(dr["lastevent"].ToString(), "dd MMM yyyy");


                            html += "<tr data-id=\"" + roster_person_ctr + "\" person_ctr=\"" + person_ctr + "\">";
                            //html += "<td style=\"text-align:center\"></td>";
                            html += "<td>" + personname + "</td>";
                            html += "<td>" + DateRange + "</td>";
                            html += "<td>" + Status + "</td>";
                            html += "<td>" + Notes + "</td>";

                            html += "<td><a class=\"personedit\">Edit</a></td>";
                            html += "</tr>";
                        }
                    }
                    dr.Close();
                    con.Close();

                }
            }

            html += "</tbody></table>";
            return (html);
        }

        protected string Pickups()
        {

            string systemPrefix = WebConfigurationManager.AppSettings["systemPrefix"];
            String connectionString = ConfigurationManager.ConnectionStrings[systemPrefix + "ConnectionString"].ConnectionString;

            string event_ctr = Request.Form["event_ctr"];
            html = "<table class=\"table table-condensed\" style=\"width:100%\">";
            html += "<thead>";
            html += "<tr><th>Name</th><th>Pickup</th><th>Attendance</th></tr>";
            html += "</thead>";
            html += "<tbody>";

            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("Compare_Pickup_Attendance", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@event_ctr", SqlDbType.VarChar).Value = event_ctr;
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    string name = dr["name"].ToString();
                    string attendance = dr["attendance"].ToString();
                    string pickup = dr["pickup"].ToString();

                    html += "<tr>";
                    html += "<td>" + name + "</td>";
                    html += "<td>" + pickup + "</td>";
                    html += "<td>" + attendance + "</td>";
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
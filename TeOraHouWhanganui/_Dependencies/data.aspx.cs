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
                case "get_employee_periods":
                    get_employee_periods();
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

                        //cmd.ExecuteScalar();
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
                
            }
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
    }
}
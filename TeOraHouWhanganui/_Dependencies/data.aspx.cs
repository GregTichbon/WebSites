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
                case "eventsearch":
                 
                    using (SqlConnection con = new SqlConnection(connectionString))
                    using (SqlCommand cmd = new SqlCommand("Get_Events", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@program", SqlDbType.VarChar).Value = Request.Form["fld_program"];
                        cmd.Parameters.Add("@date", SqlDbType.VarChar).Value = Request.Form["fld_date"];
                        con.Open();
                        dr = cmd.ExecuteReader();
                        if (dr.HasRows)
                        {

                            html = "<table class=\"table\"><thead><tr><th nowrap>Program</th><th>Description</th><th nowrap>Date</th><th></th></tr></thead><tbody>";

                            while (dr.Read())
                            {
                                string eventid = dr["ID"].ToString();
                                string program = dr["ProgramName"].ToString();
                                string description = dr["description"].ToString();
                                //string date = Functions.formatdate( dr["attendance"].ToString(),"D MMM yyyy");
                                string daterange = dr["daterange"].ToString();

                                html += "<tr><td nowrap>" + program + "</td><td>" + description + "</td><td nowrap>" + daterange + "</td><td class=\"event\" eventid=\"" + eventid + "\"><a href=\"javascript: void(0)\">View</a></td></tr>";

                            }
                            html += "</tbody></table>";
                        }
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
    }
}
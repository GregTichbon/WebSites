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
    public partial class EventMaintenance : System.Web.UI.Page
    {
        public string username = "";
        public string event_ctr;
        public string[] fld_programid = new string[1];
        public string fld_description;
        public string fld_startdate;
        public string fld_enddate;
        public string fld_note;
        public string fld_attendanceentrycomplete;
        public string fld_money;
        public string fld_venue;
        public string html_attendance = "";

        public Dictionary<string, string> options = new Dictionary<string, string>();
        public string[] nooptions = { };
        public Dictionary<string, string> programs = new Dictionary<string, string>();

        protected void Page_Load(object sender, EventArgs e)
        {
            // username = System.Security.Principal.WindowsIdentity.GetCurrent().Name.ToString().ToLower();
            username = HttpContext.Current.User.Identity.Name.ToLower();

            if (username == "")
            {
                username = "toh\\gtichbon";   //localhost
            }
            //username = HttpContext.Current.User.Identity.Name.ToLower();
            //Username += "<br />" + Environment.UserName;

            if (!IsPostBack)
            {
                event_ctr = Request.QueryString["id"] ?? "";
                if (event_ctr == "")
                {
                    Response.Redirect("eventsearch.aspx");
                }

                ViewState["event_ctr"] = event_ctr;
                ViewState["returnto"] = Request.QueryString["returnto"] + "";

                string systemPrefix = WebConfigurationManager.AppSettings["systemPrefix"];
                String connectionString = ConfigurationManager.ConnectionStrings[systemPrefix + "ConnectionString"].ConnectionString;

                options.Clear();
                options.Add("storedprocedure", "");
                options.Add("storedprocedurename", "");
                //options.Add("parameters", "|countevents|");
                options.Add("usevalues", "");
                //options.Add("insertblank", "start");
                programs = Functions.buildselectionlist(connectionString, "get_programs", options);

                if (event_ctr != "new")
                {

                    using (SqlConnection con = new SqlConnection(connectionString))
                    {
                        using (SqlCommand cmd = new SqlCommand("get_event", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add("@event_ctr", SqlDbType.VarChar).Value = event_ctr;

                            con.Open();

                            SqlDataReader dr = cmd.ExecuteReader();
                            if (dr.HasRows)
                            {
                                dr.Read();
                                fld_programid[0] = dr["programid"].ToString();
                                fld_description = dr["Description"].ToString();
                                fld_startdate = Functions.formatdate(dr["EventDate"].ToString(), "dd MMM yyyy HH:mm");
                                fld_enddate = Functions.formatdate(dr["EventEndDate"].ToString(), "dd MMM yyyy HH:mm");
                                fld_note = dr["Notes"].ToString();
                                fld_attendanceentrycomplete = dr["AttendanceEntryComplete"].ToString();
                                fld_money = dr["Money"].ToString();
                                fld_venue = dr["venue"].ToString();
                            }
                            dr.Close();
                            con.Close();
                        }

                       
                        html_attendance = "<thead>";
                        //html_attendance += "<tr><th style=\"width:50px;text-align:center\"></th><th>Name</th><th>Attendance</th><th>Capacity</th><th>Note</th><th style=\"width:100px\">Action / <a class=\"enrolmentedit\" data-mode=\"add\" href=\"javascript: void(0)\">Add</a></th></tr>";
                        html_attendance += "<tr><th>Name</th><th>Events</th><th>Attendance</th><th>Capacity</th><th>Note</th></tr>";
                        html_attendance += "</thead>";
                        html_attendance += "<tbody>";



                        using (SqlCommand cmd = new SqlCommand("get_event_attendance", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add("@event_ctr", SqlDbType.VarChar).Value = event_ctr;

                            con.Open();

                            SqlDataReader dr = cmd.ExecuteReader();
                            if (dr.HasRows)
                            {
                                while (dr.Read())
                                {
                                    string name = dr["name"].ToString();
                                    string enrolment_ctr = dr["enrolment_ctr"].ToString();
                                    string enrolmentstatus = dr["enrolmentstatus"].ToString();
                                    string attendance_ctr = dr["attendance_ctr"].ToString();
                                    string attendance = dr["attendance"].ToString();
                                    string notes = dr["notes"].ToString();
                                    string capacity = dr["capacity"].ToString();
                                    string firstevent = Functions.formatdate(dr["firstevent"].ToString(), "dd MMM yyyy");
                                    string lastevent = Functions.formatdate(dr["lastevent"].ToString(), "dd MMM yyyy");


                                    html_attendance += "<tr data-id=\"" + enrolment_ctr + "\" status=\"" + enrolmentstatus + "\">";
                                    //html_attendance += "<td style=\"text-align:center\"></td>";
                                    html_attendance += "<td>" + name + "</td>";
                                    html_attendance += "<td>" + firstevent + " - " + lastevent + "</td>";
                                    html_attendance += "<td>" + attendance + "</td>";
                                    html_attendance += "<td class=\"capacity\">" + capacity + "</td>";
                                    html_attendance += "<td class=\"note\">" + notes + "</td>";
                                    //html_attendance += "<td><a href=\"javascript:void(0)\" class=\"enrolmentedit\" data-mode=\"edit\">Edit</td>";
                                    html_attendance += "</tr>";
                                }
                            }
                            dr.Close();
                            con.Close();

                        }
                    }
                }
            }
        }


        protected void btn_submit_Click(object sender, EventArgs e)
        {
            Boolean Creating = false;
            string event_ctr = ViewState["event_ctr"].ToString();
            if (event_ctr == "new")
            {
                Creating = true;
            }

            string systemPrefix = WebConfigurationManager.AppSettings["systemPrefix"];
            String connectionString = ConfigurationManager.ConnectionStrings[systemPrefix + "ConnectionString"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("update_event", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@event_ctr", SqlDbType.VarChar).Value = event_ctr;
                    cmd.Parameters.Add("@program_ctr", SqlDbType.VarChar).Value = Request.Form["fld_program"];
                    cmd.Parameters.Add("@description", SqlDbType.VarChar).Value = Request.Form["fld_description"];
                    cmd.Parameters.Add("@eventdate", SqlDbType.VarChar).Value = Request.Form["fld_startdate"];
                    cmd.Parameters.Add("@eventenddate", SqlDbType.VarChar).Value = Request.Form["fld_enddate"];
                    cmd.Parameters.Add("@notes", SqlDbType.VarChar).Value = Request.Form["fld_note"];

                    con.Open();
                    event_ctr = cmd.ExecuteScalar().ToString();  
                    con.Close();
                }

                foreach (string key in Request.Form)
                {
                    if (key.StartsWith("update_"))
                    {
                        string[] keyparts = key.Split('_');
                        string field = keyparts[1];
                        string enrolment_ctr = keyparts[2];

                        using (SqlCommand cmd = new SqlCommand("update_attendance_field", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add("@field", SqlDbType.VarChar).Value = field;
                            cmd.Parameters.Add("@event_ctr", SqlDbType.VarChar).Value = event_ctr;
                            cmd.Parameters.Add("@enrolment_ctr", SqlDbType.VarChar).Value = enrolment_ctr;
                            cmd.Parameters.Add("@value", SqlDbType.VarChar).Value = Request.Form[key];

                            con.Open();
                            cmd.ExecuteNonQuery();
                            con.Close();
                        }
                    }
                }
            }
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

        }
    }
}
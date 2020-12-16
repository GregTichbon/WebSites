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
    public partial class ProgramMaintenance : System.Web.UI.Page
    {
        public string username = "";
        public string program_ctr;
        public string fld_program;
        public string fld_description;
        public string ids = "";

        public string html_enrolments = "";

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
                program_ctr = Request.QueryString["id"] ?? "";
                if (program_ctr == "")
                {
                    Response.Redirect("programsearch.aspx");
                }

                ViewState["program_ctr"] = program_ctr;
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

                if (program_ctr != "new")
                {

                    using (SqlConnection con = new SqlConnection(connectionString))
                    {
                        using (SqlCommand cmd = new SqlCommand("get_program", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add("@program_ctr", SqlDbType.VarChar).Value = program_ctr;

                            con.Open();

                            SqlDataReader dr = cmd.ExecuteReader();
                            if (dr.HasRows)
                            {
                                dr.Read();
                                fld_program = dr["programname"].ToString();
                                fld_description = dr["Description"].ToString();

                            }
                            dr.Close();
                            con.Close();
                        }


                        html_enrolments = "<thead>";
                        //html_enrolments += "<tr><th style=\"width:50px;text-align:center\"></th><th>Name</th><th>Attendance</th><th>Capacity</th><th>Note</th><th style=\"width:100px\">Action / <a class=\"enrolmentedit\" data-mode=\"add\" href=\"javascript: void(0)\">Add</a></th></tr>";
                        html_enrolments += "<tr><th style=\"width:50px;text-align:center\"></th><th>Name</th><th>Status</th><th>Worker</th><th>Always Pickup</th><th>Whakapakari</th><th>Note</th><th>Action / <a class=\"enrolmentedit\" data-mode=\"add\">Add</a></th></tr>";
                        //html_enrolments += "<tr><th>Name</th></tr>";
                        html_enrolments += "</thead>";
                        html_enrolments += "<tbody>";


                        html_enrolments += "<tr style=\"display:none\">";
                        html_enrolments += "<td style=\"text-align:center\"></td>";
                        html_enrolments += "<td><input class=\"name\" type=\"text\" /></td>"; //Program
                        //html_enrolments += "<td></td>"; //First Event
                        //html_enrolments += "<td></td>"; //Last Event
                        html_enrolments += "<td></td>"; //Status
                        html_enrolments += "<td></td>"; //Worker
                        html_enrolments += "<td></td>"; //Always pickup
                        html_enrolments += "<td></td>"; //Whakapakari
                        html_enrolments += "<td></td>"; //Note
                        html_enrolments += "<td><a class=\"enrolmentedit\" data-mode=\"edit\">Edit</td>";
                        html_enrolments += "</tr>";

                        

                        using (SqlCommand cmd = new SqlCommand("get_program_enrolments", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add("@program_ctr", SqlDbType.VarChar).Value = program_ctr;

                            con.Open();

                            SqlDataReader dr = cmd.ExecuteReader();
                            if (dr.HasRows)
                            {
                                string delim = "";
                                while (dr.Read())
                                {
                                    string name = dr["name"].ToString();
                                    string enrolment_ctr = dr["id"].ToString();
                                    string entity_ctr = dr["entityid"].ToString();
                                    string enrolmentstatus = dr["enrolementstatus"].ToString();
                                    string worker = dr["worker"].ToString();
                                    string alwayspickup = dr["alwayspickup"].ToString();
                                    string whakapakari = dr["whakapakari"].ToString();
                                    string note = dr["notes"].ToString();
                                    string attendance = dr["attendance"].ToString();

                                    ids += delim + entity_ctr;
                                    delim = ",";

                                    html_enrolments += "<tr data-id=\"" + enrolment_ctr + "\" status=\"" + enrolmentstatus + "\">";
                                    html_enrolments += "<td style=\"text-align:center\"></td>";
                                    html_enrolments += "<td><a class=\"entity\" data-id=\"" + entity_ctr + "\">" + name + "</a> (" + attendance + ")</td>";
                                    html_enrolments += "<td>" + enrolmentstatus + "</td>";
                                    html_enrolments += "<td>" + worker + "</td>";
                                    html_enrolments += "<td>" + alwayspickup + "</td>";
                                    html_enrolments += "<td>" + whakapakari + "</td>";
                                    html_enrolments += "<td>" + note + "</td>";

                                    html_enrolments += "<td><a class=\"enrolmentedit\" data-mode=\"edit\">Edit</td>";
                                    html_enrolments += "</tr>";
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
            string program_ctr = ViewState["program_ctr"].ToString();
            if (program_ctr == "new")
            {
                Creating = true;
            }

            string systemPrefix = WebConfigurationManager.AppSettings["systemPrefix"];
            String connectionString = ConfigurationManager.ConnectionStrings[systemPrefix + "ConnectionString"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("update_program", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@program_ctr", SqlDbType.VarChar).Value = program_ctr;
                    cmd.Parameters.Add("@programname", SqlDbType.VarChar).Value = Request.Form["fld_programname"];
                    cmd.Parameters.Add("@description", SqlDbType.VarChar).Value = Request.Form["fld_description"];

                    con.Open();
                    program_ctr = cmd.ExecuteScalar().ToString();
                    con.Close();
                }

                foreach (string key in Request.Form)
                {
                    if (key.StartsWith("name_"))
                    {
                        string[] keyparts = key.Split('_');
                        string id = keyparts[1];
                        if(id.StartsWith("new"))
                        {
                            id = "new";
                        }

                        using (SqlCommand cmd = new SqlCommand("update_person_enrolment", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add("@enrolment_ctr", SqlDbType.VarChar).Value = id;
                            cmd.Parameters.Add("@programid", SqlDbType.VarChar).Value = program_ctr;
                            cmd.Parameters.Add("@person_ctr", SqlDbType.VarChar).Value = Request.Form[key];
                            cmd.Parameters.Add("@status", SqlDbType.VarChar).Value = "Current";

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
                    returnto = "programmaintenance.aspx?id=" + program_ctr;
                }
                else
                {
                    returnto = "programmaintenance.aspx?id=" + program_ctr + "&returnto=" + returnto + ".aspx";
                }
            }
            else
            {
                if (returnto == "")
                {
                    returnto = "programsearch.aspx";
                }
            }

            Response.Redirect(returnto);

        }
    }
}
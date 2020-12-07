using Generic;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TeOraHouWhanganui.Private
{
    public partial class Roster : System.Web.UI.Page
    {
        public string username = "";
        public string roster_ctr;
        public string fld_detail;
        public string fld_datetimestart;
        public string fld_datetimeend;

        public string html_workers;
        public string html_persons;

        public Dictionary<string, string> options = new Dictionary<string, string>();
        public Dictionary<string, string> workers = new Dictionary<string, string>();

        public string[] nooptions = { };

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
                roster_ctr = Request.QueryString["id"] ?? "";
                if (roster_ctr == "")
                {
                    Response.Redirect("rostersearch.aspx");
                }

                ViewState["roster_ctr"] = roster_ctr;
                ViewState["returnto"] = Request.QueryString["returnto"] + "";

                string systemPrefix = WebConfigurationManager.AppSettings["systemPrefix"];
                String connectionString = ConfigurationManager.ConnectionStrings[systemPrefix + "ConnectionString"].ConnectionString;

                /*
                options.Clear();
                options.Add("storedprocedure", "");
                options.Add("storedprocedurename", "");
                //options.Add("parameters", "|countevents|");
                options.Add("usevalues", "");
                //options.Add("insertblank", "start");
                programs = Functions.buildselectionlist(connectionString, "get_rosters", options);
                */
                if (roster_ctr != "new")
                {

                    options.Clear();
                    options.Add("storedprocedure", "");
                    options.Add("storedprocedurename", "");
                    options.Add("usevalues", "");
                    //options.Add("insertblank", "start");
                    workers = Functions.buildselectionlist(connectionString, "get_workers", options);

                    using (SqlConnection con = new SqlConnection(connectionString))
                    {
                        using (SqlCommand cmd = new SqlCommand("get_roster", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add("@roster_ctr", SqlDbType.VarChar).Value = roster_ctr;

                            con.Open();

                            SqlDataReader dr = cmd.ExecuteReader();
                            if (dr.HasRows)
                            {
                                dr.Read();
                                fld_detail = dr["detail"].ToString();
                                fld_datetimestart = Functions.formatdate(dr["datetimestart"].ToString(), "dd MMM yyyy HH:mm");
                                fld_datetimeend = Functions.formatdate(dr["datetimeend"].ToString(), "dd MMM yyyy HH:mm");

                            }
                            dr.Close();
                            con.Close();
                        }
                    }
                    /*
                    NameValueCollection values = new NameValueCollection();
                    values.Add("mode", "get_roster_workers");
                    values.Add("id", roster_ctr);


                    string Url = HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Authority + "/_Dependencies/data.aspx";
                    Url = "/_Dependencies/data.aspx";
                    using (WebClient client = new WebClient())
                    {
                        client.UseDefaultCredentials = true;
                        client.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                        byte[] result = client.UploadValues(Url, "POST", values);
                        html_workers = System.Text.Encoding.UTF8.GetString(result);
                    }


                    values.Clear();
                    values.Add("mode", "get_roster_persons");
                    values.Add("id", roster_ctr);

                    using (WebClient client = new WebClient())
                    {
                        client.UseDefaultCredentials = true;
                        client.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                        byte[] result = client.UploadValues(Url, "POST", values);
                        html_persons = System.Text.Encoding.UTF8.GetString(result);
                    }
                    */
                }
            }
        }


        protected void btn_submit_Click(object sender, EventArgs e)
        {
            Boolean Creating = false;
            string roster_ctr = ViewState["roster_ctr"].ToString();
            if (roster_ctr == "new")
            {
                Creating = true;
            }

            string systemPrefix = WebConfigurationManager.AppSettings["systemPrefix"];
            String connectionString = ConfigurationManager.ConnectionStrings[systemPrefix + "ConnectionString"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("update_roster", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@roster_ctr", SqlDbType.VarChar).Value = roster_ctr;
                    cmd.Parameters.Add("@detail", SqlDbType.VarChar).Value = Request.Form["fld_detail"];
                    cmd.Parameters.Add("@datetimestart", SqlDbType.VarChar).Value = Request.Form["fld_datetimestart"];
                    cmd.Parameters.Add("@datetimeend", SqlDbType.VarChar).Value = Request.Form["fld_datetimeend"];

                    con.Open();
                    roster_ctr = cmd.ExecuteScalar().ToString();
                    con.Close();
                }
                /*
                foreach (string key in Request.Form)
                {
                    if (key.StartsWith("update_"))
                    {
                        string[] keyparts = key.Split('_');
                        string field = keyparts[1];
                        string enrolment_ctr = keyparts[2];

                        using (SqlCommand cmd = new SqlCommand("update_roster_worker", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add("@field", SqlDbType.VarChar).Value = field;
                            cmd.Parameters.Add("@roster_ctr", SqlDbType.VarChar).Value = roster_ctr;
                            cmd.Parameters.Add("@enrolment_ctr", SqlDbType.VarChar).Value = enrolment_ctr;
                            cmd.Parameters.Add("@value", SqlDbType.VarChar).Value = Request.Form[key];

                            con.Open();
                            cmd.ExecuteNonQuery();
                            con.Close();
                        }
                    }
                }
                */
            }
            string returnto = ViewState["returnto"].ToString();

            if (Creating)
            {
                if (returnto == "")
                {
                    returnto = "rostermaintenance.aspx?id=" + roster_ctr;
                }
                else
                {
                    returnto = "rostermaintenance.aspx?id=" + roster_ctr + "&returnto=" + returnto + ".aspx";
                }
            }
            else
            {
                if (returnto == "")
                {
                    returnto = "rostersearch.aspx";
                }
            }

            Response.Redirect(returnto);

        }
    }
}
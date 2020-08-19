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


                if (event_ctr != "new")
                {
                    SqlConnection con = new SqlConnection(connectionString);

                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "get_event";
                    cmd.Parameters.Add("@event_ctr", SqlDbType.VarChar).Value = event_ctr;

                    cmd.Connection = con;
                    con.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        dr.Read();
                        //event_ctr = dr["event_ctr"].ToString();
                        //fld_firstname = dr["firstname"].ToString();

                        //fld_birthdate = Functions.formatdate(dr["DateofBirth"].ToString(), "dd MMM yyyy");

                    }
                    dr.Close();
                    {
                    }
                    cmd.CommandText = "get_entity_enrolments";
                    cmd.Parameters.Clear();
                    cmd.Parameters.Add("@event_ctr", SqlDbType.VarChar).Value = event_ctr;
                    dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        string entity_enrolment_CTR = dr["ID"].ToString();
                    }

                    dr.Close();
                }
            }
        }
    }
}
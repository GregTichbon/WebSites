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

namespace TeOraHouWhanganui.Community.Reports
{
    public partial class ListAll_Updates : System.Web.UI.Page
    {
        public string Username = "";
        public string html = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            //Username = System.Security.Principal.WindowsIdentity.GetCurrent().Name.ToString().ToLower();
            Username = HttpContext.Current.User.Identity.Name.ToLower();
            //Username += "<br />" + Environment.UserName;

            string systemPrefix = "Community"; // WebConfigurationManager.AppSettings["systemPrefix"];
                                               //string systemPrefix = WebConfigurationManager.AppSettings["systemPrefix"];
            String connectionString = ConfigurationManager.ConnectionStrings[systemPrefix + "ConnectionString"].ConnectionString;


            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "List_All";
            cmd.Parameters.Add("@username", SqlDbType.VarChar).Value = Username;

            cmd.Connection = con;
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                string person_ctr = dr["person_ctr"].ToString();
                string name = dr["personname"].ToString();
                string contactname = dr["contactname"].ToString();
                string guid = dr["guid"].ToString();
                string update_contact = dr["updatedby"].ToString();
                string update_datetime = Functions.formatdate(dr["DateTime"].ToString(), "dd MMM yyyy HH:mm");
                string followupdate = Functions.formatdate(dr["FollowupDate"].ToString(), "dd MMM yyyy");
                string followupaction = dr["followupaction"].ToString();
                string followupdone = Functions.formatdate(dr["followupdone"].ToString(), "dd MMM yyyy HH:mm");
                string updatenote = dr["updatenote"].ToString();

                html += "<tr><td><a target=\"person\" href=\"../person.aspx?id=" + guid + "\">" + name + "</a></td><td>" + contactname + "</td><td>" + update_datetime + "</td><td>" + update_contact + "</td><td>" + updatenote + "</td><td>" + followupdate + "</td><td>" + followupaction + "</td><td>" + followupdone + "</td></tr>";

            }
            dr.Close();
            cmd.Dispose();
            con.Close();
            con.Dispose();
        }
    }
}
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


namespace TeOraHouWhanganui.Login
{
    public partial class Default : System.Web.UI.Page
    {
        public String systemPrefix;
        protected void Page_Load(object sender, EventArgs e)
        {
            systemPrefix = WebConfigurationManager.AppSettings["systemPrefix"];

            Session.Remove(systemPrefix + "_entity_ctr");
            Session.Remove(systemPrefix + "_name");
            Session.Remove(systemPrefix + "_emailaddress");
        }

        protected void btn_submit_Click(object sender, EventArgs e)
        {
            String connectionString = ConfigurationManager.ConnectionStrings[systemPrefix + "ConnectionString"].ConnectionString;
            SqlConnection con = new SqlConnection(connectionString);

            SqlCommand cmd = new SqlCommand("login", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@emailaddress", SqlDbType.VarChar).Value = Request.Form["fld_emailaddress"].Trim();
            cmd.Parameters.Add("@password", SqlDbType.VarChar).Value = Request.Form["fld_password"].Trim();

            cmd.Connection = con;
            try
            {
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    dr.Read();
                    Session[systemPrefix + "_entity_ctr"] = dr["entity_CTR"].ToString();
                    Session[systemPrefix + "_name"] = dr["name"].ToString();
                    Session[systemPrefix + "_emailaddress"] = dr["emailaddress"].ToString();
                }
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
            if (Session[systemPrefix + "_entity_ctr"] != null)
            {
                string destination = Request.QueryString["destination"] ?? "";
                if (destination != "")
                {
                    string redirect = "~/" + destination;
                    Response.Redirect(redirect);
                }
                else
                {
                    Response.Redirect("~/default.aspx");
                }
            }
            else
            {
                Session["message_title"] = "Log in";
                Session["message_head"] = "Access denied";
                Session["message_message"] = "Invalid email address / password combination";
                Session["message_redirect"] = "login.aspx?" + Request.QueryString.ToString();
                Response.Redirect("../message.aspx");
            }
        }
    }
}
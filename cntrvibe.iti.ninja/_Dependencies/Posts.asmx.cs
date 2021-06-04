using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Services;
using System.Threading;

namespace cntrvibe.iti.ninja._Dependencies
{
    /// <summary>
    /// Summary description for Posts
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class Posts : System.Web.Services.WebService
    {
        [WebMethod]
        public void update_registration(NameValue[] formVars)
        {
            //dynamic response = new JObject();

            string systemPrefix = WebConfigurationManager.AppSettings["systemPrefix"];
            String connectionString = ConfigurationManager.ConnectionStrings[systemPrefix + "ConnectionString"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("update_registration", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@type", SqlDbType.VarChar).Value = formVars.Form("fld_type");
                cmd.Parameters.Add("@groupname", SqlDbType.VarChar).Value = formVars.Form("fld_groupname");
                cmd.Parameters.Add("@groupnumber", SqlDbType.VarChar).Value = formVars.Form("fld_groupnumber");
                cmd.Parameters.Add("@firstname", SqlDbType.VarChar).Value = formVars.Form("fld_firstname");
                cmd.Parameters.Add("@surname", SqlDbType.VarChar).Value = formVars.Form("fld_surname");
                cmd.Parameters.Add("@emailaddress", SqlDbType.VarChar).Value = formVars.Form("fld_emailaddress");
                cmd.Parameters.Add("@contact", SqlDbType.VarChar).Value = formVars.Form("fld_contact");
                cmd.Parameters.Add("@description", SqlDbType.VarChar).Value = formVars.Form("fld_description");
                cmd.Parameters.Add("@requirements", SqlDbType.VarChar).Value = formVars.Form("fld_requirements");
                cmd.Parameters.Add("@otherinformation", SqlDbType.VarChar).Value = formVars.Form("fld_otherinformation");
                cmd.Parameters.Add("@declaration", SqlDbType.VarChar).Value = formVars.Form("fld_declaration");

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }

            //string x = formVars.Form("test1");
            //response.test = x;
            //Context.Response.Write(response);
            //return (response.ToString() );
        }
    }

    public class NameValue
    {
        public string name { get; set; }
        public string value { get; set; }
    }
    public static class NameValueExtensionMethods
    {
        /// <summary>
        /// Retrieves a single form variable from the list of
        /// form variables stored
        /// </summary>
        /// <param name="formVars"></param>
        /// <param name="name">formvar to retrieve</param>
        /// <returns>value or string.Empty if not found</returns>
        public static string Form(this NameValue[] formVars, string name)
        {
            var matches = formVars.Where(nv => nv.name.ToLower() == name.ToLower()).FirstOrDefault();
            if (matches != null)
                return matches.value;
            return string.Empty;
        }

        /// <summary>
        /// Retrieves multiple selection form variables from the list of 
        /// form variables stored.
        /// </summary>
        /// <param name="formVars"></param>
        /// <param name="name">The name of the form var to retrieve</param>
        /// <returns>values as string[] or null if no match is found</returns>
        public static string[] FormMultiple(this NameValue[] formVars, string name)
        {
            var matches = formVars.Where(nv => nv.name.ToLower() == name.ToLower()).Select(nv => nv.value).ToArray();
            if (matches.Length == 0)
                return null;
            return matches;
        }
    }
}

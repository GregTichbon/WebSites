using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Services;

namespace VehicleService._Dependencies
{
    /// <summary>
    /// Summary description for Data
    /// </summary>
    /// GREG [WebService(Namespace = "http://tempuri.org/")]
    /// GREG [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    /// GREG [System.ComponentModel.ToolboxItem(false)]
    /// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]   //GREG  -  THIS IS REQUIRED FOR POSTS
    public class Posts : System.Web.Services.WebService
    {

        [WebMethod]
        public void update_vehicle(NameValue[] formVars)
        {
            //dynamic response = new JObject();

            string systemPrefix = WebConfigurationManager.AppSettings["systemPrefix"];
            String connectionString = ConfigurationManager.ConnectionStrings[systemPrefix + "ConnectionString"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("update_vehicle", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@customer_vehicle_ctr", SqlDbType.VarChar).Value = formVars.Form("customer_vehicle_ctr");
                cmd.Parameters.Add("@vehicle_ctr", SqlDbType.VarChar).Value = formVars.Form("vehicle_ctr");
                cmd.Parameters.Add("@customer_ctr", SqlDbType.VarChar).Value = formVars.Form("customer_ctr");
                cmd.Parameters.Add("@registration", SqlDbType.VarChar).Value = formVars.Form("vehicle_registration");  
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }

            string x = formVars.Form("test1");
            //response.test = x;
            //Context.Response.Write(response);
            //return (response.ToString() );
        }
    }
    
}

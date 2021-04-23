using Newtonsoft.Json.Linq;
using System;
using myGeneric = Generic;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Services;
using System.Threading;

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
        public void update_customer(NameValue[] formVars)
        {
            //dynamic response = new JObject();

            string systemPrefix = WebConfigurationManager.AppSettings["systemPrefix"];
            String connectionString = ConfigurationManager.ConnectionStrings[systemPrefix + "ConnectionString"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("update_customer", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@customer_ctr", SqlDbType.VarChar).Value = formVars.Form("customer_ctr");
                cmd.Parameters.Add("@Name", SqlDbType.VarChar).Value = formVars.Form("customer_name");
                cmd.Parameters.Add("@FirstName", SqlDbType.VarChar).Value = formVars.Form("customer_FirstName");
                cmd.Parameters.Add("@Surname", SqlDbType.VarChar).Value = formVars.Form("customer_Surname");
                cmd.Parameters.Add("@Address", SqlDbType.VarChar).Value = formVars.Form("customer_Address");
                cmd.Parameters.Add("@EmailAddress", SqlDbType.VarChar).Value = formVars.Form("customer_EmailAddress");
                cmd.Parameters.Add("@MobilePhone", SqlDbType.VarChar).Value = formVars.Form("customer_MobilePhone");
                cmd.Parameters.Add("@HomePhone", SqlDbType.VarChar).Value = formVars.Form("customer_HomePhone");
                cmd.Parameters.Add("@WorkPhone", SqlDbType.VarChar).Value = formVars.Form("customer_WorkPhone");
                cmd.Parameters.Add("@CustomerType_CTR", SqlDbType.VarChar).Value = formVars.Form("customer_CustomerType");
                cmd.Parameters.Add("@note", SqlDbType.VarChar).Value = formVars.Form("customer_note");
                cmd.Parameters.Add("@knownas", SqlDbType.VarChar).Value = formVars.Form("customer_knownas");
      

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }

            //string x = formVars.Form("test1");
            //response.test = x;
            //Context.Response.Write(response);
            //return (response.ToString() );
        }

        [WebMethod]
        public void support(NameValue[] formVars)
        {
            myGeneric.Functions gFunctions = new myGeneric.Functions();

            string host = "smtp.office365.com";
            string emailfrom = "noreply@teorahou.org.nz"; // "noreply@teorahou.org.nz";
            string password = "WhanganuiInc1998"; // "Whanganui1998";
            int port = 587;
            Boolean enableSsl = true;
            string emailfromname = "Support Request";
            string emailBCC = "";
            string emailRecipient = "greg@datainn.co.nz";
            string emailsubject = "Support Request - " + formVars.Form("supportusername") + " : " + formVars.Form("supporturl");
            string emailhtml = "<html><head></head><body>";
            emailhtml += formVars.Form("supportmessage");
            emailhtml += "</body></html>";
            string[] attachments = new string[0];
            Dictionary<string, string> emailoptions = new Dictionary<string, string>();


            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                gFunctions.sendemailV5(host, port, enableSsl, emailfrom, emailfromname, password, emailsubject, emailhtml, emailRecipient, emailBCC, "", attachments, emailoptions);
            }).Start();



        }



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
                cmd.Parameters.Add("@note", SqlDbType.VarChar).Value = formVars.Form("vehicle_note");
                cmd.Parameters.Add("@description", SqlDbType.VarChar).Value = formVars.Form("vehicle_description");
                cmd.Parameters.Add("@wof_cycle", SqlDbType.VarChar).Value = formVars.Form("vehicle_wof_cycle");
                cmd.Parameters.Add("@vehiclemodel_ctr", SqlDbType.VarChar).Value = formVars.Form("vehicle_vehiclemodel");
                cmd.Parameters.Add("@wof_due", SqlDbType.VarChar).Value = formVars.Form("vehicle_wof_due");
                cmd.Parameters.Add("@year", SqlDbType.VarChar).Value = formVars.Form("vehicle_year");
                cmd.Parameters.Add("@odometer", SqlDbType.VarChar).Value = formVars.Form("vehicle_odometer");
                cmd.Parameters.Add("@vehicletype_ctr", SqlDbType.VarChar).Value = formVars.Form("vehicle_vehicletype");
                cmd.Parameters.Add("@registration_due", SqlDbType.VarChar).Value = formVars.Form("vehicle_registration_due");
                


                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }

            //string x = formVars.Form("test1");
            //response.test = x;
            //Context.Response.Write(response);
            //return (response.ToString() );
        }


        [WebMethod]
        public void update_vehicle_activity(NameValue[] formVars)
        {
            //dynamic response = new JObject();

            string systemPrefix = WebConfigurationManager.AppSettings["systemPrefix"];
            String connectionString = ConfigurationManager.ConnectionStrings[systemPrefix + "ConnectionString"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("update_vehicle_activity", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@vehicle_activity_ctr", SqlDbType.VarChar).Value = formVars.Form("vehicle_activity_ctr");
                cmd.Parameters.Add("@vehicle_ctr", SqlDbType.VarChar).Value = formVars.Form("vehicle_ctr");
                cmd.Parameters.Add("@date", SqlDbType.VarChar).Value = formVars.Form("vehicle_activity_date");
                cmd.Parameters.Add("@detail", SqlDbType.VarChar).Value = formVars.Form("vehicle_activity_detail");
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }

            //string x = formVars.Form("test1");
            //response.test = x;
            //Context.Response.Write(response);
            //return (response.ToString() );
        }



        [WebMethod]
        public void update_vehicle_followup(NameValue[] formVars)
        {
            //dynamic response = new JObject();

            string systemPrefix = WebConfigurationManager.AppSettings["systemPrefix"];
            String connectionString = ConfigurationManager.ConnectionStrings[systemPrefix + "ConnectionString"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("update_vehicle_followup", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@vehicle_followup_ctr", SqlDbType.VarChar).Value = formVars.Form("vehicle_followup_ctr");
                cmd.Parameters.Add("@vehicle_ctr", SqlDbType.VarChar).Value = formVars.Form("vehicle_ctr");
                cmd.Parameters.Add("@entrydate", SqlDbType.VarChar).Value = formVars.Form("vehicle_followup_entrydate");
                cmd.Parameters.Add("@detail", SqlDbType.VarChar).Value = formVars.Form("vehicle_followup_detail");
                cmd.Parameters.Add("@followupdate", SqlDbType.VarChar).Value = formVars.Form("vehicle_followup_followupdate");
                cmd.Parameters.Add("@actioneddate", SqlDbType.VarChar).Value = formVars.Form("vehicle_followup_actioneddate");
                cmd.Parameters.Add("@actioneddetail", SqlDbType.VarChar).Value = formVars.Form("vehicle_followup_actioneddetail");
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }

            //string x = formVars.Form("test1");
            //response.test = x;
            //Context.Response.Write(response);
            //return (response.ToString() );
        }

        [WebMethod]
        public void delete_vehicle_followup(NameValue[] formVars)
        {
            //dynamic response = new JObject();

            string systemPrefix = WebConfigurationManager.AppSettings["systemPrefix"];
            String connectionString = ConfigurationManager.ConnectionStrings[systemPrefix + "ConnectionString"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("delete_vehicle_followup", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@vehicle_followup_ctr", SqlDbType.VarChar).Value = formVars.Form("vehicle_followup_ctr");
               
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }

        }

    }
    
}

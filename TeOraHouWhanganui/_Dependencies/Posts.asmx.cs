using System;
using myGeneric = Generic;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Script.Serialization;
using System.Web.Services;
using Newtonsoft.Json.Linq;

namespace TeOraHouWhanganui._Dependencies
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
        public string update_vehicle_booking(NameValue[] formVars)
        {
            myGeneric.Functions gFunctions = new myGeneric.Functions();

            //dynamic response = new JObject();
            //vehicle_booking_ctr, vehicle_ctr, start, end, worker_ctr, details
            string systemPrefix = WebConfigurationManager.AppSettings["systemPrefix"];
            String connectionString = ConfigurationManager.ConnectionStrings[systemPrefix + "ConnectionString"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("update_vehicle_booking", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@vehicle_booking_ctr", SqlDbType.VarChar).Value = formVars.Form("vehicle_booking_ctr");
                cmd.Parameters.Add("@vehicle_ctr", SqlDbType.VarChar).Value = formVars.Form("vehicle_ctr");
                cmd.Parameters.Add("@start", SqlDbType.VarChar).Value = formVars.Form("start");
                cmd.Parameters.Add("@end", SqlDbType.VarChar).Value = formVars.Form("end");
                cmd.Parameters.Add("@worker_ctr", SqlDbType.VarChar).Value = formVars.Form("worker_ctr");
                cmd.Parameters.Add("@details", SqlDbType.VarChar).Value = formVars.Form("details");
                con.Open();


                SqlDataReader dr = cmd.ExecuteReader();

                dr.Read();
                //if ((int)formVars.Form("vehicle_booking_ctr") > 0)

                string mode = dr["mode"].ToString();

                string id = dr["Vehicle_Booking_CTR"].ToString();
                string StartDateTime = myGeneric.Functions.formatdate(dr["StartDateTime"].ToString(), "d MMM yyyy HH:mm");
                string EndDateTime = myGeneric.Functions.formatdate(dr["EndDateTime"].ToString(), "d MMM yyyy HH:mm");
                string detail = dr["detail"].ToString();
                string name = dr["name"].ToString();
                string Registration = dr["Registration"].ToString();
                string Worker = dr["Worker"].ToString();

                string extraemail = "";
                if (mode == "Update")
                {
                    string OLDStartDateTime = myGeneric.Functions.formatdate(dr["OLDStartDateTime"].ToString(), "d MMM yyyy HH:mm");
                    string OLDEndDateTime = myGeneric.Functions.formatdate(dr["OLDEndDateTime"].ToString(), "d MMM yyyy HH:mm");
                    string OLDdetail = dr["OLDdetail"].ToString();
                    string OLDname = dr["OLDname"].ToString();
                    string OLDRegistration = dr["OLDRegistration"].ToString();
                    string OLDWorker = dr["OLDWorker"].ToString();

                    extraemail += "<br /><br /><hr /><b>Original Booking</b><br />" + OLDname + " " + OLDRegistration + " <br />" + OLDStartDateTime + " - " + OLDEndDateTime + "<br />" + OLDWorker + "<br />" + OLDdetail;


                }

                dr.Close();

                string host = "smtp.office365.com";
                string emailfrom = "noreply@teorahou.org.nz"; // "noreply@teorahou.org.nz";
                string password = "Whanganui1998"; // "Whanganui1998";
                int port = 587;
                Boolean enableSsl = true;
                string emailfromname = "Vehicle booking";
                string emailBCC = "";
                string emailRecipient = "whanganui@teorahou.org.nz";
                string emailsubject = "Vehicle booking - " + mode;
                string emailhtml = "<html><head></head><body>";
                emailhtml += name + " " + Registration + " <br />" + StartDateTime + " - " + EndDateTime + "<br />" + Worker + "<br />" + detail;
                emailhtml += extraemail;
                emailhtml += "</body></html>";
                string[] attachments = new string[0];
                Dictionary<string, string> emailoptions = new Dictionary<string, string>();

                gFunctions.sendemailV5(host, port, enableSsl, emailfrom, emailfromname, password, emailsubject, emailhtml, emailRecipient, emailBCC, "", attachments, emailoptions);
                //sendemailV5(string host, int port, Boolean enableSsl, string emailfrom, string emailfromname, string password, string emailsubject, string emailhtml, string emailRecipient, string emailbcc, string replyto, string[] attachments, Dictionary<string, string> options)
                return id;
                //Context.Response.Write(response);
                //JavaScriptSerializer JS = new JavaScriptSerializer();
                //string passresult = JS.Serialize(response);

                //Context.Response.Write(passresult);
            }
        }

        [WebMethod]
        public void update_roster_worker(NameValue[] formVars)
        {
            string systemPrefix = WebConfigurationManager.AppSettings["systemPrefix"];
            String connectionString = ConfigurationManager.ConnectionStrings[systemPrefix + "ConnectionString"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("update_roster_worker", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@roster_worker_ctr", SqlDbType.VarChar).Value = formVars.Form("roster_worker_ctr");
                cmd.Parameters.Add("@roster_ctr", SqlDbType.VarChar).Value = formVars.Form("roster_ctr");
                cmd.Parameters.Add("@worker_ctr", SqlDbType.VarChar).Value = formVars.Form("fld_worker_worker");
                cmd.Parameters.Add("@datetimestart", SqlDbType.VarChar).Value = formVars.Form("fld_worker_datetimestart");
                cmd.Parameters.Add("@datetimeend", SqlDbType.VarChar).Value = formVars.Form("fld_worker_datetimeend");
                cmd.Parameters.Add("@note", SqlDbType.VarChar).Value = formVars.Form("fld_worker_note");
                cmd.Parameters.Add("@status", SqlDbType.VarChar).Value = formVars.Form("fld_worker_status");
                cmd.Parameters.Add("@datetimestartactual", SqlDbType.VarChar).Value = formVars.Form("fld_worker_datetimestartactual");
                cmd.Parameters.Add("@datetimeendactual", SqlDbType.VarChar).Value = formVars.Form("fld_worker_datetimeendactual");
                cmd.Parameters.Add("@worknote", SqlDbType.VarChar).Value = formVars.Form("fld_worker_worknote");
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        [WebMethod]
        public void update_roster_person(NameValue[] formVars)
        {
            string systemPrefix = WebConfigurationManager.AppSettings["systemPrefix"];
            String connectionString = ConfigurationManager.ConnectionStrings[systemPrefix + "ConnectionString"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("update_roster_person", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@roster_person_ctr", SqlDbType.VarChar).Value = formVars.Form("roster_person_ctr");
                cmd.Parameters.Add("@roster_ctr", SqlDbType.VarChar).Value = formVars.Form("roster_ctr");
                cmd.Parameters.Add("@person_ctr", SqlDbType.VarChar).Value = formVars.Form("person_ctr");
                cmd.Parameters.Add("@datetimestart", SqlDbType.VarChar).Value = formVars.Form("fld_person_datetimestart");
                cmd.Parameters.Add("@datetimeend", SqlDbType.VarChar).Value = formVars.Form("fld_person_datetimeend");
                cmd.Parameters.Add("@note", SqlDbType.VarChar).Value = formVars.Form("fld_person_note");
                cmd.Parameters.Add("@status", SqlDbType.VarChar).Value = formVars.Form("fld_person_status");
                cmd.Parameters.Add("@datetimestartactual", SqlDbType.VarChar).Value = formVars.Form("fld_person_datetimestartactual");
                cmd.Parameters.Add("@datetimeendactual", SqlDbType.VarChar).Value = formVars.Form("fld_person_datetimeendactual");
                cmd.Parameters.Add("@worknote", SqlDbType.VarChar).Value = formVars.Form("fld_person_worknote");
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        [WebMethod]
        public void update_employee(NameValue[] formVars)
        {
            foreach(NameValue x in formVars)
            {
                string z = x.name + "=" + x.value;
            }
            //dynamic response = new JObject();

            string systemPrefix = WebConfigurationManager.AppSettings["systemPrefix"];
            String connectionString = ConfigurationManager.ConnectionStrings[systemPrefix + "ConnectionString"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("update_employee", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@employee_ctr", SqlDbType.VarChar).Value = formVars.Form("fld_employee_ctr");
                cmd.Parameters.Add("@person_ctr", SqlDbType.VarChar).Value = formVars.Form("fld_person_ctr");
                cmd.Parameters.Add("@startdate", SqlDbType.VarChar).Value = formVars.Form("fld_startdate");
                cmd.Parameters.Add("@enddate", SqlDbType.VarChar).Value = formVars.Form("fld_enddate");
                cmd.Parameters.Add("@position", SqlDbType.VarChar).Value = formVars.Form("fld_position");
                cmd.Parameters.Add("@note", SqlDbType.VarChar).Value = formVars.Form("fld_note");
                cmd.Parameters.Add("@hoursperweek", SqlDbType.VarChar).Value = formVars.Form("fld_hoursperweek");
                cmd.Parameters.Add("@hourlyrate", SqlDbType.VarChar).Value = formVars.Form("fld_hourlyrate");
                cmd.Parameters.Add("@paytype", SqlDbType.VarChar).Value = formVars.Form("fld_paytype");

                cmd.Parameters.Add("@hoursMon", SqlDbType.VarChar).Value = formVars.Form("fld_hoursMon");
                cmd.Parameters.Add("@hoursTue", SqlDbType.VarChar).Value = formVars.Form("fld_hoursTue");
                cmd.Parameters.Add("@hoursWed", SqlDbType.VarChar).Value = formVars.Form("fld_hoursWed");
                cmd.Parameters.Add("@hoursThu", SqlDbType.VarChar).Value = formVars.Form("fld_hoursThu");
                cmd.Parameters.Add("@hoursFri", SqlDbType.VarChar).Value = formVars.Form("fld_hoursFri");
                cmd.Parameters.Add("@hoursSat", SqlDbType.VarChar).Value = formVars.Form("fld_hoursSat");
                cmd.Parameters.Add("@hoursSun", SqlDbType.VarChar).Value = formVars.Form("fld_hoursSun");

                cmd.Parameters.Add("@lunchhoursMon", SqlDbType.VarChar).Value = formVars.Form("fld_lunchhoursMon");
                cmd.Parameters.Add("@lunchhoursTue", SqlDbType.VarChar).Value = formVars.Form("fld_lunchhoursTue");
                cmd.Parameters.Add("@lunchhoursWed", SqlDbType.VarChar).Value = formVars.Form("fld_lunchhoursWed");
                cmd.Parameters.Add("@lunchhoursThu", SqlDbType.VarChar).Value = formVars.Form("fld_lunchhoursThu");
                cmd.Parameters.Add("@lunchhoursFri", SqlDbType.VarChar).Value = formVars.Form("fld_lunchhoursFri");
                cmd.Parameters.Add("@lunchhoursSat", SqlDbType.VarChar).Value = formVars.Form("fld_lunchhoursSat");
                cmd.Parameters.Add("@lunchhoursSun", SqlDbType.VarChar).Value = formVars.Form("fld_lunchhoursSun");

                cmd.Parameters.Add("@dinnerhoursMon", SqlDbType.VarChar).Value = formVars.Form("fld_dinnerhoursMon");
                cmd.Parameters.Add("@dinnerhoursTue", SqlDbType.VarChar).Value = formVars.Form("fld_dinnerhoursTue");
                cmd.Parameters.Add("@dinnerhoursWed", SqlDbType.VarChar).Value = formVars.Form("fld_dinnerhoursWed");
                cmd.Parameters.Add("@dinnerhoursThu", SqlDbType.VarChar).Value = formVars.Form("fld_dinnerhoursThu");
                cmd.Parameters.Add("@dinnerhoursFri", SqlDbType.VarChar).Value = formVars.Form("fld_dinnerhoursFri");
                cmd.Parameters.Add("@dinnerhoursSat", SqlDbType.VarChar).Value = formVars.Form("fld_dinnerhoursSat");
                cmd.Parameters.Add("@dinnerhoursSun", SqlDbType.VarChar).Value = formVars.Form("fld_dinnerhoursSun");

                cmd.Parameters.Add("@ordinarystartMon", SqlDbType.VarChar).Value = formVars.Form("fld_ordinarystartMon");
                cmd.Parameters.Add("@ordinarystartTue", SqlDbType.VarChar).Value = formVars.Form("fld_ordinarystartTue");
                cmd.Parameters.Add("@ordinarystartWed", SqlDbType.VarChar).Value = formVars.Form("fld_ordinarystartWed");
                cmd.Parameters.Add("@ordinarystartThu", SqlDbType.VarChar).Value = formVars.Form("fld_ordinarystartThu");
                cmd.Parameters.Add("@ordinarystartFri", SqlDbType.VarChar).Value = formVars.Form("fld_ordinarystartFri");
                cmd.Parameters.Add("@ordinarystartSat", SqlDbType.VarChar).Value = formVars.Form("fld_ordinarystartSat");
                cmd.Parameters.Add("@ordinarystartSun", SqlDbType.VarChar).Value = formVars.Form("fld_ordinarystartSun");

                cmd.Parameters.Add("@ordinaryendMon", SqlDbType.VarChar).Value = formVars.Form("fld_ordinaryendMon");
                cmd.Parameters.Add("@ordinaryendTue", SqlDbType.VarChar).Value = formVars.Form("fld_ordinaryendTue");
                cmd.Parameters.Add("@ordinaryendWed", SqlDbType.VarChar).Value = formVars.Form("fld_ordinaryendWed");
                cmd.Parameters.Add("@ordinaryendThu", SqlDbType.VarChar).Value = formVars.Form("fld_ordinaryendThu");
                cmd.Parameters.Add("@ordinaryendFri", SqlDbType.VarChar).Value = formVars.Form("fld_ordinaryendFri");
                cmd.Parameters.Add("@ordinaryendSat", SqlDbType.VarChar).Value = formVars.Form("fld_ordinaryendSat");
                cmd.Parameters.Add("@ordinaryendSun", SqlDbType.VarChar).Value = formVars.Form("fld_ordinaryendSun");

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
}

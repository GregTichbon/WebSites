using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Services;

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

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Web.Services;
using Newtonsoft.Json.Linq;
using Generic;

namespace TeOraHouWhanganui._Dependencies
{
    /// <summary>
    /// Summary description for Data
    /// </summary>
    //[WebService(Namespace = "http://tempuri.org/")]
    //[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    //[System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class Data : System.Web.Services.WebService
    {
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void get_employee(string id)
        {
            dynamic employee = new JObject();

            string systemPrefix = WebConfigurationManager.AppSettings["systemPrefix"];
            String connectionString = ConfigurationManager.ConnectionStrings[systemPrefix + "ConnectionString"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("get_employee", con))


            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@employee_ctr", SqlDbType.VarChar).Value = id;

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    dr.Read();
                    employee.position = dr["Position"].ToString();
                    employee.startdate = Functions.formatdate(dr["startdate"].ToString(), "dd MMM yyyy");
                    employee.enddate = Functions.formatdate(dr["enddate"].ToString(), "dd MMM yyyy");
                    employee.hoursperweek = dr["hoursperweek"].ToString();
                    employee.note = dr["note"].ToString();
                    employee.hourlyrate = dr["hourlyrate"].ToString();
                    employee.paytype = dr["paytype"].ToString();

                    employee.hoursMon = dr["hoursMon"].ToString();
                    employee.hoursTue = dr["hoursTue"].ToString();
                    employee.hoursWed = dr["hoursWed"].ToString();
                    employee.hoursThu = dr["hoursThu"].ToString();
                    employee.hoursFri = dr["hoursFri"].ToString();
                    employee.hoursSat = dr["hoursSat"].ToString();
                    employee.hoursSun = dr["hoursSun"].ToString();
                    employee.lunchhoursMon = dr["lunchhoursMon"].ToString();
                    employee.lunchhoursTue = dr["lunchhoursTue"].ToString();
                    employee.lunchhoursWed = dr["lunchhoursWed"].ToString();
                    employee.lunchhoursThu = dr["lunchhoursThu"].ToString();
                    employee.lunchhoursFri = dr["lunchhoursFri"].ToString();
                    employee.lunchhoursSat = dr["lunchhoursSat"].ToString();
                    employee.lunchhoursSun = dr["lunchhoursSun"].ToString();
                    employee.dinnerhoursMon = dr["dinnerhoursMon"].ToString();
                    employee.dinnerhoursTue = dr["dinnerhoursTue"].ToString();
                    employee.dinnerhoursWed = dr["dinnerhoursWed"].ToString();
                    employee.dinnerhoursThu = dr["dinnerhoursThu"].ToString();
                    employee.dinnerhoursFri = dr["dinnerhoursFri"].ToString();
                    employee.dinnerhoursSat = dr["dinnerhoursSat"].ToString();
                    employee.dinnerhoursSun = dr["dinnerhoursSun"].ToString();
                    employee.ordinarystartMon = dr["ordinarystartMon"].ToString();
                    employee.ordinarystartTue = dr["ordinarystartTue"].ToString();
                    employee.ordinarystartWed = dr["ordinarystartWed"].ToString();
                    employee.ordinarystartThu = dr["ordinarystartThu"].ToString();
                    employee.ordinarystartFri = dr["ordinarystartFri"].ToString();
                    employee.ordinarystartSat = dr["ordinarystartSat"].ToString();
                    employee.ordinarystartSun = dr["ordinarystartSun"].ToString();
                    employee.ordinaryendMon = dr["ordinaryendMon"].ToString();
                    employee.ordinaryendTue = dr["ordinaryendTue"].ToString();
                    employee.ordinaryendWed = dr["ordinaryendWed"].ToString();
                    employee.ordinaryendThu = dr["ordinaryendThu"].ToString();
                    employee.ordinaryendFri = dr["ordinaryendFri"].ToString();
                    employee.ordinaryendSat = dr["ordinaryendSat"].ToString();
                    employee.ordinaryendSun = dr["ordinaryendSun"].ToString();

                }
                else
                {
                    Console.WriteLine("No rows found.");
                }
                dr.Close();
                Context.Response.Write(employee);

            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void Person_name_autocomplete(string term, string options)
        {
            List<PersonClass> PersonList = new List<PersonClass>();

            string systemPrefix = WebConfigurationManager.AppSettings["systemPrefix"];
            String connectionString = ConfigurationManager.ConnectionStrings[systemPrefix + "ConnectionString"].ConnectionString;
            SqlConnection con = new SqlConnection(connectionString);

            SqlCommand cmd = new SqlCommand("person_name_autocomplete", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@search", SqlDbType.VarChar).Value = term;
            cmd.Parameters.Add("@options", SqlDbType.VarChar).Value = options;

            cmd.Connection = con;
            try
            {
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        PersonList.Add(new PersonClass
                        {
                            person_ctr = dr["person_ctr"].ToString(),
                            name = dr["name"].ToString(),
                            label = dr["name"].ToString(),
                            value = dr["person_ctr"].ToString()
                        });
                    }

                    dr.Close();
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

            JavaScriptSerializer JS = new JavaScriptSerializer();
            Context.Response.Write(JS.Serialize(PersonList));

        }
        public class PersonClass
        {
            public string person_ctr;
            public string name;
            public string label;
            public string value;

        }
    }
}

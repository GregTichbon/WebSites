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
        public void get_roster(string program)
        {
            //http://localhost:58611/_Dependencies/data.asmx/get_vehicle_bookings?start=2020-12-24T00%3A00%3A00%2B13%3A00&end=2020-12-25T00%3A00%3A00%2B13%3A00
            JArray bookings = new JArray();

            string systemPrefix = WebConfigurationManager.AppSettings["systemPrefix"];
            String connectionString = ConfigurationManager.ConnectionStrings[systemPrefix + "ConnectionString"].ConnectionString;
            /*
            SqlConnection con = new SqlConnection(connectionString);

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "get_vehicle_bookings";
            //cmd.Parameters.Add("@vehicle_ctr", SqlDbType.VarChar).Value = id;

            cmd.Connection = con;
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                dynamic booking = new JObject();
                booking.id = dr["vehicle_booking_ctr"].ToString();
                booking.resourceId = dr["vehicle_ctr"].ToString();
                booking.title = dr["name"].ToString() + " - " + dr["detail"].ToString();
                booking.start = Convert.ToDateTime(dr["startdatetime"]).ToString("yyyy-MM-ddTHH:mm:ss");
                booking.end = Convert.ToDateTime(dr["enddatetime"]).ToString("yyyy-MM-ddTHH:mm:ss");
                booking.worker = dr["name"].ToString();
                booking.worker_ctr = dr["worker_ctr"].ToString();
                booking.detail = dr["detail"].ToString();
                bookings.Add(booking);

            }
            dr.Close();

            */
             
            dynamic booking1 = new JObject();
            booking1.id = "1";
            booking1.resourceId = "2";
            booking1.title = "Greg - Mentoring";
            booking1.start = DateTime.Today.ToString("yyyy-MM-ddT02:00:00");
            booking1.end = DateTime.Today.ToString("yyyy-MM-ddT07:00:00");
            booking1.worker = "Greg";
            booking1.worker_ctr = 27;
            booking1.detail = "Mentoring";
            bookings.Add(booking1);

            dynamic booking2 = new JObject();
            booking2.id = "2";
            booking2.resourceId = "3";
            booking2.title = "Keegan - Rally car racing";
            booking2.start = DateTime.Today.ToString("yyyy-MM-ddT06:00:00");
            booking2.end = DateTime.Today.ToString("yyyy-MM-ddT09:00:00");
            booking2.name = "Keegan";
            booking2.worker_ctr = 99;
            booking2.detail = "Rally car racing";
            bookings.Add(booking2);
           

            /*
             * { id: '1', resourceId: '2', start: '2020-12-23T02:00:00', end: '2020-12-23T07:00:00', title: 'Greg - Mentoring' },
                    { id: '2', resourceId: '3', start: '2020-12-23T06:00:00', end: '2020-12-23T09:00:00', title: 'Keegan - Rally car racing' },
                    */

            Context.Response.Write(bookings);

        }
        
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void get_entitys_for_program(string program, string worker, string start, string end)
        {

            JArray vehicles = new JArray();

            string systemPrefix = WebConfigurationManager.AppSettings["systemPrefix"];
            String connectionString = ConfigurationManager.ConnectionStrings[systemPrefix + "ConnectionString"].ConnectionString;

            SqlConnection con = new SqlConnection(connectionString);

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "Get_Entitys_for_Program";
            cmd.Parameters.Add("@start", SqlDbType.VarChar).Value = start;
            cmd.Parameters.Add("@end", SqlDbType.VarChar).Value = end;
            cmd.Parameters.Add("@program", SqlDbType.VarChar).Value = program;
            cmd.Parameters.Add("@worker", SqlDbType.VarChar).Value = worker;

            cmd.Connection = con;
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                dynamic vehicle = new JObject();
                vehicle.id = dr["vehicle_ctr"].ToString();
                vehicle.title = dr["name"].ToString();
                vehicle.sequence = Convert.ToInt32(dr["sequence"]).ToString("000");
                vehicles.Add(vehicle);

            }
            dr.Close();

            Context.Response.Write(vehicles);

        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void get_vehicles()
        {
 
            JArray vehicles = new JArray();

            string systemPrefix = WebConfigurationManager.AppSettings["systemPrefix"];
            String connectionString = ConfigurationManager.ConnectionStrings[systemPrefix + "ConnectionString"].ConnectionString;

            SqlConnection con = new SqlConnection(connectionString);

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "get_vehicles";
            //cmd.Parameters.Add("@vehicle_ctr", SqlDbType.VarChar).Value = id;

            cmd.Connection = con;
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                dynamic vehicle = new JObject();
                vehicle.id = dr["vehicle_ctr"].ToString();
                vehicle.title = dr["name"].ToString();
                vehicle.sequence = Convert.ToInt32(dr["sequence"]).ToString("000");
                vehicles.Add(vehicle);

            }
            dr.Close();

            Context.Response.Write(vehicles);

        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void get_vehicle_bookings()
        {
            //http://localhost:58611/_Dependencies/data.asmx/get_vehicle_bookings?start=2020-12-24T00%3A00%3A00%2B13%3A00&end=2020-12-25T00%3A00%3A00%2B13%3A00
            JArray bookings = new JArray();

            string systemPrefix = WebConfigurationManager.AppSettings["systemPrefix"];
            String connectionString = ConfigurationManager.ConnectionStrings[systemPrefix + "ConnectionString"].ConnectionString;

            SqlConnection con = new SqlConnection(connectionString);

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "get_vehicle_bookings";
            //cmd.Parameters.Add("@vehicle_ctr", SqlDbType.VarChar).Value = id;

            cmd.Connection = con;
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                dynamic booking = new JObject();
                booking.id = dr["vehicle_booking_ctr"].ToString();
                booking.resourceId = dr["vehicle_ctr"].ToString();
                booking.title = dr["name"].ToString() + " - " + dr["detail"].ToString();
                booking.start = Convert.ToDateTime(dr["startdatetime"]).ToString("yyyy-MM-ddTHH:mm:ss");
                booking.end = Convert.ToDateTime(dr["enddatetime"]).ToString("yyyy-MM-ddTHH:mm:ss");
                booking.worker = dr["name"].ToString();
                booking.worker_ctr = dr["worker_ctr"].ToString();
                booking.detail = dr["detail"].ToString();
                bookings.Add(booking);

            }
            dr.Close();
            /*
            dynamic booking1 = new JObject();
            booking1.id = "1";
            booking1.resourceId = "2";
            booking1.title = "Greg - Mentoring";
            booking1.start = DateTime.Today.ToString("yyyy-MM-ddT02:00:00");
            booking1.end = DateTime.Today.ToString("yyyy-MM-ddT07:00:00");
            booking1.worker = "Greg";
            booking1.worker_ctr = 27;
            booking1.detail = "Mentoring";
            bookings.Add(booking1);

            dynamic booking2 = new JObject();
            booking2.id = "2";
            booking2.resourceId = "3";
            booking2.title = "Keegan - Rally car racing";
            booking2.start = DateTime.Today.ToString("yyyy-MM-ddT06:00:00");
            booking2.end = DateTime.Today.ToString("yyyy-MM-ddT09:00:00");
            booking2.name = "Keegan";
            booking2.worker_ctr = 99;
            booking2.detail = "Rally car racing";
            bookings.Add(booking2);
            */

            /*
             * { id: '1', resourceId: '2', start: '2020-12-23T02:00:00', end: '2020-12-23T07:00:00', title: 'Greg - Mentoring' },
                    { id: '2', resourceId: '3', start: '2020-12-23T06:00:00', end: '2020-12-23T09:00:00', title: 'Keegan - Rally car racing' },
                    */

            Context.Response.Write(bookings);

        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void get_roster_worker(string id)
        {
            dynamic roster_worker = new JObject();
         
            string systemPrefix = WebConfigurationManager.AppSettings["systemPrefix"];
            String connectionString = ConfigurationManager.ConnectionStrings[systemPrefix + "ConnectionString"].ConnectionString;

            SqlConnection con = new SqlConnection(connectionString);

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "get_roster_worker";
            cmd.Parameters.Add("@roster_worker_ctr", SqlDbType.VarChar).Value = id; 

            cmd.Connection = con;
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                dr.Read();
                roster_worker.Roster_CTR = dr["Roster_CTR"].ToString();
                roster_worker.Worker_CTR = dr["Worker_CTR"].ToString();
                roster_worker.DateTimeStart = Functions.formatdate(dr["DateTimeStart"].ToString(), "dd MMM yyyy HH:mm");
                roster_worker.DateTimeEnd = Functions.formatdate(dr["DateTimeEnd"].ToString(), "dd MMM yyyy HH:mm");
                roster_worker.Note = dr["Notes"].ToString();
                roster_worker.DateTimeStartActual = Functions.formatdate(dr["DateTimeStartActual"].ToString(), "dd MMM yyyy HH:mm");
                roster_worker.DateTimeEndActual = Functions.formatdate(dr["DateTimeEndActual"].ToString(), "dd MMM yyyy HH:mm");
                roster_worker.Status = dr["Status"].ToString();
                roster_worker.WorkNotes = dr["WorkNotes"].ToString();
                roster_worker.workername = dr["workername"].ToString();
                roster_worker.DateRange = dr["DateRange"].ToString();
            }
            dr.Close();
            
            Context.Response.Write(roster_worker);

        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void get_roster_person(string id)
        {
            dynamic roster_person = new JObject();

            string systemPrefix = WebConfigurationManager.AppSettings["systemPrefix"];
            String connectionString = ConfigurationManager.ConnectionStrings[systemPrefix + "ConnectionString"].ConnectionString;

            SqlConnection con = new SqlConnection(connectionString);

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "get_roster_person";
            cmd.Parameters.Add("@roster_person_ctr", SqlDbType.VarChar).Value = id;

            cmd.Connection = con;
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                dr.Read();
                roster_person.Roster_CTR = dr["Roster_CTR"].ToString();
                roster_person.Person_CTR = dr["person_CTR"].ToString();
                roster_person.DateTimeStart = Functions.formatdate(dr["DateTimeStart"].ToString(), "dd MMM yyyy HH:mm");
                roster_person.DateTimeEnd = Functions.formatdate(dr["DateTimeEnd"].ToString(), "dd MMM yyyy HH:mm");
                roster_person.Note = dr["Notes"].ToString();
                roster_person.DateTimeStartActual = Functions.formatdate(dr["DateTimeStartActual"].ToString(), "dd MMM yyyy HH:mm");
                roster_person.DateTimeEndActual = Functions.formatdate(dr["DateTimeEndActual"].ToString(), "dd MMM yyyy HH:mm");
                roster_person.Status = dr["Status"].ToString();
                roster_person.WorkNotes = dr["WorkNotes"].ToString();
                roster_person.personname = dr["personname"].ToString();
                roster_person.DateRange = dr["DateRange"].ToString();
            }
            dr.Close();

            Context.Response.Write(roster_person);

        }

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

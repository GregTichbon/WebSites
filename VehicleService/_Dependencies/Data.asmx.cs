using Generic;
using Newtonsoft.Json.Linq;
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
        //public List<CustomerClass> Customer_name_autocomplete(string term, string options)
        public void Customer_name_autocomplete(string term, string options)
        {
            List<CustomerClass> CustomerList = new List<CustomerClass>();

            string systemPrefix = WebConfigurationManager.AppSettings["systemPrefix"];
            String connectionString = ConfigurationManager.ConnectionStrings[systemPrefix + "ConnectionString"].ConnectionString;
            SqlConnection con = new SqlConnection(connectionString);

            SqlCommand cmd = new SqlCommand("Customer_name_autocomplete", con);
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
                        CustomerList.Add(new CustomerClass
                        {
                            customer_ctr = dr["Customer_ctr"].ToString(),
                            name = dr["name"].ToString(),
                            label = dr["name"].ToString(),
                            value = dr["Customer_ctr"].ToString()
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
            //return CustomerList;
            JavaScriptSerializer JS = new JavaScriptSerializer();
            Context.Response.Write(JS.Serialize(CustomerList));

        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        //public List<CustomerClass> Customer_name_autocomplete(string term, string options)
        public void get_customer(string id)
        {
            dynamic customer = new JObject();

            string systemPrefix = WebConfigurationManager.AppSettings["systemPrefix"];
            String connectionString = ConfigurationManager.ConnectionStrings[systemPrefix + "ConnectionString"].ConnectionString;

            SqlConnection con = new SqlConnection(connectionString);

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "get_customer";
            cmd.Parameters.Add("@customer_ctr", SqlDbType.VarChar).Value = id;

            cmd.Connection = con;
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                dr.Read();
                //fld_customertype_ctr[0] = dr["customertype_ctr"].ToString();
                customer.name = dr["fullname"].ToString();
                customer.firstname = dr["firstname"].ToString();
                customer.surname = dr["surname"].ToString();
                customer.knownas = dr["knownas"].ToString();
                customer.address = dr["address"].ToString();
                customer.emailaddress = dr["emailaddress"].ToString();
                customer.mobilephone = dr["mobilephone"].ToString();
                customer.homephone = dr["homephone"].ToString();
                customer.workphone = dr["workphone"].ToString();
                customer.note = dr["note"].ToString();
                customer.customertype_ctr = dr["customertype_ctr"].ToString();
                customer.xeroid = dr["xeroid"].ToString();
                customer.guid = dr["guid"].ToString();
            }
            dr.Close();
            Context.Response.Write(customer);

        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        //public List<CustomerClass> Customer_name_autocomplete(string term, string options)
        public void get_customer_vehicle(string id)
        {
            dynamic vehicle = new JObject();

            string systemPrefix = WebConfigurationManager.AppSettings["systemPrefix"];
            String connectionString = ConfigurationManager.ConnectionStrings[systemPrefix + "ConnectionString"].ConnectionString;

            SqlConnection con = new SqlConnection(connectionString);

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "get_customer_vehicle";
            cmd.Parameters.Add("@customer_vehicle_ctr", SqlDbType.VarChar).Value = id;

            cmd.Connection = con;
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                dr.Read();
                //fld_customertype_ctr[0] = dr["customertype_ctr"].ToString();
                vehicle.vehicle_ctr = dr["vehicle_ctr"].ToString();
                vehicle.registration = dr["registration"].ToString();
                vehicle.description = dr["description"].ToString();
                vehicle.vehiclenote = dr["vehiclenote"].ToString();
                vehicle.vehiclemodel = dr["vehiclemodel_ctr"].ToString();
                vehicle.vehicletype = dr["vehicletype_ctr"].ToString();
                vehicle.wof_cycle = dr["wof_cycle"].ToString();
                vehicle.wof_due = Functions.formatdate(dr["wof_due"].ToString(), "dd MMM yyyy");
                vehicle.registration_due = Functions.formatdate(dr["registration_due"].ToString(), "dd MMM yyyy");
                vehicle.year = dr["year"].ToString();
                vehicle.odometer = dr["odometer"].ToString();
                /*
                vehicle.address = dr["address"].ToString();
                vehicle.emailaddress = dr["emailaddress"].ToString();
                vehicle.mobilephone = dr["mobilephone"].ToString();
                vehicle.homephone = dr["homephone"].ToString();
                vehicle.workphone = dr["workphone"].ToString();
                vehicle.note = dr["note"].ToString();
                vehicle.xeroid = dr["xeroid"].ToString();
                */
            }
            dr.Close();
            Context.Response.Write(vehicle);

        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        //public List<CustomerClass> Customer_name_autocomplete(string term, string options)
        public void get_vehicle_activity(string id)
        {
            dynamic vehicle_activity = new JObject();

            string systemPrefix = WebConfigurationManager.AppSettings["systemPrefix"];
            String connectionString = ConfigurationManager.ConnectionStrings[systemPrefix + "ConnectionString"].ConnectionString;

            SqlConnection con = new SqlConnection(connectionString);

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "get_vehicle_activity";
            cmd.Parameters.Add("@vehicle_activity_ctr", SqlDbType.VarChar).Value = id;

            cmd.Connection = con;
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                dr.Read();
                //fld_customertype_ctr[0] = dr["customertype_ctr"].ToString();
                vehicle_activity.vehicle_ctr = dr["vehicle_ctr"].ToString();
                vehicle_activity.date = Functions.formatdate(dr["date"].ToString(),"dd MMM yyyy");
                vehicle_activity.detail = dr["detail"].ToString();
            }
            dr.Close();
            Context.Response.Write(vehicle_activity);

        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        //public List<CustomerClass> Customer_name_autocomplete(string term, string options)
        public void get_vehicle_followup(string id)
        {
            dynamic vehicle_followup = new JObject();

            string systemPrefix = WebConfigurationManager.AppSettings["systemPrefix"];
            String connectionString = ConfigurationManager.ConnectionStrings[systemPrefix + "ConnectionString"].ConnectionString;

            SqlConnection con = new SqlConnection(connectionString);

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "get_vehicle_followup";
            cmd.Parameters.Add("@vehicle_followup_ctr", SqlDbType.VarChar).Value = id;

            cmd.Connection = con;
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                dr.Read();
                //fld_customertype_ctr[0] = dr["customertype_ctr"].ToString();
                vehicle_followup.vehicle_ctr = dr["vehicle_ctr"].ToString();
                vehicle_followup.entrydate = Functions.formatdate(dr["entrydate"].ToString(), "dd MMM yyyy");
                vehicle_followup.detail = dr["detail"].ToString();
                vehicle_followup.followupdate = Functions.formatdate(dr["followupdate"].ToString(), "dd MMM yyyy");
                vehicle_followup.actioneddate = Functions.formatdate(dr["actioneddate"].ToString(), "dd MMM yyyy");
                vehicle_followup.actioneddetail = dr["actioneddetail"].ToString();
            }
            dr.Close();
            Context.Response.Write(vehicle_followup);

        }

        public class CustomerClass
        {
            public string customer_ctr;
            public string name;
            public string label;
            public string value;

        }
    }
}

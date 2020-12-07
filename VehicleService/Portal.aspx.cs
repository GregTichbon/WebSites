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

namespace VehicleService
{
    public partial class Portal : System.Web.UI.Page
    {
        public string customer_ctr;
        public string name;
        public string firstname;
        public string surname;
        public string knownas;
        public string address;
        public string emailaddress;
        public string mobilephone;
        public string homephone;

        public string html = "";


        protected void Page_Load(object sender, EventArgs e)
        {
            string id = Request.QueryString["ID"] ?? "";

            if (id != "")
            {
                string systemPrefix = WebConfigurationManager.AppSettings["systemPrefix"];
                String connectionString = ConfigurationManager.ConnectionStrings[systemPrefix + "ConnectionString"].ConnectionString;

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("get_customer", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@guid", SqlDbType.VarChar).Value = id;
                        
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr.HasRows)
                        {
                            dr.Read();

                            customer_ctr = dr["customer_ctr"].ToString();
                            name = dr["name"].ToString();
                            firstname = dr["firstname"].ToString();
                            surname = dr["surname"].ToString();
                            knownas = dr["knownas"].ToString();
                            address = dr["address"].ToString();
                            emailaddress = dr["emailaddress"].ToString();
                            mobilephone = dr["mobilephone"].ToString();
                            homephone = dr["homephone"].ToString();

                            html += "<b>name:" + name + "</b><br/>";
                            html += "<b>firstname:" + firstname + "</b><br/>";
                            html += "<b>surname:" + surname + "</b><br/>";
                            html += "<b>knownas:" + knownas + "</b><br/>";
                            html += "<b>address:" + address + "</b><br/>";
                            html += "<b>emailaddress:" + emailaddress + "</b><br/>";
                            html += "<b>mobilephone:" + mobilephone + "</b><br/>";
                            html += "<b>homephone:" + homephone + "<br/><hr/><br/>";

                        }
                        dr.Close();
                    }
                    using (SqlCommand cmd = new SqlCommand("get_customer_vehicles", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@customer_ctr", SqlDbType.VarChar).Value = customer_ctr;
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                string customer_vehicle_ctr = dr["customer_vehicle_ctr"].ToString();
                                string vehicle_ctr = dr["vehicle_ctr"].ToString();
                                string registration = dr["registration"].ToString();
                                string description = dr["description"].ToString();
                                string model = dr["model"].ToString();
                                string make = dr["make"].ToString();
                                string type = dr["type"].ToString();
                                string year = dr["year"].ToString();
                                string wof_cycle = dr["wof_cycle"].ToString();
                                string wof_due = dr["wof_due"].ToString();
                                string registration_due = dr["registration_due"].ToString();
                                string odometer = dr["odometer"].ToString();

                                html += "<b>registration:" + registration + "</b><br/>";
                                html += "description:" + description + "<br/>";
                                html += "model:" + model + "<br/>";
                                html += "make:" + make + "<br/>";
                                html += "type:" + type + "<br/>";
                                html += "year:" + year + "<br/>";
                                html += "wof_cycle:" + wof_cycle + "<br/>";
                                html += "wof_due:" + wof_due + "<br/>";
                                html += "registration_due:" + registration_due + "<br/>";
                                html += "odometer:" + odometer + "<br/><hr/><br/>";

                            }
                        }
                        dr.Close();
                    }
                }
            }
        }
    }
}
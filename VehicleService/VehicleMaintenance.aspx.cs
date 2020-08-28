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
    public partial class VehicleMaintenance : System.Web.UI.Page
    {
        public string username = "";
        public string vehicle_ctr;
        public string[] fld_vehicletype_ctr = new string[1];
        public string[] fld_vehiclemodel_ctr = new string[1];
        public string xxxxxxx;

        public string customername;

        public string html_activity;

        public Dictionary<string, string> options = new Dictionary<string, string>();
        public string[] nooptions = { };
        public Dictionary<string, string> wofcycles = new Dictionary<string, string>();
        public Dictionary<string, string> vehicletypes = new Dictionary<string, string>();
        public Dictionary<string, string> vehiclemodels = new Dictionary<string, string>();


        protected void Page_Load(object sender, EventArgs e)
        {
            // username = System.Security.Principal.WindowsIdentity.GetCurrent().Name.ToString().ToLower();
            username = HttpContext.Current.User.Identity.Name.ToLower();

            if (username == "")
            {
                username = "toh\\gtichbon";   //localhost
            }
            //username = HttpContext.Current.User.Identity.Name.ToLower();
            //Username += "<br />" + Environment.UserName;

            if (!IsPostBack)
            {
                vehicle_ctr = Request.QueryString["id"] ?? "";
                if (vehicle_ctr == "")
                {
                    Response.Redirect("vehiclesearch.aspx");
                }

                ViewState["vehicle_ctr"] = vehicle_ctr;
                ViewState["returnto"] = Request.QueryString["returnto"] + "";

                string systemPrefix = WebConfigurationManager.AppSettings["systemPrefix"];
                String connectionString = ConfigurationManager.ConnectionStrings[systemPrefix + "ConnectionString"].ConnectionString;

                options.Clear();
                options.Add("storedprocedure", "");
                options.Add("storedprocedurename", "");
                //options.Add("parameters", "|countcustomers|");
                options.Add("usevalues", "");
                //options.Add("insertblank", "start");
                vehicletypes = Functions.buildselectionlist(connectionString, "get_vehicletypes", options);

                options.Clear();
                options.Add("storedprocedure", "");
                options.Add("storedprocedurename", "");
                //options.Add("parameters", "|countcustomers|");
                options.Add("usevalues", "");
                //options.Add("insertblank", "start");
                vehiclemodels = Functions.buildselectionlist(connectionString, "get_vehiclemodels", options);


                if (vehicle_ctr != "new")
                {
                    wofcycles.Add("6 monthly", "");
                    wofcycles.Add("Annually", "");

                    using (SqlConnection con = new SqlConnection(connectionString))
                    {
                        using (SqlCommand cmd = new SqlCommand("get_vehicle", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add("@vehicle_ctr", SqlDbType.VarChar).Value = vehicle_ctr;
                            con.Open();
                            SqlDataReader dr = cmd.ExecuteReader();
                            if (dr.HasRows)
                            {
                                dr.Read();
                                fld_vehicletype_ctr[0] = dr["customertype_ctr"].ToString();
                                customername = dr["customername"].ToString();
                                xxxxxxx = dr["name"].ToString();
                            }
                            dr.Close();
                        }

                        #region ACTIVITY
                        //-------------------------------ACTIVITY TAB------------------------------------------------------
                        //if (Functions.accessstringtest(Session["UBC_AccessString"].ToString(), "1"))
                        //{

                        //html_tab += "<li><a data-target=\"#div_vehicles\">Activity</a></li>";
                        html_activity = "<thead>";
                        html_activity += "<tr><th style=\"width:50px;text-align:center\"></th><th>Registration</th><th>Make/Model</th><th>Description</th><th>WOF Cycle</th><th>WOF Due</th><th>Note</th><th style=\"width:100px\">Action / <a class=\"vehicleedit\" link=\"new\" customer=\"" + vehicle_ctr + "\">Add</a></th></tr>";
                        html_activity += "</thead>";
                        html_activity += "<tbody>";

                        using (SqlCommand cmd = new SqlCommand("get_vehicle_activity", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add("@vehicle_ctr", SqlDbType.VarChar).Value = vehicle_ctr;
                            con.Open();
                            SqlDataReader dr = cmd.ExecuteReader();
                            while (dr.Read())
                            {
                                string customer_vehicle_CTR = dr["ID"].ToString();
                                string registration = dr["registration"].ToString();
                                string description = dr["description"].ToString();
                                string wofcycle = dr["wof_cycle"].ToString();
                                //string date = Functions.formatdate(dr["date"].ToString(), "dd MMM yyyy");
                                string note = dr["note"].ToString();

                                html_activity += "<tr id=\"vehicle_" + customer_vehicle_CTR + "\">";
                                html_activity += "<td style=\"text-align:center\"></td>";
                                //html_activity += "<td programid=\"" + programid + "\">" + programname + "</td>";
                                html_activity += "<td>" + registration + "</td>";
                                html_activity += "<td>" + "To do" + "</td>";
                                html_activity += "<td>" + description + "</td>";
                                //html_vehicles += "<td worker=\"" + worker + "\">" + YesNoBit.FirstOrDefault(x => x.Value == worker).Key + "</td>";
                                html_activity += "<td>" + wofcycle + "</td>";
                                html_activity += "<td>" + "To do" + "</td>";
                                html_activity += "<td>" + note + "</td>";
                                html_activity += "<td><a href=\"\" link=\"" + customer_vehicle_CTR + "\" class=\"vehicleedit\">Edit</td>";
                                html_activity += "</tr>";
                            }
                            dr.Close();
                        }
                        //}

                        #endregion ACTIVITY
                    }
                }
                string customer_ctr = Request.QueryString["customerid"] ?? "";
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("get_customer", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@customer_ctr", SqlDbType.VarChar).Value = customer_ctr;
                        con.Open();
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr.HasRows)
                        {
                            dr.Read();
                            customername = "<h2>" + dr["name"].ToString() + "</h2>";
                        }
                        dr.Close();
                    }
                }
            }
        }

        protected void btn_submit_Click(object sender, EventArgs e)
        {
            Response.Redirect("customersearch.aspx");
        }
    }
}
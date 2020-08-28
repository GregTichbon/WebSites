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
    public partial class CustomerMaintenance : System.Web.UI.Page
    {
        public string username = "";
        public string customer_ctr;
        public string[] fld_customertype_ctr = new string[1];
        public string fld_name;
        public string fld_firstname;
        public string fld_surname;
        public string fld_knownas;
        public string fld_address;
        public string fld_emailaddress;
        public string fld_mobilephone;
        public string fld_homephone;
        public string fld_workphone;
        public string fld_note;
        public string fld_xeroid;

        public string html_vehicles;

        public Dictionary<string, string> options = new Dictionary<string, string>();
        public string[] nooptions = { };
        public Dictionary<string, string> customertypes = new Dictionary<string, string>();
        public Dictionary<string, string> wofcycles = new Dictionary<string, string>();
        public Dictionary<string, string> vehciletypes = new Dictionary<string, string>();

        

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
                customer_ctr = Request.QueryString["id"] ?? "";
                if (customer_ctr == "")
                {
                    Response.Redirect("customersearch.aspx");
                }

                ViewState["customer_ctr"] = customer_ctr;
                ViewState["returnto"] = Request.QueryString["returnto"] + "";

                string systemPrefix = WebConfigurationManager.AppSettings["systemPrefix"];
                String connectionString = ConfigurationManager.ConnectionStrings[systemPrefix + "ConnectionString"].ConnectionString;




                if (customer_ctr != "new")
                {
                    wofcycles.Add("6 monthly", "");
                    wofcycles.Add("Annually", "");

                    /*
                    options.Clear();
                    options.Add("storedprocedure", "");
                    options.Add("storedprocedurename", "");
                    //options.Add("parameters", "|countcustomers|");
                    options.Add("usevalues", "");
                    //options.Add("insertblank", "start");
                    vehciletypes = Functions.buildselectionlist(connectionString, "get_vehicletypes", options);
                    */
                    SqlConnection con = new SqlConnection(connectionString);

                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "get_customer";
                    cmd.Parameters.Add("@customer_ctr", SqlDbType.VarChar).Value = customer_ctr;

                    cmd.Connection = con;
                    con.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        dr.Read();
                        fld_customertype_ctr[0] = dr["customertype_ctr"].ToString();
                        fld_name = dr["name"].ToString();
                        fld_firstname = dr["firstname"].ToString();
                        fld_surname = dr["surname"].ToString();
                        fld_knownas = dr["knownas"].ToString();
                        fld_address = dr["address"].ToString();
                        fld_emailaddress = dr["emailaddress"].ToString();
                        fld_mobilephone = dr["mobilephone"].ToString();
                        fld_homephone = dr["homephone"].ToString();
                        fld_workphone = dr["workphone"].ToString();
                        fld_note = dr["note"].ToString();
                        fld_xeroid = dr["xeroid"].ToString();
                    }
                    dr.Close();

                    #region VEHICLES
                    //-------------------------------VEHICLES TAB------------------------------------------------------
                    //if (Functions.accessstringtest(Session["UBC_AccessString"].ToString(), "1"))
                    //{

                    //html_tab += "<li><a data-target=\"#div_vehicles\">vehicles</a></li>";
                    html_vehicles = "<thead>";
                    html_vehicles += "<tr><th style=\"width:50px;text-align:center\"></th><th>Registration</th><th>Make/Model</th><th>Description</th><th>WOF Cycle</th><th>WOF Due</th><th>Note</th><th style=\"width:100px\">Action / <a class=\"vehicleedit\" link=\"new\" customer=\"" + customer_ctr + "\">Add</a></th></tr>";
                    html_vehicles += "</thead>";
                    html_vehicles += "<tbody>";

                   

                    cmd.CommandText = "get_customer_vehicles";
                    cmd.Parameters.Clear();
                    cmd.Parameters.Add("@customer_ctr", SqlDbType.VarChar).Value = customer_ctr;
                    dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        string customer_vehicle_CTR = dr["ID"].ToString();
                        string registration = dr["registration"].ToString();
                        string description = dr["description"].ToString();
                        string wofcycle = dr["wof_cycle"].ToString();
                        //string date = Functions.formatdate(dr["date"].ToString(), "dd MMM yyyy");
                        string note = dr["note"].ToString();

                        html_vehicles += "<tr id=\"vehicle_" + customer_vehicle_CTR + "\">";
                        html_vehicles += "<td style=\"text-align:center\"></td>";
                        //html_vehicles += "<td programid=\"" + programid + "\">" + programname + "</td>";
                        html_vehicles += "<td>" + registration + "</td>";
                        html_vehicles += "<td>" + "To do" + "</td>";
                        html_vehicles += "<td>" + description + "</td>";
                        //html_vehicles += "<td worker=\"" + worker + "\">" + YesNoBit.FirstOrDefault(x => x.Value == worker).Key + "</td>";
                        html_vehicles += "<td>" + wofcycle + "</td>";
                        html_vehicles += "<td>" + "To do" + "</td>";
                        html_vehicles += "<td>" + note + "</td>";
                        html_vehicles += "<td><a href=\"\" link=\"" + customer_vehicle_CTR + "\" class=\"vehicleedit\">Edit</td>";
                        html_vehicles += "</tr>";
                    }
                    dr.Close();

                    //}

                    #endregion VEHICLES
                }
            }
        }

        protected void btn_submit_Click(object sender, EventArgs e)
        {
            Response.Redirect("customersearch.aspx");
        }
    }
}
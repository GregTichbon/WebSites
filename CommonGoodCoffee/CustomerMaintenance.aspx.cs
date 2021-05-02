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
using Generic;

namespace CommonGoodCoffee
{
    public partial class CustomerMaintenance : System.Web.UI.Page
    {
        public string customer_ctr;
        public string fld_firstname;
        public string fld_surname;
        public string fld_businessname;
        public string[] fld_customertype = new string[1];
        public string[] fld_businesstype = new string[1];
        public string fld_greeting;
        public string fld_emailaddress;
        public string fld_deliveryaddress;
        public string fld_notes;


        public string fld_xxxxxx;

        public string displaybusinessfields = "none";

        public string html_tab = "";

        public string html_orders = "";
        public string html_subscriptions = "";

        public Dictionary<string, string> options = new Dictionary<string, string>();
        public string[] nooptions = { };

        public Dictionary<string, string> customertypes = new Dictionary<string, string>();
        public Dictionary<string, string> businesstypes = new Dictionary<string, string>();
        public Dictionary<string, string> YesNo = new Dictionary<string, string>();
        public Dictionary<string, string> stockitems = new Dictionary<string, string>();
        public Dictionary<string, string> grinds = new Dictionary<string, string>();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                customer_ctr = Request.QueryString["id"] ?? "";
                if (customer_ctr == "")
                {
                    Response.Redirect("customersearch.aspx");
                }

                ViewState["customer_ctr"] = customer_ctr;
                ViewState["returnto"] = Request.QueryString["returnto"] + "";

                //genders.Add("Female", "Female");
                //genders.Add("Male", "Male");

                string systemPrefix = WebConfigurationManager.AppSettings["systemPrefix"];
                String connectionString = ConfigurationManager.ConnectionStrings[systemPrefix + "ConnectionString"].ConnectionString;

                options.Clear();
                options.Add("storedprocedure", "");
                options.Add("storedprocedurename", "");
                options.Add("usevalues", "");
                //options.Add("insertblank", "start");
                customertypes = Functions.buildselectionlist(connectionString, "get_customertypes", options);

                options.Clear();
                options.Add("storedprocedure", "");
                options.Add("storedprocedurename", "");
                options.Add("usevalues", "");
                //options.Add("insertblank", "start");
                businesstypes = Functions.buildselectionlist(connectionString, "get_businesstypes", options);

                if (customer_ctr != "new")
                {
                    options.Clear();
                    options.Add("storedprocedure", "");
                    options.Add("storedprocedurename", "");
                    options.Add("usevalues", "");
                    //options.Add("insertblank", "start");
                    stockitems = Functions.buildselectionlist(connectionString, "get_stockitems", options);


                    options.Clear();
                    options.Add("storedprocedure", "");
                    options.Add("storedprocedurename", "");
                    options.Add("usevalues", "");
                    //options.Add("insertblank", "start");
                    grinds = Functions.buildselectionlist(connectionString, "get_grinds", options);


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
                                //customer_ctr = dr["customer_ctr"].ToString();
                                fld_firstname = dr["firstname"].ToString();
                                fld_surname = dr["surname"].ToString();
                                fld_businessname = dr["businessname"].ToString();
                                fld_customertype[0] = dr["customertype_ctr"].ToString();
                                fld_businesstype[0] = dr["businesstype_ctr"].ToString();

                                if (fld_customertype[0] == "1")
                                {
                                    displaybusinessfields = "";
                                }
                                
                                fld_greeting = dr["greeting"].ToString();
                                fld_emailaddress = dr["emailaddress"].ToString();
                                fld_deliveryaddress = dr["deliveryaddress"].ToString();
                                fld_notes = dr["notes"].ToString();
                            }
                            dr.Close();
                        }

                        #region ORDERS
                        //-------------------------------ORDERS TAB------------------------------------------------------

                        html_tab += "<li><a data-target=\"#div_orders\">Orders</a></li>";
                        html_orders = "<thead>";
                        //html_orders += "<tr><th style=\"width:50px;text-align:center\"></th><th>Date</th><th>Reference</th><th>Type</th><th>Item Date</th><th>Grind</th><th>Quantity</th><th>Amount</th><th>Delivered</th><th>Invoice</th><th>Note</th><th style=\"width:100px\">Action / <a class=\"orderedit\" data-mode=\"add\" href=\"javascript: void(0)\">Add</a></th></tr>";
                        html_orders += "<tr><th style=\"width:50px;text-align:center\"></th><th>Date</th><th>Reference</th><th>Type</th><th>Grind</th><th>Quantity</th><th>Amount</th><th>Delivered</th><th>Batch</th><th>Invoice</th><th>Note</th><th style=\"width:100px\">Action / <a class=\"orderedit\" data-mode=\"add\" href=\"javascript: void(0)\">Add</a></th></tr>";
                        html_orders += "</thead>";
                        html_orders += "<tbody>";

                        //hidden row, used for creating new rows client side
                        html_orders += "<tr style=\"display:none\">";
                        html_orders += "<td style=\"text-align:center\"></td>";
                        html_orders += "<td></td>"; //Date
                        html_orders += "<td></td>"; //Reference
                        html_orders += "<td></td>"; //Type
                        //html_orders += "<td></td>"; //Stock Item Date
                        html_orders += "<td></td>"; //Grind
                        html_orders += "<td></td>"; //Quantity
                        html_orders += "<td></td>"; //Amount
                        html_orders += "<td></td>"; //Delivered Date
                        html_orders += "<td></td>"; //Batch
                        html_orders += "<td></td>"; //Invoice Reference
                        html_orders += "<td></td>"; //Note
                        html_orders += "<td><a href=\"javascript:void(0)\" class=\"orderedit\" data-mode=\"edit\">Edit</td>";
                        html_orders += "</tr>";

                        using (SqlCommand cmd = new SqlCommand("get_customer_orders", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add("@customer_ctr", SqlDbType.VarChar).Value = customer_ctr;
                            SqlDataReader dr = cmd.ExecuteReader();

                            while (dr.Read())
                            {
                                string order_CTR = dr["order_ctr"].ToString();
                                string date = Functions.formatdate(dr["date"].ToString(), "dd MMM yyyy");
                                string reference = dr["reference"].ToString();
                                string stockitem_ctr = dr["stockitem_ctr"].ToString();
                                string stockitem = dr["stockitem"].ToString();
                                //string stockitemdate = Functions.formatdate(dr["stockitemdate"].ToString(), "dd MMM yyyy");
                                string grind_ctr = dr["grind_ctr"].ToString();
                                string grind = dr["grind"].ToString();
                                string quantity = Convert.ToDecimal(dr["quantity"]).ToString("0.00");
                                string amount = Convert.ToDecimal(dr["amount"]).ToString("0.00");
                                string delivereddate = Functions.formatdate(dr["delivereddate"].ToString(), "dd MMM yyyy");
                                string batchstockitem_ctr = dr["batchstockitem_ctr"].ToString();
                                string batchstockitem = dr["batchstockitem"].ToString();
                                string invoicereference = dr["invoicereference"].ToString();
                                string note = dr["note"].ToString();
                                html_orders += "<tr id=\"order_" + order_CTR + "\">";
                                html_orders += "<td style=\"text-align:center\"></td>";

                                html_orders += "<td>" + date + "</td>";
                                html_orders += "<td>" + reference + "</td>";
                                html_orders += "<td stockitem_ctr=\"" + stockitem_ctr + "\">" + stockitem + "</td>";
                                //html_orders += "<td>" + stockitemdate + "</td>";
                                html_orders += "<td grind_ctr=\"" + grind_ctr + "\">" + grind + "</td>";
                                html_orders += "<td>" + quantity + "</td>";
                                html_orders += "<td>" + amount + "</td>";
                                html_orders += "<td>" + delivereddate + "</td>";
                                html_orders += "<td data-id=\"" + batchstockitem_ctr + "\">" + batchstockitem + "</td>";
                                html_orders += "<td>" + invoicereference + "</td>"; 
                                html_orders += "<td>" + note + "</td>";

                                //html_orders += "<td colspan=\"9\">Working on</td>";
                                html_orders += "<td><a href=\"javascript:void(0)\" class=\"orderedit\" data-mode=\"edit\">Edit</td>";
                                html_orders += "</tr>";


                            }
                            dr.Close();
                        }

                        #endregion ORDERS

                        #region SUBSCRIPTIONS
                        //-------------------------------SUBSCRIPTIONS TAB------------------------------------------------------

                        html_tab += "<li><a data-target=\"#div_subscriptions\">Subscriptions</a></li>";
                        html_subscriptions = "<thead>";
                        html_subscriptions += "<tr><th style=\"width:50px;text-align:center\"></th><th>Frequency</th><th>Period</th><th>Start Date</th><th>Coffee</th><th>Grind</th><th>Quantity</th><th>Amount</th><th>Note</th><th style=\"width:100px\">Action / <a class=\"subscriptionedit\" data-mode=\"add\" href=\"javascript: void(0)\">Add</a></th></tr>";
                        html_subscriptions += "</thead>";
                        html_subscriptions += "<tbody>";

                        //hidden row, used for creating new rows client side
                        html_subscriptions += "<tr style=\"display:none\">";
                        html_subscriptions += "<td style=\"text-align:center\"></td>";
                        html_subscriptions += "<td></td>"; //Frequency
                        html_subscriptions += "<td></td>"; //Period
                        html_subscriptions += "<td></td>"; //Start Date
                        html_subscriptions += "<td></td>"; //Coffee
                        html_subscriptions += "<td></td>"; //Grind
                        html_subscriptions += "<td></td>"; //Quantity
                        html_subscriptions += "<td></td>"; //Amount
                        html_subscriptions += "<td></td>"; //Note
                        html_subscriptions += "<td><a href=\"javascript:void(0)\" class=\"subscriptionedit\" data-mode=\"edit\">Edit</td>";
                        html_subscriptions += "</tr>";


                        using (SqlCommand cmd = new SqlCommand("get_customer_subscriptions", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add("@customer_ctr", SqlDbType.VarChar).Value = customer_ctr;
                            SqlDataReader dr = cmd.ExecuteReader();

                            while (dr.Read())
                            {
                                string subscription_CTR = dr["subscription_ctr"].ToString();
                                string frequency = dr["frequency"].ToString();
                                string period = dr["period"].ToString();
                                string startdate = Functions.formatdate(dr["startdate"].ToString(), "dd MMM yyyy");
                                string stockitem_ctr = dr["stockitem_ctr"].ToString();
                                string stockitem = dr["stockitem"].ToString();
                                string grind_ctr = dr["grind_ctr"].ToString();
                                string grind = dr["grind"].ToString();
                                string quantity = Convert.ToDecimal(dr["quantity"]).ToString("0.00");
                                string amount = Convert.ToDecimal(dr["amount"]).ToString("0.00");
                                string note = dr["note"].ToString();

                                html_subscriptions += "<tr id=\"subscription_" + subscription_CTR + "\">";
                                html_subscriptions += "<td style=\"text-align:center\"></td>";

                                html_subscriptions += "<td>" + frequency + "</td>";
                                html_subscriptions += "<td>" + period + "</td>";
                                html_subscriptions += "<td>" + startdate + "</td>";
                                html_subscriptions += "<td stockitem_ctr=\"" + stockitem_ctr + "\">" + stockitem + "</td>";
                                html_subscriptions += "<td grind_ctr=\"" + grind_ctr + "\">" + grind + "</td>";
                                html_subscriptions += "<td>" + quantity + "</td>";
                                html_subscriptions += "<td>" + amount + "</td>";
                                html_subscriptions += "<td>" + note + "</td>";

                                html_subscriptions += "<td><a href=\"javascript:void(0)\" class=\"subscriptionedit\" data-mode=\"edit\">Edit</td>";
                                html_subscriptions += "</tr>";


                            }
                            dr.Close();
                        }
                    
                    }

                    #endregion SUBSCRIPTIONS


                }
            }
        }

        protected void btn_submit_Click(object sender, EventArgs e)
        {
            Boolean Creating = false;
            string systemPrefix = WebConfigurationManager.AppSettings["systemPrefix"];
            String connectionString = ConfigurationManager.ConnectionStrings[systemPrefix + "ConnectionString"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("Update_customer", con))
                {

                    cmd.CommandType = CommandType.StoredProcedure;

                    customer_ctr = ViewState["customer_ctr"].ToString();
                    if (customer_ctr == "new")
                    {
                        Creating = true;
                    }

                    //cmd.CommandText = "Update_customer";
                    cmd.Parameters.Add("@customer_ctr", SqlDbType.VarChar).Value = customer_ctr;
                    cmd.Parameters.Add("@customertype_ctr", SqlDbType.VarChar).Value = Request.Form["fld_customertype"].Trim();
                    cmd.Parameters.Add("@firstname", SqlDbType.VarChar).Value = Request.Form["fld_firstname"].Trim();
                    cmd.Parameters.Add("@surname", SqlDbType.VarChar).Value = Request.Form["fld_surname"].Trim();
                    cmd.Parameters.Add("@businessname", SqlDbType.VarChar).Value = Request.Form["fld_businessname"].Trim();
                    cmd.Parameters.Add("@businesstype_ctr", SqlDbType.VarChar).Value = Request.Form["fld_businesstype"].Trim();
                    cmd.Parameters.Add("@greeting", SqlDbType.VarChar).Value = Request.Form["fld_greeting"].Trim();
                    cmd.Parameters.Add("@emailaddress", SqlDbType.VarChar).Value = Request.Form["fld_emailaddress"].Trim();
                    cmd.Parameters.Add("@deliveryaddress", SqlDbType.VarChar).Value = Request.Form["fld_deliveryaddress"].Trim();
                    cmd.Parameters.Add("@notes", SqlDbType.VarChar).Value = Request.Form["fld_notes"].Trim();

                    cmd.Connection = con;
                    //try
                    //{
                    con.Open();
                    customer_ctr = cmd.ExecuteScalar().ToString();
                    con.Close();
                    //}
                    //catch (Exception ex)
                    //{
                    //    throw ex;
                    //}
                }


                foreach (string key in Request.Form)
                {
                    if (key.StartsWith("order_"))
                    {
                        int keylength = "order_".Length;
                        string order_ctr = key.Substring(keylength);   
                        if (order_ctr.EndsWith("_delete"))
                        {
                            using (SqlCommand cmd = new SqlCommand("Delete_Order", con))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.Add("@order_ctr", SqlDbType.VarChar).Value = order_ctr.Substring(0, order_ctr.Length - 7);
                                con.Open();
                                cmd.ExecuteScalar();
                                con.Close();
                            }
                        }
                        else
                        {
                            if (order_ctr.StartsWith("new"))
                            {
                                order_ctr = "new";
                            }
                            using (SqlCommand cmd = new SqlCommand("Update_Order", con))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.Add("@order_ctr", SqlDbType.VarChar).Value = order_ctr;
                                cmd.Parameters.Add("@customer_ctr", SqlDbType.VarChar).Value = customer_ctr;
                                string[] valuesSplit = Request.Form[key].Split('\x00FE');
                                cmd.Parameters.Add("@date", SqlDbType.VarChar).Value = valuesSplit[0];
                                cmd.Parameters.Add("@reference", SqlDbType.VarChar).Value = valuesSplit[1];
                                cmd.Parameters.Add("@stockitem_ctr", SqlDbType.VarChar).Value = valuesSplit[2];
                                cmd.Parameters.Add("@grind_ctr", SqlDbType.VarChar).Value = valuesSplit[3];
                                cmd.Parameters.Add("@quantity", SqlDbType.VarChar).Value = valuesSplit[4];
                                cmd.Parameters.Add("@amount", SqlDbType.VarChar).Value = valuesSplit[5];
                                cmd.Parameters.Add("@deliveredDate", SqlDbType.VarChar).Value = valuesSplit[6];
                                cmd.Parameters.Add("@stockitembatch_ctr", SqlDbType.VarChar).Value = valuesSplit[7];
                                cmd.Parameters.Add("@invoicereference", SqlDbType.VarChar).Value = valuesSplit[8];
                                cmd.Parameters.Add("@note", SqlDbType.VarChar).Value = valuesSplit[9];
                                con.Open();
                                cmd.ExecuteScalar();
                                con.Close();
                            }
                        }
                    }
                    else if (key.StartsWith("subscription_"))
                    {
                        int keylength = "subscription_".Length;
                        string subscription_ctr = key.Substring(keylength);
                        if (subscription_ctr.EndsWith("_delete"))
                        {
                            using (SqlCommand cmd = new SqlCommand("Delete_subscription", con))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.Add("@subscription_ctr", SqlDbType.VarChar).Value = subscription_ctr.Substring(0, subscription_ctr.Length - 7);
                                con.Open();
                                cmd.ExecuteScalar();
                                con.Close();
                            }
                        }
                        else
                        {
                            if (subscription_ctr.StartsWith("new"))
                            {
                                subscription_ctr = "new";
                            }
                            using (SqlCommand cmd = new SqlCommand("Update_subscription", con))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.Add("@subscription_ctr", SqlDbType.VarChar).Value = subscription_ctr;
                                cmd.Parameters.Add("@customer_ctr", SqlDbType.VarChar).Value = customer_ctr;
                                string[] valuesSplit = Request.Form[key].Split('\x00FE');
                                cmd.Parameters.Add("@frequency", SqlDbType.VarChar).Value = valuesSplit[0];
                                cmd.Parameters.Add("@period", SqlDbType.VarChar).Value = valuesSplit[1];
                                cmd.Parameters.Add("@startdate", SqlDbType.VarChar).Value = valuesSplit[2];
                                cmd.Parameters.Add("@stockitem_ctr", SqlDbType.VarChar).Value = valuesSplit[3];
                                cmd.Parameters.Add("@grind_ctr", SqlDbType.VarChar).Value = valuesSplit[4];
                                cmd.Parameters.Add("@quantity", SqlDbType.VarChar).Value = valuesSplit[5];
                                cmd.Parameters.Add("@amount", SqlDbType.VarChar).Value = valuesSplit[6];
                                cmd.Parameters.Add("@note", SqlDbType.VarChar).Value = valuesSplit[7];
                                con.Open();
                                cmd.ExecuteScalar();
                                con.Close();
                            }
                        }
                    }
                }
            }
            string returnto = ViewState["returnto"].ToString();

            if (Creating)
            {
                if (returnto == "")
                {
                    returnto = "customermaintenance.aspx?id=" + customer_ctr;
                }
                else
                {
                    returnto = "customermaintenance.aspx?id=" + customer_ctr + "&returnto=" + returnto + ".aspx";
                }
            }
            else
            {
                if (returnto == "")
                {
                    returnto = "customersearch.aspx";
                }
            }

            Response.Redirect(returnto);
        }
    }
}
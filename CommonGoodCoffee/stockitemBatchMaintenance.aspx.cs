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
using localfunctions = CommonGoodCoffee._Dependencies.myFuntions;

namespace CommonGoodCoffee
{
    public partial class stockitemBatchMaintenance : System.Web.UI.Page
    {
        public string stockitembatch_ctr;
        public string stockitem_ctr;
        public string stockItem;
        public string fld_date;
        public string fld_ordered;
        public string fld_reference;
        public string fld_note;

        public string html_tab = "";

        public string html_transactions = "";
        public string html_orders = "";

        public string showtransaction = "none";

        public Dictionary<string, string> options = new Dictionary<string, string>();

        public string[] nooptions = { };
        public Dictionary<string, string> transactiontypes = new Dictionary<string, string>();


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!localfunctions.AccessStringTest(""))
                {
                    Response.Redirect("login.aspx");
                }

                stockitembatch_ctr = Request.QueryString["id"] ?? "new";
                if (stockitembatch_ctr == "new")
                {
                    stockitem_ctr = Request.QueryString["stockid"] ?? "";
                    if (stockitem_ctr == "")
                    {
                        Response.Redirect("stockitemlist.aspx");
                    }
                }

                ViewState["stockitembatch_ctr"] = stockitembatch_ctr;
                ViewState["stockitem_ctr"] = stockitem_ctr;
                ViewState["returnto"] = Request.UrlReferrer.ToString();

                string systemPrefix = WebConfigurationManager.AppSettings["systemPrefix"];
                String connectionString = ConfigurationManager.ConnectionStrings[systemPrefix + "ConnectionString"].ConnectionString + "; MultipleActiveResultSets=True";

                if (stockitembatch_ctr != "new")
                {
                    options.Clear();
                    options.Add("storedprocedure", "");
                    options.Add("storedprocedurename", "");
                    options.Add("usevalues", "");
                    //options.Add("insertblank", "start");
                    transactiontypes = Functions.buildselectionlist(connectionString, "get_stocktransactiontypes", options);

                    using (SqlConnection con = new SqlConnection(connectionString))
                    {
                        con.Open();
                        using (SqlCommand cmd = new SqlCommand("get_stockitembatch", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add("@stockitembatch_ctr", SqlDbType.VarChar).Value = stockitembatch_ctr;

                            using (SqlDataReader dr = cmd.ExecuteReader())
                            {
                                if (dr.HasRows)
                                {
                                    dr.Read();

                                    stockItem = dr["stockItem"].ToString();
                                    fld_date = Functions.formatdate(dr["date"].ToString(), "dd MMM yyyy");
                                    fld_reference = dr["reference"].ToString();
                                    fld_ordered = dr["ordered"].ToString(); 
                                    fld_note = dr["note"].ToString();
                                }
                            }
                        }


                        //-------------------------------TRANSACTIONS TAB------------------------------------------------------
                        html_tab += "<li class=\"active\"><a data-target=\"#div_transactions\">Transactions</a></li>";
                        html_transactions = "<thead>";
                        html_transactions += "<tr><th style=\"width:50px;text-align:center\"></th><th>Date</th><th>Type</th><th>Quantity</th><th>Note</th><th style=\"width:100px\">Action / <a class=\"transactionedit\" data-mode=\"add\" href=\"javascript: void(0)\">Add</a></th></tr>";
                        html_transactions += "</thead>";
                        html_transactions += "<tbody>";

                        //hidden row, used for creating new rows client side
                        html_transactions += "<tr style=\"display:none\">";
                        html_transactions += "<td style=\"text-align:center\"></td>";
                        html_transactions += "<td class=\"transaction\"></td>"; //Date
                        html_transactions += "<td></td>"; //Type
                        html_transactions += "<td></td>"; //Quantity
                        html_transactions += "<td></td>"; //Note
                        html_transactions += "<td><a href=\"javascript:void(0)\" class=\"transactionedit\" data-mode=\"edit\">Edit</td>";
                        html_transactions += "</tr>";

                        using (SqlCommand cmd = new SqlCommand("get_stockitembatch_transactions", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add("@stockitembatch_ctr", SqlDbType.VarChar).Value = stockitembatch_ctr;
                            using (SqlDataReader dr = cmd.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    string stocktransaction_ctr = dr["stocktransaction_ctr"].ToString();
                                    string date = Functions.formatdate(dr["date"].ToString(), "dd MMM yyyy");
                                    string StockTransactionType_CTR = dr["StockTransactionType_CTR"].ToString();
                                    string StockTransactionType = dr["StockTransactionType"].ToString();
                                    string quantity = Convert.ToDecimal(dr["quantity"]).ToString("0.00");
                                    string note = dr["note"].ToString();

                                    html_transactions += "<tr id=\"transaction_" + stocktransaction_ctr + "\">";
                                    html_transactions += "<td style=\"text-align:center\"></td>";

                                    html_transactions += "<td>" + date + "</td>";
                                    html_transactions += "<td data-id=\"" + StockTransactionType_CTR + "\">" + StockTransactionType + "</td>";
                                    html_transactions += "<td>" + quantity + "</td>";
                                    html_transactions += "<td>" + note + "</td>";

                                    html_transactions += "<td><a href=\"javascript:void(0)\" class=\"transactionedit\" data-mode=\"edit\">Edit</td>";
                                    html_transactions += "</tr>";


                                }
                            }
                        }

                        //-------------------------------ORDERS TAB------------------------------------------------------

                        /*
                         * Order_CTR
                        Order_CTR	Customer_CTR	Date	Reference	StockItem_CTR	Grind_CTR	Quantity	Amount	DeliveredDate	InvoiceReference	StockItemBatch_CTR	Note	FromSubscription	Customer
                        1	7	2021-04-26	442	1	2	5	44	2021-04-27	0129	1		NULL	Chris Kirby
                        */
                        html_tab += "<li><a data-target=\"#div_orders\">Orders</a></li>";
                        html_orders = "<thead>";
                        html_orders += "<tr><th style=\"width:50px;text-align:center\"></th><th>Date</th><th>Customer</th><th>Quantity</th><th>Dispatched</th><th style=\"width:100px\">Action / <a class=\"orderedit\" data-mode=\"add\" href=\"javascript: void(0)\">Add</a></th></tr>";
                        html_orders += "</thead>";
                        html_orders += "<tbody>";

                        //hidden row, used for creating new rows client side
                        html_orders += "<tr style=\"display:none\">";
                        html_orders += "<td style=\"text-align:center\"></td>";
                        html_orders += "<td></td>"; //Date
                        html_orders += "<td></td>"; //Customer
                        html_orders += "<td></td>"; //Quantity
                        html_orders += "<td></td>"; //Delivered
                        html_orders += "<td><a href=\"javascript:void(0)\" class=\"orderedit\" data-mode=\"edit\">Edit</td>";
                        html_orders += "</tr>";

                        using (SqlCommand cmd = new SqlCommand("get_stockitembatch_orders", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add("@stockitembatch_ctr", SqlDbType.VarChar).Value = stockitembatch_ctr;
                            using (SqlDataReader dr = cmd.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    string order_ctr = dr["order_ctr"].ToString();
                                    string date = Functions.formatdate(dr["date"].ToString(), "dd MMM yyyy");
                                    string customer_CTR = dr["customer_CTR"].ToString();
                                    string customer = dr["customer"].ToString();
                                    string quantity = Convert.ToDecimal(dr["quantity"]).ToString("0.00");
                                    string deliveredDate = Functions.formatdate(dr["deliveredDate"].ToString(), "dd MMM yyyy");

                                    html_orders += "<tr id=\"order_" + order_ctr + "\">";
                                    html_orders += "<td style=\"text-align:center\"></td>";

                                    html_orders += "<td>" + date + "</td>";
                                    html_orders += "<td data-id=\"" + customer_CTR + "\">" + customer + "</td>";
                                    html_orders += "<td>" + quantity + "</td>";
                                    html_orders += "<td>" + deliveredDate + "</td>";

                                    html_orders += "<td><a href=\"javascript:void(0)\" class=\"orderedit\" data-mode=\"edit\">Edit</td>";
                                    html_orders += "</tr>";


                                }
                            }
                        }
                        //-------------------------------END OF TABS------------------------------------------------------


                    }
                }
                else
                {
                    showtransaction = "";

                    using (SqlConnection con = new SqlConnection(connectionString))
                    {
                        con.Open();
                        using (SqlCommand cmd = new SqlCommand("get_stockitem", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add("@stockitem_ctr", SqlDbType.VarChar).Value = stockitem_ctr;

                            using (SqlDataReader dr = cmd.ExecuteReader())
                            {
                                dr.Read();
                                stockItem = dr["stockItem"].ToString();
                            }
                        }
                    }
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
                using (SqlCommand cmd = new SqlCommand("Update_stockitembatch", con))
                {

                    cmd.CommandType = CommandType.StoredProcedure;

                    stockitembatch_ctr = ViewState["stockitembatch_ctr"].ToString();
                    if (stockitembatch_ctr == "new")
                    {
                        Creating = true;
                        stockitem_ctr = ViewState["stockitem_ctr"].ToString();
                    }

                    cmd.Parameters.Add("@stockitembatch_ctr", SqlDbType.VarChar).Value = stockitembatch_ctr;
                    cmd.Parameters.Add("@stockitem_ctr", SqlDbType.VarChar).Value = stockitem_ctr;
                    cmd.Parameters.Add("@date", SqlDbType.VarChar).Value = Request.Form["fld_date"].Trim();
                    cmd.Parameters.Add("@reference", SqlDbType.VarChar).Value = Request.Form["fld_reference"].Trim();
                    cmd.Parameters.Add("@ordered", SqlDbType.VarChar).Value = Request.Form["fld_ordered"].Trim();
                    cmd.Parameters.Add("@note", SqlDbType.VarChar).Value = Request.Form["fld_note"].Trim();
                    cmd.Parameters.Add("@transaction_quantity", SqlDbType.VarChar).Value = Request.Form["fld_takeon_quantity"].Trim();
                    cmd.Parameters.Add("@transaction_note", SqlDbType.VarChar).Value = Request.Form["fld_takeon_note"].Trim();

                    cmd.Connection = con;
                    //try
                    //{
                    con.Open();
                    stockitembatch_ctr = cmd.ExecuteScalar().ToString();
                    con.Close();
                    //}
                    //catch (Exception ex)
                    //{
                    //    throw ex;
                    //}
                }


                foreach (string key in Request.Form)
                {
                    if (key.StartsWith("transaction_"))
                    {
                        int keylength = "transaction_".Length;
                        string stocktransaction_ctr = key.Substring(keylength);
                        if (stocktransaction_ctr.EndsWith("_delete"))
                        {
                            using (SqlCommand cmd = new SqlCommand("Delete_StockTransaction", con))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.Add("@stocktransaction_ctr", SqlDbType.VarChar).Value = stocktransaction_ctr;
                                con.Open();
                                cmd.ExecuteScalar().ToString();
                                con.Close();
                            }
                        }
                        else
                        {
                            if (stocktransaction_ctr.StartsWith("new"))
                            {
                                stocktransaction_ctr = "new";
                            }
                            using (SqlCommand cmd = new SqlCommand("Update_StockTransaction", con))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.Add("@stocktransaction_ctr", SqlDbType.VarChar).Value = stocktransaction_ctr;
                                cmd.Parameters.Add("@stockitembatch_ctr", SqlDbType.VarChar).Value = stockitembatch_ctr;
                                string[] valuesSplit = Request.Form[key].Split('\x00FE');
                                cmd.Parameters.Add("@date", SqlDbType.VarChar).Value = valuesSplit[0];
                                cmd.Parameters.Add("@type", SqlDbType.VarChar).Value = valuesSplit[1];
                                cmd.Parameters.Add("@quantity", SqlDbType.VarChar).Value = valuesSplit[2];
                                cmd.Parameters.Add("@note", SqlDbType.VarChar).Value = valuesSplit[3];
                                con.Open();
                                cmd.ExecuteScalar().ToString();
                                con.Close();
                            }
                        }
                    }
                }
            }
            //string returnto = ViewState["returnto"].ToString();


            string returnto = "";
            if (Creating)
            {
                returnto = "stockitembatchmaintenance.aspx?id=" + stockitembatch_ctr;
            } else
            {
                returnto = ViewState["returnto"].ToString();
                //returnto = "stockitemmaintenance.aspx?id=" + stockitem_ctr;
            }

                /*

                if (Creating)
                {
                    if (returnto == "")
                    {
                        returnto = "stockitembatchmaintenance.aspx?id=" + stockitembatch_ctr;
                    }
                    else
                    {
                        returnto = "stockitemmaintenance.aspx?id=" + stockitem_ctr + "&returnto=" + returnto + ".aspx";
                    }
                }
                else
                {
                    if (returnto == "")
                    {
                        returnto = "stockitemlist.aspx";
                    }
                }
                */

                Response.Redirect(returnto);
        }
    }
}
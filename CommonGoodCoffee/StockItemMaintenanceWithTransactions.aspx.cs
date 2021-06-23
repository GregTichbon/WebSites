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
    public partial class StockItemMaintenanceWithTransactions : System.Web.UI.Page
    {
        public string stockitem_ctr;
        public string fld_stockitem;
        public string fld_description;
        public string fld_note;

        public string html_tab = "";

        public string html_batches = "";
        public string html_transactions = "";

        public Dictionary<string, string> options = new Dictionary<string, string>();
        public string[] nooptions = { };

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!localfunctions.AccessStringTest(""))
                {
                    Response.Redirect("/login.aspx");
                }

                stockitem_ctr = Request.QueryString["id"] ?? "";
                if (stockitem_ctr == "")
                {
                    Response.Redirect("stockitemlist.aspx");
                }

                ViewState["stockitem_ctr"] = stockitem_ctr;
                ViewState["returnto"] = Request.QueryString["returnto"] + "";


                string systemPrefix = WebConfigurationManager.AppSettings["systemPrefix"];
                String connectionString = ConfigurationManager.ConnectionStrings[systemPrefix + "ConnectionString"].ConnectionString + "; MultipleActiveResultSets=True";

                if (stockitem_ctr != "new")
                {

                    using (SqlConnection con = new SqlConnection(connectionString))
                    {
                        con.Open();
                        using (SqlCommand cmd = new SqlCommand("get_stockitem", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add("@stockitem_ctr", SqlDbType.VarChar).Value = stockitem_ctr;

                            using (SqlDataReader dr = cmd.ExecuteReader())
                            {
                                if (dr.HasRows)
                                {
                                    dr.Read();
                                    //stockitem_ctr = dr["stockitem_ctr"].ToString();
                                    fld_stockitem = dr["stockitem"].ToString();
                                    fld_description = dr["description"].ToString();
                                    fld_note = dr["note"].ToString();
                                }
                            }
                        }

                        /*
                        #region TRANSACTIONS
                        //-------------------------------TRANSACTIONS TAB------------------------------------------------------
                        html_tab += "<li class=\"active\"><a data-target=\"#div_transactions\">Transactions</a></li>";
                        html_transactions = "<thead>";
                        html_transactions += "<tr><th style=\"width:50px;text-align:center\"></th><th>Date</th><th>Quantity</th><th>Note</th><th style=\"width:100px\">Action / <a class=\"transactionedit\" data-mode=\"add\" href=\"javascript: void(0)\">Add</a></th></tr>";
                        html_transactions += "</thead>";
                        html_transactions += "<tbody>";

                        //hidden row, used for creating new rows client side
                        html_transactions += "<tr style=\"display:none\">";
                        html_transactions += "<td style=\"text-align:center\"></td>";
                        html_transactions += "<td></td>"; //Date
                        html_transactions += "<td></td>"; //Quantity
                        html_transactions += "<td></td>"; //Note
                        html_transactions += "<td><a href=\"javascript:void(0)\" class=\"transactionedit\" data-mode=\"edit\">Edit</td>";
                        html_transactions += "</tr>";

                        using (SqlCommand cmd = new SqlCommand("get_stockitem_transactions", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add("@stockitem_ctr", SqlDbType.VarChar).Value = stockitem_ctr;
                            SqlDataReader dr = cmd.ExecuteReader();

                            while (dr.Read())
                            {
                                string stocktransaction_CTR = dr["stocktransaction_ctr"].ToString();
                                string date = Functions.formatdate(dr["date"].ToString(), "dd MMM yyyy");
                                string quantity = Convert.ToDecimal(dr["quantity"]).ToString("0.00");
                                string note = dr["note"].ToString();
                                html_transactions += "<tr id=\"transaction_" + stocktransaction_CTR + "\">";
                                html_transactions += "<td style=\"text-align:center\"></td>";

                                html_transactions += "<td>" + date + "</td>";
                                html_transactions += "<td>" + quantity + "</td>";
                                html_transactions += "<td>" + note + "</td>";

                                //html_transactions += "<td colspan=\"9\">Working on</td>";
                                html_transactions += "<td><a href=\"javascript:void(0)\" class=\"transactionedit\" data-mode=\"edit\">Edit</td>";
                                html_transactions += "</tr>";


                            }
                            dr.Close();
                        }

                        #endregion TRANSACTIONS
                        */
                        #region BATCHES
                        //-------------------------------BATCHES TAB------------------------------------------------------
                        html_tab += "<li class=\"active\"><a data-target=\"#div_batches\">Batches</a></li>";
                        html_batches = "<thead>";
                        html_batches += "<tr><th style=\"width:50px;text-align:center\"></th><th>Date</th><th>Quantity</th><th>Note</th><th style=\"width:100px\">Action / <a class=\"batchedit\" data-mode=\"add\" href=\"javascript: void(0)\">Add</a></th></tr>";
                        html_batches += "</thead>";
                        html_batches += "<tbody>";

                        //hidden row, used for creating new rows client side
                        html_batches += "<tr style=\"display:none\">";
                        html_batches += "<td style=\"text-align:center\"></td>";
                        html_batches += "<td class=\"transaction\"></td>"; //Date
                        html_batches += "<td></td>"; //Quantity
                        html_batches += "<td></td>"; //Note
                        html_batches += "<td><a href=\"javascript:void(0)\" class=\"batchedit\" data-mode=\"edit\">Edit</td>";
                        html_batches += "</tr>";

                        using (SqlCommand cmd = new SqlCommand("get_stockitem_batches", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add("@stockitem_ctr", SqlDbType.VarChar).Value = stockitem_ctr;
                            using (SqlDataReader dr = cmd.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    string stockitembatch_ctr = dr["stockitembatch_ctr"].ToString();
                                    string date = Functions.formatdate(dr["date"].ToString(), "dd MMM yyyy");
                                    string quantity = Convert.ToDecimal(dr["quantity"]).ToString("0.00");
                                    string note = dr["note"].ToString();
                                    html_batches += "<tr id=\"batch_" + stockitembatch_ctr + "\">";
                                    html_batches += "<td style=\"text-align:center\"></td>";

                                    html_batches += "<td class=\"transaction\">" + date + "</td>";
                                    html_batches += "<td>" + quantity + "</td>";
                                    html_batches += "<td>" + note + "</td>";

                                    //html_batches += "<td colspan=\"9\">Working on</td>";
                                    html_batches += "<td><a href=\"javascript:void(0)\" class=\"batchedit\" data-mode=\"edit\">Edit</td>";
                                    html_batches += "</tr>";

                                    html_batches += "<tr style=\"display:none\"><td colspan=\"5\">";

                                    html_batches += "<table class=\"table table-bordered\">";
                                    html_batches += "<thead>";
                                    html_batches += "<tr><th style=\"width:50px;text-align:center\"></th><th class=\"transaction_date\">Date</th><th class=\"transaction_type\">Type</th><th class=\"transaction_quantity\">Quantity</th><th class=\"transaction_note\">Note</th><th style=\"width:100px\">Action / <a class=\"transactionedit\" data-mode=\"add\" href=\"javascript: void(0)\">Add</a></th></tr>";
                                    html_batches += "</thead>";
                                    html_batches += "<tbody>";

                                    using (SqlCommand cmd11 = new SqlCommand("Get_stockitem_batch_transactions", con))
                                    {
                                        cmd11.CommandType = CommandType.StoredProcedure;
                                        cmd11.Parameters.Add("@StockItemBatch_CTR", SqlDbType.VarChar).Value = stockitembatch_ctr;
                                        using (SqlDataReader dr1 = cmd11.ExecuteReader())
                                        {

                                            while (dr1.Read())
                                            {
                                                string stocktransaction_ctr = dr1["stocktransaction_ctr"].ToString();
                                                string tdate = Functions.formatdate(dr1["date"].ToString(), "dd MMM yyyy");
                                                string tquantity = Convert.ToDecimal(dr1["quantity"]).ToString("0.00");
                                                string tnote = dr1["note"].ToString();
                                                string StockTransactionType_CTR = dr1["StockTransactionType_CTR"].ToString();
                                                string StockTransactionType = dr1["StockTransactionType"].ToString();

                                                html_batches += "<tr id=\"transaction_" + stocktransaction_ctr + "\">";
                                                html_batches += "<td style=\"text-align:center\"></td>";

                                                html_batches += "<td>" + tdate + "</td>";
                                                html_batches += "<td data-id=\"" + StockTransactionType_CTR + "\">" + StockTransactionType + "</td>";
                                                html_batches += "<td>" + tquantity + "</td>";
                                                html_batches += "<td>" + tnote + "</td>";

                                                //html_batches += "<td colspan=\"9\">Working on</td>";
                                                html_batches += "<td><a href=\"javascript:void(0)\" class=\"transactionedit\" data-mode=\"edit\">Edit</td>";
                                                html_batches += "</tr>";
                                            }
                                        }

                                    }
                                    html_batches += "</tbody>";
                                    html_batches += "</table>";

                                }
                            }

                            #endregion BATCHES

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
                using (SqlCommand cmd = new SqlCommand("Update_stockitem", con))
                {

                    cmd.CommandType = CommandType.StoredProcedure;

                    stockitem_ctr = ViewState["stockitem_ctr"].ToString();
                    if (stockitem_ctr == "new")
                    {
                        Creating = true;
                    }

                    cmd.Parameters.Add("@stockitem_ctr", SqlDbType.VarChar).Value = stockitem_ctr;
                    cmd.Parameters.Add("@stockitem", SqlDbType.VarChar).Value = Request.Form["fld_stockitem"].Trim();
                    cmd.Parameters.Add("@description", SqlDbType.VarChar).Value = Request.Form["fld_description"].Trim();
                    cmd.Parameters.Add("@note", SqlDbType.VarChar).Value = Request.Form["fld_note"].Trim();

                    cmd.Connection = con;
                    //try
                    //{
                    con.Open();
                    stockitem_ctr = cmd.ExecuteScalar().ToString();
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
                                cmd.Parameters.Add("@stockitem_ctr", SqlDbType.VarChar).Value = stockitem_ctr;
                                string[] valuesSplit = Request.Form[key].Split('\x00FE');
                                cmd.Parameters.Add("@date", SqlDbType.VarChar).Value = valuesSplit[0];
                                cmd.Parameters.Add("@quantity", SqlDbType.VarChar).Value = valuesSplit[1];
                                cmd.Parameters.Add("@note", SqlDbType.VarChar).Value = valuesSplit[2];
                                con.Open();
                                cmd.ExecuteScalar().ToString();
                                con.Close();
                            }
                        }
                    }
                    else if (key.StartsWith("subscription_"))
                    {

                    }
                }
            }
            string returnto = ViewState["returnto"].ToString();

            if (Creating)
            {
                if (returnto == "")
                {
                    returnto = "stockitemmaintenance.aspx?id=" + stockitem_ctr;
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

            Response.Redirect(returnto);
        }
    }
}
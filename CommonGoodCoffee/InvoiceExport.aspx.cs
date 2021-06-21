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
    public partial class InvoiceExport : System.Web.UI.Page
    {
        public string html = "";
        protected void Page_Load(object sender, EventArgs e)
        {


            /*

1) Show data on screen and allow input Invoice numbers - can change things? Quantiy, amount?
2) Create real temp table, select from temp table, Show summary of invoices that will be created and allow option to add freight
3) Process

*ContactName	1
EmailAddress	2
*InvoiceNumber	11
Reference		12
*InvoiceDate	13
*DueDate		14
*Description	16
*Quantity		17
*UnitAmount		18
*AccountCode	20
*TaxType		21

*/

            if (!IsPostBack)
            {

                if (!localfunctions.AccessStringTest(""))
                {
                    Response.Redirect("login.aspx");
                }

                string systemPrefix = WebConfigurationManager.AppSettings["systemPrefix"];
                String connectionString = ConfigurationManager.ConnectionStrings[systemPrefix + "ConnectionString"].ConnectionString;

                string mode = (string)Session["mode"] ?? ""; //btn_submit.Text; // (string)ViewState["Mode"] ?? "";
                if (mode == "")
                {
                    btn_submit.Text = "Submit";
                    btn_cancel.Visible = false;
                    html = "<table class=\"table\"><thead>"; 
                    html += "<tr><th>Name</th><th>Xero Name</th><th>Email Address</th><th>Invoice Date</th><th>Due</th><th>Description</th><th>Quantity</th><th>Unit Price</th><th>Amount</th><th>Invoice No.</th></tr>";
                    html += "</thead><tbody>";

                    using (SqlConnection con = new SqlConnection(connectionString))
                    {
                        using (SqlCommand cmd = new SqlCommand("export_invoices", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add("@mode", SqlDbType.VarChar).Value = "Start";
                            con.Open();
                            SqlDataReader dr = cmd.ExecuteReader();

                            if (dr.HasRows)
                            {
                                while (dr.Read())
                                {
                                    html += "";
                                    string order_ctr = dr["order_ctr"].ToString();
                                    string customer_ctr = dr["customer_ctr"].ToString();
                                    string name = dr["name"].ToString();
                                    string xeroname = dr["Xero_ContactName"].ToString();
                                    string EmailAddress = dr["EmailAddress"].ToString();
                                    DateTime InvoiceDate = (DateTime)dr["DeliveredDate"];
                                    string InvoiceDateStr = InvoiceDate.ToString("d MMM yyyy");
                                    DateTime Due = InvoiceDate.AddDays(7);
                                    string DueStr = Due.ToString("d MMM yyyy");
                                    string Item = dr["item"].ToString();
                                    string Grind = dr["grind"].ToString();
                                    string Batch = dr["Batch"].ToString();
                                    string Description = Item + " " + Grind + " (Batch: " + Batch + ")";
                                    decimal quantity = (decimal)dr["quantity"];
                                    decimal amount = (decimal)dr["amount"];
                                    decimal unitprice = amount / quantity;

                                    html += "<tr><td><a href=\"customerMaintenance.aspx?id=" + customer_ctr + "\">" + name + "</a></td><td>" + xeroname + "</td><td>" + EmailAddress + "</td><td>" + InvoiceDateStr + "</td><td>" + DueStr + "</td><td>" + Description + "</td><td>" + quantity.ToString("0.00") + "</td><td>" + unitprice.ToString("0.00") + "</td><td>" + amount.ToString("0.00") + "</td><td><input type=\"text\" id=\"inv_" + order_ctr + "\" name=\"inv_" + order_ctr + "\"></td></tr>";
                                }


                            }
                            dr.Close();
                        }

                    }
                    html += "</tbody></table>";
                }
                else if (mode == "Confirm")
                {
                    btn_submit.Text = "Confirm";
                    btn_cancel.Visible = true;
                    html = "<table class=\"table\"><thead>"; 
                    html += "<tr><th>Name</th><th>Xero Name</th><th>Email Address</th><th>Invoice No.</th><th>Invoice Date</th><th>Due</th><th>Description</th><th>Quantity</th><th>Unit Price</th><th>Amount</th></tr>";
                    html += "</thead><tbody>";

                    using (SqlConnection con = new SqlConnection(connectionString))
                    {
                        using (SqlCommand cmd = new SqlCommand("export_invoices", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add("@mode", SqlDbType.VarChar).Value = "Start";
                            con.Open();
                            SqlDataReader dr = cmd.ExecuteReader();

                            if (dr.HasRows)
                            {
                                while (dr.Read())
                                {
                                    html += "";
                                    string order_ctr = dr["order_ctr"].ToString();
                                    string customer_ctr = dr["customer_ctr"].ToString();
                                    string name = dr["name"].ToString();
                                    string xeroname = dr["Xero_ContactName"].ToString();
                                    string EmailAddress = dr["EmailAddress"].ToString();
                                    DateTime InvoiceDate = (DateTime)dr["DeliveredDate"];
                                    string InvoiceDateStr = InvoiceDate.ToString("d MMM yyyy");
                                    DateTime Due = InvoiceDate.AddDays(7);
                                    string DueStr = Due.ToString("d MMM yyyy");
                                    string Item = dr["item"].ToString();
                                    string Grind = dr["grind"].ToString();
                                    string Batch = dr["Batch"].ToString();
                                    string Description = Item + " " + Grind + " (Batch: " + Batch + ")";
                                    decimal quantity = (decimal)dr["quantity"];
                                    decimal amount = (decimal)dr["amount"];
                                    decimal unitprice = amount / quantity;

                                    html += "<tr><td><a href=\"customerMaintenance.aspx?id=" + customer_ctr + "\">" + name + "</a></td><td>" + xeroname + "</td><td>" + EmailAddress + "</td><td>" + "to do" + "</td><td>" + InvoiceDateStr + "</td><td>" + DueStr + "</td><td>" + Description + "</td><td>" + quantity.ToString("0.00") + "</td><td>" + unitprice.ToString("0.00") + "</td><td>" + amount.ToString("0.00") + "</td></tr>";
                                }


                            }
                            dr.Close();
                        }

                    }
                    html += "</tbody></table>";


                }
                else if (mode == "Export")
                {
                    html = "Orders updated with invoice numbers, Here is the file";
                }
            }
        }

        protected void btn_submit_Click(object sender, EventArgs e)
        {
            if (btn_submit.Text == "Submit") //  ViewState["Mode"].ToString() == "")
            {
                //Write to real temporary table
                //ViewState["Mode"] = "Confirm";
                Session["Mode"] = "Confirm";
                //btn_submit.Text = "Confirm";
                //btn_cancel.Visible = true;
                Response.Redirect(Request.RawUrl, true);
                //Server.Transfer(Request.RawUrl,true);
            }
            else if (btn_submit.Text == "Confirm")
            {
                Session["Mode"] = "Export";
                Response.Redirect(Request.RawUrl, true);
            }
        }

        protected void btn_cancel_Click(object sender, EventArgs e)
        {
            Session["Mode"] = "";
            Response.Redirect(Request.RawUrl, true);
        }
    }
}
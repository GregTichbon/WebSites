using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
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
        public string highestinvoice = "";
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
                    Response.Redirect("/login.aspx");
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
                        con.Open();
                        using (SqlCommand cmd = new SqlCommand("select max(cast(InvoiceReference as int)) from [order] where  isnumeric(InvoiceReference) = 1", con))
                        {
                            cmd.CommandType = CommandType.Text;
                            highestinvoice = "Highest invoice: " + cmd.ExecuteScalar().ToString();
                        }
                            using (SqlCommand cmd = new SqlCommand("export_invoices", con))
                        {

                     

                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add("@mode", SqlDbType.VarChar).Value = "Start";
                           
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
                                    if (Batch == "0")
                                    {
                                        Batch = "Drop Shipped";
                                    }
                                    string Description = Item + " " + Grind + " (Batch: " + Batch + ")";
                                    decimal quantity = (decimal)dr["quantity"];
                                    decimal amount = (decimal)dr["amount"];
                                    decimal unitprice = amount / quantity;
                                    string XeroInvoiceReference = dr["XeroInvoiceReference"].ToString();

                                    string invoiceinput = "No Xero Contact Name";
                                    if (xeroname != "")
                                    {
                                        invoiceinput = "<input data-customer=\"" + customer_ctr + "\" type=\"text\" class=\"invoice\" id=\"inv_" + order_ctr + "\" name=\"inv_" + order_ctr + "\" value=\"" + XeroInvoiceReference + "\">";
                                    }

                                    html += "<tr><td><a href=\"customerMaintenance.aspx?id=" + customer_ctr + "\">" + name + "</a></td><td>" + xeroname + "</td><td>" + EmailAddress + "</td><td>" + InvoiceDateStr + "</td><td>" + DueStr + "</td><td>" + Description + "</td><td>" + quantity.ToString("0.00") + "</td><td>" + unitprice.ToString("0.00") + "</td><td>" + amount.ToString("0.00") + "</td><td>" + invoiceinput + "</td></tr>";
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
                            cmd.Parameters.Add("@mode", SqlDbType.VarChar).Value = "Confirm";
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
                                    string XeroInvoiceReference = dr["XeroInvoiceReference"].ToString();

                                    string invoiceinput = "";
                                    if (XeroInvoiceReference != "" && !XeroInvoiceReference.StartsWith("Error - "))
                                    {
                                        invoiceinput = "<input type=\"hidden\" id=\"inv_" + order_ctr + "\" name=\"inv_" + order_ctr + "\" value=\"" + XeroInvoiceReference + "\">";
                                    }
                                    html += "<tr><td>" + invoiceinput + "<a href=\"customerMaintenance.aspx?id=" + customer_ctr + "\">" + name + "</a></td><td>" + xeroname + "</td><td>" + EmailAddress + "</td><td>" + XeroInvoiceReference + "</td><td>" + InvoiceDateStr + "</td><td>" + DueStr + "</td><td>" + Description + "</td><td>" + quantity.ToString("0.00") + "</td><td>" + unitprice.ToString("0.00") + "</td><td>" + amount.ToString("0.00") + "</td></tr>";
                                }


                            }
                            dr.Close();
                        }

                    }
                    html += "</tbody></table>";


                }
                else if (mode == "Export")
                {
                    btn_submit.Visible = false;
                    html = Session["html"].ToString();
                    Session["html"] = "";
                    Session["Mode"] = "";
                }
            }
        }

        protected void btn_submit_Click(object sender, EventArgs e)
        {
            if (btn_submit.Text == "Submit") //  ViewState["Mode"].ToString() == "")
            {
                string systemPrefix = WebConfigurationManager.AppSettings["systemPrefix"];
                String connectionString = ConfigurationManager.ConnectionStrings[systemPrefix + "ConnectionString"].ConnectionString;

                using (SqlConnection con = new SqlConnection(connectionString))
                {

                    foreach (string key in Request.Form)
                    {
                        if (key.StartsWith("inv_"))
                        {
                            int keylength = "inv_".Length;
                            string order_ctr = key.Substring(keylength);
                            string value = Request.Form[key].ToString();
                            if (value != "")
                            {
                                using (SqlCommand cmd = new SqlCommand("Update_XeroInvoice", con))
                                {
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.Parameters.Add("@mode", SqlDbType.VarChar).Value = "Test";
                                    cmd.Parameters.Add("@order_ctr", SqlDbType.VarChar).Value = order_ctr;
                                    cmd.Parameters.Add("@XeroInvoiceReference", SqlDbType.VarChar).Value = Request.Form[key].ToString();

                                    con.Open();
                                    string invoicereference = cmd.ExecuteScalar().ToString();
                                    con.Close();
                                }
                            }
                        }
                    }
                }


                Session["Mode"] = "Confirm";
                Response.Redirect(Request.RawUrl, true);
                //Server.Transfer(Request.RawUrl,true);
            }
            else if (btn_submit.Text == "Confirm")
            {
                string export = "";
                string exportbatch = DateTime.Now.ToString("ddMMyyyyHHmmss");
                string delim = "";
                string q = "\"";
                string qc = q + "," + q;
                string path = Server.MapPath(@"\");
                string fileName = path + @"\Export\InvoiceExport" + exportbatch + ".csv";
  
                string systemPrefix = WebConfigurationManager.AppSettings["systemPrefix"];
                String connectionString = ConfigurationManager.ConnectionStrings[systemPrefix + "ConnectionString"].ConnectionString;

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    foreach (string key in Request.Form)
                    {
                        if (key.StartsWith("inv_"))
                        {
                            int keylength = "inv_".Length;
                            string order_ctr = key.Substring(keylength);
                            string value = Request.Form[key].ToString();
                            if (value != "")
                            {
                                using (SqlCommand cmd = new SqlCommand("Update_XeroInvoice", con))
                                {
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.Parameters.Add("@mode", SqlDbType.VarChar).Value = "Export";
                                    cmd.Parameters.Add("@batch", SqlDbType.VarChar).Value = exportbatch;
                                    cmd.Parameters.Add("@order_ctr", SqlDbType.VarChar).Value = order_ctr;
                                    cmd.Parameters.Add("@XeroInvoiceReference", SqlDbType.VarChar).Value = Request.Form[key].ToString();

                                    con.Open();
                                    SqlDataReader dr = cmd.ExecuteReader();

                                    if (dr.HasRows)
                                    {
                                        dr.Read();
                                        string invoicereference = dr["invoicereference"].ToString();
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

                                        /*
                                        ContactName	1
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

                                        if (invoicereference.StartsWith("Error - "))
                                        {
                                            html += "An invoiced has not been exported for order: " + order_ctr + " - " + invoicereference;
                                        }
                                        else
                                        {
                                            html += "Invoice: " + invoicereference + " created for order: " + order_ctr + "<br />";
                                            export += delim + q + xeroname + qc + EmailAddress + qc + invoicereference + qc + "Reference" + qc + InvoiceDateStr + qc + DueStr + qc + "Description" + qc + quantity + qc + unitprice + qc + "AccountCode" + qc + "TaxType" + q;
                                            delim = "\r\n";
                                        }
                                    }

                                    con.Close();
                                }
                            }
                        }
                    }
                }

                html += "<a href=\"export/InvoiceExport" + exportbatch + ".csv\">" + exportbatch + ".csv</a>";

                TextWriter tw = new StreamWriter(fileName, true);
                tw.WriteLine(export);
                tw.Close();

                Session["Mode"] = "Export";
                Session["html"] = html;
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
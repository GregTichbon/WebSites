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

namespace CommonGoodCoffee.Reporting
{
    public partial class stockconsignments : System.Web.UI.Page
    {
        public string html = "";
        protected void Page_Load(object sender, EventArgs e)
        {

            string lastdate = "";
            string lastreference = "";

            string systemPrefix = WebConfigurationManager.AppSettings["systemPrefix"];
            String connectionString = ConfigurationManager.ConnectionStrings[systemPrefix + "ConnectionString"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("get_stockconsignments", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.Add("@id", SqlDbType.VarChar).Value = id;
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {

                    html += "<table class=\"table\"><thead><tr>";
                    html += "<th>Date</th><th>Reference</th><th>Item</th><th>Take on</th><th>Other Transactions</th><th>Committed</th><th>Completed Orders</th><th>Available</th>";
                    html += "</tr></thead><tbody>";

                    while (dr.Read())
                    {
                        string thisdate = Functions.formatdate(dr["Date"].ToString(), "dd MMM yyyy");
                        string thisreference = dr["reference"].ToString();

                        if(thisdate == lastdate)
                        {
                            thisdate = "";
                            if(thisreference == lastreference)
                            {
                                thisreference = "";
                            } else
                            {
                                lastreference = thisreference;
                            }
                        } else
                        {
                            lastdate = thisdate;
                            lastreference = thisreference;
                        }


                        //stockitemBatchMaintenance.aspx?id=80
                        string StockItemBatch_CTR = dr["StockItemBatch_CTR"].ToString();

                        html += "<tr>";
                        html += "<td>" + thisdate + "</td>";
                        html += "<td>" + thisreference + "</td>";
                        html += "<td><a href=\"/stockitemBatchMaintenance.aspx?id=" + StockItemBatch_CTR + "\">" + dr["stockitem"].ToString() + "</a></td>";
                        html += "<td>" + dr["TakeonQuantity"].ToString() + "</td>";
                        html += "<td>" + dr["NonTakeonTransactions"].ToString() + "</td>";
                        html += "<td>" + dr["Committed"].ToString() + "</td>";
                        html += "<td>" + dr["CompletedOrders"].ToString() + "</td>";
                        html += "<td>" + dr["Available"].ToString() + "</td>";
                        html += "</tr>";
                    }
                    html += "</tbody></table>";
                }
                dr.Close();
                con.Close();
            }
        }
    }
}

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
    public partial class Subscriptions : System.Web.UI.Page
    {
        public string html = "";
        protected void Page_Load(object sender, EventArgs e)
        {

            string lastdate = "";
            string lastreference = "";

            string systemPrefix = WebConfigurationManager.AppSettings["systemPrefix"];
            String connectionString = ConfigurationManager.ConnectionStrings[systemPrefix + "ConnectionString"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("get_subscriptions", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.Add("@id", SqlDbType.VarChar).Value = id;
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {

                    html += "<table class=\"table\"><thead><tr>";
                    html += "<th>Customer</th><th>Frequency</th><th>Start Date</th><th>Item</th><th>Grind</th><th>Quantity</th>";
                    html += "</tr></thead><tbody>";

                    while (dr.Read())
                    {
                        html += "<tr>";
                        html += "<td><a href=\"/CustomerMaintenance.aspx?id=" + dr["Customer_CTR"].ToString() + "\">" + dr["Customer"].ToString() + "</a></td>";
                        html += "<td>" + dr["Frequency"].ToString() + " - " + dr["Period"].ToString() + "</td>";
                        html += "<td>" + Functions.formatdate(dr["StartDate"].ToString(), "dd MMM yyyy") + "</td>";
                        html += "<td><a href=\"/stockitemMaintenance.aspx?id=" + dr["StockItem_CTR"].ToString() + "\">" + dr["StockItem"].ToString() + "</a></td>";
                        html += "<td>" + dr["Grind"].ToString() + "</td>";
                        html += "<td>" + dr["Quantity"].ToString() + "</td>";
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

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

namespace CommonGoodCoffee
{
    public partial class StockItemList : System.Web.UI.Page
    {
        public string html = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            string systemPrefix = WebConfigurationManager.AppSettings["systemPrefix"];
            String connectionString = ConfigurationManager.ConnectionStrings[systemPrefix + "ConnectionString"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("stockitem_List", con))
            {
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    html = "<thead><tr><th>Item</th><th>Description</th><th>Committed</th><th>Sold</th><th>Transactions</th><th>Balance</th></tr></thead>";
                    html += "<tbody>";
                    while (dr.Read())
                    {
                         
                        String stockitem_ctr = dr["stockitem_ctr"].ToString();
                        String item = dr["stockitem"].ToString();
                        String description = dr["description"].ToString();
                        decimal committed = (decimal)dr["committed"];
                        decimal sold = (decimal)dr["sold"];
                        decimal transactions = (decimal)dr["transactions"];
                        decimal balance = transactions - sold - committed;


                        html += "<tr><td><a href=\"stockitemMaintenance.aspx?id=" + stockitem_ctr + "\">" + item + "</a></td><td>" + description + "</td><td>" + committed + "</td><td>" + sold + "</td><td>" + transactions + "</td><td>" + balance + "</td></tr>";
                    }
                    html += "</tbody>";
                }
                dr.Close();
            }
        }
    }
}
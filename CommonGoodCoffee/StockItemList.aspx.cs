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
            string laststockitem = "";
            string systemPrefix = WebConfigurationManager.AppSettings["systemPrefix"];
            String connectionString = ConfigurationManager.ConnectionStrings[systemPrefix + "ConnectionString"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("stockitem_List", con))
            {
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {

                    html = "<thead><tr><th>Batch</th><th>Committed</th><th>Delivered</th><th>Transactions</th><th>Balance</th></tr></thead>";
                    html += "<tbody>";
                    while (dr.Read())
                    {
                         
                        String stockitem_ctr = dr["stockitem_ctr"].ToString();
                        String item = dr["stockitem"].ToString();
                        String description = dr["description"].ToString();
                        String stockitembatch_ctr = dr["stockitembatch_ctr"].ToString();
                        string batchdate = Functions.formatdate(dr["date"].ToString(), "dd MMM yyyy");
                        string reference = dr["reference"].ToString();
                        decimal committed = (decimal)dr["committed"];
                        decimal sold = (decimal)dr["sold"];
                        decimal transactions = (decimal)dr["transactions"];
                        decimal balance = transactions - sold - committed;

                        string batchclass = "";


                        if ((DateTime.Today - Convert.ToDateTime(batchdate)).Days > 6)
                        {
                            batchclass = " class=\"highlight\"";
                        }
                        string hide0 = "";
                        if(transactions - sold == 0)
                        {
                            hide0 = " class=\"hide0\"";
                        }

                        string headeritem = "";
                        string headerdescription = "";
                        if(stockitem_ctr != laststockitem)
                        {
                            headeritem = item;
                            headerdescription = description;
                            html += "<tr><td colspan=\"5\"><a href=\"stockitemMaintenance.aspx?id=" + stockitem_ctr + "\"><b>" + headeritem + "</b></a> - " + headerdescription + "</td></tr>";

                            laststockitem = stockitem_ctr;
                        }
                        html += "<tr" + hide0 + "><td><span" + batchclass + "><a href=\"stockitemBatchMaintenance.aspx?id=" + stockitembatch_ctr + "\">" + batchdate + " - " + reference + "</a></span></td><td>" + committed + "</td><td>" + sold + "</td><td>" + transactions + "</td><td>" + balance + "</td></tr>";
                    }
                    html += "</tbody>";
                }
                dr.Close();
            }
        }
    }
}
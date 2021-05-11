using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using Generic;
using System.Web.Configuration;
using System.Configuration;

namespace CommonGoodCoffee._Dependencies
{

    public partial class Data1 : System.Web.UI.Page
    {
        public string html = "";
        protected void Page_Load(object sender, EventArgs e)
        {

            string mode = Request.Form["mode"];

            switch (mode)
            {
                case "get_stockitembatches":
                    get_stockitembatches();
                    break;
               
            }
        }

        protected string get_stockitembatches()
        {
            html += "<option value=\"\">--- Please Select ---</option>";
            string systemPrefix = WebConfigurationManager.AppSettings["systemPrefix"];
            String connectionString = ConfigurationManager.ConnectionStrings[systemPrefix + "ConnectionString"].ConnectionString;

            string stockitem_ctr = Request.Form["stockitem_ctr"];
            string stockitembatch_ctr = Request.Form["stockitembatch_ctr"];
            string order_ctr = Request.Form["order_ctr"];
            if(order_ctr.StartsWith("new_"))
            {
                order_ctr = "0";
            }
            decimal quantity = Convert.ToDecimal(Request.Form["quantity"]);

            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("get_stockitembatches", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@stockitem_ctr", SqlDbType.VarChar).Value = stockitem_ctr;
                cmd.Parameters.Add("@order_ctr", SqlDbType.VarChar).Value = order_ctr;

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {

                    string value = dr["StockItemBatch_CTR"].ToString();
                    string selected = "";
                    if(value == stockitembatch_ctr)
                    {
                        selected = " selected";
                    }
                    string warning = "";

                    decimal available = (decimal)dr["Available"];
                    decimal committed = (decimal)dr["Committed"];

                    if (quantity > available || quantity > available - committed)
                    {
                        warning = " Warning!";
                    }
                    string label = Functions.formatdate(dr["Date"].ToString(), "dd MMM yyyy") + " - " + dr["reference"].ToString() + " (Available:" + dr["Available"].ToString() + ", Committed:" + dr["Committed"].ToString() + warning + ")";


                    html += "<option" + selected + " value=\"" + value + "\">" + label + "</option>";
                    
                }
                dr.Close();
                con.Close();
            }
            return (html);
        }
    }
}
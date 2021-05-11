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
    public partial class StockReceived : System.Web.UI.Page
    {
        public string html_tab = "";

        public string html_items = "";

        public Dictionary<string, string> options = new Dictionary<string, string>();

        public string[] nooptions = { };
        public Dictionary<string, string> itemtypes = new Dictionary<string, string>();


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string systemPrefix = WebConfigurationManager.AppSettings["systemPrefix"];
                String connectionString = ConfigurationManager.ConnectionStrings[systemPrefix + "ConnectionString"].ConnectionString + "; MultipleActiveResultSets=True";

                options.Clear();
                options.Add("storedprocedure", "");
                options.Add("storedprocedurename", "");
                options.Add("usevalues", "");
                //options.Add("insertblank", "start");
                itemtypes = Functions.buildselectionlist(connectionString, "get_stockitems", options);

                html_tab += "<li class=\"active\"><a data-target=\"#div_items\">Items</a></li>";
                html_items = "<thead>";
                html_items += "<tr><th style=\"width:50px;text-align:center\"></th><th>Type</th><th>Quantity</th><th>Note</th><th style=\"width:100px\">Action / <a class=\"itemedit\" data-mode=\"add\" href=\"javascript: void(0)\">Add</a></th></tr>";
                html_items += "</thead>";
                html_items += "<tbody>";

                //hidden row, used for creating new rows client side
                html_items += "<tr style=\"display:none\">";
                html_items += "<td style=\"text-align:center\"></td>";
                html_items += "<td></td>"; //Type
                html_items += "<td></td>"; //Quantity
                html_items += "<td></td>"; //Note
                html_items += "<td><a href=\"javascript:void(0)\" class=\"itemedit\" data-mode=\"edit\">Edit</td>";
                html_items += "</tr>";

            }
        }

        protected void btn_submit_Click(object sender, EventArgs e)
        {

            string systemPrefix = WebConfigurationManager.AppSettings["systemPrefix"];
            String connectionString = ConfigurationManager.ConnectionStrings[systemPrefix + "ConnectionString"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
               
                foreach (string key in Request.Form)
                {
                    if (key.StartsWith("item_"))
                    {
                        int keylength = "item_".Length;
                        //string stockitem_ctr = key.Substring(keylength);

                        using (SqlCommand cmd = new SqlCommand("Update_stockitembatch", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add("@stockitembatch_ctr", SqlDbType.VarChar).Value = "new";
                            string[] valuesSplit = Request.Form[key].Split('\x00FE');
                            cmd.Parameters.Add("@stockitem_ctr", SqlDbType.VarChar).Value = valuesSplit[0];
                            cmd.Parameters.Add("@date", SqlDbType.VarChar).Value = Request.Form["fld_date"].ToString();
                            cmd.Parameters.Add("@reference", SqlDbType.VarChar).Value = Request.Form["fld_reference"].ToString();
                            cmd.Parameters.Add("@note", SqlDbType.VarChar).Value = Request.Form["fld_note"].ToString();

                            cmd.Parameters.Add("@transaction_quantity", SqlDbType.VarChar).Value = valuesSplit[1];
                            cmd.Parameters.Add("@transaction_note", SqlDbType.VarChar).Value = valuesSplit[2];
                            con.Open();
                            cmd.ExecuteScalar().ToString();
                            con.Close();
                        }
                    }
                }
            }

      
            Response.Redirect("Default.aspx");
        }
    }
}
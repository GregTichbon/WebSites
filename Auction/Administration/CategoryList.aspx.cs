using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Auction.Administration
{
    public partial class CategoryList : System.Web.UI.Page
    {
        public string html = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            Dictionary<string, string> parameters = _Dependencies.Functions.functions.get_Auction_Parameters(Request.Url.AbsoluteUri);

            string category_ctr;
            string category;
            string sequence;

            String strConnString = ConfigurationManager.ConnectionStrings["AuctionConnectionString"].ConnectionString;

            using (SqlConnection con = new SqlConnection(strConnString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Get_Categories", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@auction_ctr", SqlDbType.Int).Value = parameters["Auction_ID"];
                    cmd.Connection = con;

                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            category_ctr = dr["category_ctr"].ToString();
                            category = dr["category"].ToString();
                            sequence = dr["sequence"].ToString();

                            html += "<tr><td><a href=category.aspx?id=" + category_ctr + ">" + category + "</a><td>" + sequence + "</td></tr>";
                        }
                    }
                }
            }
        }
    }
}
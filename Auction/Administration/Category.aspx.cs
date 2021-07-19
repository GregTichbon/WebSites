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
    public partial class Category : System.Web.UI.Page
    {
        public string category_ctr;
        public string category;
        public string sequence;
        public Dictionary<string, string> parameters;

        protected void Page_Load(object sender, EventArgs e)
        {
            //parameters = _Dependencies.Functions.functions.get_Auction_Parameters(Request.Url.AbsoluteUri);
            category_ctr = Request.QueryString["id"];
            if (!string.IsNullOrEmpty(category_ctr))
            {
                String strConnString = ConfigurationManager.ConnectionStrings["AuctionConnectionString"].ConnectionString;

                using (SqlConnection con = new SqlConnection(strConnString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("Get_Category", con))
                    {


                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@category_ctr", SqlDbType.Int).Value = category_ctr;
                        cmd.Connection = con;
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr.HasRows)
                        {
                            dr.Read();
                            category = dr["category"].ToString();
                            sequence = dr["sequence"].ToString();
                        }
                    }
                }
            }
        }



        protected void btn_submit_Click(object sender, EventArgs e)
        {
            parameters = _Dependencies.Functions.functions.get_Auction_Parameters(Request.Url.AbsoluteUri);
            
            string category_ctr = Request.Form["category_ctr"];
            
            String strConnString = ConfigurationManager.ConnectionStrings["AuctionConnectionString"].ConnectionString;

            using (SqlConnection con = new SqlConnection(strConnString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Update_Category", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@auction_ctr", SqlDbType.VarChar).Value = parameters["Auction_ID"];
                    cmd.Parameters.Add("@category_ctr", SqlDbType.VarChar).Value = category_ctr;
                    cmd.Parameters.Add("@category", SqlDbType.VarChar).Value = Request.Form["category"];
                    cmd.Parameters.Add("@sequence", SqlDbType.VarChar).Value = Request.Form["sequence"];

                    cmd.Connection = con;

                    SqlDataReader dr = cmd.ExecuteReader();

                    if (dr.HasRows)
                    {
                        dr.Read();
                        category_ctr = dr["category_ctr"].ToString();
                    }

                    Response.Redirect("CategoryList.aspx");
                }
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Auction.Administration
{
    public partial class ItemList : System.Web.UI.Page
    {
        public string html = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            Dictionary<string, string> parameters = _Dependencies.Functions.functions.get_Auction_Parameters(Request.Url.AbsoluteUri);


            string item_ctr;
            string title;
            string seq;
            string hide;
            string bids;
            string donorname;
            string donors = "";
            string delim = "";
            string images = "";
            //string[] validimages = new string[] { ".jpg", ".gif", ".png", ".jpeg" };


            /*<tr>
            <th>Item</th><th>Seq</th><th>Hide</th><th>Donor(s)</th><th>Images(s)</th><th>Bids</th></tr>
            */

            html = "<thead><tr><th>Item</th><th>Seq</th><th>Hide</th>";
            if (parameters["DoDonors"] == "Yes")
            {
                html += "<th>Donor(s)</th>";
            }
            html += "<th>Images(s)</th><th>Bids</th></tr></thead><tbody>";

            String strConnString = ConfigurationManager.ConnectionStrings["AuctionConnectionString"].ConnectionString;

            using (SqlConnection con = new SqlConnection(strConnString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Get_Items", con))
                {

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@auction_ctr", SqlDbType.Int).Value = parameters["Auction_ID"];
                    cmd.Connection = con;

                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            //Item.Item_CTR, Item.seq, Item.Title, AuctionType.AuctionType
                            item_ctr = dr["item_ctr"].ToString();
                            title = dr["title"].ToString();
                            if (title == "")
                            {
                                title = "UNTITLED";
                            }
                            seq = dr["seq"].ToString();
                            hide = dr["hide"].ToString();
                            bids = dr["bids"].ToString();
                            donors = "";
                            delim = "";

                            if (bids == "0")
                            {
                                bids = "";
                            }
                            else
                            {
                                bids = "<span class=\"itembids\" id=\"item_" + item_ctr + "\">" + bids + " View</span>";
                            }

                            if (parameters["DoDonors"] == "Yes")
                            {
                                using (SqlCommand cmd2 = new SqlCommand("Get_Item_Donors", con))
                                {

                                    cmd2.CommandType = CommandType.StoredProcedure;
                                    cmd.Parameters.Add("@auction_ctr", SqlDbType.Int).Value = parameters["Auction_ID"];
                                    cmd.Connection = con;
                                    cmd2.Parameters.Add("@item_ctr", SqlDbType.Int).Value = item_ctr;
                                    cmd2.Connection = con;

                                    SqlDataReader dr2 = cmd.ExecuteReader();
                                    if (dr2.HasRows)
                                    {
                                        while (dr2.Read())
                                        {
                                            donorname = dr2["donorname"].ToString();

                                            donors += delim + donorname;
                                            delim = "<br />";
                                        }
                                    }
                                }
                            }


                            //string imagepath = path + "\\auction\\items\\" + item_ctr;
                            string path = Server.MapPath("..\\images\\auction" + parameters["Auction_ID"] + "\\items\\" + item_ctr);
                            if (Directory.Exists(path))
                            {
                                //images = "<div class=\"cycle-slideshow\" data-cycle-fx=scrollHorz data-cycle-timeout=2000 data-cycle-center-horz=true data-cycle-center-vert=true data-cycle-log=false>";
                                images = "<div class=\"cycle-slideshow\" data-cycle-timeout=2000 data-cycle-log=false>";

                                //foreach (string dirFile in Directory.GetDirectories(path))
                                //{
                                foreach (string fileName in Directory.GetFiles(path))
                                {
                                    //if (validimages.Contains(Path.GetExtension(fileName).ToLower()))
                                    //{
                                    images += "<img src=\"../images/auction" + parameters["Auction_ID"] + "/items/" + item_ctr + "/" + Path.GetFileName(fileName) + "\" border=\"0\" />";
                                    //}
                                }
                                //}

                                images = "<div class=\"cycle-slideshow\" data-cycle-fx=scrollHorz data-cycle-timeout=2000 data-cycle-log=false>" + images + "</div>";
                            }
                            html += "<tr><td><a href=item.aspx?id=" + item_ctr + ">" + title + "</a><td>" + seq + "</td><td>" + hide + "</td>";
                            if (parameters["DoDonors"] == "Yes")
                            {
                                html += "<td>" + donors + "</td>";
                            }
                            html += "<td>" + images + "</td><td>" + bids + "</td></tr>";
                        }
                    }
                }
            }
            html += "</tbody>";
        }
    }
}

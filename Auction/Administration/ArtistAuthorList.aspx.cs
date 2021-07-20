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
    public partial class ArtistAuthorList : System.Web.UI.Page
    {
        public string html;
        protected void Page_Load(object sender, EventArgs e)
        {
            Dictionary<string, string> parameters = _Dependencies.Functions.functions.get_Auction_Parameters(Request.Url.AbsoluteUri);

            string[] validimages = new string[] { ".jpg", ".gif", ".png" };

            String strConnString = ConfigurationManager.ConnectionStrings["AuctionConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(strConnString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Get_artistauthors", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    //cmd.Parameters.Add("@auction_ctr", SqlDbType.Int).Value = parameters["Auction_ID"];
                    cmd.Connection = con;

                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            string artistauthor_ctr = dr["artistauthor_ctr"].ToString();
                            string name = dr["name"].ToString();
                            string sequence = dr["sequence"].ToString();
                            string hide = dr["hide"].ToString();
                            string items = "";
                            string delim = "";

                            using (SqlCommand cmd2 = new SqlCommand("Get_artistauthor_Items", con))
                            {
                                cmd2.CommandType = CommandType.StoredProcedure;
                                cmd2.Parameters.Add("@artistauthor_ctr", SqlDbType.Int).Value = artistauthor_ctr;
                                cmd2.Parameters.Add("@auction_ctr", SqlDbType.Int).Value = parameters["Auction_ID"];
                                cmd2.Connection = con;

                                SqlDataReader dr2 = cmd.ExecuteReader();
                                if (dr2.HasRows)
                                {
                                    while (dr2.Read())
                                    {
                                        string item_ctr = dr2["item_ctr"].ToString();
                                        string title = dr2["title"].ToString();

                                        items += delim + title;
                                        delim = "<br />";

                                    }
                                }
                            }

                            string images = "";
                            //string imagepath = path + "\\auction" + parameters["Auction_ID"] + "\\artistauthors\\" + artistauthor_ctr;
                            string path = Server.MapPath("..\\images\\auction" + parameters["Auction_ID"] + "\\artistauthors\\" + artistauthor_ctr);

                            if (Directory.Exists(path))
                            {
                                //foreach (string dirFile in Directory.GetDirectories(imagepath))
                                //{
                                foreach (string fileName in Directory.GetFiles(path))
                                {
                                    if (validimages.Contains(Path.GetExtension(fileName).ToLower()))
                                    {
                                        images += "<img src=\"../images/auction" + parameters["Auction_ID"] + "/artistauthors/" + artistauthor_ctr + "/" + Path.GetFileName(fileName) + "\" width=\"160\" border=\"0\" />";
                                    }
                                }
                                //}
                                images = "<div class=\"cycle-slideshow\" data-cycle-timeout=2000 data-cycle-log=false>" + images + "</div>";
                            }
                            html += "<tr><td><a href=artistauthor.aspx?id=" + artistauthor_ctr + ">" + name + "</a><td>" + sequence + "</td><td>" + hide + "</td><td>" + items + "</td><td>" + images + "</td></tr>";
                        }
                    }
                }
            }
        }
    }
}
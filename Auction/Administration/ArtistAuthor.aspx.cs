using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Auction.Administration
{
    public partial class ArtistAuthor : System.Web.UI.Page
    {
        public string artistauthor_ctr;
        //public string All;
        public string name;
        public string information;
        public string alive;
        public string url;
        public string seq;
        public string hide;
        public string images;
        string[] validimages = new string[] { ".jpg", ".gif", ".png" };
        public string[] yesno_values = new string[2] { "Yes", "No" };
        public Dictionary<string, string> parameters;

        protected void Page_Load(object sender, EventArgs e)
        {
            parameters = _Dependencies.Functions.functions.get_Auction_Parameters(Request.Url.AbsoluteUri);
            artistauthor_ctr = Request.QueryString["id"];
            if (!string.IsNullOrEmpty(artistauthor_ctr))
            {
                String strConnString = ConfigurationManager.ConnectionStrings["AuctionConnectionString"].ConnectionString;
                using (SqlConnection con = new SqlConnection(strConnString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("Get_artistauthor", con))
                    {

                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@artistauthor_ctr", SqlDbType.Int).Value = artistauthor_ctr;
                        cmd.Connection = con;

                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr.HasRows)
                        {
                            dr.Read();
                            name = dr["name"].ToString();
                            information = dr["information"].ToString();
                            alive = dr["alive"].ToString();
                            url = dr["url"].ToString();
                            seq = dr["seq"].ToString();
                            hide = dr["hide"].ToString();
                        }
                    }

                    string path = Server.MapPath("..\\images\\auction" + parameters["Auction_ID"] + "\\artistauthors\\" + artistauthor_ctr);

                    if (Directory.Exists(path))
                    {
                        images = "<table><tr>";
                        //foreach (string dirFile in Directory.GetDirectories(path))
                        //{
                        foreach (string fileName in Directory.GetFiles(path))
                        {

                            if (validimages.Contains(Path.GetExtension(fileName).ToLower()))
                            {

                                images += "<td><img src=\"../images/auction" + parameters["Auction_ID"] + "/artistauthors/" + artistauthor_ctr + "/" + Path.GetFileName(fileName) + "\" height=\"160\" border=\"0\" alt=\"" + Path.GetFileName(fileName) + "\"><br />Delete <input name=\"_imgdelete_" + Path.GetFileName(fileName) + "\" type=\"checkbox\" id=\"_imgdelete_" + Path.GetFileName(fileName) + "\" value=\"-1\"></td>";
                            }
                        }
                        //}
                        images += "</tr></table>";
                    }
                }
            }
        }

        private Bitmap ResizeBitmap(Bitmap image, int nWidth, int nHeight)
        {
            int originalWidth = image.Width;
            int originalHeight = image.Height;
            float percentWidth = (float)nWidth / (float)originalWidth;
            float percentHeight = (float)nHeight / (float)originalHeight;
            float percent = percentHeight < percentWidth ? percentHeight : percentWidth;
            int newWidth = (int)(originalWidth * percent);
            int newHeight = (int)(originalHeight * percent);

            Bitmap result = new Bitmap(newWidth, newHeight);
            result.SetResolution(96, 96);
            using (Graphics g = Graphics.FromImage((System.Drawing.Image)result))
                g.DrawImage(image, 0, 0, newWidth, newHeight);
            return result;
        }

        protected void btn_submit_Click(object sender, EventArgs e)
        {
            string artistauthor_ctr = Request.Form["artistauthor_ctr"];
            String strConnString = ConfigurationManager.ConnectionStrings["AuctionConnectionString"].ConnectionString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.CommandText = "Update_artistauthor";
            cmd.Parameters.Add("@auction_ctr", SqlDbType.VarChar).Value = parameters["Auction_ID"];
            cmd.Parameters.Add("@artistauthor_ctr", SqlDbType.VarChar).Value = artistauthor_ctr;
            cmd.Parameters.Add("@name", SqlDbType.VarChar).Value = Request.Form["name"];
            cmd.Parameters.Add("@alive", SqlDbType.VarChar).Value = Request.Form["alive"];
            cmd.Parameters.Add("@information", SqlDbType.VarChar).Value = Request.Form["information"];
            cmd.Parameters.Add("@url", SqlDbType.VarChar).Value = Request.Form["url"];
            cmd.Parameters.Add("@seq", SqlDbType.VarChar).Value = Request.Form["seq"];
            cmd.Parameters.Add("@hide", SqlDbType.VarChar).Value = Request.Form["hide"];

            cmd.Connection = con;
            //try
            //{
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();

            if (dr.HasRows)
            {
                dr.Read();
                artistauthor_ctr = dr["artistauthor_ctr"].ToString();
            }
            //}
            //catch (Exception ex)
            //{
            //    General.Functions.Functions.Log(Request.RawUrl, ex.Message, "greg@datainn.co.nz");
            //}
            //finally
            //{
            con.Close();
            con.Dispose();
            //}
            //don't need to go through here if it's a new artistauthor - will fix sometime
            string path = Server.MapPath("..\\images\\auction" + parameters["Auction_ID"] + "\\artistauthors\\" + artistauthor_ctr);
            string deletepath = Server.MapPath("..\\images\\auction" + parameters["Auction_ID"] + "\\artistauthors\\" + artistauthor_ctr + "\\deleted");

            foreach (string fld in Request.Form)
            {
                if (fld.StartsWith("_imgdelete_"))
                {
                    string filename = fld.Substring(11);
                    if (!Directory.Exists(deletepath))
                    {
                        Directory.CreateDirectory(deletepath);
                    }
                    int c1 = 0;
                    string newfilename = "";
                    string wpextension = System.IO.Path.GetExtension(filename);
                    string wpfilename = System.IO.Path.GetFileNameWithoutExtension(filename);

                    do
                    {
                        c1++;
                        newfilename = wpfilename + "_" + c1.ToString("000") + wpextension;
                    } while (File.Exists(deletepath + "\\" + newfilename));
                    File.Move(path + "\\" + filename, deletepath + "\\" + newfilename);
                }
            }

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string originalpath = Server.MapPath("..\\images\\auction" + parameters["Auction_ID"] + "\\artistauthors\\" + artistauthor_ctr + "\\originals");
            if (!Directory.Exists(originalpath))
            {
                Directory.CreateDirectory(originalpath);
            }

            foreach (HttpPostedFile postedFile in fu_images.PostedFiles)
            {
                if (postedFile.FileName != "")
                {
                    int c1 = 0;
                    string newfilename = "";
                    string wpextension = System.IO.Path.GetExtension(postedFile.FileName);
                    string wpfilename = System.IO.Path.GetFileNameWithoutExtension(postedFile.FileName);

                    do
                    {
                        c1++;
                        newfilename = wpfilename + "_" + c1.ToString("000") + wpextension;
                    } while (File.Exists(newfilename));

                    postedFile.SaveAs(originalpath + "\\" + newfilename);

                    System.Drawing.Image bm = System.Drawing.Image.FromStream(postedFile.InputStream);
                    bm = ResizeBitmap((Bitmap)bm, 100, 100); /// new width, height
                    bm.Save(path + "\\" + newfilename, ImageFormat.Jpeg);
                }

            }

            /*
            if all <> "" then
                response.redirect "artistauthor.asp?all=true&id=" & id
            else
                response.redirect "artistauthorlist.asp"
            end if		
            */
            Response.Redirect("artistauthorList.aspx");
        }
    }
}
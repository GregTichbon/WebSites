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

namespace Auction
{
    public partial class Default : System.Web.UI.Page
    {
        public string html = "";
        public string categories = "";
        public string startupmessage = "";
        public Dictionary<string, string> parameters;
        public string buttonclasses;
        protected void Page_Load(object sender, EventArgs e)
        {

            buttonclasses = myGlobal.buttonclasses;

            String strConnString = ConfigurationManager.ConnectionStrings["AuctionConnectionString"].ConnectionString;

            #region 3 column
            /*
            string[] id = new string[100];
            string[] seq = new string[100];
            string[] title = new string[100];
            string[] description = new string[100];
            int[] donors_cnt = new int[100];
            string[,] donors_ID = new string[10, 2];
            string[,] donors_Title = new string[10, 2];
            string[,] donors_Images = new string[10, 2];

            int c1 = 0;
            int auctiontype = 1;
            string canclick = "";
            if (auctiontype == 1)
            {
                canclick = " canclick";
            }
            else
            {
                canclick = "";
            }
            //string imagefolder = HttpContext.Current.Request.PhysicalApplicationPath + "auction\\images\\auction\\items";
            string imagefolder = Server.MapPath("images\\auction\\items");


            String strConnString = ConfigurationManager.ConnectionStrings["AuctionConnectionString"].ConnectionString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand("Get_Items", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@auction_ctr", SqlDbType.Int).Value = parameters["Auction_ID"];;
            cmd.Parameters.Add("@auctiontype_ctr", SqlDbType.Int).Value = auctiontype;
            cmd.Connection = con;
            try
            {
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        c1++;
                        id[c1] = dr["item_ctr"].ToString();
                        seq[c1] = dr["seq"].ToString();
                        title[c1] = dr["title"].ToString();
                        description[c1] = dr["description"].ToString();

                        if(c1 == 3) { 
                        break;
                        }

                        //html += title[c1] + " - " + description[c1] + "<br />";

                        //Get code from WDC for delimited data from SP in a column.  This is for the potential of multiple donors
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
                con.Dispose();
            }

            int realc1 = 0;

            for (int f1 = 1; f1 < c1; f1 += 3)
            {
                if (f1 != 1)
                {
                    html += "<hr />";
                }
                html += "<div class=\"row\">"; //; //& vbcrlf
                for (int f2 = 0; f2 < 2; f2++)
                {
                    html += "<div class=\"col-sm-4\" >"; //; //& vbcrlf
                    if (f1 + f2 <= realc1)
                    {
                        html += "<div class=\"mycentered\">"; //& vbcrlf
                        html += "<div style=\"height: 50px\"><h3>" + seq[f1 + f2] + ". " + title[f1 + f2] + "</h3></div>"; //& vbcrlf
                    }
                    html += "<div class=\"slideshow " + canclick + " id=\"viewitem" + id[f1 + f2] + " data-cycle-fx=scrollHorz data-cycle-timeout=2000 data-cycle-center-horz=true data-cycle-center-vert=true>";
                    string thisimagefolder = imagefolder + "\\" + id[f1 + f2];
                    html += thisimagefolder;
                    if (Directory.Exists(thisimagefolder))
                    {
                        string[] files = Directory.GetFiles(thisimagefolder, "*.*", SearchOption.TopDirectoryOnly);
                        foreach (string filename in files)
                        {
                            string justfilename = System.IO.Path.GetFileName(filename);
                            //if (filename.EndsWith("gif") || filename.EndsWith("jpg") || filename.EndsWith("png"))
                            //{
                            html += "<img src=\"images/items/" + id[f1 + f2] + "/" + justfilename + "\" height=\"160\" border=\"0\" alt=\"" + filename + "\">";
                            //}
                        }
                    }

                    html += "</div>";
                    html += "</div>"; //& vbcrlf
                    html += "<div style=\"text-align: justify; text-justify: inter-word;\">" + description[f1 + f2] + "</div>"; //& vbcrlf
                    html += "<div class=\"mycentered\">"; //& vbcrlf
                    if (auctiontype == 1)
                    {
                        html += "<img class=\"showitem\" id=\"showitem" + id[f1 + f2] + "\" src=\"bidnow.png\">";
                    }
                    html += "<h3>Generously donated by</h3>"; //& vbcrlf
                    html += "<div class=\"slideshow\" data-cycle-fx=scrollHorz data-cycle-timeout=2000 data-cycle-center-horz=true data-cycle-center-vert=true>";
                    for (int f3 = 1; f3 < donors_cnt[f1 + f2]; f3++)
                    {
                        {
                            string[] a = donors_Images[f1 + f2, f3].Split('|');
                            foreach (string x in a)
                            {
                                html += "Test: " + x;
                            }
                        }
                    }
                    html += "</div>";
                    for (int f3 = 1; f3 < donors_cnt[f1 + f2]; f3++)
                    {
                        html += "<p>" + donors_Title[f1 + f2, f3] + "</p>"; //& vbcrlf
                    }
                    html += "</div>"; //& vbcrlf
                    html += "</div>"; //& vbcrlf
                }
                html += "</div>"; //& vbcrlf
            }
        }
        */
            #endregion

            parameters = _Dependencies.Functions.functions.get_Auction_Parameters(Request.Url.AbsoluteUri);
            if (parameters["Closeat"] != "")
            {
                if (DateTime.Now > Convert.ToDateTime(parameters["Closeat"]))
                {
                    //logout
                    HttpContext.Current.Session.Remove("Auction_user_ctr");
                    HttpContext.Current.Session.Remove("Auction_Fullname");

                    HttpContext.Current.Response.Cookies["Auction_user_ctr"].Expires = DateTime.Now.AddDays(-1);
                    HttpContext.Current.Response.Cookies["Auction_Fullname"].Expires = DateTime.Now.AddDays(-1);

                    startupmessage = "startupmessage('showmessage.aspx?id=closedmessage','This Auction has closed.')";
                }
            }

            //---------------------------------------

            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd;

            if (parameters["EnableCategories"] == "Yes")
            {
                categories += "<button class=\"categoryselect categoryselected " + buttonclasses + "\" id=\"btn_category_All\" type=\"button\">All</button>";

                string ctrl_category_ctr;
                string ctrl_category;



                cmd = new SqlCommand("Get_Categories", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@auction_ctr", SqlDbType.Int).Value = parameters["Auction_ID"];
                cmd.Connection = con;
                try
                {
                    con.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            ctrl_category_ctr = dr["category_ctr"].ToString();
                            ctrl_category = dr["category"].ToString();

                            categories += "&nbsp;&nbsp;<button class=\"categoryselect " + buttonclasses + "\" id=\"btn_category_" + ctrl_category_ctr + "\" type=\"button\">" + ctrl_category + "</button>";
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }

            //======================================


            string id = "";
            string seq = "";
            string title = "";
            string description = "";
            string shortdescription = "";
            string category = "";

            string hide = "";
            string donor_ctr;
            string donors = "";
            string delim = "";
            string donorimages = "";


            int[] donors_cnt = new int[100];
            string[,] donors_ID = new string[10, 2];
            string[,] donors_Title = new string[10, 2];
            string[,] donors_Images = new string[10, 2];

            string canclick = "";
            if (parameters["AuctionType"] == "Silent")
            {
                canclick = " canclick";
            }
            //string imagefolder = HttpContext.Current.Request.PhysicalApplicationPath + "auction\\images\\auction\\items";
            string imagefolder = Server.MapPath("images\\auction" + parameters["Auction_ID"] + "\\items");
            string donorimagefolder = Server.MapPath("images\\auction" + parameters["Auction_ID"] + "\\donors");


            con = new SqlConnection(strConnString);
            SqlConnection con2 = new SqlConnection(strConnString);
            cmd = new SqlCommand("Get_Items", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@auction_ctr", SqlDbType.VarChar).Value = parameters["Auction_ID"];
            cmd.Connection = con;
            //int c1 = 0;
            try
            {
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        //c1++;
                        //if (c1 < 30000)
                        //{
                        id = dr["item_ctr"].ToString();
                        seq = dr["seq"].ToString();
                        title = dr["title"].ToString();
                        description = dr["description"].ToString();
                        shortdescription = dr["shortdescription"].ToString();
                        hide = dr["hide"].ToString();
                        category = dr["category_ctr"].ToString();
                        donors = "";
                        delim = "";

                        if (hide != "Yes")
                        {

                            cmd = new SqlCommand("Get_Item_Donors", con);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add("@item_ctr", SqlDbType.Int).Value = id;
                            cmd.Connection = con2;
                            try
                            {
                                con2.Open();
                                SqlDataReader dr2 = cmd.ExecuteReader();
                                if (dr2.HasRows)
                                {
                                    while (dr2.Read())
                                    {
                                        donor_ctr = dr2["donor_ctr"].ToString();
                                        string donor_url = dr2["url"].ToString();
                                        if (donor_url == "")
                                        {
                                            donors += delim + dr2["donorname"].ToString();
                                        }
                                        else
                                        {
                                            donors += delim + "<a href=\"" + donor_url + "\" target=\"_blank\" class=\"donor-link donor_name\" data-id=\"" + id + "\">" + dr2["donorname"].ToString() + "</a>";
                                        }
                                        delim = "<br />";
                                        donorimages = "";

                                        string thisdonorimagefolder = donorimagefolder + "\\" + id;
                                        if (Directory.Exists(thisdonorimagefolder))
                                        {
                                            string[] files = Directory.GetFiles(thisdonorimagefolder, "*.*", SearchOption.TopDirectoryOnly);
                                            foreach (string filename in files)
                                            {
                                                string justfilename = System.IO.Path.GetFileName(filename);
                                                //if (filename.EndsWith("gif") || filename.EndsWith("jpg") || filename.EndsWith("png"))
                                                //{
                                                donorimages += "<img class=\"donor-link donor_image\" data-id=\"" + id + "\" src=\"images/auction" + parameters["Auction_ID"] + "/donors/" + id + "/" + justfilename + "\" border=\"0\" alt=\"\" />";
                                                //}
                                            }
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                throw ex;
                            }
                            finally
                            {
                                con2.Close();
                            }


                            /*
                                     html += "<div class=\"items_title\">" + title + "</div>";
                                     html += "<div class=\"items_shortdescription\" > " + shortdescription + "</div>";




                                     html += "<div class=\"items_donated_head\">Generously Donated by";
                                     html += donors;
                                     if (donorimages != "")
                                     {
                                         html += "<div class=\"cycle-slideshow donor-slideshow\" data-cycle-timeout=2000 data-cycle-log=false>";
                                         html += donorimages;
                                         html += "</div>"; //slideshow
                                     }
                                     html += "</div>"; //<div class=\"items_donated_head\">Generously Donated by

                                     html += "<hr />";
                                     html += "</div>";
                                     */
                            //Joe start

                            html += "<div class=\"pa3 w-100 w-50-m w-third-l div_category\" data-category=\"" + category + "\">"; //D1 +
                            //html += "<div class=\"item pointer bg-white shadow-5 grow\" id=\"viewitem4\">"; //D2 +
                            html += "<div class=\"item pointer bg-white shadow-5 growx\">"; //D2 +
                                                                                            //Item image / slideshow  
                            html += " <div class=\"w-100 mb4\">"; //D3 Slidshow +
                            string images = "";
                            string thisimagefolder = imagefolder + "\\" + id;
                            if (Directory.Exists(thisimagefolder))
                            {
                                string[] files = Directory.GetFiles(thisimagefolder, "*.*", SearchOption.TopDirectoryOnly);
                                foreach (string filename in files)
                                {
                                    string justfilename = System.IO.Path.GetFileName(filename);
                                    //if (filename.EndsWith("gif") || filename.EndsWith("jpg") || filename.EndsWith("png"))
                                    //{
                                    images += "<img src=\"images/auction" + parameters["Auction_ID"] + "/items/" + id + "/" + justfilename + "\" class=\"w-100\" alt=\"\" />";
                                    //}
                                }
                            }
                            if (images != "")
                            {
                                html += "<div class=\"cycle-slideshow item-slideshow" + canclick + "\" id=\"viewitem" + id + "\" data-title=\"" + title + "\" data-cycle-timeout=2000 data-cycle-center-horz=true data-cycle-log=false>"; //D4
                                html += images;
                                html += "</div>"; // -D4
                            }

                            html += " </div>"; // D3 Slideshow -
                                               //item details  
                            html += "<div class=\"item_details ph4 pb4\">"; //D5
                            html += "<h2 class=\"items_title f4 b\">" + title + "</h2>";
                            html += "<div class=\"items_shortdescription lh-copy f6\">"; //D6
                            html += shortdescription;
                            html += "</div>"; //D6-
                            if (parameters["DoDonors"] == "Yes")
                            {
                                html += "<div class=\"items_donated_head ttu f6 mt4 mb3 tracked bt pt3\">Generously Donated by</div>"; //D
                                html += "<div class=\"items_donated_donor b\">" + donors + "</div>"; //D
                            }
                            html += "</div>"; //D5-
                            html += "</div>"; //D2-
                            html += "</div>"; //D1-

                            //Joe End

                        }

                        //}
                    }
                    con2.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
        }
    }
}

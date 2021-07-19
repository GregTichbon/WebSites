using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace Auction._Dependencies
{
    public partial class Data1 : System.Web.UI.Page
    {
        public string html = "";
        public string buttonclasses = myGlobal.buttonclasses;

        protected void Page_Load(object sender, EventArgs e)
        {

            string mode = Request.Form["mode"];

            switch (mode)
            {
                case "get_item":
                    get_item();
                    break;

            }
        }

        protected string get_item()
        {
            string itemimages = "";

            Dictionary<string, string> parameters = _Dependencies.Functions.functions.get_Auction_Parameters(Request.Url.AbsoluteUri);
            /*
            if (parameters["Closeat"] != "")
            {
                if (DateTime.Now > Convert.ToDateTime(parameters["Closeat"]))
                {
                    //logout
                    HttpContext.Current.Session.Remove("Auction_user_ctr");
                    HttpContext.Current.Session.Remove("Auction_Fullname");

                    HttpContext.Current.Response.Cookies["Auction_user_ctr"].Expires = DateTime.Now.AddDays(-1);
                    HttpContext.Current.Response.Cookies["Auction_Fullname"].Expires = DateTime.Now.AddDays(-1);

                }
            }
            */

         
            string user_ctr = (string)Session["Auction_user_ctr"] ?? "";
            string fullname = (string)Session["Auction_Fullname"] ?? "";

            if (user_ctr == "")
            {
                if (Request.Cookies["Auction_user_ctr"] != null)
                {
                    user_ctr = Request.Cookies["Auction_user_ctr"].Value + "";
                    fullname = Request.Cookies["Auction_Fullname"].Value + "";
                }
            }
        

            string item_ctr = Request.Form["id"];
            string title = "";
            string shortdescription = "";
            string description = "";
            Double reserve = 0;
            string retailprice = "";
            Double increment = 0;
            Double startbid = 0;

            string you = "";
            double autobid = 0;
            string hf_highestbid;
            string hf_highestbidder;
            string highestbidmessage;
            string highestbidder;
            string nextminimum;
            string displayregister;
            string displayloggedin;
            string displaylogin;
            string displayautobid = "None";
            string autobidamount = "";
            string autobidchecked = "";
            double currentbid = 0;
            string reservenote = "";


            String strConnString = ConfigurationManager.ConnectionStrings["AuctionConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(strConnString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Get_Item", con))
                {

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@item_ctr", SqlDbType.VarChar).Value = item_ctr;
                    cmd.Connection = con;

                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        dr.Read();
                        title = dr["title"].ToString();
                         shortdescription = dr["shortdescription"].ToString();
                         description = dr["Description"].ToString();
                         reserve = Convert.ToDouble(dr["Reserve"]);
                         retailprice = dr["RetailPrice"].ToString();
                         increment = Convert.ToDouble(dr["increment"]);
                        if (increment == 0)
                        {
                            increment = Convert.ToDouble(parameters["Increment"]);
                        }
                         startbid = Convert.ToDouble(dr["startbid"]);
                    }
                    dr.Close();
                }

                string imagefolder = Server.MapPath("\\images\\auction" + parameters["Auction_ID"] + "\\items");
                //string donorimagefolder = Server.MapPath("\\images\\auction" + parameters["Auction_ID"] + "\\donors");

                string thisimagefolder = imagefolder + "\\" + item_ctr;
                if (Directory.Exists(thisimagefolder))
                {
                    string[] files = Directory.GetFiles(thisimagefolder, "*.*", SearchOption.TopDirectoryOnly);
                    foreach (string filename in files)
                    {
                        string justfilename = System.IO.Path.GetFileName(filename);
                        //if (filename.EndsWith("gif") || filename.EndsWith("jpg") || filename.EndsWith("png"))
                        //{
                        string src = "\"/images/auction" + parameters["Auction_ID"] + "/items/" + item_ctr + "/" + justfilename + "\"";
                        itemimages += "<img src=" + src + "\" class=\"magnifier\" data-magnify-src=" + src + " />";
                        //}
                    }
                }
                if (itemimages != "")
                {
                    itemimages = "<div class=\"cycle-slideshow showitem-slideshow\" data-cycle-timeout=2000 data-cycle-log=false>" + itemimages + "</div>";
                }
            
                double yourbid;
           
                using (SqlCommand cmd = new SqlCommand("Get_bid_information", con))
                {
                    //con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@item_ctr", SqlDbType.VarChar).Value = item_ctr;
                    cmd.Connection = con;

                    //con.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        dr.Read();

                        currentbid = Convert.ToDouble(dr["amount"]);

                        ;
                        if (reserve > currentbid)
                        {
                            reservenote = " <span class=\"reservenote\">Reserve not met</span>";
                        }

                        string autobidnote = "";

                         hf_highestbid = currentbid.ToString("#.00");
                         hf_highestbidder = dr["user_ctr"].ToString();

                        if (hf_highestbidder == user_ctr)
                        {
                            you = " <b>(YOU!)</b>";
                            autobid = Convert.ToDouble(dr["autobid"]);
                            if (autobid > currentbid)
                            {
                                autobidnote = " <b>Autobid</b>: $" + autobid.ToString("#.00");
                            }
                        }
                        else
                        {
                            you = "";
                        }
                        highestbidmessage = "$" + hf_highestbid + autobidnote + reservenote;
                        highestbidder = dr["fullname"].ToString() + you;
                        yourbid = currentbid + increment;
                        if (yourbid < startbid)
                        {
                            yourbid = startbid;
                        }
                        nextminimum = yourbid.ToString("#.00");
                    }
                    else
                    {
                        hf_highestbid = "0";
                        highestbidmessage = "No bids yet .... be the first";
                        highestbidder = "Give it a go";
                        yourbid = startbid;
                        nextminimum = yourbid.ToString("#.00");
                    }
                }
                if (user_ctr == "")
                {
                    // usernamelabel = "Returning user?<br />Enter your pass code to bid:";
                    //usernamedisplay = "<input name=\"passcode\" type=\"text\" id=\"passcode\">"; //replace with style
                    displayloggedin = "none";
                    displaylogin = "";
                    displayregister = "";
                }
                else
                {
                    //usernamelabel = "Logged in as:";
                    //usernamedisplay = username + "&nbsp;&nbsp;<input type=\"button\" name=\"logout\" id=\"logout\" value=\"Log out\">"; //replace with style
                    displayloggedin = "";
                    displaylogin = "none";
                    displayregister = "none";


                    using (SqlCommand cmd = new SqlCommand("Get_bid_information", con))
                    {

                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@user_ctr", SqlDbType.VarChar).Value = user_ctr;
                        cmd.Parameters.Add("@item_ctr", SqlDbType.VarChar).Value = item_ctr;
                        cmd.Connection = con;


                        autobid = Convert.ToDouble(cmd.ExecuteScalar());

                        //Get the an auto bid for this user and item if it is greater than the current bid -- This will also need to be done in data.asmx
                        if (autobid != 0)
                        {
                            displayautobid = "";
                            autobidamount = autobid.ToString("#.00");
                            //autobidchecked = " checked";
                        }
                    }
                }
            }


            html = "<div class=\"todo\">" + title + "</div>";
            html += "<div class=\"mb4\">" + itemimages + "</div>";
            html += "<div class=\"lh-copy f6\">" + shortdescription + "</div>";
            html += "<div class=\"todo\">" + description + "</div>";

            if(user_ctr == "")
            {
                html += "<button type=\"button\" class=\"register " + buttonclasses + "\" data-thisdiv=\"div_item\">Register</button>";
                html += "<button type=\"button\" class=\"login " + buttonclasses + "\" data-thisdiv=\"div_item\">Login</button>";
            }


            html += "<div class=\"todo\">" + "currentbid:" + currentbid + "</div>";
            html += "<div class=\"todo\">" + "reservenote:" + reservenote + "</div>";
            if (parameters["ShowHighestBidder"] == "Yes")
            {
                html += "<div class=\"todo\">" + "highestbidder:" + highestbidder + "</div>";
            }
            html += "<div class=\"todo\">" + "highestbidmessage:" + highestbidmessage + "</div>";
            html += "<div class=\"todo\">" + "you:" + you + "</div>";
            html += "<div class=\"todo\">" + "autobid:" + autobid.ToString() + "</div>";
            html += "<div class=\"todo\">" + "nextminimum:" + nextminimum + "</div>";
            html += "<div class=\"todo\">" + "autobidamount:" + autobidamount + "</div>";

            /*
            string hf_highestbid;
            string hf_highestbidder;
            string displayregister;
            string displayloggedin;
            string displaylogin;
            string displayautobid = "None";
            string autobidchecked = "";
            */


            return (html);
        }
    }
}
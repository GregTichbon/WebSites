using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Generic;


namespace Auction.Administration
{
    public partial class Setup : System.Web.UI.Page
    {

        public static string auction_ctr;
        public string auction;
        public string message;
        public string increment;
        public string openfrom;
        public string closeat;
        public string closedmessage;
        public string termsandconditions;
        public string url;
        public string auctiontype;
        public string emailalerts;
        public string textalerts;
        public string enablecategories;
        public string bidemail;
        public string bidtext;
        public string emailfrom;
        public string emailfromname;
        public string emailhost;
        public string emailpassword;
        public string emailreplyto;
        public string bidlog;
        public string advisetest;
        public string dodonors;
        public string doartistsauthors;
        public string showhighestbidder;
        public string lowervalue;
        public string uppervalue;


        public string[] auctiontype_values = new string[2] { "Silent", "Live" };
        public string[] yesno_values = new string[2] { "Yes", "No" };
        //public string[] valuetype_values = new string[3] { "None", "Retail","Upper/Lower"};

        public Dictionary<string, string> parameters;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                parameters = _Dependencies.Functions.functions.get_Auction_Parameters(Request.Url.AbsoluteUri);

                auction_ctr = parameters["Auction_CTR"];
                auction = parameters["Auction"];
                message = parameters["Message"];
                increment = parameters["Increment"];
                openfrom = parameters["OpenFrom"];
                if (openfrom != "")
                {
                    openfrom = Convert.ToDateTime(openfrom).ToString("d-MMM-yy hh:mm");
                }
                closeat = parameters["Closeat"];
                if (closeat != "")
                {
                    closeat = Convert.ToDateTime(closeat).ToString("d-MMM-yy hh:mm");
                }
                closedmessage = parameters["ClosedMessage"];                                                 //   CASE SENSITIVE
                termsandconditions = parameters["TermsAndConditions"];
                url = parameters["URL"];
                auctiontype = parameters["AuctionType"];
                emailalerts = parameters["EmailAlerts"];
                textalerts = parameters["TextAlerts"];
                enablecategories = parameters["EnableCategories"];
                bidemail = parameters["BidEmail"];
                bidtext = parameters["BidText"];
                emailfrom = parameters["EmailFrom"];
                emailfromname = parameters["EmailFromName"];
                emailhost = parameters["EmailHost"];
                emailpassword = parameters["EmailPassword"];
                emailreplyto = parameters["EmailReplyTo"];
                bidlog = parameters["BidLog"];
                advisetest = parameters["AdviseTest"];
                dodonors = parameters["DoDonors"];
                doartistsauthors = parameters["DoArtistsAuthors"];
                showhighestbidder = parameters["ShowHighestBidder"];
                lowervalue = parameters["LowerValue"];
                uppervalue = parameters["UpperValue"];
            }


        }

        protected void btn_submit_Click(object sender, EventArgs e)
        {

            String strConnString = ConfigurationManager.ConnectionStrings["AuctionConnectionString"].ConnectionString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.CommandText = "Update_Auction";
            cmd.Parameters.Add("@auction_ctr", SqlDbType.VarChar).Value = auction_ctr;
            cmd.Parameters.Add("@auction", SqlDbType.VarChar).Value = Request.Form["auction"];
            cmd.Parameters.Add("@message", SqlDbType.VarChar).Value = Request.Form["message"];
            cmd.Parameters.Add("@increment", SqlDbType.VarChar).Value = Request.Form["increment"];
            cmd.Parameters.Add("@openfrom", SqlDbType.VarChar).Value = Request.Form["openfrom"];
            cmd.Parameters.Add("@closeat", SqlDbType.VarChar).Value = Request.Form["closeat"];
            cmd.Parameters.Add("@closedmessage", SqlDbType.VarChar).Value = Request.Form["closedmessage"];
            cmd.Parameters.Add("@termsandconditions", SqlDbType.VarChar).Value = Request.Form["termsandconditions"];
            cmd.Parameters.Add("@url", SqlDbType.VarChar).Value = Request.Form["url"];
            cmd.Parameters.Add("@auctiontype", SqlDbType.VarChar).Value = Request.Form["auctiontype"];
            cmd.Parameters.Add("@emailalerts", SqlDbType.VarChar).Value = Request.Form["emailalerts"];
            cmd.Parameters.Add("@textalerts", SqlDbType.VarChar).Value = Request.Form["textalerts"];

            cmd.Parameters.Add("@enablecategories", SqlDbType.VarChar).Value = Request.Form["enablecategories"];
            cmd.Parameters.Add("@bidemail", SqlDbType.VarChar).Value = Request.Form["bidemail"];
            cmd.Parameters.Add("@bidtext", SqlDbType.VarChar).Value = Request.Form["bidtext"];
            cmd.Parameters.Add("@emailfrom", SqlDbType.VarChar).Value = Request.Form["emailfrom"];
            cmd.Parameters.Add("@emailfromname", SqlDbType.VarChar).Value = Request.Form["emailfromname"];
            cmd.Parameters.Add("@emailhost", SqlDbType.VarChar).Value = Request.Form["emailhost"];
            cmd.Parameters.Add("@emailpassword", SqlDbType.VarChar).Value = Request.Form["emailpassword"];
            cmd.Parameters.Add("@emailreplyto", SqlDbType.VarChar).Value = Request.Form["emailreplyto"];
            cmd.Parameters.Add("@bidlog", SqlDbType.VarChar).Value = Request.Form["bidlog"];
            cmd.Parameters.Add("@advisetest", SqlDbType.VarChar).Value = Request.Form["advisetest"];
            cmd.Parameters.Add("@dodonors", SqlDbType.VarChar).Value = Request.Form["dodonors"];
            cmd.Parameters.Add("@doartistsauthors", SqlDbType.VarChar).Value = Request.Form["doartistsauthors"];
            cmd.Parameters.Add("@showhighestbidder", SqlDbType.VarChar).Value = Request.Form["showhighestbidder"];
            cmd.Parameters.Add("@lowervalue", SqlDbType.VarChar).Value = Request.Form["lowervalue"];
            cmd.Parameters.Add("@uppervalue", SqlDbType.VarChar).Value = Request.Form["lowervalue"];

            cmd.Connection = con;
            //try
            //{
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();

            if (dr.HasRows)
            {
                dr.Read();
                //item_ctr = dr["item_ctr"].ToString();
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
            Response.Redirect(Request.RawUrl);
        }
    }
}
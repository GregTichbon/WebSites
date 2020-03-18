using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace office.dataInn.co.nz.Raffles.UBC2019B
{
    public partial class CommunicateWithWinners : System.Web.UI.Page
    {
        public string html = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            string strConnString = "Data Source=toh-app;Initial Catalog=DataInnovations;Integrated Security=False;user id=OnlineServices;password=Whanganui497";

            Generic.Functions gFunctions = new Generic.Functions();

            int c1 = 0;


            html += "<tr><td colspan=\"11\"><input id=\"cb_toggleall\" type=\"checkbox\" /></td></tr>";
            html += "<tr><td>Draw</td><td>Ticket</td><td>Purchaser</td><td>Greeting</td><td>Mobile</td><td>Email Address</td><td>Voucher</td><td>Iti.Ninja</td><td>Winner Status</td><td>Winner Response</td><td>Winner Note</td></tr>";

            string sql = @"select W.rafflewinner_id, R.identifier, T.RaffleTicket_ID, T.purchaser, t.greeting, t.mobile, t.TicketNumber, t.EmailAddress, t.Greeting, W.guid, W.status, W.Draw, W.Notes, W.Response, W.itininjaid, W.drawndate
                from RaffleWinner W
                inner join raffleticket T on T.RaffleTicket_ID = W.RaffleTicket_ID
                inner join Raffle R on R.Raffle_ID = T.Raffle_ID 
                where isnull(w.status,'') <> '' 
                and [code] = 'UBC2019B'
                order by isnull(W.status,'') desc, R.identifier, w.Draw";

            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand(sql, con)
            {
                CommandType = CommandType.Text,
                Connection = con
            };
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.Connection = con;


            //try
            //{
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {

                string link = "voucher.aspx?id=";

                while (dr.Read())
                {
                    string rafflewinner_id = dr["rafflewinner_id"].ToString();
                    string identifier = dr["identifier"].ToString();
                    string id = dr["RaffleTicket_ID"].ToString();
                    string purchaser = dr["Purchaser"].ToString();
                    string mobile = dr["mobile"].ToString();
                    string ticketnumber = dr["ticketnumber"].ToString();
                    string emailaddress = dr["emailaddress"].ToString();
                    string itininjaid = dr["itininjaid"].ToString();
                    string greeting = dr["greeting"].ToString();
                    string guid = dr["guid"].ToString();
                    string winnerstatus = dr["status"].ToString();
                    string winnerresponse = dr["response"].ToString();
                    string winnernote = dr["notes"].ToString();
                    string draw = dr["draw"].ToString();
                    string drawndate = "";
                    if (dr["drawndate"] != DBNull.Value)
                    {
                        drawndate = Convert.ToDateTime(dr["drawndate"]).ToString("dd MMM yy");
                    }

                    string itininjahtml = "";

                    string url = "http://iti.ninja/data.aspx?mode=Track_link&link=" + itininjaid + "&format=simple";
                    HttpWebRequest httprequest = WebRequest.Create(url) as HttpWebRequest;
                    HttpWebResponse httpresponse = (HttpWebResponse)httprequest.GetResponse();
                    WebHeaderCollection header = httpresponse.Headers;

                    var httpencoding = System.Text.ASCIIEncoding.ASCII;
                    using (var reader = new System.IO.StreamReader(httpresponse.GetResponseStream(), httpencoding))
                    {
                        itininjahtml = reader.ReadToEnd();
                    }

                    html += "<tr><td><input class=\"checkbox\" id=\"cb_" + id + "\" name=\"cb_" + id + "\" value=\"x\" type=\"checkbox\" /><a href=\"" + link + guid + "\" target=\"order\">" + identifier + " " + draw + "</a><br />" + drawndate + "</td><td>" + ticketnumber + "</td><td>" + purchaser + "</td><td>" + greeting + "</td><td>" + mobile + "</td><td>" + emailaddress + "</td><td><a target=\"voucher\" href=\"../images/vouchers/" + guid + ".jpg\">View</a></td><td class=\"itininja\">" + itininjahtml + "</td><td>" + winnerstatus + "</td><td>" + winnerresponse + "</td><td>" + winnernote + "</td></tr>";

                    if (IsPostBack)
                    {
                        string val = Request.Form["cb_" + id];
                        if (val == "x")
                        {
                            string textmessage = tb_message.Text;
                            if (textmessage.Contains("||voucher||"))
                            {
                                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://iti.ninja/posts.asmx/Create_Link");
                                request.Method = "POST";

                                string jsonContent = @"{
                                ""link"": """",
                                ""datetimefrom"": """",
                                ""datetimeto"": """",
                                ""url"": ""http://office.datainn.co.nz/raffles/images/vouchers/" + guid + @".jpg"",
                                ""description"": ""Raffle - Winner: " + purchaser + @""",
                                ""recordlog"": ""Yes""}";

                                System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
                                Byte[] byteArray = encoding.GetBytes(jsonContent);
                                request.ContentLength = byteArray.Length;
                                request.ContentType = @"application/json";
                                using (Stream dataStream = request.GetRequestStream())
                                {
                                    dataStream.Write(byteArray, 0, byteArray.Length);
                                }
                                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                                Stream receiveStream = response.GetResponseStream();
                                StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);
                                string ans = readStream.ReadToEnd();
                                dynamic result = JsonConvert.DeserializeObject(ans);
                                string itininja = result.d.value;
                                response.Close();
                                readStream.Close();
                                textmessage = textmessage.Replace("||voucher||", "http://iti.ninja/" + itininja);

                                sql = "update rafflewinner set itininjaID = '" + itininja + "' where raffleWinner_id = " + rafflewinner_id;
                                SqlConnection con2 = new SqlConnection(strConnString);
                                SqlCommand cmd2 = new SqlCommand(sql, con2);
                                cmd2.CommandType = CommandType.Text;
                                cmd2.Connection = con2;
                                con2.Open();
                                //SqlDataReader dr = cmd.ExecuteReader();
                                cmd2.ExecuteNonQuery();
                                cmd2.Dispose();
                                con2.Close();
                                con2.Dispose();

                            }

                            textmessage = textmessage.Replace("||greeting||", greeting);
                            textmessage = textmessage.Replace("||ticketnumber||", ticketnumber);
                            textmessage = textmessage.Replace("||guid||", guid);
                            textmessage = textmessage.Replace("||identifier||", identifier);
                            foreach (string mobilex in mobile.Split(';'))
                            {
                                Response.Write(gFunctions.SendRemoteMessage(mobilex, textmessage, "Raffle Communication") + "<br />");
                                c1++;
                            }
                        }
                    }
                }
            }

            dr.Close();
            con.Close();
            con.Dispose();

            if (IsPostBack)
            {
                Response.Write(c1.ToString() + " messages sent");
            }
        }

        protected void btn_submit_Click(object sender, EventArgs e)
        {

        }
    }
}
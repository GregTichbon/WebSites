using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace office.dataInn.co.nz.Raffles.UBC2019B
{
    public partial class Default : System.Web.UI.Page
    {
        public string raffle;
        public int available1 = 0;
        public int available2 = 0;
        public string identifier;
        public string bankaccount;
        public string rafflename;
        public string detail;
        public string MobileToText;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string strConnString = "Data Source=toh-app;Initial Catalog=DataInnovations;Integrated Security=False;user id=OnlineServices;password=Whanganui497";


                string purchaser;
                int c;
                string line1;
                string line2;
                string format;
                int columns = 10;
                int firstticket = 1;
                int lastticket = 50;
                Boolean rafflefound = true;


                string guid = Request.QueryString["id"] + "";
                string named = Request.QueryString["n"] + "";

                SqlConnection con = new SqlConnection(strConnString);
                SqlCommand cmd = new SqlCommand();
                SqlDataReader dr;

                if (guid == "" && named == "")
                {
                    LitRows2.Text = "Sorry, there is no raffle available.";
                    rafflefound = false;
                }
                else
                {
                    string select = "";
                    if (guid != "")
                    {
                        select = "cast([guid] as varchar(50)) = '" + guid + "'";
                    }
                    else if (named != "")
                    {
                        select = "named = '" + named + "'";
                    }

                    string sql1 = "select * from raffle where " + select;
                    cmd = new SqlCommand(sql1, con);

                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = con;
                    //try
                    //{
                    con.Open();
                    dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        dr.Read();
                        raffle = dr["raffle_id"].ToString();
                        identifier = dr["identifier"].ToString();
                        columns = Convert.ToInt16(dr["columns"]);
                        firstticket = Convert.ToInt16(dr["firstticket"]);
                        lastticket = Convert.ToInt16(dr["lastticket"]);
                        bankaccount = dr["bankaccount"].ToString();
                        rafflename = dr["name"].ToString();
                        detail = dr["detail"].ToString();
                        MobileToText = dr["MobileToText"].ToString();

                    }
                    else
                    {
                        LitRows2.Text = "Sorry, there is no raffle available.";
                        rafflefound = false;
                    }
                    dr.Close();
                }
                if (rafflefound)
                {

                    LitRows2.Text += "<tr><td colspan=\"" + (columns + 1) + "\"><h2>THE " + identifier + " RAFFLE</h2></td></tr>";

                    c = 0;
                    line1 = "";
                    line2 = "";
                    format = "";

                    string sql2 = "select * from raffleticket where raffle_id = " + raffle + " and ticketnumber between " + firstticket + " and " + lastticket + " order by [TicketNumber]";

                    cmd = new SqlCommand(sql2, con)
                    {
                        CommandType = CommandType.Text,
                        Connection = con
                    };
                    //try
                    //{

                    dr = cmd.ExecuteReader();


                    while (dr.Read())
                    {
                        c++;

                        purchaser = dr["Purchaser"].ToString();
                        if (purchaser == "")
                        {
                            purchaser = "<input type=\"button\" class=\"iwantthisticket\" value=\"Buy\" id=\"ticket_" + raffle + "-" + dr["ticketnumber"] + "\" />";
                            available2++;
                        }
                        else
                        {
                            if (dr["Paid"].ToString() == "")
                            {
                                purchaser = "On hold";
                            }
                            else
                            {
                                purchaser = "Taken";
                            }
                        }
                        if (c % columns == 1)
                        {
                            if (c > 1)
                            {
                                if (c > columns + 1)
                                {
                                    LitRows2.Text += "<tr><td colspan=\"" + (columns + 1) + "\"><hr /></td></tr>";
                                }
                                LitRows2.Text += line1 + "</tr>" + line2 + "</tr>";
                                line1 = "";
                                line2 = "";
                            }
                            line1 += "<tr><td>Ticket</td>";
                            line2 += "<tr><td>Status</td>";
                        }
                        format = (c % 2).ToString();
                        line1 += "<td class=\"td" + format + "a\">" + dr["ticketnumber"] + "</td>";
                        line2 += "<td id=\"td_" + raffle + "-" + dr["ticketnumber"] + "\" class=\"td" + format + "b\">" + purchaser + "</td>";
                    }
                    LitRows2.Text += "<tr><td colspan=\"" + (columns + 1) + "\"><hr /></td></tr>" + line1 + "</tr>" + line2 + "</tr>";
                    dr.Close();

                    dr.Close();
                }
                con.Close();
                con.Dispose();
            }
        }
    }
}
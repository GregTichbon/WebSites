using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TeOraHouWhanganui.Private.Reports
{
    public partial class Report : System.Web.UI.Page
    {
        public string reportname = "";
        public string html = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            string id = Request.QueryString["id"];
            switch (id)
            {
                case "AccessLevels":
                    reportname = "Access Levels";
                    createreport();
                    break;
                case "WorkerAccessLevelsWorker":
                    reportname = "Worker Access Levels - by Worker";
                    createreport();
                    break;
                case "WorkerAccessLevelsYouth":
                    reportname = "Worker Access Levels - by Youth";
                    createreport();
                    break;
                case "Everyone":
                    reportname = "Everyone";
                    string connectionString = "Data Source=toh-app;Initial Catalog=TeOraHou;Integrated Security=False;user id=OnlineServices;password=Whanganui497";

                    using (SqlConnection con = new SqlConnection(connectionString))
                    using (SqlCommand cmd = new SqlCommand("Reports", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@reportname", SqlDbType.VarChar).Value = reportname;
                        con.Open();

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                html += "<tr>";
                                //for (int f1 = 0; f1 < dr.FieldCount; f1++)
                                //{
                                //    html += "<th>" + dr.GetName(f1) + "</th>";
                                //}
                                html += "<th>Person</th><th>Photo</th>";
                                html += "</tr>";
                            }
                            while (dr.Read())
                            {
                                html += "<tr>";

                                //for (int f1 = 0; f1 < dr.FieldCount; f1++)
                                //{
                                //    html += "<td>";
                                //    html += dr.GetValue(f1).ToString();
                                //    html += "</td>";
                                //}
                                html += "<td><a href=\"../personmaintenance.aspx?id=" + dr["Entity_CTR"] + "\">" + dr["name"] + "</a></td><td><img style=\"width:400px\" src=\"../images/" + dr["entity_ctr"] + ".jpg\"></td>";
                                html += "</tr>";
                            }
                        }
                    }
                    break;
            }
        }


        public void createreport()
        {
            string connectionString = "Data Source=toh-app;Initial Catalog=TeOraHou;Integrated Security=False;user id=OnlineServices;password=Whanganui497";

            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("Reports", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@reportname", SqlDbType.VarChar).Value = reportname;
                con.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if(dr.HasRows)
                    {
                        html += "<tr>";
                        for (int f1 = 0; f1 < dr.FieldCount; f1++)
                        {
                          html += "<th>" +  dr.GetName(f1) + "</th>";
                        }
                        html += "</tr>";
                    }
                    while (dr.Read())
                    {
                        html += "<tr>";

                        for (int f1 = 0; f1 < dr.FieldCount; f1++)
                        {
                            html += "<td>";
                            html += dr.GetValue(f1).ToString();
                            html += "</td>";
                        }
                        html += "</tr>";
                    }
                }
            }
        }
    }
}
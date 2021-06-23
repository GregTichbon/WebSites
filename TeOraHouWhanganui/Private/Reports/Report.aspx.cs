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
                    string connectionString = ConfigurationManager.ConnectionStrings["TOHWConnectionString"].ConnectionString;

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
                case "EncountersSummary":
                    string FromDate = Request.QueryString["fromdate"] + "";
                    string ToDate = Request.QueryString["todate"] + "";
                    if (FromDate == "" || ToDate == "")
                    {
                        Response.Redirect("selector.aspx?id=EncountersSummary");
                    }
                    else
                    {
                        string [,] fields = new string[,] { { "startdate", FromDate }, { "enddate", ToDate } };
                        reportname = "Encounters Summary Audit";
                        createreport(fields);
                    }
                    break;
            }
        }


        public void createreport(string[,] fields = null)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["TOHWConnectionString"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("Reports", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@reportname", SqlDbType.VarChar).Value = reportname;
                if (fields != null)
                {
                    for (int f1 = 0; f1 < fields.GetLength(0); f1++)
                    {
                        cmd.Parameters.Add("@" + fields[f1, 0], SqlDbType.VarChar).Value = fields[f1, 1];
                    }
                }
                con.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.HasRows)
                    {
                        string[,] parameters = new string[5, dr.FieldCount];
                        html += "<table class=\"table\"><thead><tr>";
                        for (int f1 = 0; f1 <= dr.FieldCount - 1; f1++)
                        {
                            string[] theseparameters = dr.GetName(f1).ToString().Split('|');
                            for (int f2 = 0; f2 <= theseparameters.Length - 1; f2++)
                            {
                                parameters[f2, f1] = theseparameters[f2];
                            }
                            if (parameters[0, f1] != "")
                            {
                                html += "<th>";
                                html += parameters[0, f1].Replace("_", " ");
                                html += "</th>";
                            }
                        }
                        html += "</tr></thead><tbody>";

                        while (dr.Read())
                        {
                            html += "<tr>";
                            for (int f1 = 0; f1 <= dr.FieldCount - 1; f1++)
                            {
                                string useclass = "";
                                if (parameters[0, f1] != "")
                                {
                                    if (parameters[2, f1] + "" != "")
                                    {
                                        useclass = " class=\"" + parameters[2, f1] + "\"";
                                    }
                                    html += "<td" + useclass + ">";

                                    string val = dr[f1].ToString();
                                    if (parameters[1, f1] == "Date")
                                    {
                                        //val = Functions.formatdate(val, "dd/MM/yy");
                                        val = Generic.Functions.formatdate(val, "dd/MM/yy");
                                    }
                                    else if (parameters[1, f1] == "Email")
                                    {
                                        val = "<a href=\"mailto:" + val + "\">" + val + "</a>";
                                    }


                                    if (parameters[3, f1] + "" != "")
                                    {
                                        int fld = Convert.ToInt32(parameters[4, f1]);
                                        html += "<a href=\"" + parameters[3, f1] + dr[fld].ToString() + "\">" + val + "</a>";
                                    }
                                    else
                                    {
                                        html += val;
                                    }



                                    html += "</td>";
                                }
                            }
                            html += "</tr>";

                        }
                        /*
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
                    */
                    }
                }
            }
        }

        
    }
}
using Generic;
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
        public string heading = "";


        protected void Page_Load(object sender, EventArgs e)
        {
            string id = Request.QueryString["id"];
            //id = "Current Workers";
            switch (id)
            {
                case "AccessLevels":
                    reportname = "Access Levels";
                    createreportNEW(reportname);
                    break;
                case "WorkerAccessLevelsWorker":
                    reportname = "Worker Access Levels - by Worker";
                    createreportNEW(reportname);
                    break;
                case "WorkerAccessLevelsYouth":
                    reportname = "Worker Access Levels - by Youth";
                    createreportNEW(reportname);
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
                        string[,] fields = new string[,] { { "startdate", FromDate }, { "enddate", ToDate } };
                        reportname = "Encounters Summary Audit";
                        createreportNEW(reportname,fields);
                    }
                    break;
                case "Current Workers":
                    createreportNEW("Current Workers");
                    break;
            }
        }

        public void createreportNEW(string id, string[,] fields = null)
        {
            string groupingparameter = "";
            string[][] groupingparameters = new string[1][];
            string[] lastvals = new string[] { };


            string connectionString = ConfigurationManager.ConnectionStrings["TOHWConnectionString"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connectionString + ";MultipleActiveResultSets=true"))
            using (SqlCommand cmd = new SqlCommand("reports", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@reportname", SqlDbType.VarChar).Value = id;
                if (fields != null)
                {
                    for (int f1 = 0; f1 < fields.GetLength(0); f1++)
                    {
                        cmd.Parameters.Add("@" + fields[f1, 0], SqlDbType.VarChar).Value = fields[f1, 1];
                    }
                }

                SqlDataAdapter da = new SqlDataAdapter();
                DataSet ds = new DataSet();

                da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                cmd.Dispose();

                Boolean ingroup = false;
                foreach (DataTable thistable in ds.Tables)
                {
                    if (thistable.Rows.Count > 0)
                    {
                        //int x = thistable.Columns.Count;
                        //string z = thistable.Columns[0].ColumnName;

                        if (thistable.Columns[0].ColumnName == "Heading")
                        {
                            heading = thistable.Rows[0]["Heading"].ToString();

                            if (thistable.Columns.Count > 1)
                            {
                                groupingparameter = thistable.Rows[0][1].ToString();  //"1,2,3,4"; //group by these columns  "1,2,3,4|9,8,7"
                                var tempgroupingparameters = groupingparameter.Split('|').Select(x => x.Split(',')).ToArray();
                                Array.Resize(ref groupingparameters, tempgroupingparameters.Length);
                                for (int i = 0; i != tempgroupingparameters.Length; i++)
                                {
                                    for (int j = 0; j != tempgroupingparameters[i].Length; j++)
                                    {
                                        Array.Resize(ref groupingparameters[i], tempgroupingparameters[0].Length);
                                        groupingparameters[i][j] = tempgroupingparameters[i][j];
                                    }
                                }
                                lastvals = new string[groupingparameters.Length];
                            }
                        }
                        else
                        {
                            string[,] parameters = new string[5, thistable.Columns.Count];
                            html += "<table class=\"table\"><thead><tr>";
                            for (int f1 = 0; f1 <= thistable.Columns.Count - 1; f1++)
                            {
                                string[] theseparameters = thistable.Columns[f1].ColumnName.Split('|');
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
                            foreach (DataRow dr in thistable.Rows)
                            {
                                int c1 = 0;
                                html += "<tr>";
                                string val = "";
                                for (int f1 = 0; f1 <= thistable.Columns.Count - 1; f1++)
                                {
                                    string useclass = "";
                                    if (parameters[0, f1] != "")
                                    {
                                        c1++;
                                        val = dr[f1].ToString();

                                        if (groupingparameter != "" && c1 == Convert.ToInt32(groupingparameters[0][0]))  //only doing 1 group by at the moment, put this in a loop in the future
                                        {
                                            if (val == lastvals[c1-1])
                                            {
                                                //val = "";
                                                ingroup = true;
                                            }
                                            else
                                            {
                                                lastvals[c1-1] = val;
                                                ingroup = false;
                                            }
                                        }
                                        if(ingroup && groupingparameters[0].Contains((c1).ToString()))
                                        {
                                            val = "";
                                        }
                                        if (parameters[2, f1] + "" != "")
                                        {
                                            useclass = " class=\"" + parameters[2, f1] + "\"";
                                        }
                                        html += "<td" + useclass + ">";
                                        if (val != "")
                                        {
                                            switch (parameters[1, f1] + "")
                                            {
                                                case "Date": //Change this to a format string
                                                    {
                                                        val = Functions.formatdate(val, "dd/MM/yy");
                                                        break;
                                                    }
                                                case "Email":
                                                    {
                                                        val = "<a href=\"mailto:" + val + "\">" + val + "</a>";
                                                        break;
                                                    }
                                                case "":
                                                    {
                                                        break;
                                                    }
                                                default:
                                                    {
                                                        val = Functions.formatdate(val, parameters[1, f1]);
                                                        break;
                                                    }
                                            }

                                            if (parameters[3, f1] + "" != "")
                                            {
                                                int fld = Convert.ToInt32(parameters[4, f1]);
                                                val = "<a href=\"" + parameters[3, f1] + dr[fld].ToString() + "\">" + val + "</a>";
                                            }
                                        }
                                        html += val;
                                        html += "</td>";
                                    }
                                }
                                html += "</tr>";
                            }
                            html += "</tbody></table>";
                        }
                    }
                }
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
                                        val = "<a href=\"" + parameters[3, f1] + dr[fld].ToString() + "\">" + val + "</a>";
                                    }
                                    html += val;
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
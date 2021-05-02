using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using localfunctions = TeOraHouWhanganui._Dependencies;

namespace TeOraHouWhanganui.Private.Administration
{
    public partial class TestEncounterAccess : System.Web.UI.Page
    {
        public string username = "";
        public string[] users = { "toh\\wthompson" };
        public string html_encounter = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            username = HttpContext.Current.User.Identity.Name.ToLower();

            if (username == "")
            {
                username = "toh\\gtichbon";   //localhost
            }

            if (localfunctions.functions.AccessStringTest(username, "11"))
            {
                if (fld_username.SelectedValue != "")
                {

                    html_encounter = "<thead>";
                    html_encounter += "<tr><th>Start Date/Time</th><th>End Date/Time</th><th style=\"width:50%\">Narrative</th><th>Worker(s)</th><th>Level</th></tr>";
                    html_encounter += "</thead>";
                    html_encounter += "<tbody>";


                    string systemPrefix = WebConfigurationManager.AppSettings["systemPrefix"];
                    String connectionString = ConfigurationManager.ConnectionStrings[systemPrefix + "ConnectionString"].ConnectionString;

                    SqlConnection con = new SqlConnection(connectionString);

                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.CommandText = "Get_Encounters";
                    cmd.Parameters.Clear();
                    cmd.Parameters.Add("@username", SqlDbType.VarChar).Value = fld_username.SelectedValue;
                    cmd.Parameters.Add("@person_ctr", SqlDbType.VarChar).Value = Request.Form["fld_person_ctr"];
                    //cmd.Parameters.Add("@testing", SqlDbType.VarChar).Value = "Yes";
                    cmd.Connection = con;
                    con.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {

                        string Encounter_CTR = dr["Encounter_CTR"].ToString();
                        string StartDateTime = Convert.ToDateTime(dr["StartDateTime"]).ToString("dd MMM yyyy HH:mm");
                        string EndDateTime = Convert.ToDateTime(dr["EndDateTime"]).ToString("dd MMM yyyy HH:mm");
                        string Narrative = dr["Narrative"].ToString();
                        string Worker = dr["Worker"].ToString();
                        string Encounteraccesslevel = dr["Encounteraccesslevel"].ToString();
                        string EncounteraccesslevelDisplay = dr["EncounteraccesslevelDisplay"].ToString();
                        string WorkerCTR = "";

                        string WorkerDisplay = "";
                        if (Worker != "")
                        {
                            string[] WorkerSplit = Worker.Split(Convert.ToChar(254));
                            string delim1 = "";
                            string delim2 = "";
                            foreach (String thisWorker in WorkerSplit)
                            {
                                string[] thisWorkerSplit = thisWorker.Split(Convert.ToChar(253));
                                WorkerCTR += delim1 + thisWorkerSplit[0];
                                delim1 = "|";
                                WorkerDisplay += delim2 + thisWorkerSplit[1];
                                delim2 = "<br />";
                            }
                        }


                        html_encounter += "<tr>";
                        html_encounter += "<td>" + StartDateTime + "</td>";
                        html_encounter += "<td>" + EndDateTime + "</td>";
                        html_encounter += "<td>" + Narrative + "</td>";
                        html_encounter += "<td workerid=\"" + WorkerCTR + "\">" + WorkerDisplay + "</td>";
                        html_encounter += "<td encounteraccesslevel=\"" + Encounteraccesslevel + "\">" + EncounteraccesslevelDisplay + "</td>";
                        html_encounter += "</tr>";

                    }
                    dr.Close();
                    con.Close();
                    con.Dispose();
                }
            }

        }

        protected void btn_test_Click(object sender, EventArgs e)
        {

        }
    }
}
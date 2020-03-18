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

namespace TeOraHouWhanganui.Private
{
    public partial class PersonMaintenance : System.Web.UI.Page
    {
        public string person_ctr;
        public string firstname;
        public string surname;
        public string knownas;
        public string dietary;
        public string medical;
        public string notes;
        public string birthdate;

        public string html_tab = "";
        public string html_encounter = "";

        public string[] yesno = new string[2] { "Yes", "No" };

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                person_ctr = Request.QueryString["id"] ?? "";
                if (person_ctr == "")
                {
                    Response.Redirect("personsearch.aspx");
                }

                string systemPrefix = WebConfigurationManager.AppSettings["systemPrefix"];
                String connectionString = ConfigurationManager.ConnectionStrings[systemPrefix + "ConnectionString"].ConnectionString;

                if (person_ctr != "new")
                {
                    SqlConnection con = new SqlConnection(connectionString);

                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "get_person";
                    cmd.Parameters.Add("@person_ctr", SqlDbType.VarChar).Value = person_ctr;

                    cmd.Connection = con;
                    con.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        dr.Read();
                        //person_ctr = dr["person_id"].ToString();
                        firstname = dr["firstname"].ToString();
                        surname = dr["surname"].ToString();
                        knownas = dr["knownas"].ToString();
                    }
                    dr.Close();



                    //-------------------------------------------------------------------------------------
                    //ENCOUNTERS
                    //if (Functions.accessstringtest(Session["UBC_AccessString"].ToString(), "1"))
                    //{

                    html_tab += "<li><a data-target=\"#div_encounter\">Encounters</a></li>";

                    html_encounter = "<thead>";

                    html_encounter += "<tr><th style=\"width:50px;text-align:center\"></th><th>Start Date/Time</th><th>End Date/Time</th><th>Narrative</th><th>Worker</th><th style=\"width:100px\">Action / <a class=\"encounteredit\" data-mode=\"add\" href=\"javascript: void(0)\">Add</a></th></tr>";
                    html_encounter += "</thead>";
                    html_encounter += "<tbody>";

                    //hidden row, used for creating new rows client side
                    html_encounter += "<tr style=\"display:none\">";
                    html_encounter += "<td style=\"text-align:center\"></td>";
                    html_encounter += "<td></td>";
                    html_encounter += "<td></td>";
                    html_encounter += "<td></td>";
                    html_encounter += "<td></td>";
                    html_encounter += "<td><a href=\"javascript:void(0)\" class=\"encounteredit\" data-mode=\"edit\">Edit</td>";
                    html_encounter += "</tr>";

                    cmd.CommandText = "Get_Encounters";
                    cmd.Parameters.Clear();
                    cmd.Parameters.Add("@person_ctr", SqlDbType.VarChar).Value = person_ctr;
                    dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {

                        string Encounter_CTR = dr["Encounter_CTR"].ToString();
                        string StartDateTime = Convert.ToDateTime(dr["StartDateTime"]).ToString("dd MMM yyyy HH:mm");
                        string EndDateTime = Convert.ToDateTime(dr["EndDateTime"]).ToString("dd MMM yyyy HH:mm");
                        string Narrative = dr["Narrative"].ToString();
                        string Worker = dr["Worker"].ToString();
                        string WorkerDisplay = "";
                        string WorkerCTR = "";
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

                        html_encounter += "<tr id=\"encounter_" + Encounter_CTR + "\">";
                        html_encounter += "<td style=\"text-align:center\"></td>";
                        html_encounter += "<td>" + StartDateTime + "</td>";
                        html_encounter += "<td>" + EndDateTime + "</td>";
                        html_encounter += "<td>" + Narrative + "</td>";
                        html_encounter += "<td data-workerid=\"" + WorkerCTR + "\">" + WorkerDisplay + "</td>";
                        html_encounter += "<td><a href=\"javascript:void(0)\" class=\"encounteredit\" data-mode=\"edit\">Edit</td>";
                        html_encounter += "</tr>";

                    }
                    dr.Close();
                }
            }
        }


        protected void btn_submit_Click(object sender, EventArgs e)
        {

        }
    }
}
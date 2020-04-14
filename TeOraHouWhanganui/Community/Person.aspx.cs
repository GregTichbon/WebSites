using Generic;
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

namespace TeOraHouWhanganui.Community
{
    public partial class Person : System.Web.UI.Page
    {
        public string person_guid = "";
        public string fld_firstname = "";
        public string fld_lastname = "";
        public string fld_knownas = "";
        public string fld_phone = "";
        public string fld_note = "";
        public string fld_location = "";
        public string fld_PAF = "";
        public string[] fld_gender = new string[1];
        public string fld_age = "";
        public string[] fld_contact_ctr = new string[1];


        public string html_tab = "";
        public string html_updates = "";
        public string html_encounters = "";

        public Dictionary<string, string> contacts;
        public Dictionary<string, string> genders = new Dictionary<string, string>();
        public Dictionary<string, string> options = new Dictionary<string, string>();
        public string Username = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            //Username = System.Security.Principal.WindowsIdentity.GetCurrent().Name.ToString().ToLower();
            Username = HttpContext.Current.User.Identity.Name.ToLower();
            //Username += "<br />" + Environment.UserName;



            if (!IsPostBack)
            {
                person_guid = Request.QueryString["id"] + "";
                if (person_guid == "")
                {
                    Response.Redirect("search.aspx");
                }

                ViewState["person_guid"] = person_guid;
                ViewState["returnto"] = Request.QueryString["returnto"] + "";

                string systemPrefix = "Community"; // WebConfigurationManager.AppSettings["systemPrefix"];
                //string systemPrefix = WebConfigurationManager.AppSettings["systemPrefix"];
                String connectionString = ConfigurationManager.ConnectionStrings[systemPrefix + "ConnectionString"].ConnectionString;

                options.Clear();
                options.Add("usevalues", "");
                options.Add("insertblank", "start");
                contacts = Functions.buildselectionlist(connectionString, "select [contact_ctr] as [Value], [Name] as [Label] from Contact order by [Name]", options);

                genders.Add("Female", "Female");
                genders.Add("Male", "Male");

                if (person_guid != "new")
                {
                    string person_ctr = "";
                    SqlConnection con = new SqlConnection(connectionString);
                    con.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "get_person";
                    cmd.Parameters.Add("@username", SqlDbType.VarChar).Value = Username;
                    cmd.Parameters.Add("@person_guid", SqlDbType.VarChar).Value = person_guid;

                    cmd.Connection = con;
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        dr.Read();
                        person_ctr = dr["person_ctr"].ToString();
                        fld_firstname = dr["firstname"].ToString();
                        fld_lastname = dr["lastname"].ToString();
                        fld_knownas = dr["knownas"].ToString();
                        fld_phone = dr["phone"].ToString();
                        fld_note = dr["note"].ToString();
                        fld_location = dr["location"].ToString();
                        fld_PAF = dr["PAF"].ToString();
                        fld_gender[0] = dr["gender"].ToString();
                        fld_age = dr["age"].ToString();
                        fld_contact_ctr[0] = dr["contact_ctr"].ToString();
                    }
                    dr.Close();

                    #region Updates
                    //-------------------------------UPDATES TAB------------------------------------------------------
                    //if (Functions.accessstringtest(Session["UBC_AccessString"].ToString(), "1"))
                    //{

                    html_tab += "<li><a data-target=\"#div_update\">Updates</a></li>";

                    html_updates = "<thead>";
                    html_updates += "<tr><th style=\"width:50px;text-align:center\"></th><th>Date/Time</th><th>Done by</th><th>Note</th><th>Followup Action</th><th>Followup Date</th><th>Followup Done</th>";
                    /*
                    if(Username == "toh\\gtichbon")
                    {
                        html_updates += "<th>Private</th>";
                    } else
                    {
                        html_updates += "<th></th>";
                    }
                    */
                    html_updates += "<th style=\"width:100px\">Action / <a class=\"updateedit\" data-mode=\"add\" href=\"javascript: void(0)\">Add</a></th></tr>";

                    html_updates += "</thead>";
                    html_updates += "<tbody>";

                    //hidden row, used for creating new rows client side
                    html_updates += "<tr style=\"display:none\">";
                    html_updates += "<td style=\"text-align:center\"></td>";
                    html_updates += "<td></td>";
                    html_updates += "<td></td>";
                    html_updates += "<td></td>";
                    html_updates += "<td></td>";
                    html_updates += "<td></td>";
                    html_updates += "<td></td>";
                    //html_updates += "<td></td>";  //Private
                    html_updates += "<td><a href=\"javascript:void(0)\" class=\"updateedit\" data-mode=\"edit\">Edit</td>";
                    html_updates += "</tr>";

                    cmd.CommandText = "Get_person_updates";
                    cmd.Parameters.Clear();
                    cmd.Parameters.Add("@username", SqlDbType.VarChar).Value = Username;
                    cmd.Parameters.Add("@person_ctr", SqlDbType.VarChar).Value = person_ctr;
                    dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {

                        string person_update_CTR = dr["person_update_CTR"].ToString();
                        string myDateTime = Functions.formatdate(dr["DateTime"].ToString(), "dd MMM yyyy HH:mm");
                        string Contact_CTR = dr["Contact_CTR"].ToString();
                        string ContactName = dr["ContactName"].ToString();
                        string Note = dr["Note"].ToString();
                        string FollowupAction = dr["FollowupAction"].ToString();
                        string FollowupDate = Functions.formatdate(dr["FollowupDate"].ToString(), "dd MMM yyyy");
                        string UpdatedBy = dr["UpdatedBy"].ToString();
                        string FollowUpDone = Functions.formatdate(dr["FollowUpDone"].ToString(), "dd MMM yyyy HH:mm");
                        /*
                        string Private = "";
                        if (Username == "toh\\gtichbon")
                        {
                            Private = dr["private"].ToString();
                        }
                        */
                        html_updates += "<tr id=\"update_" + person_update_CTR + "\">";
                        html_updates += "<td style=\"text-align:center\"></td>";
                        html_updates += "<td>" + myDateTime + "</td>";
                        html_updates += "<td contact_ctr=\"" + Contact_CTR + "\">" + ContactName + "</td>";
                        html_updates += "<td>" + Note + "</td>";
                        html_updates += "<td>" + FollowupAction + "</td>";
                        html_updates += "<td>" + FollowupDate + "</td>";
                        html_updates += "<td>" + FollowUpDone + "</td>";
                        //html_updates += "<td>" + Private + "</td>";
                        string mode = "View";
                        if (Username == UpdatedBy)
                        {
                            mode = "Edit";
                        }
                        html_updates += "<td><a href=\"javascript:void(0)\" class=\"updateedit\" data-mode=\"" + mode + "\">" + mode + "</td>";
                        html_updates += "</tr>";

                    }
                    dr.Close();
                    #endregion //Updates

                    #region Encounters
                    /*
                    //-------------------------------TOH ENCOUNTERS TAB------------------------------------------------------
                    //if (Functions.accessstringtest(Session["UBC_AccessString"].ToString(), "1"))
                    //{

                    html_tab += "<li><a data-target=\"#div_encounter\">Encounters</a></li>";

                    html_encounters = "<thead>";
                    html_encounters += "<tr><th style=\"width:50px;text-align:center\"></th><th>Date/Time</th><th>Done by</th><th>Note</th><th>Followup Action</th><th>Followup Date</th>";
                                      html_encounters += "<th style=\"width:100px\">Action / <a class=\"encounteredit\" data-mode=\"add\" href=\"javascript: void(0)\">Add</a></th></tr>";

                    html_encounters += "</thead>";
                    html_encounters += "<tbody>";

                    //hidden row, used for creating new rows client side
                    html_encounters += "<tr style=\"display:none\">";
                    html_encounters += "<td style=\"text-align:center\"></td>";
                    html_encounters += "<td></td>";
                    html_encounters += "<td></td>";
                    html_encounters += "<td></td>";
                    html_encounters += "<td></td>";
                    html_encounters += "<td></td>";
                    //html_encounters += "<td></td>";  //Private
                    html_encounters += "<td><a href=\"javascript:void(0)\" class=\"encounteredit\" data-mode=\"edit\">Edit</td>";
                    html_encounters += "</tr>";

                    cmd.CommandText = "Get_person_TOH_encounters";
                    cmd.Parameters.Clear();
                    cmd.Parameters.Add("@username", SqlDbType.VarChar).Value = Username;
                    cmd.Parameters.Add("@person_ctr", SqlDbType.VarChar).Value = person_ctr;
                    dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {

                      string person_encounter_CTR = dr["person_encounter_CTR"].ToString();
                      string myDateTime = Functions.formatdate(dr["DateTime"].ToString(), "dd MMM yyyy HH:mm");
                      string Contact_CTR = dr["Contact_CTR"].ToString();
                      string ContactName = dr["ContactName"].ToString();
                      string Note = dr["Note"].ToString();
                      string FollowupAction = dr["FollowupAction"].ToString();
                      string Followencounter = Functions.formatdate(dr["Followencounter"].ToString(), "dd MMM yyyy");
                      string encounterdBy = dr["encounterdBy"].ToString();

                      html_encounters += "<tr id=\"encounter_" + person_encounter_CTR + "\">";
                      html_encounters += "<td style=\"text-align:center\"></td>";
                      html_encounters += "<td>" + myDateTime + "</td>";
                      html_encounters += "<td contact_ctr=\"" + Contact_CTR + "\">" + ContactName + "</td>";
                      html_encounters += "<td>" + Note + "</td>";
                      html_encounters += "<td>" + FollowupAction + "</td>";
                      html_encounters += "<td>" + Followencounter + "</td>";
                      //html_encounters += "<td>" + Private + "</td>";
                      string mode = "View";
                      if (Username == encounterdBy)
                      {
                          mode = "Edit";
                      }
                      html_encounters += "<td><a href=\"javascript:void(0)\" class=\"encounteredit\" data-mode=\"" + mode + "\">" + mode + "</td>";
                      html_encounters += "</tr>";

                    }
                    dr.Close();
                    */
                    #endregion //Encounters

                    //} //security

                }
            }
        }

        protected void btn_submit_Click(object sender, EventArgs e)
        {
            Boolean Creating = false;
            string systemPrefix = "Community"; // WebConfigurationManager.AppSettings["systemPrefix"];
                                               //string systemPrefix = WebConfigurationManager.AppSettings["systemPrefix"];
            String connectionString = ConfigurationManager.ConnectionStrings[systemPrefix + "ConnectionString"].ConnectionString;
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            person_guid = ViewState["person_guid"].ToString();
            if (person_guid == "new")
            {
                Creating = true;
            }

            string lng = "0";
            string lat = "0";
            if (Request.Form["fld_location"].Trim() != "")
            {
                options.Clear();
                google_geocodeClass google_geocode = Functions.google_geocode("AIzaSyCCpsWhkuuHlAe6EKhSi5zSlmmIVMN9M8c", Request.Form["fld_location"].Trim(), options);
                lng = google_geocode.lng;
                lat = google_geocode.lat;
            }

            cmd.CommandText = "Update_Person";
            cmd.Parameters.Add("@person_guid", SqlDbType.VarChar).Value = person_guid;

            cmd.Parameters.Add("@username", SqlDbType.VarChar).Value = Username;
            cmd.Parameters.Add("@firstname", SqlDbType.VarChar).Value = Request.Form["fld_firstname"].Trim();
            cmd.Parameters.Add("@lastname", SqlDbType.VarChar).Value = Request.Form["fld_lastname"].Trim();
            cmd.Parameters.Add("@knownas", SqlDbType.VarChar).Value = Request.Form["fld_knownas"].Trim();
            cmd.Parameters.Add("@gender", SqlDbType.VarChar).Value = Request.Form["fld_gender"].Trim();
            cmd.Parameters.Add("@note", SqlDbType.VarChar).Value = Request.Form["fld_note"].Trim();
            cmd.Parameters.Add("@location", SqlDbType.VarChar).Value = Request.Form["fld_location"].Trim();
            cmd.Parameters.Add("@PAF", SqlDbType.VarChar).Value = Request.Form["fld_PAF"].Trim();
            cmd.Parameters.Add("@phone", SqlDbType.VarChar).Value = Request.Form["fld_phone"].Trim();
            cmd.Parameters.Add("@age", SqlDbType.VarChar).Value = Request.Form["fld_age"].Trim();
            cmd.Parameters.Add("@lng", SqlDbType.VarChar).Value = lng;
            cmd.Parameters.Add("@lat", SqlDbType.VarChar).Value = lat;


            cmd.Parameters.Add("@contact_ctr", SqlDbType.VarChar).Value = Request.Form["fld_contact"].Trim();

            cmd.Connection = con;

            con.Open();
            person_guid = cmd.ExecuteScalar().ToString();
            con.Close();

            foreach (string key in Request.Form)
            {
                if (key.StartsWith("update_"))
                {
                    string person_update_id = key.Substring(7);
                    if (person_update_id.EndsWith("_delete"))
                    {
                        cmd.CommandText = "Delete_Person_update";
                        cmd.Parameters.Clear();
                        cmd.Parameters.Add("@username", SqlDbType.VarChar).Value = Username;
                        cmd.Parameters.Add("@person_update_ctr", SqlDbType.VarChar).Value = person_update_id.Substring(0, person_update_id.Length - 7);
                    }
                    else
                    {
                        if (person_update_id.StartsWith("new"))
                        {
                            person_update_id = "new";
                        }

                        string[] valuesSplit = Request.Form[key].Split('\x00FE');
                        cmd.CommandText = "Update_Person_update";
                        cmd.Parameters.Clear();
                        cmd.Parameters.Add("@username", SqlDbType.VarChar).Value = Username;
                        cmd.Parameters.Add("@person_update_ctr", SqlDbType.VarChar).Value = person_update_id;
                        cmd.Parameters.Add("@person_guid", SqlDbType.VarChar).Value = person_guid;
                        //cmd.Parameters.Add("@person_ctr", SqlDbType.VarChar).Value = ;
                        cmd.Parameters.Add("@datetime", SqlDbType.VarChar).Value = valuesSplit[0];
                        cmd.Parameters.Add("@Contact_CTR", SqlDbType.VarChar).Value = valuesSplit[1];
                        cmd.Parameters.Add("@note", SqlDbType.VarChar).Value = valuesSplit[2];
                        cmd.Parameters.Add("@followupaction", SqlDbType.VarChar).Value = valuesSplit[3];
                        cmd.Parameters.Add("@followupdate", SqlDbType.VarChar).Value = valuesSplit[4];
                        cmd.Parameters.Add("@followupdone", SqlDbType.VarChar).Value = valuesSplit[5];
                    }
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            con.Dispose();
            //}

            string returnto = ViewState["returnto"].ToString();
            ;
            if (Creating)
            {
                if (returnto == "")
                {
                    returnto = "person.aspx?id=" + person_guid;
                }
                else
                {
                    returnto = "person.aspx?id=" + person_guid + "&returnto=" + returnto + ".aspx";
                }
            }
            else
            {
                if (returnto == "")
                {
                    returnto = "search.aspx";
                }
            }

            Response.Redirect(returnto);
        }
    }
}

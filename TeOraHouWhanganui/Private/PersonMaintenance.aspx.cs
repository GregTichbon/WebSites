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
using Generic;
using localfunctions = TeOraHouWhanganui._Dependencies;

namespace TeOraHouWhanganui.Private
{
    public partial class PersonMaintenance : System.Web.UI.Page
    {
        public string username = "";
        public string person_ctr;
        public string fld_firstname;
        public string fld_surname;
        public string fld_knownas;
        public string fld_dietary;
        public string fld_medical;
        public string fld_notes;
        public string fld_birthdate;
        public string fld_photoalbumlink;
        public string[] fld_gender = new string[1];


        public string html_tab = "";
        public string html_encounter = "";
        //public string html_encounter_workers = "";
        public string html_worker = "";
        public string html_workerroles = "";
        public string html_assigned = "";
        public string show_assigned_level = "";
        public string html_addresses = "";
        public string photoalbumlink = "";

        public Dictionary<string, string> options = new Dictionary<string, string>();
        public string[] nooptions = { };

        public Dictionary<string, string> genders = new Dictionary<string, string>();
        public Dictionary<string, string> workers = new Dictionary<string, string>();
        public Dictionary<string, string> workerRoles = new Dictionary<string, string>();
        public Dictionary<string, string> assignmenttypes = new Dictionary<string, string>();
        public Dictionary<string, string> encounterAccessLevels = new Dictionary<string, string>();
        public Dictionary<string, string> YesNo = new Dictionary<string, string>();

        //public Dictionary<string, string> persons = new Dictionary<string, string>();


        protected void Page_Load(object sender, EventArgs e)
        {
            // username = System.Security.Principal.WindowsIdentity.GetCurrent().Name.ToString().ToLower();
            username = HttpContext.Current.User.Identity.Name.ToLower();
            
            if (username == "")
            {
                username = "toh\\gtichbon";   //localhost
            }
            //username = HttpContext.Current.User.Identity.Name.ToLower();
            //Username += "<br />" + Environment.UserName;
            
            if (!IsPostBack)
            {
                person_ctr = Request.QueryString["id"] ?? "";
                if (person_ctr == "")
                {
                    Response.Redirect("personsearch.aspx");
                }

                ViewState["person_ctr"] = person_ctr;
                ViewState["returnto"] = Request.QueryString["returnto"] + "";

                genders.Add("Female", "Female");
                genders.Add("Male", "Male");

                string systemPrefix = WebConfigurationManager.AppSettings["systemPrefix"];
                String connectionString = ConfigurationManager.ConnectionStrings[systemPrefix + "ConnectionString"].ConnectionString;


                if (person_ctr != "new")
                {
                    options.Clear();
                    options.Add("storedprocedure", "");
                    options.Add("storedprocedurename", "");
                    options.Add("usevalues", "");
                    //options.Add("insertblank", "start");
                    workers = Functions.buildselectionlist(connectionString, "get_workers", options);

                    options.Clear();
                    options.Add("storedprocedure", "");
                    options.Add("storedprocedurename", "");
                    options.Add("usevalues", "");
                    //options.Add("insertblank", "start");
                    workerRoles = Functions.buildselectionlist(connectionString, "get_workerRoles", options);

                    options.Clear();
                    options.Add("storedprocedure", "");
                    options.Add("storedprocedurename", "");
                    options.Add("usevalues", "");
                    //options.Add("insertblank", "start");
                    encounterAccessLevels = Functions.buildselectionlist(connectionString, "get_EncounterAccessLevels", options);

                    //options.Clear();
                    //options.Add("type", "uiselectable");
                    //options.Add("valuefield", "value");
                    //options.Add("name", "EncounterWorker");
                    //string[] selectedoptions = { };
                    //html_encounter_workers = Functions.buildselection(workers, selectedoptions, options);


                    //options.Clear();
                    //options.Add("storedprocedure", "");
                    //options.Add("storedprocedurename", "");
                    //options.Add("usevalues", "");
                    ////options.Add("insertblank", "start");
                    //persons = Functions.buildselectionlist(connectionString, "get_all_entities", options);

                    assignmenttypes.Add("Youth", "");
                    assignmenttypes.Add("Worker", "");

                    YesNo.Add("Yes", "1");
                    YesNo.Add("No", "0");


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
                        //person_ctr = dr["person_ctr"].ToString();
                        fld_firstname = dr["firstname"].ToString();
                        fld_surname = dr["surname"].ToString();
                        fld_knownas = dr["knownas"].ToString();
                        fld_gender[0] = dr["gender"].ToString();
                        fld_birthdate = Functions.formatdate(dr["DateofBirth"].ToString(), "dd MMM yyyy");
                        fld_dietary = dr["dietary"].ToString();
                        fld_medical = dr["medical"].ToString();
                        fld_notes = dr["notes"].ToString();
                        fld_photoalbumlink = dr["photoalbumlink"].ToString();
                        if(fld_photoalbumlink != "") {
                            photoalbumlink = "<a href=\"" + fld_photoalbumlink + "\" target=\"photos\">Google</a>";
                        }
                    }
                    dr.Close();

                    #region ENCOUNTERS

                    //-------------------------------ENCOUNTERS TAB------------------------------------------------------
                    if (localfunctions.functions.AccessStringTest(username, "111"))
                    {

                        html_tab += "<li><a data-target=\"#div_encounter\">Encounters</a></li>";

                        html_encounter = "<thead>";
                        html_encounter += "<tr><th style=\"width:50px;text-align:center\"></th><th>Start Date/Time</th><th>End Date/Time</th><th style=\"width:50%\">Narrative</th><th>Worker(s)</th><th>Level</th><th style=\"width:100px\">Action / <a class=\"encounteredit\" data-mode=\"add\" href=\"javascript: void(0)\">Add</a></th></tr>";
                        html_encounter += "</thead>";
                        html_encounter += "<tbody>";

                        //hidden row, used for creating new rows client side
                        html_encounter += "<tr style=\"display:none\">";
                        html_encounter += "<td style=\"text-align:center\"></td>";
                        html_encounter += "<td></td>";
                        html_encounter += "<td></td>";
                        html_encounter += "<td></td>";
                        html_encounter += "<td></td>";
                        html_encounter += "<td></td>";
                        html_encounter += "<td><a href=\"javascript:void(0)\" class=\"encounteredit\" data-mode=\"edit\">Edit</td>";
                        html_encounter += "</tr>";

                        cmd.CommandText = "Get_Encounters";
                        cmd.Parameters.Clear();
                        cmd.Parameters.Add("@username", SqlDbType.VarChar).Value = username;
                        cmd.Parameters.Add("@person_ctr", SqlDbType.VarChar).Value = person_ctr;
                        dr = cmd.ExecuteReader();
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
                       

                            html_encounter += "<tr id=\"encounter_" + Encounter_CTR + "\">";
                            html_encounter += "<td style=\"text-align:center\"></td>";
                            html_encounter += "<td>" + StartDateTime + "</td>";
                            html_encounter += "<td>" + EndDateTime + "</td>";
                            html_encounter += "<td>" + Narrative + "</td>";
                            html_encounter += "<td workerid=\"" + WorkerCTR + "\">" + WorkerDisplay + "</td>";
                            html_encounter += "<td encounteraccesslevel=\"" + Encounteraccesslevel + "\">" + EncounteraccesslevelDisplay + "</td>";
                            if(Narrative == "Restricted")
                            {
                                html_encounter += "<td></td>";
                            } else
                            {
                                html_encounter += "<td><a href=\"javascript:void(0)\" class=\"encounteredit\" data-mode=\"edit\">Edit</td>";
                            }
                            html_encounter += "</tr>";

                        }
                        dr.Close();
                    }

                    #endregion ENCOUNTERS

                    #region PHONE
                    //-------------------------------PHONE TAB------------------------------------------------------
                    //if (Functions.accessstringtest(Session["UBC_AccessString"].ToString(), "1"))
                    //{

                    html_tab += "<li><a data-target=\"#div_phone\">X Phones</a></li>";

                    //}

                    #endregion PHONE


                    #region FINANCIAL
                    //-------------------------------FINANCIAL TAB------------------------------------------------------
                    //if (Functions.accessstringtest(Session["UBC_AccessString"].ToString(), "1"))
                    //{

                    html_tab += "<li><a data-target=\"#div_financial\">X Financial</a></li>";

                    //}

                    #endregion FINANCIAL

                    #region FUTURE EVENTS
                    //-------------------------------FUTURE EVENTS TAB------------------------------------------------------
                    //if (Functions.accessstringtest(Session["UBC_AccessString"].ToString(), "1"))
                    //{

                    html_tab += "<li><a data-target=\"#div_futureevent\">X Future Events</a></li>";

                    //}

                    #endregion FUTURE EVENTS

                    #region RELATIONSHIPS
                    //-------------------------------RELATIONSHIPS TAB------------------------------------------------------
                    //if (Functions.accessstringtest(Session["UBC_AccessString"].ToString(), "1"))
                    //{

                    html_tab += "<li><a data-target=\"#div_relationship\">X Relationships</a></li>";

                    //}

                    #endregion RELATIONSHIPS

                    #region EDUCATION
                    //-------------------------------EDUCATION TAB------------------------------------------------------
                    //if (Functions.accessstringtest(Session["UBC_AccessString"].ToString(), "1"))
                    //{

                    html_tab += "<li><a data-target=\"#div_education\">X Education</a></li>";

                    //}

                    #endregion EDUCATION

                    #region INTERNET
                    //-------------------------------INTERNET TAB------------------------------------------------------
                    //if (Functions.accessstringtest(Session["UBC_AccessString"].ToString(), "1"))
                    //{

                    html_tab += "<li><a data-target=\"#div_internet\">X Internet Presence</a></li>";

                    //}

                    #endregion INTERNET

                    #region EVENTS
                    //-------------------------------EVENTS TAB------------------------------------------------------
                    //if (Functions.accessstringtest(Session["UBC_AccessString"].ToString(), "1"))
                    //{

                    html_tab += "<li><a data-target=\"#div_event\">X Events</a></li>";

                    //}

                    #endregion EVENTS


                    #region ADDRESSES
                    //-------------------------------ADDRESSES TAB------------------------------------------------------
                    //if (Functions.accessstringtest(Session["UBC_AccessString"].ToString(), "1"))
                    //{

                    html_tab += "<li><a data-target=\"#div_address\">Addresses</a></li>";

                    html_addresses = "<thead>";
                    html_addresses += "<tr><th style=\"width:50px;text-align:center\"></th><th>Address</th><th>Current</th><th>Note</th><th>Coordinates</th><th style=\"width:100px\">Action / <a class=\"addressedit\" data-mode=\"add\" href=\"javascript: void(0)\">Add</a></th></tr>";
                    html_addresses += "</thead>";
                    html_addresses += "<tbody>";

                    //hidden row, used for creating new rows client side
                    html_addresses += "<tr style=\"display:none\">";
                    html_addresses += "<td style=\"text-align:center\"></td>";
                    html_addresses += "<td></td>";
                    html_addresses += "<td></td>";
                    html_addresses += "<td></td>";
                    html_addresses += "<td></td>";
                    html_addresses += "<td><a href=\"javascript:void(0)\" class=\"addressedit\" data-mode=\"edit\">Edit</td>";
                    html_addresses += "</tr>";

                    cmd.CommandText = "get_entity_addresses";
                    cmd.Parameters.Clear();
                    cmd.Parameters.Add("@person_ctr", SqlDbType.VarChar).Value = person_ctr;
                    dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        string entity_address_CTR = dr["entity_address_CTR"].ToString();
                        string address = dr["address"].ToString();
                        string longitude = dr["Longitude"].ToString();
                        string latitude = dr["latitude"].ToString();
                        string current = dr["current"].ToString();

                        string Note = dr["Note"].ToString();

                        string googlelink = "";
                        if (longitude != "" && latitude != "")
                        {
                            googlelink = "<a href=\"https://maps.google.com/?q=" + latitude + "," + longitude + "\" target=\"map\"> " + latitude + "," + longitude + "</a>";
                        }

                        html_addresses += "<tr id=\"address_" + entity_address_CTR + "\">";
                        html_addresses += "<td style=\"text-align:center\"></td>";
                        html_addresses += "<td>" + address + "</td>";
                        html_addresses += "<td current=\"" + current + "\">" + YesNo.FirstOrDefault(x => x.Value == current).Key + "</td>";
                        html_addresses += "<td>" + Note + "</td>";
                        html_addresses += "<td>" + googlelink + "</td>";
                        html_addresses += "<td><a href=\"javascript:void(0)\" class=\"addressedit\" data-mode=\"edit\">Edit</td>";
                        html_addresses += "</tr>";
                    }
                    dr.Close();

                    //}
                    #endregion //ADDRESSES

                    #region WORKER ROLES
                    //-------------------------------WORKER ROLES TAB------------------------------------------------------
                    //if (Functions.accessstringtest(Session["UBC_AccessString"].ToString(), "1"))
                    //{

                    html_tab += "<li><a data-target=\"#div_workerrole\">Worker Roles</a></li>";

                    html_workerroles = "<thead>";
                    html_workerroles += "<tr><th style=\"width:50px;text-align:center\"></th><th>Role</th><th>Start Date</th><th>End Date</th><th>Note</th><th style=\"width:100px\">Action / <a class=\"workerroleedit\" data-mode=\"add\" href=\"javascript: void(0)\">Add</a></th></tr>";
                    html_workerroles += "</thead>";
                    html_workerroles += "<tbody>";

                    //hidden row, used for creating new rows client side
                    html_workerroles += "<tr style=\"display:none\">";
                    html_workerroles += "<td style=\"text-align:center\"></td>";
                    html_workerroles += "<td></td>";
                    html_workerroles += "<td></td>";
                    html_workerroles += "<td></td>";
                    html_workerroles += "<td></td>";
                    html_workerroles += "<td><a href=\"javascript:void(0)\" class=\"workerroleedit\" data-mode=\"edit\">Edit</td>";
                    html_workerroles += "</tr>";

                    cmd.CommandText = "get_entity_workerRoles";
                    cmd.Parameters.Clear();
                    cmd.Parameters.Add("@person_ctr", SqlDbType.VarChar).Value = person_ctr;
                    dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {

                        string entity_workerRole_CTR = dr["entity_workerRole_CTR"].ToString();
                        string Role = dr["Role"].ToString();
                        string StartDate = Functions.formatdate(dr["StartDate"].ToString(), "dd MMM yyyy");
                        string EndDate = Functions.formatdate(dr["EndDate"].ToString(), "dd MMM yyyy");
                        string Note = dr["Note"].ToString();
                        string workerrole_ctr = dr["workerrole_ctr"].ToString();

                        html_workerroles += "<tr id=\"workerrole_" + entity_workerRole_CTR + "\">";
                        html_workerroles += "<td style=\"text-align:center\"></td>";
                        html_workerroles += "<td role_ctr=\"" + workerrole_ctr + "\">" + Role + "</td>";
                        html_workerroles += "<td>" + StartDate + "</td>";
                        html_workerroles += "<td>" + EndDate + "</td>";
                        html_workerroles += "<td>" + Note + "</td>";
                        if (EndDate != "")
                        {
                            html_assigned += "<td></td>";
                        }
                        else
                        {
                            html_workerroles += "<td><a href=\"javascript:void(0)\" class=\"workerroleedit\" data-mode=\"edit\">Edit</td>";
                        }
                        html_workerroles += "</tr>";

                    }
                    dr.Close();

                    //}
                    #endregion //WORKERS

                    #region ASSIGNED
                    //-------------------------------ASSIGNED TAB------------------------------------------------------
                    //if (Functions.accessstringtest(Session["UBC_AccessString"].ToString(), "1"))
                    //{

                    if (!localfunctions.functions.AccessStringTest(username, "11"))
                    {
                        show_assigned_level = " style=\"display:none\"";
                    }

                    html_tab += "<li><a data-target=\"#div_assigned\">Worker Assigments</a></li>";

                    html_assigned = "<thead>";
                    html_assigned += "<tr><th style=\"width:50px;text-align:center\"></th><th>Type</th><th>Person</th><th>Start Date</th><th>End Date</th><th>Note</th><th" + show_assigned_level + ">Level</th><th style=\"width:100px\">Action / <a class=\"assignededit\" data-mode=\"add\" href=\"javascript: void(0)\">Add</a></th></tr>";
                    html_assigned += "</thead>";
                    html_assigned += "<tbody>";

                    //hidden row, used for creating new rows client side
                    html_assigned += "<tr style=\"display:none\">";
                    html_assigned += "<td style=\"text-align:center\"></td>";
                    html_assigned += "<td></td>";
                    html_assigned += "<td></td>";
                    html_assigned += "<td></td>";
                    html_assigned += "<td></td>";
                    html_assigned += "<td></td>";
                    html_assigned += "<td" + show_assigned_level + "></td>"; //level
                    html_assigned += "<td><a href=\"javascript:void(0)\" class=\"assignededit\" data-mode=\"edit\">Edit</td>";
                    html_assigned += "</tr>";



                    cmd.CommandText = "get_entity_assigned";
                    cmd.Parameters.Clear();
                    cmd.Parameters.Add("@person_ctr", SqlDbType.VarChar).Value = person_ctr;
                    dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {

                        string entity_assigned_CTR = dr["entity_worker_id"].ToString();
                        string type = dr["type"].ToString();
                        string person_ctr = dr["person_ctr"].ToString();
                        string person = dr["person"].ToString();
                        string StartDate = Functions.formatdate(dr["StartDate"].ToString(), "dd MMM yyyy");
                        string EndDate = Functions.formatdate(dr["EndDate"].ToString(), "dd MMM yyyy");
                        string Note = dr["Note"].ToString();
                        string AccessLevel = dr["AccessLevel"].ToString();
                        string AccessLevelDisplay = dr["AccessLevelDisplay"].ToString();

                        html_assigned += "<tr id=\"assigned_" + entity_assigned_CTR + "\">";
                        html_assigned += "<td style=\"text-align:center\"></td>";
                        html_assigned += "<td>" + type + "</td>";
                        html_assigned += "<td person_ctr=\"" + person_ctr + "\">" + person + "</td>";
                        html_assigned += "<td>" + StartDate + "</td>";
                        html_assigned += "<td>" + EndDate + "</td>";
                        html_assigned += "<td>" + Note + "</td>";
                        html_assigned += "<td" + show_assigned_level + " accesslevel=\"" + AccessLevel + "\">" + AccessLevelDisplay + "</td>";
                        if (EndDate != "")
                        {
                            html_assigned += "<td></td>";
                        }
                        else
                        {
                            html_assigned += "<td><a href=\"javascript:void(0)\" class=\"assignededit\" data-mode=\"edit\">Edit</td>";
                        }
                        html_assigned += "</tr>";

                    }
                    dr.Close();

                    //}
                    #endregion //ASSIGNED

                    //-------------------------------END TABS------------------------------------------------------
                }
            }
        }


        protected void btn_submit_Click(object sender, EventArgs e)
        {
            Boolean Creating = false;
            string strConnString = "Data Source=toh-app;Initial Catalog=TeOraHou;Integrated Security=False;user id=OnlineServices;password=Whanganui497";
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            person_ctr = ViewState["person_ctr"].ToString();
            if (person_ctr == "new")
            {
                Creating = true;
            }

            cmd.CommandText = "Update_person";
            cmd.Parameters.Add("@username", SqlDbType.VarChar).Value = username;
            cmd.Parameters.Add("@person_ctr", SqlDbType.VarChar).Value = person_ctr;
            cmd.Parameters.Add("@firstname", SqlDbType.VarChar).Value = Request.Form["fld_firstname"].Trim();
            cmd.Parameters.Add("@lastname", SqlDbType.VarChar).Value = Request.Form["fld_surname"].Trim();
            cmd.Parameters.Add("@knownas", SqlDbType.VarChar).Value = Request.Form["fld_knownas"].Trim();
            cmd.Parameters.Add("@birthdate", SqlDbType.VarChar).Value = Request.Form["fld_birthdate"].Trim();
            cmd.Parameters.Add("@gender", SqlDbType.VarChar).Value = Request.Form["fld_gender"].Trim();
            cmd.Parameters.Add("@medical", SqlDbType.VarChar).Value = Request.Form["fld_medical"].Trim();
            cmd.Parameters.Add("@dietary", SqlDbType.VarChar).Value = Request.Form["fld_dietary"].Trim();
            cmd.Parameters.Add("@notes", SqlDbType.VarChar).Value = Request.Form["fld_notes"].Trim();

            cmd.Connection = con;
            //try
            //{
            con.Open();
            person_ctr = cmd.ExecuteScalar().ToString();
            con.Close();
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}

            foreach (string key in Request.Form)
            {
                if (key.StartsWith("assigned_"))
                {
                    string person_worker_ctr = key.Substring(9);  //key length 
                    if (person_worker_ctr.EndsWith("_delete"))
                    {
                        cmd.CommandText = "Delete_person_Worker";
                        cmd.Parameters.Clear();
                        cmd.Parameters.Add("@person_worker_ctr", SqlDbType.VarChar).Value = person_worker_ctr.Substring(0, person_worker_ctr.Length - 7);
                    }
                    else
                    {
                        if (person_worker_ctr.StartsWith("new"))
                        {
                            person_worker_ctr = "new";
                        }

                        string[] valuesSplit = Request.Form[key].Split('\x00FE');
                        cmd.CommandText = "Update_person_Worker";
                        cmd.Parameters.Clear();
                        cmd.Parameters.Add("@person_worker_ctr", SqlDbType.VarChar).Value = person_worker_ctr;
                        cmd.Parameters.Add("@person_ctr", SqlDbType.VarChar).Value = person_ctr;
                        cmd.Parameters.Add("@type", SqlDbType.VarChar).Value = valuesSplit[0];
                        cmd.Parameters.Add("@worker_ctr", SqlDbType.VarChar).Value = valuesSplit[1];
                        cmd.Parameters.Add("@startdate", SqlDbType.VarChar).Value = valuesSplit[2];
                        cmd.Parameters.Add("@enddate", SqlDbType.VarChar).Value = valuesSplit[3];
                        cmd.Parameters.Add("@note", SqlDbType.VarChar).Value = valuesSplit[4];
                        cmd.Parameters.Add("@accesslevel", SqlDbType.VarChar).Value = valuesSplit[5];
                    }
                    con.Open();
                    cmd.ExecuteScalar().ToString();
                    con.Close();
                }
                else if (key.StartsWith("workerrole_"))
                {
                    string person_workerrole_ctr = key.Substring(11);   //key length 
                    //if (person_worker_ctr.EndsWith("_delete"))
                    //{
                    //    cmd.CommandText = "Delete_person_Worker";
                    //    cmd.Parameters.Clear();
                    //    cmd.Parameters.Add("@person_worker_ctr", SqlDbType.VarChar).Value = person_worker_ctr.Substring(0, person_worker_ctr.Length - 7);
                    //}
                    //else
                    //{
                    if (person_workerrole_ctr.StartsWith("new"))
                    {
                        person_workerrole_ctr = "new";
                    }

                    string[] valuesSplit = Request.Form[key].Split('\x00FE');
                    cmd.CommandText = "Update_person_workerrole";
                    cmd.Parameters.Clear();
                    cmd.Parameters.Add("@person_workerrole_ctr", SqlDbType.VarChar).Value = person_workerrole_ctr;
                    cmd.Parameters.Add("@person_ctr", SqlDbType.VarChar).Value = person_ctr;
                    cmd.Parameters.Add("@workerrole_ctr", SqlDbType.VarChar).Value = valuesSplit[0];
                    cmd.Parameters.Add("@startdate", SqlDbType.VarChar).Value = valuesSplit[1];
                    cmd.Parameters.Add("@enddate", SqlDbType.VarChar).Value = valuesSplit[2];
                    cmd.Parameters.Add("@note", SqlDbType.VarChar).Value = valuesSplit[3];
                    //}
                    con.Open();
                    cmd.ExecuteScalar().ToString();
                    con.Close();
                }
                else if (key.StartsWith("encounter_"))
                {
                    string encounter_ctr = key.Substring(10);   //key length 
                    //if (encounter_ctr.EndsWith("_delete"))
                    //{
                    //    cmd.CommandText = "Delete_encounter";
                    //    cmd.Parameters.Clear();
                    //    cmd.Parameters.Add("@encounter_ctr", SqlDbType.VarChar).Value = encounter.Substring(0, encounter_ctr.Length - ???);
                    //}
                    //else
                    //{
                    if (encounter_ctr.StartsWith("new"))
                    {
                        encounter_ctr = "new";
                    }

                    string[] valuesSplit = Request.Form[key].Split('\x00FE');

                    cmd.CommandText = "Update_encounter";
                    cmd.Parameters.Clear();
                    cmd.Parameters.Add("@encounter_ctr", SqlDbType.VarChar).Value = encounter_ctr;
                    cmd.Parameters.Add("@person_ctr", SqlDbType.VarChar).Value = person_ctr;
                    cmd.Parameters.Add("@narrative", SqlDbType.VarChar).Value = valuesSplit[2];
                    cmd.Parameters.Add("@startdatetime", SqlDbType.VarChar).Value = valuesSplit[0];
                    cmd.Parameters.Add("@enddatetime", SqlDbType.VarChar).Value = valuesSplit[1];
                    cmd.Parameters.Add("@encounteraccesslevel", SqlDbType.VarChar).Value = valuesSplit[4];
                    cmd.Parameters.Add("@workers", SqlDbType.VarChar).Value = valuesSplit[3];
                    //}
                    con.Open();
                    cmd.ExecuteScalar().ToString();
                    con.Close();
                }
                else if (key.StartsWith("address_"))
                {
                    string address_ctr = key.Substring(8);   //key length 
                    //if (encounter_ctr.EndsWith("_delete"))
                    //{
                    //    cmd.CommandText = "Delete_address";
                    //    cmd.Parameters.Clear();
                    //    cmd.Parameters.Add("@encounter_ctr", SqlDbType.VarChar).Value = encounter.Substring(0, encounter_ctr.Length - ???);
                    //}
                    //else
                    //{
                    if (address_ctr.StartsWith("new"))
                    {
                        address_ctr = "new";
                    }

                    string[] valuesSplit = Request.Form[key].Split('\x00FE');

                    cmd.CommandText = "Update_person_address";
                    cmd.Parameters.Clear();
                    cmd.Parameters.Add("@username", SqlDbType.VarChar).Value = username;
                    cmd.Parameters.Add("@address_ctr", SqlDbType.VarChar).Value = address_ctr;
                    cmd.Parameters.Add("@person_ctr", SqlDbType.VarChar).Value = person_ctr;
                    cmd.Parameters.Add("@address", SqlDbType.VarChar).Value = valuesSplit[0];
                    cmd.Parameters.Add("@current", SqlDbType.VarChar).Value = valuesSplit[1];
                    cmd.Parameters.Add("@notes", SqlDbType.VarChar).Value = valuesSplit[2];
                    if (valuesSplit[3] != "")
                    {
                        cmd.Parameters.Add("@latitude", SqlDbType.VarChar).Value = valuesSplit[3].Split(',')[0];
                        cmd.Parameters.Add("@longitude", SqlDbType.VarChar).Value = valuesSplit[3].Split(',')[1];
                    }
                    //}
                    con.Open();
                    cmd.ExecuteScalar().ToString();
                    con.Close();
                }
            }
            //finally
            //{

            con.Dispose();
            //}

            string returnto = ViewState["returnto"].ToString();

            if (Creating)
            {
                if (returnto == "")
                {
                    returnto = "personmaintenance.aspx?id=" + person_ctr;
                }
                else
                {
                    returnto = "personmaintenance.aspx?id=" + person_ctr + "&returnto=" + returnto + ".aspx";
                }
            }
            else
            {
                if (returnto == "")
                {
                    returnto = "personsearch.aspx";
                }
            }

            Response.Redirect(returnto);
        }
    }
}
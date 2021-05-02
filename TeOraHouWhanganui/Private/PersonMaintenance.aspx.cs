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
        public string fld_windowsuser;
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
        public string html_phones = "";
        public string html_email = "";
        public string html_attendance = "";
        public string html_enrolments = "";
        public string photoalbumlink = "";

        public Dictionary<string, string> options = new Dictionary<string, string>();
        public string[] nooptions = { };

        public Dictionary<string, string> genders = new Dictionary<string, string>();
        public Dictionary<string, string> workers = new Dictionary<string, string>();
        public Dictionary<string, string> workerRoles = new Dictionary<string, string>();
        public Dictionary<string, string> assignmenttypes = new Dictionary<string, string>();
        public Dictionary<string, string> encounterAccessLevels = new Dictionary<string, string>();
        public Dictionary<string, string> AllencounterAccessLevels = new Dictionary<string, string>();
        public Dictionary<string, string> YesNoBit = new Dictionary<string, string>();
        public Dictionary<string, string> YesNo = new Dictionary<string, string>();
        public Dictionary<string, string> programs = new Dictionary<string, string>();
        public Dictionary<string, string> enrolmentstatus = new Dictionary<string, string>();

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
                    options.Add("parameters", "notnegative");
                    //options.Add("insertblank", "start");
                    encounterAccessLevels = Functions.buildselectionlist(connectionString, "get_EncounterAccessLevels", options);

                    options.Clear();
                    options.Add("storedprocedure", "");
                    options.Add("storedprocedurename", "");
                    options.Add("usevalues", "");
                    //options.Add("insertblank", "start");
                    AllencounterAccessLevels = Functions.buildselectionlist(connectionString, "get_EncounterAccessLevels", options);

                    options.Clear();
                    options.Add("storedprocedure", "");
                    options.Add("storedprocedurename", "");
                    options.Add("usevalues", "");
                    //options.Add("insertblank", "start");
                    programs = Functions.buildselectionlist(connectionString, "get_Programs", options);


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

                    YesNoBit.Add("Yes", "1");  //SQL returns 1 or 0 but toString() changes it to True or False
                    YesNoBit.Add("No", "0");
                    YesNo.Add("Yes", "Yes");
                    YesNo.Add("No", "No");

                    enrolmentstatus.Add("Current", "Current");
                    enrolmentstatus.Add("Casual", "Casual");
                    enrolmentstatus.Add("Finished", "Finished");
                    enrolmentstatus.Add("Deceased", "Deceased");

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
                        if (fld_photoalbumlink != "")
                        {
                            photoalbumlink = "<a href=\"" + fld_photoalbumlink + "\" target=\"photos\">Google</a>";
                        }
                        fld_windowsuser = dr["windowsuser"].ToString();
                    }
                    dr.Close();

                    #region ENROLEMENTS
                    //-------------------------------ENROLMENT TAB------------------------------------------------------
                    //if (Functions.accessstringtest(Session["UBC_AccessString"].ToString(), "1"))
                    //{

                    html_tab += "<li><a data-target=\"#div_enrolment\">Enrolments</a></li>";
                    html_enrolments = "<thead>";
                    //html_enrolments += "<tr><th style=\"width:50px;text-align:center\"></th><th>Program</th><th>Started</th><th>Ended</th><th>First Event</th><th>Last Event</th><th>Status</th><th>Worker</th><th>Always Pickup</th><th>Note</th><th style=\"width:100px\">Action / <a class=\"enrolmentedit\" data-mode=\"add\" href=\"javascript: void(0)\">Add</a></th></tr>";
                    html_enrolments += "<tr><th style=\"width:50px;text-align:center\"></th><th>Program</th><th>First Event</th><th>Last Event</th><th>Events</th><th>Status</th><th>Worker</th><th>Always Pickup</th><th>Note</th><th style=\"width:100px\">Action / <a class=\"enrolmentedit\" data-mode=\"add\" href=\"javascript: void(0)\">Add</a></th></tr>";
                    html_enrolments += "</thead>";
                    html_enrolments += "<tbody>";

                    //hidden row, used for creating new rows client side
                    html_enrolments += "<tr style=\"display:none\">";
                    html_enrolments += "<td style=\"text-align:center\"></td>";
                    html_enrolments += "<td></td>"; //Program
                    //html_enrolments += "<td></td>"; //Started
                    //html_enrolments += "<td></td>"; //Ended
                    html_enrolments += "<td></td>"; //First Event
                    html_enrolments += "<td></td>"; //Last Event
                    html_enrolments += "<td></td>"; //Events
                    //html_enrolments += "<td></td>"; //Encounters
                    html_enrolments += "<td></td>"; //Status
                    html_enrolments += "<td></td>"; //Worker
                    html_enrolments += "<td></td>"; //Always pickup
                    html_enrolments += "<td></td>"; //Note
                    html_enrolments += "<td><a href=\"javascript:void(0)\" class=\"enrolmentedit\" data-mode=\"edit\">Edit</td>";
                    html_enrolments += "</tr>";

                    cmd.CommandText = "get_entity_enrolments";
                    cmd.Parameters.Clear();
                    cmd.Parameters.Add("@person_ctr", SqlDbType.VarChar).Value = person_ctr;
                    dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        string entity_enrolment_CTR = dr["ID"].ToString();
                        string programid = dr["programid"].ToString();
                        string programname = dr["programname"].ToString();
                        string firstevent = Functions.formatdate(dr["firstevent"].ToString(), "dd MMM yyyy");
                        string lastevent = Functions.formatdate(dr["lastevent"].ToString(), "dd MMM yyyy");
                        string events = dr["eventsattended"].ToString();
                        //string events = Functions.formatdate(dr["firstevent"].ToString(), "dd MMM yyyy") + " - " + Functions.formatdate(dr["lastevent"].ToString(), "dd MMM yyyy");
                        //string encounters = Functions.formatdate(dr["firstencounter"].ToString(), "dd MMM yyyy") + " - " + Functions.formatdate(dr["lastencounter"].ToString(), "dd MMM yyyy");
                        string status = dr["enrolementstatus"].ToString();
                        string worker = dr["worker"].ToString();
                        string alwayspickup = dr["alwayspickup"].ToString();
                        string note = dr["notes"].ToString();
                        //string startdate = Functions.formatdate(dr["startdate"].ToString(), "dd MMM yyyy");
                        //string enddate = Functions.formatdate(dr["enddate"].ToString(), "dd MMM yyyy");
                        html_enrolments += "<tr id=\"enrolment_" + entity_enrolment_CTR + "\">";
                        html_enrolments += "<td style=\"text-align:center\"></td>";
                        html_enrolments += "<td programid=\"" + programid + "\">" + programname + "</td>";
                        //html_enrolments += "<td>" + startdate + "</td>";
                        //html_enrolments += "<td>" + enddate + "</td>";
                        html_enrolments += "<td>" + firstevent + "</td>";
                        html_enrolments += "<td>" + lastevent + "</td>";
                        html_enrolments += "<td>" + events + "</td>";
                        //html_enrolments += "<td>" + encounters + "</td>";
                        html_enrolments += "<td>" + status + "</td>";
                        html_enrolments += "<td worker=\"" + worker + "\">" + YesNoBit.FirstOrDefault(x => x.Value == worker).Key + "</td>";
                        html_enrolments += "<td alwayspickup=\"" + alwayspickup + "\">" + YesNoBit.FirstOrDefault(x => x.Value == alwayspickup).Key + "</td>";
                        html_enrolments += "<td>" + note + "</td>";
                        html_enrolments += "<td><a href=\"javascript:void(0)\" class=\"enrolmentedit\" data-mode=\"edit\">Edit</td>";
                        html_enrolments += "</tr>";
                    }
                    dr.Close();

                    //}

                    #endregion ENROLEMENTS

                    #region ENCOUNTERS

                    //-------------------------------ENCOUNTERS TAB------------------------------------------------------
                    //if (localfunctions.functions.AccessStringTest(username, "111"))
                    //{

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
                        if (Narrative == "Restricted")
                        {
                            html_encounter += "<td></td>";
                        }
                        else
                        {
                            html_encounter += "<td><a href=\"javascript:void(0)\" class=\"encounteredit\" data-mode=\"edit\">Edit</td>";
                        }
                        html_encounter += "</tr>";

                    }
                    dr.Close();
                    //}

                    #endregion ENCOUNTERS

                    #region PHONE
                    //-------------------------------PHONE TAB------------------------------------------------------
                    //if (Functions.accessstringtest(Session["UBC_AccessString"].ToString(), "1"))
                    //{

                    html_tab += "<li><a data-target=\"#div_phone\">Phones</a></li>";
                    html_phones = "<thead>";
                    html_phones += "<tr><th style=\"width:50px;text-align:center\"></th><th>Number</th><th>Mobile</th><th>Own</th><th>Send Texts</th><th>Send Now</th><th>Note</th><th style=\"width:100px\">Action / <a class=\"phoneedit\" data-mode=\"add\" href=\"javascript: void(0)\">Add</a></th></tr>";
                    html_phones += "</thead>";
                    html_phones += "<tbody>";

                    //hidden row, used for creating new rows client side
                    html_phones += "<tr style=\"display:none\">";
                    html_phones += "<td style=\"text-align:center\"></td>";
                    html_phones += "<td></td>"; //Number
                    html_phones += "<td></td>"; //Mobile
                    html_phones += "<td></td>"; //Own
                    html_phones += "<td></td>"; //Send Texts
                    html_phones += "<td></td>"; //Send Now
                    html_phones += "<td></td>"; //Note
                    html_phones += "<td><a href=\"javascript:void(0)\" class=\"phoneedit\" data-mode=\"edit\">Edit</td>";
                    html_phones += "</tr>";

                    cmd.CommandText = "get_entity_phones";
                    cmd.Parameters.Clear();
                    cmd.Parameters.Add("@person_ctr", SqlDbType.VarChar).Value = person_ctr;
                    dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        string entity_phone_CTR = dr["entity_phone_ID"].ToString();
                        string phone = dr["phone"].ToString();
                        string mobile = dr["mobile"].ToString();
                        string text = dr["text"].ToString();
                        string sendnow = "";
                        if (mobile == "Yes")
                        {
                            //if (sendtexts == "1")
                            //{
                            sendnow = "<a class=\"send_text\">Send</a>";
                            //}
                        }
                        string own = dr["own"].ToString();
                        string Note = dr["Note"].ToString();


                        if (phone != "")
                        {
                            phone = "<a href=\"tel:" + phone + "\">" + phone + "</>";
                        }

                        html_phones += "<tr id=\"phone_" + entity_phone_CTR + "\">";
                        html_phones += "<td style=\"text-align:center\"></td>";
                        html_phones += "<td>" + phone + "</td>";
                        //html_phones += "<td mobile=\"" + mobile + "\">" + YesNo.FirstOrDefault(x => x.Value == mobile).Key + "</td>";
                        html_phones += "<td>" + mobile + "</td>";
                        //html_phones += "<td own=\"" + own + "\">" + YesNo.FirstOrDefault(x => x.Value == own).Key + "</td>";
                        html_phones += "<td>" + own + "</td>";
                        //html_phones += "<td sendtexts=\"" + sendtexts + "\">" + YesNo.FirstOrDefault(x => x.Value == sendtexts).Key + "</td>";
                        html_phones += "<td>" + text + "</td>";
                        html_phones += "<td>" + sendnow + "</td>";
                        html_phones += "<td>" + Note + "</td>";
                        html_phones += "<td><a href=\"javascript:void(0)\" class=\"phoneedit\" data-mode=\"edit\">Edit</td>";
                        html_phones += "</tr>";
                    }
                    dr.Close();

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
                    //-------------------------------ATTENDANCE TAB------------------------------------------------------
                    //if (Functions.accessstringtest(Session["UBC_AccessString"].ToString(), "1"))
                    //{

                    html_tab += "<li><a data-target=\"#div_attendance\">Attendance</a></li>";

                    html_attendance = "<thead>";
                    html_attendance += "<tr><th style=\"width:50px;text-align:center\"></th><th>Date</th><th>Program</th><th>Description</th><th>Attendance</th><th>Capacity</th><th>Note</th><th style=\"width:100px\">Action / <a class=\"eventedit\" data-mode=\"add\" href=\"javascript: void(0)\">Add</a></th></tr>";
                    html_attendance += "</thead>";
                    html_attendance += "<tbody>";

                    //hidden row, used for creating new rows client side
                    html_attendance += "<tr style=\"display:none\">";
                    html_attendance += "<td style=\"text-align:center\"></td>";
                    html_attendance += "<td></td>"; //Date
                    html_attendance += "<td></td>"; //Program
                    html_attendance += "<td></td>"; //Description
                    //html_attendance += "<td></td>"; //Event Notes
                    html_attendance += "<td></td>"; //Attendance
                    html_attendance += "<td></td>"; //Capacity
                    html_attendance += "<td></td>"; //Attendance Notes
                    html_attendance += "<td><a href=\"javascript:void(0)\" class=\"eventedit\" data-mode=\"edit\">Edit</td>";
                    html_attendance += "</tr>";

                    cmd.CommandText = "get_entity_eventattendance";
                    cmd.Parameters.Clear();
                    cmd.Parameters.Add("@person_ctr", SqlDbType.VarChar).Value = person_ctr;
                    dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        string attendance_CTR = dr["attendance_CTR"].ToString();
                        string description = dr["description"].ToString();
                        string program = dr["programname"].ToString();
                        string eventNotes = dr["EventNotes"].ToString();
                        string attendance = dr["Attendance"].ToString();
                        string attendanceNotes = dr["AttendanceNotes"].ToString();
                        string capacity = dr["Capacity"].ToString();
                        string date = dr["Date"].ToString();

                        html_attendance += "<tr id=\"attendance_" + attendance_CTR + "\">";
                        html_attendance += "<td style=\"text-align:center\"></td>";
                        html_attendance += "<td nowrap>" + date + "</td>";
                        html_attendance += "<td>" + program + "</td>";
                        html_attendance += "<td>" + description + "</td>";
                        html_attendance += "<td>" + attendance + "</td>";
                        html_attendance += "<td>" + capacity + "</td>";
                        html_attendance += "<td>" + attendanceNotes + "</td>";
                        html_attendance += "<td><a href=\"javascript:void(0)\" class=\"attendanceedit\" data-mode=\"edit\">Edit</td>";
                        html_attendance += "</tr>";
                    }
                    dr.Close();
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
                        html_addresses += "<td current=\"" + current + "\">" + YesNoBit.FirstOrDefault(x => x.Value == current).Key + "</td>";
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
                    Boolean allowedit = true;
                    if (!localfunctions.functions.AccessStringTest(username, "11"))
                    {
                        allowedit = false;
                    }

                    if (!localfunctions.functions.AccessStringTest(username, "1"))
                    {
                        show_assigned_level = " style=\"display:none\"";
                    }

                    html_tab += "<li><a data-target=\"#div_assigned\">Worker Assigments</a></li>";

                    html_assigned = "<thead>";
                    html_assigned += "<tr><th style=\"width:50px;text-align:center\"></th><th>Type</th><th>Person</th><th>Start Date</th><th>End Date</th><th>Note</th><th" + show_assigned_level + ">Level</th>";
                    if (allowedit)
                    {
                        html_assigned += "<th style=\"width:100px\">Action / <a class=\"assignededit\" data-mode=\"add\" href=\"javascript: void(0)\">Add</a></th>";
                    }
                    html_assigned += "</tr>";
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
                    if (allowedit)
                    {
                        html_assigned += "<td><a href=\"javascript:void(0)\" class=\"assignededit\" data-mode=\"edit\">Edit</td>";
                    }
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

                        if (AccessLevel != "-1" || allowedit)
                        {
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
                                if (allowedit)
                                {
                                    html_assigned += "<td><a href=\"javascript:void(0)\" class=\"assignededit\" data-mode=\"edit\">Edit</td>";
                                }
                            }
                            html_assigned += "</tr>";
                        }
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
            cmd.Parameters.Add("@windowsuser", SqlDbType.VarChar).Value = Request.Form["fld_windowsuser"].Trim();

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
                    if (encounter_ctr.EndsWith("_delete"))
                    {
                        cmd.CommandText = "Delete_encounter";
                        cmd.Parameters.Clear();
                        cmd.Parameters.Add("@encounter_ctr", SqlDbType.VarChar).Value = encounter_ctr;
                    }
                    else
                    {
                        if (encounter_ctr.StartsWith("new"))
                        {
                            encounter_ctr = "new";
                        }

                        string[] valuesSplit = Request.Form[key].Split('\x00FE');

                        cmd.CommandText = "Update_encounter";
                        cmd.Parameters.Clear();
                        cmd.Parameters.Add("@username", SqlDbType.VarChar).Value = username;
                        cmd.Parameters.Add("@encounter_ctr", SqlDbType.VarChar).Value = encounter_ctr;
                        cmd.Parameters.Add("@person_ctr", SqlDbType.VarChar).Value = person_ctr;
                        cmd.Parameters.Add("@narrative", SqlDbType.VarChar).Value = valuesSplit[2];
                        cmd.Parameters.Add("@startdatetime", SqlDbType.VarChar).Value = valuesSplit[0];
                        cmd.Parameters.Add("@enddatetime", SqlDbType.VarChar).Value = valuesSplit[1];
                        cmd.Parameters.Add("@encounteraccesslevel", SqlDbType.VarChar).Value = valuesSplit[4];
                        cmd.Parameters.Add("@workers", SqlDbType.VarChar).Value = valuesSplit[3];
                    }
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
                else if (key.StartsWith("phone_"))
                {
                    string phone_ctr = key.Substring(6);   //key length 
                    if (phone_ctr.EndsWith("_delete"))
                    {
                        cmd.CommandText = "Delete_phone";
                        cmd.Parameters.Clear();
                        cmd.Parameters.Add("@phone_ctr", SqlDbType.VarChar).Value = phone_ctr.Substring(0, phone_ctr.Length - 99);
                    }
                    else
                    {
                        if (phone_ctr.StartsWith("new"))
                        {
                            phone_ctr = "new";
                        }

                        string[] valuesSplit = Request.Form[key].Split('\x00FE');

                        cmd.CommandText = "Update_person_phone";
                        cmd.Parameters.Clear();
                        cmd.Parameters.Add("@username", SqlDbType.VarChar).Value = username;
                        cmd.Parameters.Add("@person_phone_ctr", SqlDbType.VarChar).Value = phone_ctr;
                        cmd.Parameters.Add("@person_ctr", SqlDbType.VarChar).Value = person_ctr;
                        cmd.Parameters.Add("@number", SqlDbType.VarChar).Value = valuesSplit[0];
                        cmd.Parameters.Add("@mobile", SqlDbType.VarChar).Value = valuesSplit[1];
                        cmd.Parameters.Add("@own", SqlDbType.VarChar).Value = valuesSplit[2];
                        cmd.Parameters.Add("@text", SqlDbType.VarChar).Value = valuesSplit[3];
                        cmd.Parameters.Add("@note", SqlDbType.VarChar).Value = valuesSplit[4];
                    }
                    con.Open();
                    cmd.ExecuteScalar().ToString();
                    con.Close();
                }
                else if (key.StartsWith("enrolment_"))
                {
                    string enrolment_ctr = key.Substring(10);   //key length 
                                                                //if (enrolment_ctr.EndsWith("_delete"))
                                                                //{
                                                                //    cmd.CommandText = "Delete_enrolment";
                                                                //    cmd.Parameters.Clear();
                                                                //    cmd.Parameters.Add("@enrolment_ctr", SqlDbType.VarChar).Value = enrolment.Substring(0, enrolment_ctr.Length - ???);
                                                                //}
                                                                //else
                                                                //{
                    if (enrolment_ctr.StartsWith("new"))
                    {
                        enrolment_ctr = "new";
                    }

                    string[] valuesSplit = Request.Form[key].Split('\x00FE');

                    //value = tr_program + delim + tr_status + delim + tr_worker + delim + tr_alwayspickup + delim + tr_note;

                    cmd.CommandText = "Update_person_enrolment";
                    cmd.Parameters.Clear();
                    cmd.Parameters.Add("@enrolment_ctr", SqlDbType.VarChar).Value = enrolment_ctr;
                    cmd.Parameters.Add("@person_ctr", SqlDbType.VarChar).Value = person_ctr;
                    cmd.Parameters.Add("@programid", SqlDbType.VarChar).Value = valuesSplit[0];
                    cmd.Parameters.Add("@status", SqlDbType.VarChar).Value = valuesSplit[1];
                    cmd.Parameters.Add("@worker", SqlDbType.VarChar).Value = valuesSplit[2];
                    cmd.Parameters.Add("@alwayspickup", SqlDbType.VarChar).Value = valuesSplit[3];
                    cmd.Parameters.Add("@note", SqlDbType.VarChar).Value = valuesSplit[4];
                    cmd.Parameters.Add("@startdate", SqlDbType.VarChar).Value = valuesSplit[5];
                    cmd.Parameters.Add("@enddate", SqlDbType.VarChar).Value = valuesSplit[6];
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


using Generic;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Cemeteries.Administration.Sexton
{
    public partial class Entry : System.Web.UI.Page
    {
        #region fields
        public string hf_id;
        public string warrant_no;
        public string register_no;
        public string surname;
        public string givennames;
        public string lastpermanentaddress;
        public string city;
        public string placeofdeath;
        public string dateofbirth;
        public string age;
        public string age_period;
        public string gender;
        public string denomination;
        public string minister;
        public string dateofdeath;
        public string dateofmedcert;
        public string rankoroccupation;
        public string maritalstatus;
        public string funeralcoordinator;
        public string remarks;
        //public string[] stillborn = new string[1];
        public string stillborn;
        public string created_by;
        public string date_created;
        public string modified_by;
        public string date_modified;
        public string transactions;

        public string transactiontypes;
        public string jsobj_transactiontypes;

        //public Dictionary<string, string> yesno = new Dictionary<string, string>();
        public Dictionary<string, string> options = new Dictionary<string, string>();
        public string[] yesno_values = new string[2] { "Yes", "No" };
        public Dictionary<string, string> transactiontype_values;


        String strConnString = ConfigurationManager.ConnectionStrings["Cemetery"].ConnectionString;



        //public string[] transactiontype_values = new string[7] { "Burial", "Burial Ashes", "Cremation", "Disinterment", "Memorial (only)", "Ashes Scattered", "Ashes Taken" };
        //"Burial", "Burialash", "Cremation", "Disinter", "Memorial", "Scattered", "Taken"

        public string html_images = "";

        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            //yesno.Add("Yes", "Yes");
            //yesno.Add("No", "No");

            options.Clear();
            options.Add("usevalues", "");
            options.Add("insertblank", "start");
            transactiontype_values = Functions.buildselectionlist(strConnString, "select [TransactionTypeID] as [Value], [Title] as [Label] from [TransactionType] order by [Title]", options);

            if (!IsPostBack)
            {
                hf_id = Request.QueryString["ID"];

                //List<BurialRecord> BurialRecordList = new List<BurialRecord>();

                Dictionary<string, string> options = new Dictionary<string, string>();

                options.Clear();
                options["mode"] = "sql";
                options["connectionstring"] = strConnString;
                options["sqltype"] = "text";
                options["sqltext"] = "select transactiontypeID as [Value], Title as [Label], * from transactiontype order by Description";
                jsobj_transactiontypes = Functions.buildJSobject(options);




                if (hf_id != "S")
                {
                    SqlConnection con = new SqlConnection(strConnString);

                    SqlCommand cmd = new SqlCommand("get_cemetery_person", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@accessid", SqlDbType.VarChar).Value = hf_id;

                    cmd.Connection = con;
                    try
                    {
                        con.Open();
                        SqlDataReader dr = cmd.ExecuteReader();

                        if (dr.HasRows)
                        {
                            dr.Read();
                            //BurialRecordList.Add(new BurialRecord
                            //{
                            warrant_no = dr["Warrant_No"].ToString();
                            register_no = dr["Register_No"].ToString();
                            surname = dr["surname"].ToString();
                            givennames = dr["givenNames"].ToString();
                            lastpermanentaddress = dr["lastPermanentAddress"].ToString();
                            city = dr["city"].ToString();
                            placeofdeath = dr["placeOfDeath"].ToString();
                            //dateofbirth = dr["dateOfDeath"].ToString();
                            dateofbirth = Functions.formatdate(dr["dateOfBirth"].ToString(), "dd MMM yyyy");
                            age = dr["age"].ToString();
                            age_period = dr["age_Period"].ToString();
                            gender = dr["gender"].ToString();
                            denomination = dr["denomination"].ToString();
                            minister = dr["minister"].ToString();
                            //dateofdeath = dr["dateOfDeath"].ToString();
                            //dateofmedcert = dr["dateOfMedCert"].ToString();
                            dateofdeath = Functions.formatdate(dr["dateofdeath"].ToString(), "dd MMM yyyy");
                            dateofmedcert = Functions.formatdate(dr["dateofmedcert"].ToString(), "dd MMM yyyy");
                            rankoroccupation = dr["rankOrOccupation"].ToString();
                            maritalstatus = dr["maritalStatus"].ToString();
                            funeralcoordinator = dr["funeralCoOrdinator"].ToString();
                            remarks = dr["remarks"].ToString();
                            //stillborn[0] = dr["stillBorn"].ToString();
                            stillborn = dr["stillBorn"].ToString();
                            created_by = dr["created_by"].ToString();
                            date_created = dr["date_created"].ToString();
                            modified_by = dr["modified_by"].ToString();
                            date_modified = dr["date_modified"].ToString();


                            /*
                            if (Location != "")
                            {
                                string GISIDRef = "";
                                lit_table.Text += "<tr><td class=\"view_label\">Location</td><td>" + Location + GISIDRef + "</td></tr>";
                            }
                            if (GISID != "")
                            {
                                lit_map.Text = "<iframe style=\"width:100%;height:400px\" src=\"http://maps.whanganui.govt.nz/IntraMaps/MapControls/EasiMaps/index.html?mapkey=" + GISID + "&project=WhanganuiMapControls&module=WDCCemeteries&layer=~Cemetery%20Plots&search=false&info=false&slider=false&expand=false\"></iframe>";

                                lit_tabs.Text += "<li><a data-toggle=\"tab\"href=\"#div_map\">Map</a></li>";
                            } else
                            {
                                if(Area == "Rose Garden") { 
                                    lit_map.Text = "<iframe style=\"width:100%;height:400px\" src=\"https://mangomap.com/wdc/maps/74982/Whanganui-Cemeteries?field=location&layer=038ea1c2-7fd2-11e8-bfd8-06765ea3034e&preview=true&value=Rose+Garden\"></iframe>";
                                    lit_tabs.Text += "<li><a data-toggle=\"tab\"href=\"#div_map\">Map</a></li>";
                                }
                            }
                            */
                            String imagepath = ConfigurationManager.AppSettings["Cemetery.ImageFolder"] + hf_id;
                            String imageurl = ConfigurationManager.AppSettings["Cemetery.ImageURL"] + hf_id + "/";

                            if (Directory.Exists(imagepath))
                            {
                                var files = Directory.EnumerateFiles(imagepath, "*.*", SearchOption.TopDirectoryOnly)
                                    .Where(s => s.ToLower().EndsWith(".jpg") || s.ToLower().EndsWith(".jpeg"));

                                if (files.Count() > 0)
                                {
                                    //html_images += "<a data-toggle=\"tab\"href=\"#div_images\">Images</a></li>";
                                    foreach (string currentFile in files)
                                    {
                                        html_images += "<img src=\"" + imageurl + Path.GetFileName(currentFile) + "\" />";
                                    }
                                }

                            }
                            /*
                            if(inscription != "")
                            {
                                lit_tabs.Text += "<li><a data-toggle=\"tab\"href=\"#div_inscription\">Inscription</a></li>";
                                lit_inscription.Text = inscription;
                            }
                            */

                            /*
                            lit_tabs.Text += "<li><a data-toggle=\"tab\"href=\"#div_notes\">Notes</a></li>";
                            lit_tabs.Text += "<li><a data-toggle=\"tab\"href=\"#div_feedback\">Feedback</a></li>";
                            */

                            dr.Close();
                            cmd = new SqlCommand("get_cemetery_person_transactions", con);
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.Add("@accessid", SqlDbType.VarChar).Value = hf_id;

                            cmd.Connection = con;
                            try
                            {
                                //con.Open();
                                dr = cmd.ExecuteReader();

                                if (dr.HasRows)
                                {
                                    while (dr.Read())
                                    {
                                        string delim = "";

                                        string plotid = dr["plotid"].ToString();
                                        string plot = "";
                                        if (plotid != "")
                                        {
                                            plot = "Plot: " + dr["plotname"].ToString() + ", " + dr["cemeteryname"].ToString() + ", " + dr["areaname"].ToString() + ", " + dr["divisionname"].ToString();
                                            delim = " ";
                                        }
                                        string depth = dr["depth"].ToString();
                                        string displaydepth = "";
                                        if (depth != "")
                                        {
                                            displaydepth = delim + "Depth: " + depth;
                                            delim = " ";
                                        }
                                        string takenby = dr["taken_by"].ToString();
                                        string displaytakenby = "";
                                        if (takenby != "")
                                        {
                                            displaytakenby = delim + "Taken by: " + takenby;
                                            delim = " ";
                                        }


                                        transactions += "<tr id=\"T_" + dr["transactionID"] + "\"  transactiontypeid=\"" + dr["transactiontypeid"] + "\" transactiondepth=\"" + depth + "\" transactiontakenby=\"" + takenby + "\" transactionplot=\"" + plotid + "\" transaction_cemetery=\"" + dr["cemeteryid"].ToString() + "\" transaction_area=\"" + dr["areaid"].ToString() + "\" transaction_division=\"" + dr["divisionid"].ToString() + "\" transaction_GISplot=\"" + dr["GISplotid"].ToString() + "\">";
                                        transactions += "<td>" + Convert.ToDateTime(dr["actiondate"]).ToString("dd MMM yyyy") + "</td>";
                                        transactions += "<td>" + dr["transactiondescription"] + "</td>";


                                        transactions += "<td>" + plot + displaydepth + displaytakenby + "</td>";
                                        transactions += "<td>" + dr["remarks"] + "</td>";
                                        transactions += "<td class=\"transaction_edit\">Edit</td>";
                                        transactions += "</tr>";

                                    }
                                    dr.Close();
                                }
                            }
                            catch (Exception ex)
                            {
                                throw ex;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        con.Close();
                        con.Dispose();
                    }
                }
            }
        }


        protected void btn_submit_Click(object sender, EventArgs e)
        {
            string estateid;

            SqlConnection con = new SqlConnection(strConnString);

            SqlCommand cmd = new SqlCommand("update_cemetery_person", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@accessid", SqlDbType.VarChar).Value = Request.Form["hf_id"];
            cmd.Parameters.Add("@warrant_no", SqlDbType.VarChar).Value = Request.Form["tb_warrant_no"];
            cmd.Parameters.Add("@register_no", SqlDbType.VarChar).Value = Request.Form["tb_register_no"];
            cmd.Parameters.Add("@surname", SqlDbType.VarChar).Value = Request.Form["tb_surname"];
            cmd.Parameters.Add("@givennames", SqlDbType.VarChar).Value = Request.Form["tb_givennames"];
            cmd.Parameters.Add("@lastpermanentaddress", SqlDbType.VarChar).Value = Request.Form["tb_lastpermanentaddress"];
            cmd.Parameters.Add("@city", SqlDbType.VarChar).Value = Request.Form["tb_city"];
            cmd.Parameters.Add("@placeofdeath", SqlDbType.VarChar).Value = Request.Form["tb_placeofdeath"];
            cmd.Parameters.Add("@dateofbirth", SqlDbType.VarChar).Value = Request.Form["tb_dateofbirth"];
            cmd.Parameters.Add("@age", SqlDbType.VarChar).Value = Request.Form["tb_age"];
            cmd.Parameters.Add("@age_period", SqlDbType.VarChar).Value = Request.Form["dd_ageperiod"];
            cmd.Parameters.Add("@gender", SqlDbType.VarChar).Value = Request.Form["dd_gender"];
            cmd.Parameters.Add("@denomination", SqlDbType.VarChar).Value = Request.Form["tb_denomination"];
            cmd.Parameters.Add("@minister", SqlDbType.VarChar).Value = Request.Form["tb_minister"];
            cmd.Parameters.Add("@dateofdeath", SqlDbType.VarChar).Value = Request.Form["tb_dateofdeath"];
            cmd.Parameters.Add("@dateofmedcert", SqlDbType.VarChar).Value = Request.Form["tb_dateofmedcert"];
            cmd.Parameters.Add("@rankoroccupation", SqlDbType.VarChar).Value = Request.Form["tb_rankoccupation"];
            cmd.Parameters.Add("@maritalstatus", SqlDbType.VarChar).Value = Request.Form["dd_maritalstatus"];
            cmd.Parameters.Add("@funeralcoordinator", SqlDbType.VarChar).Value = Request.Form["tb_funeralcoordinator"];
            cmd.Parameters.Add("@remarks", SqlDbType.VarChar).Value = Request.Form["tb_remarks"];
            cmd.Parameters.Add("@stillborn", SqlDbType.VarChar).Value = Request.Form["dd_stillborn"];

            cmd.Connection = con;

            try
            {
                con.Open();
                estateid = cmd.ExecuteScalar().ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
                con.Dispose();
            }




            foreach (string key in Request.Form)
            {
                if (key.StartsWith("hf_T_"))
                {
                    string transactionid = key.Substring(5);
                    string[] transaction_parts = Request.Form[key].Split('~');

                    /* val = $(this).attr("transactiontypeid") 0
                     * + '~' + $(this).attr("transactiondepth") 1
                     * + '~' + $(this).attr("transactiontakenby") 2
                     * + '~' + $(this).attr("transactionplot") 3 
                     * + '~' + $(this).attr("transaction_gisplot") 4
                     * + '~' + $(this).find('td').eq(0).text() 5 
                     * + '~' + $(this).find('td').eq(3).text(); 6
                     */
                    con = new SqlConnection(strConnString);

                    cmd = new SqlCommand("update_cemetery_person_transactions", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Clear();
                    cmd.Parameters.Add("@transactionid", SqlDbType.VarChar).Value = transactionid;
                    cmd.Parameters.Add("@estateid", SqlDbType.VarChar).Value = estateid;
                    cmd.Parameters.Add("@plotid", SqlDbType.VarChar).Value = transaction_parts[3];
                    cmd.Parameters.Add("@GISplotid", SqlDbType.VarChar).Value = transaction_parts[4];
                    cmd.Parameters.Add("@actiondate", SqlDbType.VarChar).Value = transaction_parts[5];
                    cmd.Parameters.Add("@remarks", SqlDbType.VarChar).Value = transaction_parts[6];
                    cmd.Parameters.Add("@depth", SqlDbType.VarChar).Value = transaction_parts[1];
                    cmd.Parameters.Add("@taken_by", SqlDbType.VarChar).Value = transaction_parts[2];
                    cmd.Parameters.Add("@transactiontypeid", SqlDbType.VarChar).Value = transaction_parts[0];

                    cmd.Connection = con;
                    try
                    {
                        con.Open();
                        //string response = cmd.ExecuteScalar().ToString();
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        con.Close();
                        con.Dispose();
                    }
                }
            }
            Response.Redirect("../default.aspx");
        }
    }
}

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

namespace Cemeteries.Administration.Archive
{
    public partial class Entry : System.Web.UI.Page
    {
        #region fields
        public string hf_id;
        public string warrant;
        public string burydate;
        public string dob;
        public string dod;
        public string fullname;
        public string age;
        public string residence;
        public string occupation;
        public string minister;
        public string director;
        //public string thearea;
        //public string theblock;
        //public string thediv;
        //public string theplot;
        public string remarks;
        public string dateentered;
        public string datechecked;
        public string[] ischecked = new string[1];
        public string actiontype;
        //public string cemetery;
        public string sextonreference;
        public string oldwarrant;
        //public string gisid;
        public string book;
        public string pageref;
        public string surname;
        public string forenames;
        public string dateupdated;
        public string gisoverride;
        public string inscription;

        public string burialdate;
        public string dateofbirth;
        public string dateofdeath;

        public Dictionary<string, string> yesno = new Dictionary<string, string>();

        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            hf_id = Request.QueryString["ID"];
            string mode = Request.QueryString["mode"];

            yesno.Add("Yes", "Yes");
            yesno.Add("No", "No");

            //List<BurialRecord> BurialRecordList = new List<BurialRecord>();

            String strConnString = ConfigurationManager.ConnectionStrings["Cemetery"].ConnectionString;
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
                    warrant = dr["warrant"].ToString();
                    burydate = Functions.formatdate(dr["burydate"].ToString(), "dd MMM yyyy");
                    dob = Functions.formatdate(dr["dob"].ToString(), "dd MMM yyyy");
                    dod = Functions.formatdate(dr["dod"].ToString(), "dd MMM yyyy");
                    fullname = dr["fullname"].ToString();
                    age = dr["age"].ToString();
                    residence = dr["residence"].ToString();
                    occupation = dr["occupation"].ToString();
                    minister = dr["minister"].ToString();
                    director = dr["director"].ToString();
                    //thearea = dr["thearea"].ToString();
                    //theblock = dr["theblock"].ToString();
                    //thediv = dr["thediv"].ToString();
                    //theplot = dr["theplot"].ToString();
                    remarks = dr["remarks"].ToString();
                    dateentered = dr["dateentered"].ToString();
                    datechecked = dr["datechecked"].ToString();
                    ischecked[0] = dr["ischecked"].ToString();
                    actiontype = dr["actiontype"].ToString();
                    //cemetery = dr["cemetery"].ToString();
                    sextonreference = dr["sextonreference"].ToString();
                    oldwarrant = dr["oldwarrant"].ToString();
                    //gisid = dr["gisid"].ToString();
                    book = dr["book"].ToString();
                    pageref = dr["pageref"].ToString();
                    surname = dr["surname"].ToString();
                    forenames = dr["forenames"].ToString();
                    dateupdated = dr["dateupdated"].ToString();
                    //gisoverride = dr["gisoverride"].ToString();
                    //inscription = dr["inscription"].ToString();
                    burialdate = Functions.formatdate(dr["burialdate"].ToString(),"dd MMM yyyy");
                    //dateofbirth = dr["dateofbirth"].ToString();
                    //dateofdeath = dr["dateofdeath"].ToString();




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

                    String imagepath = ConfigurationManager.AppSettings["Cemetery.ImageFolder"] + hf_id;
                    String imageurl = ConfigurationManager.AppSettings["Cemetery.ImageURL"] + hf_id + "/";

                    //lit_debug.Text += imagepath;
                    if (Directory.Exists(imagepath)) {
                        var files = Directory.EnumerateFiles(imagepath, "*.*", SearchOption.TopDirectoryOnly)
                            .Where(s => s.ToLower().EndsWith(".jpg") || s.ToLower().EndsWith(".jpeg"));

                        if(files.Count() > 0)
                        {
                            lit_tabs.Text += "<li><a data-toggle=\"tab\"href=\"#div_images\">Images</a></li>";
                            foreach (string currentFile in files)
                            {
                                lit_images.Text += "<img src=\"" + imageurl + Path.GetFileName(currentFile) + "\" />";
                            }
                        }

                    }
                    if(inscription != "")
                    {
                        lit_tabs.Text += "<li><a data-toggle=\"tab\"href=\"#div_inscription\">Inscription</a></li>";
                        lit_inscription.Text = inscription;
                    }

                    if (mode == "admin")
                    {
                        lit_tabs.Text += "<li><a data-toggle=\"tab\"href=\"#div_notes\">Notes</a></li>";
                        lit_tabs.Text += "<li><a data-toggle=\"tab\"href=\"#div_feedback\">Feedback</a></li>";
                    }
                    */
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

        protected void btn_submit_Click(object sender, EventArgs e)
        {

        }
    }
    /*
        public class BurialRecord
    {
        public string id;
public string warrant;
public string burydate;
public string dob;
public string dod;
public string fullname;
public string age;
public string residence;
public string occupation;
public string minister;
public string director;
public string thearea;
public string theblock;
public string thediv;
public string theplot;
public string remarks;
public string dateentered;
public string datechecked;
public string ischecked;
public string actiontype;
public string cemetery;
public string sextonreference;
public string oldwarrant;
public string gisid;
public string book;
public string pageref;
public string surname;
public string forenames;
public string dateupdated;
public string gisoverride;
public string inscription;
public string ;

    }
    */
}
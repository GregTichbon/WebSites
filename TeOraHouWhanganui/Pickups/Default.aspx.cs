using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;
using System.Data.SqlClient;
using System.Configuration;

using System.IO;
using System.Web.Configuration;

using System.Web.Script.Serialization;

namespace TeOraHouWhanganui.Pickups
{
    public partial class Default : System.Web.UI.Page
    {

        public string[] yesno_values = new string[2] { "Yes", "No" };
        //public string[] status_values = new string[7] { "Coming", "Not Coming", "No Response", "Call in", "Picked up", "Picked up from another address", "Called in - not coming" };
        //public string[] assignedto_values = new string[8] { "Jay", "Keith", "Jordi", "Greg", "Nate", "Watties", "Isa", "Judy"};

        public string status_values = "Coming,Not Coming,No Response,Call in,Picked up,Picked up from another address,Called in - not coming,Called in - not home,Called in - no response,Will make their own way,Made own way";
        public string assignedto_values = "Jay,Keith,Jordi,Greg,Chris,Zeana,Jessie,Chelsea,Nate,Watties,Judy,Charlie Boy,Keegan,Rebecca,Mahanga,Koralee,Aimee,Whakapakari,Driver 1,Driver 2,Driver 3";
        public string missingicons = "";

        public string formattedDate;

        public string passresult = "";

        public string EnrolementID = "";
        public string name = "";
        public string Status = "";
        public string OtherEnrolementID = "";
        public string PickupRunAddress = "";
        public string note = "";
        public string Assignedto = "";
        public string gender = "";
        public string EnrolementStatus = "";
        public string address = "";
        public Int32 version_ctr = 0;
        public string worker = "";
        public string whakapakari = "";



        //public string LastEnrolementID = "";
        //public string Lastname = "";
        //public string LastStatus = "";
        //public string LastOtherEnrolementID = "";
        //public string LastPickupRunAddress = "";
        //public string Lastnote = "";
        //public string LastAssignedto = "";
        //public string Lastgender = "";
        //public string LastEnrolementStatus = "";
        //public Int32 Lastversion_ctr = 0;

        public string html = "";

        public string Pickedup = "";

        public string selected = "";
        //public string name_addresses = "";
        //public string name_addresses_delim = "";
        public string thisstatus = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["pickups_loggedin"] == null)
                {
                    //Response.Redirect("login.aspx");
                }
                string icon;
                string delim = "";


                foreach (string assigned in assignedto_values.Split(','))
                {
                    icon = "http://whanganui.teorahou.org.nz/pickups/icons/" + assigned + ".png";
                    /*
                    if (Functions.Functions.isValidURL(icon) == false)
                    {
                        missingicons = missingicons + delim + assigned;
                        delim = ",";
                    }
                    */
                }

                string strConnString = "Data Source=toh-app;Initial Catalog=TeOraHou;Integrated Security=False;user id=OnlineServices;password=Whanganui497";
                SqlConnection con = new SqlConnection(strConnString);
                SqlCommand cmd = new SqlCommand("Get_Parameter", con);
                cmd.Parameters.Add("@pName", SqlDbType.VarChar).Value = "CurrentPickupDate";


                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = con;
                try
                {
                    con.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        dr.Read();
                        string DBDate = dr["pdate1"].ToString();
                        DateTime theDate;
                        if (DateTime.TryParse(DBDate, out theDate))
                        {
                            formattedDate = theDate.ToString("d MMM yyyy");
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

                var Personlist = new List<PersonClass>();

                //String strConnString = ConfigurationManager.ConnectionStrings["TOHWConnectionString"].ConnectionString;
                SqlConnection con2 = new SqlConnection(strConnString);
                SqlCommand cmd2 = new SqlCommand("Get_Pickups", con2);
                cmd2.CommandType = CommandType.StoredProcedure;
                cmd2.Parameters.Add("@program", SqlDbType.Int).Value = 1;
                cmd2.Parameters.Add("@debug", SqlDbType.VarChar).Value = Session["pickups_name"];

                cmd2.Connection = con2;
                try
                {
                    con2.Open();
                    SqlDataReader dr = cmd2.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            EnrolementID = dr["EnrolementID"].ToString();
                            name = dr["name"].ToString();
                            //PickupRunID = dr["PickupRunID"].ToString();
                            Status = dr["Status"].ToString();
                            //OtherEnrolementID = dr["OtherEnrolementID"].ToString();
                            PickupRunAddress = dr["PickupRunAddress"].ToString();
                            //UpdateAddress = dr["UpdateAddress"].ToString();
                            note = dr["note"].ToString();
                            Assignedto = dr["Assignedto"].ToString();
                            address = dr["address"].ToString();
                            gender = dr["Gender"].ToString();
                            EnrolementStatus = dr["EnrolementStatus"].ToString();
                            //Pickedup = dr["Pickedup"].ToString();
                            version_ctr = Convert.ToInt32(dr["version_ctr"]);
                            worker = dr["worker"].ToString();
                            whakapakari = dr["whakapakari"].ToString();
                            //if (address != "")
                            //{
                            //    name_addresses += name_addresses_delim + name + "~" + address;
                            //    name_addresses_delim = "|";
                            //}
                            var Addresslist = new List<AddressClass>();
                            foreach (var item in address.Split('|'))
                            {
                                Addresslist.Add(new AddressClass()
                                {
                                    address = item
                                });
                            };

                            Personlist.Add(new PersonClass()
                            {
                                enrollmentId = EnrolementID,
                                name = name,
                                status = Status,
                                otherEnrolementId = OtherEnrolementID,
                                pickupRunAddress = PickupRunAddress,
                                note = note,
                                assignedTo = Assignedto,
                                gender = gender,
                                enrolementStatus = EnrolementStatus,
                                version_ctr = version_ctr,
                                worker = worker,
                                whakapakari = whakapakari,
                                address = Addresslist
                            });





                            //if (EnrolementID != LastEnrolementID)
                            //{
                            //    if (LastEnrolementID != "")
                            //    {
                            //        Personlist.Add(new PersonClass()
                            //        {
                            //            enrollmentId = LastEnrolementID,
                            //            name = Lastname,
                            //            status = LastStatus ,
                            //            otherEnrolementId = LastOtherEnrolementID,
                            //            pickupRunAddress= LastPickupRunAddress,
                            //            note = Lastnote,
                            //            assignedTo = LastAssignedto,
                            //            gender = Lastgender,
                            //            enrolementStatus = LastEnrolementStatus,
                            //            version_ctr = Lastversion_ctr,
                            //            address = Addresslist
                            //        });
                            //        Addresslist.Clear();
                            //    }
                            //    LastEnrolementID = EnrolementID;
                            //    Lastname = name;
                            //    LastStatus = Status;
                            //    LastOtherEnrolementID = OtherEnrolementID;
                            //    LastPickupRunAddress = PickupRunAddress;
                            //    Lastnote = note;
                            //    LastAssignedto = Assignedto;
                            //    Lastgender = gender;
                            //    LastEnrolementStatus = EnrolementStatus;
                            //    Lastversion_ctr = version_ctr;

                            //}
                            //Addresslist.Add(new AddressClass()
                            //{
                            //    address = address
                            //});

                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    con2.Close();
                    con2.Dispose();
                }

                JavaScriptSerializer JS = new JavaScriptSerializer();
                passresult = JS.Serialize(Personlist);

                //Literal1.Text = passresult;

            }
        }

    }
    public class PersonClass
    {
        public string enrollmentId;
        public string name;
        public string status;
        public string otherEnrolementId;
        public string pickupRunAddress;
        public string note;
        public string assignedTo;
        public string gender;
        public string enrolementStatus;
        public Int32 version_ctr;
        public string worker;
        public string whakapakari;
        public List<AddressClass> address;
    }
    public class AddressClass
    {
        public string address;
    }
}

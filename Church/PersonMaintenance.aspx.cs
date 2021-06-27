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
using localfunctions = Church._Dependencies.myFuntions;
namespace Church
{
    public partial class PersonMaintenance : System.Web.UI.Page
    {
        public string person_ctr;
        public string fld_firstname;
        public string fld_surname;
        public string[] fld_persontype = new string[1];
        public string fld_greeting;
        public string fld_emailaddress;
        public string fld_address;
        public string fld_dateofbirth;
        public string fld_gender;

        public string fld_note;
        public string fld_xxxxxx;

        public string html_tab = "";

        public string html_attendance= "";
        public string html_notes = "";

        public Dictionary<string, string> options = new Dictionary<string, string>();
        public string[] nooptions = { };

        public Dictionary<string, string> persontypes = new Dictionary<string, string>();
        public Dictionary<string, string> YesNo = new Dictionary<string, string>();

        protected void Page_Load(object sender, EventArgs e)
        {



            if (!IsPostBack)
            {
                if (!localfunctions.AccessStringTest(""))
                {
                    Response.Redirect("/login.aspx");
                }

                person_ctr = Request.QueryString["id"] ?? "";
                if (person_ctr == "")
                {
                    Response.Redirect("personsearch.aspx");
                }

                ViewState["person_ctr"] = person_ctr;
                ViewState["returnto"] = Request.QueryString["returnto"] + "";

                YesNo.Add("Yes", "Yes");
                YesNo.Add("No", "No");

                string systemPrefix = WebConfigurationManager.AppSettings["systemPrefix"];
                String connectionString = ConfigurationManager.ConnectionStrings[systemPrefix + "ConnectionString"].ConnectionString;

                options.Clear();
                options.Add("storedprocedure", "");
                options.Add("storedprocedurename", "");
                options.Add("usevalues", "");
                //options.Add("insertblank", "start");
                persontypes = Functions.buildselectionlist(connectionString, "get_persontypes", options);


                if (person_ctr != "new")
                {
                    using (SqlConnection con = new SqlConnection(connectionString))
                    {
                        using (SqlCommand cmd = new SqlCommand("get_person", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add("@person_ctr", SqlDbType.VarChar).Value = person_ctr;
                            con.Open();
                            SqlDataReader dr = cmd.ExecuteReader();

                            if (dr.HasRows)
                            {
                                dr.Read();
                                //person_ctr = dr["person_ctr"].ToString();
                                fld_firstname = dr["firstname"].ToString();
                                fld_surname = dr["surname"].ToString();
                                fld_persontype[0] = dr["persontype_ctr"].ToString();
                                //fld_greeting = dr["greeting"].ToString();
                                //fld_emailaddress = dr["emailaddress"].ToString();
                                fld_gender = dr["gender"].ToString();
                                fld_dateofbirth = dr["dateofbirth"].ToString();
                                fld_note = dr["note"].ToString();
                            }
                            dr.Close();
                        }

                        #region ATTENDANCE
                        //-------------------------------ATTENDANCE TAB------------------------------------------------------

                        html_tab += "<li><a data-target=\"#div_attendance\">Attendance</a></li>";
                        html_attendance= "<thead>";
                        //html_attendance+= "<tr><th style=\"width:50px;text-align:center\"></th><th>Date</th><th>Reference</th><th>Type</th><th>Item Date</th><th>Grind</th><th>Quantity</th><th>Amount</th><th>Dispatched</th><th>Invoice</th><th>Note</th><th style=\"width:100px\">Action / <a class=\"orderedit\" data-mode=\"add\" href=\"javascript: void(0)\">Add</a></th></tr>";
                        html_attendance+= "<tr><th style=\"width:50px;text-align:center\"></th><th>Date</th><th>Reference</th><th>Type</th><th>Grind</th><th>Quantity</th><th>Amount</th><th>Dispatched</th><th>Batch</th><th>Invoice</th><th>From Subscription</th><th>Note</th><th style=\"width:100px\">Action / <a class=\"orderedit\" data-mode=\"add\" href=\"javascript: void(0)\">Add</a></th></tr>";
                        html_attendance+= "</thead>";
                        html_attendance+= "<tbody>";

                        //hidden row, used for creating new rows client side
                        html_attendance+= "<tr style=\"display:none\">";
                        html_attendance+= "<td style=\"text-align:center\"></td>";
                        html_attendance+= "<td></td>"; //Event
                        html_attendance+= "<td></td>"; //Attendance
                        html_attendance+= "<td></td>"; //Note
                        html_attendance+= "<td><a href=\"javascript:void(0)\" class=\"attendanceedit\" data-mode=\"edit\">Edit</td>";
                        html_attendance+= "</tr>";

                        using (SqlCommand cmd = new SqlCommand("get_person_attendance", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add("@person_ctr", SqlDbType.VarChar).Value = person_ctr;
                            SqlDataReader dr = cmd.ExecuteReader();

                            while (dr.Read())
                            {
                                string attendance_CTR = dr["attendance_ctr"].ToString();
                                //string date = Functions.formatdate(dr["date"].ToString(), "dd MMM yyyy");
                                string eventname = dr["event"].ToString();
                                string attendance = dr["attendance"].ToString();
                                string note = dr["note"].ToString();


                                html_attendance+= "<tr id=\"attendance_" + attendance_CTR + "\">";
                                html_attendance+= "<td style=\"text-align:center\"></td>";

                                html_attendance+= "<td>" + eventname + "</td>";
                                html_attendance+= "<td>" + attendance + "</td>";
                                html_attendance+= "<td>" + note + "</td>";

                                html_attendance+= "<td><a href=\"javascript:void(0)\" class=\"attendanceedit\" data-mode=\"edit\">Edit</td>";
                                html_attendance+= "</tr>";


                            }
                            dr.Close();
                        }

                        #endregion ATTENDANCE
                        #region NOTES
                      
                        //-------------------------------NOTES TAB------------------------------------------------------

                        html_tab += "<li><a data-target=\"#div_notes\">Notes</a></li>";
                        /*
                      html_notes = "<thead>";
                      html_notes += "<tr><th style=\"width:50px;text-align:center\"></th><th>Frequency</th><th>Period</th><th>Start Date</th><th>Last Order</th><th>Next Order</th><th>Coffee</th><th>Grind</th><th>Quantity</th><th>Amount</th><th>Drop Ship</th><th>Note</th><th style=\"width:100px\">Action / <a class=\"subscriptionedit\" data-mode=\"add\" href=\"javascript: void(0)\">Add</a></th></tr>";
                      html_notes += "</thead>";
                      html_notes += "<tbody>";

                      //hidden row, used for creating new rows client side
                      html_notes += "<tr style=\"display:none\">";
                      html_notes += "<td style=\"text-align:center\"></td>";
                      html_notes += "<td></td>"; //Frequency
                      html_notes += "<td></td>"; //Period
                      html_notes += "<td></td>"; //Start Date
                      html_notes += "<td></td>"; //Last Date
                      html_notes += "<td></td>"; //Next Date
                      html_notes += "<td></td>"; //Coffee
                      html_notes += "<td></td>"; //Grind
                      html_notes += "<td></td>"; //Quantity
                      html_notes += "<td></td>"; //Amount
                      html_notes += "<td></td>"; //Drop Ship
                      html_notes += "<td></td>"; //Note
                      html_notes += "<td><a href=\"javascript:void(0)\" class=\"subscriptionedit\" data-mode=\"edit\">Edit</td>";
                      html_notes += "</tr>";


                      using (SqlCommand cmd = new SqlCommand("get_person_subscriptions", con))
                      {
                          cmd.CommandType = CommandType.StoredProcedure;
                          cmd.Parameters.Add("@person_ctr", SqlDbType.VarChar).Value = person_ctr;
                          SqlDataReader dr = cmd.ExecuteReader();

                          while (dr.Read())
                          {
                              string subscription_CTR = dr["subscription_ctr"].ToString();
                              string frequency = dr["frequency"].ToString();
                              string period = dr["period"].ToString();
                              string startdate = Functions.formatdate(dr["startdate"].ToString(), "dd MMM yyyy");
                              string lastorder = Functions.formatdate(dr["lastorder"].ToString(), "dd MMM yyyy");
                              string nextorder = Functions.formatdate(dr["nextorder"].ToString(), "dd MMM yyyy");
                              string stockitem_ctr = dr["stockitem_ctr"].ToString();
                              string stockitem = dr["stockitem"].ToString();
                              string grind_ctr = dr["grind_ctr"].ToString();
                              string grind = dr["grind"].ToString();
                              string quantity = Convert.ToDecimal(dr["quantity"]).ToString("0.00");
                              string amount = Convert.ToDecimal(dr["amount"]).ToString("0.00");
                              string dropship = dr["dropship"].ToString();
                              string note = dr["note"].ToString();

                              html_notes += "<tr id=\"subscription_" + subscription_CTR + "\">";
                              html_notes += "<td style=\"text-align:center\"></td>";

                              html_notes += "<td>" + frequency + "</td>";
                              html_notes += "<td>" + period + "</td>";
                              html_notes += "<td>" + startdate + "</td>";
                              html_notes += "<td>" + lastorder + "</td>";
                              html_notes += "<td>" + nextorder + "</td>";
                              html_notes += "<td stockitem_ctr=\"" + stockitem_ctr + "\">" + stockitem + "</td>";
                              html_notes += "<td grind_ctr=\"" + grind_ctr + "\">" + grind + "</td>";
                              html_notes += "<td>" + quantity + "</td>";
                              html_notes += "<td>" + amount + "</td>";
                              html_notes += "<td>" + dropship + "</td>";
                              html_notes += "<td>" + note + "</td>";
                              html_notes += "<td><a href=\"javascript:void(0)\" class=\"subscriptionedit\" data-mode=\"edit\">Edit</td>";
                              html_notes += "</tr>";


                          }
                          dr.Close();
                      }
                      */
                        #endregion NOTES
                    }
                }
            }
        }

        protected void btn_submit_Click(object sender, EventArgs e)
        {
            Boolean Creating = false;
            string systemPrefix = WebConfigurationManager.AppSettings["systemPrefix"];
            String connectionString = ConfigurationManager.ConnectionStrings[systemPrefix + "ConnectionString"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("Update_person", con))
                {

                    cmd.CommandType = CommandType.StoredProcedure;

                    person_ctr = ViewState["person_ctr"].ToString();
                    if (person_ctr == "new")
                    {
                        Creating = true;
                    }

                    //cmd.CommandText = "Update_person";
                    cmd.Parameters.Add("@person_ctr", SqlDbType.VarChar).Value = person_ctr;
                    cmd.Parameters.Add("@persontype_ctr", SqlDbType.VarChar).Value = Request.Form["fld_persontype"].Trim();
                    cmd.Parameters.Add("@firstname", SqlDbType.VarChar).Value = Request.Form["fld_firstname"].Trim();
                    cmd.Parameters.Add("@surname", SqlDbType.VarChar).Value = Request.Form["fld_surname"].Trim();
                    cmd.Parameters.Add("@greeting", SqlDbType.VarChar).Value = Request.Form["fld_greeting"].Trim();
                    cmd.Parameters.Add("@emailaddress", SqlDbType.VarChar).Value = Request.Form["fld_emailaddress"].Trim();
                    cmd.Parameters.Add("@address", SqlDbType.VarChar).Value = Request.Form["fld_address"].Trim();
                    cmd.Parameters.Add("@note", SqlDbType.VarChar).Value = Request.Form["fld_note"].Trim();

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
                }

                /*
                foreach (string key in Request.Form)
                {
                    if (key.StartsWith("order_"))
                    {
                        int keylength = "order_".Length;
                        string order_ctr = key.Substring(keylength);
                        if (order_ctr.EndsWith("_delete"))
                        {
                            using (SqlCommand cmd = new SqlCommand("Delete_Order", con))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.Add("@order_ctr", SqlDbType.VarChar).Value = order_ctr.Substring(0, order_ctr.Length - 7);
                                con.Open();
                                cmd.ExecuteScalar();
                                con.Close();
                            }
                        }
                        else
                        {
                            if (order_ctr.StartsWith("new"))
                            {
                                order_ctr = "new";
                            }
                            using (SqlCommand cmd = new SqlCommand("Update_Order", con))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.Add("@order_ctr", SqlDbType.VarChar).Value = order_ctr;
                                cmd.Parameters.Add("@person_ctr", SqlDbType.VarChar).Value = person_ctr;
                                string[] valuesSplit = Request.Form[key].Split('\x00FE');
                                cmd.Parameters.Add("@date", SqlDbType.VarChar).Value = valuesSplit[0];
                                cmd.Parameters.Add("@reference", SqlDbType.VarChar).Value = valuesSplit[1];
                                cmd.Parameters.Add("@stockitem_ctr", SqlDbType.VarChar).Value = valuesSplit[2];
                                cmd.Parameters.Add("@grind_ctr", SqlDbType.VarChar).Value = valuesSplit[3];
                                cmd.Parameters.Add("@quantity", SqlDbType.VarChar).Value = valuesSplit[4];
                                cmd.Parameters.Add("@amount", SqlDbType.VarChar).Value = valuesSplit[5];
                                cmd.Parameters.Add("@deliveredDate", SqlDbType.VarChar).Value = valuesSplit[6];
                                cmd.Parameters.Add("@stockitembatch_ctr", SqlDbType.VarChar).Value = valuesSplit[7];
                                cmd.Parameters.Add("@invoicereference", SqlDbType.VarChar).Value = valuesSplit[8];
                                cmd.Parameters.Add("@note", SqlDbType.VarChar).Value = valuesSplit[9];
                                con.Open();
                                cmd.ExecuteScalar();
                                con.Close();
                            }
                        }
                    }
                    else if (key.StartsWith("subscription_"))
                    {
                        int keylength = "subscription_".Length;
                        string subscription_ctr = key.Substring(keylength);
                        if (subscription_ctr.EndsWith("_delete"))
                        {
                            using (SqlCommand cmd = new SqlCommand("Delete_subscription", con))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.Add("@subscription_ctr", SqlDbType.VarChar).Value = subscription_ctr.Substring(0, subscription_ctr.Length - 7);
                                con.Open();
                                cmd.ExecuteScalar();
                                con.Close();
                            }
                        }
                        else
                        {
                            if (subscription_ctr.StartsWith("new"))
                            {
                                subscription_ctr = "new";
                            }
                            using (SqlCommand cmd = new SqlCommand("Update_subscription", con))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.Add("@subscription_ctr", SqlDbType.VarChar).Value = subscription_ctr;
                                cmd.Parameters.Add("@person_ctr", SqlDbType.VarChar).Value = person_ctr;
                                string[] valuesSplit = Request.Form[key].Split('\x00FE');
                                cmd.Parameters.Add("@frequency", SqlDbType.VarChar).Value = valuesSplit[0];
                                cmd.Parameters.Add("@period", SqlDbType.VarChar).Value = valuesSplit[1];
                                cmd.Parameters.Add("@startdate", SqlDbType.VarChar).Value = valuesSplit[2];
                                cmd.Parameters.Add("@lastorder", SqlDbType.VarChar).Value = valuesSplit[3];
                                //cmd.Parameters.Add("@nextorder", SqlDbType.VarChar).Value = valuesSplit[4];
                                cmd.Parameters.Add("@stockitem_ctr", SqlDbType.VarChar).Value = valuesSplit[4];
                                cmd.Parameters.Add("@grind_ctr", SqlDbType.VarChar).Value = valuesSplit[5];
                                cmd.Parameters.Add("@quantity", SqlDbType.VarChar).Value = valuesSplit[6];
                                cmd.Parameters.Add("@amount", SqlDbType.VarChar).Value = valuesSplit[7];
                                cmd.Parameters.Add("@dropship", SqlDbType.VarChar).Value = valuesSplit[8];
                                cmd.Parameters.Add("@note", SqlDbType.VarChar).Value = valuesSplit[9];
                                con.Open();
                                cmd.ExecuteScalar();
                                con.Close();
                            }
                        }
                    }
                }
                */
            }
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
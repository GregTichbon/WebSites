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

namespace TeOraHouWhanganui.Private
{
    public partial class Vehicles : System.Web.UI.Page
    {
        public string username = "";
        public string html = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            string sql = "get_vehicles";

            string systemPrefix = WebConfigurationManager.AppSettings["systemPrefix"];
            String connectionString = ConfigurationManager.ConnectionStrings[systemPrefix + "ConnectionString"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;


                    con.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    html = "<table class=\"table\"><thead><tr><th style=\"text-align: right\">Sequence</th><th>Registration</th><th>Name</th><th>Description</th><th>WOF Due</th><th>Registration Due</th><th>Start Date</th><th>End Date</th><th>Note</th><th>Alert</th><th><a class=\"add\">Add</a></tr></thead><tbody>";
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            string vehicle_ctr = dr["vehicle_ctr"].ToString();
                            string registration = dr["registration"].ToString();
                            string Name = dr["Name"].ToString();
                            string description = dr["description"].ToString();
                            string wofcycle = dr["wofcycle"].ToString();
                            string wofdue = Functions.formatdate(dr["wofdue"].ToString(), "d MMM yyyy");
                            string registrationdue = Functions.formatdate(dr["registrationdue"].ToString(), "d MMM yyyy");
                            string startdate = Functions.formatdate(dr["startdate"].ToString(), "d MMM yyyy");
                            string enddate = Functions.formatdate(dr["enddate"].ToString(), "d MMM yyyy");
                            string note = dr["note"].ToString();
                            string alert = dr["alert"].ToString();
                            string Sequence = dr["sequence"].ToString();


                            string wofclass = "";
                            if(wofdue != "" && (Convert.ToDateTime(wofdue) - DateTime.Today).Days < 14)
                            {
                                wofclass = " highlight";
                            }
                            string registrationclass = "";
                            if (registrationdue != "" && (Convert.ToDateTime(registrationdue) - DateTime.Today).Days < 14)
                            {
                                registrationclass = " highlight";
                            }


                            html += "<tr data-id=\"" + vehicle_ctr + "\">";
                            html += "<td class=\"sorter\"><input id=\"sequence_" + vehicle_ctr + "\" name=\"sequence_" + vehicle_ctr + "\" type=\"hidden\" value=\"" + Sequence + "\" /></td>";
                            html += "<td><input class=\"form-control fld_registration\" id=\"registration_" + vehicle_ctr + "\" name=\"registration_" + vehicle_ctr + "\" value=\"" + registration + "\" maxlength=\"6\" /></td>";
                            html += "<td><input class=\"form-control fld_name\" id=\"name_" + vehicle_ctr + "\" name=\"name_" + vehicle_ctr + "\" value=\"" + Name + "\" /></td>";
                            string descriptionDisplay = description;
                            if (descriptionDisplay.Length > 200)
                            {
                                descriptionDisplay = descriptionDisplay.Substring(0, 200) + "<b>...</b>";
                            }
                            html += "<td class=\"editcontent\" data-type=\"description\" data-id=\"" + vehicle_ctr + "\"><input id=\"description_" + vehicle_ctr + "\" name=\"description_" + vehicle_ctr + "\" type=\"hidden\" value=\"" + description + "\"><span id=\"spandescription_" + vehicle_ctr + "\">" + descriptionDisplay + "</span></td>";
                           
                            html += "<td><input class=\"form-control date fld_wofdue" + wofclass + "\" id=\"wofdue_" + vehicle_ctr + "\" name=\"wofdue_" + vehicle_ctr + "\" value=\"" + wofdue + "\"></td>";
                            html += "<td><input class=\"form-control date fld_registrationdue" + registrationclass +"\" id=\"registrationdue_" + vehicle_ctr + "\" name=\"registrationdue_" + vehicle_ctr + "\" value=\"" + registrationdue + "\"></td>";

                            html += "<td><input class=\"form-control date fld_startdate\" id=\"startdate_" + vehicle_ctr + "\" name=\"startdate_" + vehicle_ctr + "\" value=\"" + startdate + "\"></td>";
                            html += "<td><input class=\"form-control date fld_enddate\" id=\"enddate_" + vehicle_ctr + "\" name=\"enddate_" + vehicle_ctr + "\" value=\"" + enddate + "\"></td>";
                            string noteDisplay = note;
                            if (noteDisplay.Length > 200)
                            {
                                noteDisplay = noteDisplay.Substring(0, 200) + "<b>...</b>";
                            }
                            html += "<td class=\"editcontent\" data-type=\"note\" data-id=\"" + vehicle_ctr + "\"><input id=\"note_" + vehicle_ctr + "\" name=\"note_" + vehicle_ctr + "\" type=\"hidden\" value=\"" + note + "\"><span id=\"spannote_" + vehicle_ctr + "\">" + noteDisplay + "</span></td>";
                            string alertDisplay = alert;
                            if (alertDisplay.Length > 200)
                            {
                                alertDisplay = alertDisplay.Substring(0, 200) + "<b>...</b>";
                            }
                            html += "<td colspan=\"2\" class=\"editcontent\" data-type=\"alert\" data-id=\"" + vehicle_ctr + "\"><input id=\"alert_" + vehicle_ctr + "\" name=\"alert_" + vehicle_ctr + "\" type=\"hidden\" value=\"" + alert + "\"><span id=\"spanalert_" + vehicle_ctr + "\">" + alertDisplay + "</span></td>";

                            //html += "<td><a><img alt=\"Add Below\" class=\"icon add\" src=\"/images/plus.png\"></a> <a><img id=\"remove_" + vehicle_ctr + "\" alt=\"Remove\" class=\"icon remove\" src=\"/images/cross.png\"></a></td>";
                            html += "</tr>";

                        }
                    }
                    html += "</tbody></table>";
                }


            }

        }

        protected void btn_submit_Click(object sender, EventArgs e)
        {
            string systemPrefix = WebConfigurationManager.AppSettings["systemPrefix"];
            String connectionString = ConfigurationManager.ConnectionStrings[systemPrefix + "ConnectionString"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("update_vehicle", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    foreach (string key in Request.Form)
                    {
                        if (key.StartsWith("sequence_"))
                        {
                            string id = key.Substring(9);
                            Response.Write(key + ", " + Request.Form[key]);
                            cmd.Parameters.Clear();

                            cmd.Parameters.Add("@vehicle_ctr", SqlDbType.VarChar).Value = id;
                            cmd.Parameters.Add("@registration", SqlDbType.VarChar).Value = Request.Form["registration_" + id] ?? "";
                            cmd.Parameters.Add("@Name", SqlDbType.VarChar).Value = Request.Form["name_" + id] ?? "";
                            cmd.Parameters.Add("@description", SqlDbType.VarChar).Value = Request.Form["description_" + id] ?? "";
                            cmd.Parameters.Add("@wofcycle", SqlDbType.VarChar).Value = "";
                            cmd.Parameters.Add("@wofdue", SqlDbType.VarChar).Value = Request.Form["wofdue_" + id] ?? "";
                            cmd.Parameters.Add("@registrationdue", SqlDbType.VarChar).Value = Request.Form["registrationdue_" + id] ?? "";
                            cmd.Parameters.Add("@startdate", SqlDbType.VarChar).Value = Request.Form["startdate_" + id] ?? "";
                            cmd.Parameters.Add("@enddate", SqlDbType.VarChar).Value = Request.Form["enddate_" + id] ?? "";
                            cmd.Parameters.Add("@note", SqlDbType.VarChar).Value = Request.Form["note_" + id] ?? "";
                            cmd.Parameters.Add("@alert", SqlDbType.VarChar).Value = Request.Form["alert_" + id] ?? "";
                            cmd.Parameters.Add("@Sequence", SqlDbType.VarChar).Value = Request.Form["sequence_" + id] ?? "";


                            cmd.ExecuteNonQuery();
                        }
                    }


                    Response.Redirect(Request.RawUrl);
                }
            }
        }
    }
}
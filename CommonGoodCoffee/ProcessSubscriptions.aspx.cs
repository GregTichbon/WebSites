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

namespace CommonGoodCoffee
{
    public partial class ProcessSubscriptions : System.Web.UI.Page
    {
        public string html = "";

        protected void Page_Load(object sender, EventArgs e)
        {

            /*
            0 = Heading
            1 = Format
            2 = Class
            3 = Link
            4 = Link key
            */
            if (!Page.IsPostBack)
            {
                string uptodate = (string)Session["uptodate"] ?? "";
                fld_uptodate.Text = uptodate;
                if (uptodate != "")
                {
                    Session["uptodate"] = "";
                    fld_uptodate.ReadOnly = true;

                    string systemPrefix = WebConfigurationManager.AppSettings["systemPrefix"];
                    String connectionString = ConfigurationManager.ConnectionStrings[systemPrefix + "ConnectionString"].ConnectionString;

                    using (SqlConnection con = new SqlConnection(connectionString))
                    using (SqlCommand cmd = new SqlCommand("Get_Subscriptions_Due", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@uptodate", SqlDbType.VarChar).Value = uptodate;
                        con.Open();
                        SqlDataReader dr = cmd.ExecuteReader();

                        if (dr.HasRows)
                        {
                            string[,] parameters = new string[5, dr.FieldCount];
                            html += "<table class=\"table\"><thead><tr>";
                            for (int f1 = 0; f1 <= dr.FieldCount - 1; f1++)
                            {
                                string[] theseparameters = dr.GetName(f1).ToString().Split('|');
                                for (int f2 = 0; f2 <= theseparameters.Length - 1; f2++)
                                {
                                    parameters[f2, f1] = theseparameters[f2];
                                }
                                if (parameters[0, f1] != "")
                                {
                                    html += "<th>";
                                    html += parameters[0, f1].Replace("_", " ");
                                    html += "</th>";
                                }
                            }
                            html += "<th>Create</th>";
                            html += "</tr></thead><tbody>";

                            while (dr.Read())
                            {
                                html += "<tr>";
                                for (int f1 = 0; f1 <= dr.FieldCount - 1; f1++)
                                {
                                    string useclass = "";
                                    if (parameters[0, f1] != "")
                                    {
                                        if (parameters[2, f1] + "" != "")
                                        {
                                            useclass = " class=\"" + parameters[2, f1] + "\"";
                                        }
                                        html += "<td" + useclass + ">";

                                        string val = dr[f1].ToString();
                                        if (parameters[1, f1] == "Date")
                                        {
                                            val = Functions.formatdate(val, "dd/MM/yy");
                                        }
                                        else if (parameters[1, f1] == "Email")
                                        {
                                            val = "<a href=\"mailto:" + val + "\">" + val + "</a>";
                                        }


                                        if (parameters[3, f1] + "" != "")
                                        {
                                            int fld = Convert.ToInt32(parameters[4, f1]);
                                            html += "<a href=\"" + parameters[3, f1] + dr[fld].ToString() + "\">" + val + "</a>";
                                        }
                                        else
                                        {
                                            html += val;
                                        }



                                        html += "</td>";
                                    }
                                }

                                html += "<td><input type=\"checkbox\" name=\"fld_process\" value=\"" + dr[0].ToString() + "\"></td>";
                                html += "</tr>";

                            }
                            html += "</tbody></table>";
                        }
                        dr.Close();
                        con.Close();
                    }
                }
            }
        }

        protected void btn_submit_Click(object sender, EventArgs e)
        {

            Session["uptodate"] = fld_uptodate.Text;
            string fld_process = Request.Form["fld_process"] + "";
            if (fld_process != "")
            {
                string[] ids = Request.Form["fld_process"].Split(',');

                string systemPrefix = WebConfigurationManager.AppSettings["systemPrefix"];
                String connectionString = ConfigurationManager.ConnectionStrings[systemPrefix + "ConnectionString"].ConnectionString;

                using (SqlConnection con = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand("Process_Subscription", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();

                    foreach (string id in ids)
                    {
                        cmd.Parameters.Clear();
                        cmd.Parameters.Add("@uptodate", SqlDbType.VarChar).Value = fld_uptodate.Text;
                        cmd.Parameters.Add("@subscription_ctr", SqlDbType.VarChar).Value = id;
                        cmd.ExecuteScalar();
                    }
                }

            }
            Response.Redirect(Request.RawUrl);
        }

        protected void btn_cancel_Click(object sender, EventArgs e)
        {
            Session["uptodate"] = "";
            Response.Redirect(Request.RawUrl);
        }
    }
}
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
using localfunctions = CommonGoodCoffee._Dependencies.myFuntions;

namespace CommonGoodCoffee.Reports
{
    public partial class Report : System.Web.UI.Page
    {
        public string heading = "";
        public string html = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!localfunctions.AccessStringTest(""))
            {
                Response.Redirect("/login.aspx");
            }

            /*
            0 = Heading
            1 = Format
            2 = Class
            3 = Link
            4 = Link key
            */

            string id = Request.QueryString["id"] + "";
            if (id != "")
            {
                string systemPrefix = WebConfigurationManager.AppSettings["systemPrefix"];
                String connectionString = ConfigurationManager.ConnectionStrings[systemPrefix + "ConnectionString"].ConnectionString;

                using (SqlConnection con = new SqlConnection(connectionString + ";MultipleActiveResultSets=true"))
                using (SqlCommand cmd = new SqlCommand("reports", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@id", SqlDbType.VarChar).Value = id;

                    SqlDataAdapter da = new SqlDataAdapter();
                    DataSet ds = new DataSet();

                    da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                    cmd.Dispose();

                    foreach (DataTable thistable in ds.Tables)
                    {
                        if (thistable.Rows.Count > 0)
                        {
                            int x = thistable.Columns.Count;
                            string z = thistable.Columns[0].ColumnName;
 
                            if (thistable.Columns[0].ColumnName == "Heading")
                            {
                                heading = thistable.Rows[0]["Heading"].ToString();
                            }
                            else
                            {
                                string[,] parameters = new string[5, thistable.Columns.Count];
                                html += "<table class=\"table\"><thead><tr>";
                                for (int f1 = 0; f1 <= thistable.Columns.Count - 1; f1++)
                                {
                                    string[] theseparameters = thistable.Columns[f1].ColumnName.Split('|');
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
                                html += "</tr></thead><tbody>";
                                foreach (DataRow dr in thistable.Rows)
                                {
                                    html += "<tr>";
                                    for (int f1 = 0; f1 <= thistable.Columns.Count - 1; f1++)
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
                                    html += "</tr>";

                                }
                                html += "</tbody></table>";
                            }
                        }
                    }
                }
            }
        }
    }
}

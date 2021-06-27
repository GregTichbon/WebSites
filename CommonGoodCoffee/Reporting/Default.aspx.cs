using Generic;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using localfunctions = CommonGoodCoffee._Dependencies.myFuntions;

namespace CommonGoodCoffee.Reports
{
    public partial class Default : System.Web.UI.Page
    {

        public string html = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!localfunctions.AccessStringTest(""))
            {
                Response.Redirect("/login.aspx");
            }


            string systemPrefix = WebConfigurationManager.AppSettings["systemPrefix"];
            String connectionString = ConfigurationManager.ConnectionStrings[systemPrefix + "ConnectionString"].ConnectionString + ";MultipleActiveResultSets=true";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("build_menu", con))
                {
                    cmd.Parameters.Add("@menuname", SqlDbType.VarChar).Value = "Reports";
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataReader dr = cmd.ExecuteReader();

                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            string link = dr["link"].ToString();
                            string label = dr["label"].ToString();


                            Regex rx = new Regex(@"@@.*?@@");
                            foreach (Match match in rx.Matches(label))
                            {
                                string mv = match.Value;
                                string sql = mv.Substring(2, mv.Length - 4);
                                using (SqlCommand cmd1 = new SqlCommand(sql, con))
                                {
                                    cmd1.CommandType = CommandType.Text;
                                    string response = cmd1.ExecuteScalar().ToString();

                                    label = label.Replace(mv, response);
                                }

                            }
                            html += "<p><a href=\"" + link + "\" class=\"btn btn-info\">" + label + "</a></p>";
                        }
                        dr.Close();
                    }
                }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Generic;

namespace TeOraHouWhanganui.Private.Administration
{
    public partial class Access : System.Web.UI.Page
    {
        public string username = "";
        public string html = "";
        public Dictionary<string, string> options = new Dictionary<string, string>();
        public string[] nooptions = { };
        public Dictionary<string, string> access = new Dictionary<string, string>();
        protected void Page_Load(object sender, EventArgs e)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["TOHWConnectionString"].ConnectionString;

            options.Clear();
            options.Add("storedprocedure", "");
            options.Add("storedprocedurename", "");
            options.Add("usevalues", "");
            //options.Add("insertblank", "start");
            access = Functions.buildselectionlist(connectionString, "get_access", options);

            html = "<thead>";
            html += "<tr><th style=\"width:50px;text-align:center\"></th><th>Person</th><th>Access</th><th>Note</th><th style=\"width:100px\">Action / <a class=\"edit\" data-mode=\"add\" href=\"javascript: void(0)\">Add</a></th></tr>";
            html += "</thead>";
            html += "<tbody>";

            //hidden row, used for creating new rows client side
            html += "<tr style=\"display:none\">";
            html += "<td style=\"text-align:center\"></td>";
            html += "<td></td>";
            html += "<td></td>";
            html += "<td></td>";
            html += "<td><a href=\"javascript:void(0)\" class=\"edit\" data-mode=\"edit\">Edit</td>";
            html += "</tr>";

            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("Get_Person_Access", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.Add("@reportname", SqlDbType.VarChar).Value = reportname;
                con.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
 
                        while (dr.Read())
                        {
                        string Person_Access_CTR = dr["Person_Access_CTR"].ToString();
                        string Person_CTR = dr["Person_CTR"].ToString();
                        string Access = dr["Access"].ToString();
                        string Person = dr["Person"].ToString();
                        string Description = dr["Description"].ToString();
                        string Disallow = dr["Disallow"].ToString();
                        string note = dr["note"].ToString();
                        string accessnote = dr["accessnote"].ToString();
                        if(accessnote != "")
                        {
                            accessnote = " (" + accessnote + ")";
                        }

                        html += "<tr id=\"id_" + Person_Access_CTR + "\">";
                        html += "<td style=\"text-align:center\"></td>";
                        html += "<td person_CTR=\"" + Person_CTR + "\">" + Person + "</td>";
                        html += "<td access=\"" + Access + "\">" + Description + accessnote + "</td>";
                        html += "<td>" + note + "</td>";
                        html += "<td><a href=\"javascript:void(0)\" class=\"edit\" data-mode=\"edit\">Edit</td>";
                        html += "</tr>";
                    }

                }
            }
        }

        protected void btn_submit_Click(object sender, EventArgs e)
        {
            string strConnString = ConfigurationManager.ConnectionStrings["TOHWConnectionString"].ConnectionString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = con;

            foreach (string key in Request.Form)
            {
                if (!key.StartsWith("__") && !key.StartsWith("ctl"))
                {
                    string person_access_ctr = key.Substring(3);  //key length 
                    if (person_access_ctr.EndsWith("_delete"))
                    {
                        cmd.CommandText = "Delete_person_access";
                        cmd.Parameters.Clear();
                        cmd.Parameters.Add("@username", SqlDbType.VarChar).Value = username;
                        cmd.Parameters.Add("@person_access_ctr", SqlDbType.VarChar).Value = person_access_ctr.Substring(0, person_access_ctr.Length - 7);
                    }
                    else
                    {
                        if (person_access_ctr.StartsWith("new"))
                        {
                            person_access_ctr = "new";
                        }

                        string[] valuesSplit = Request.Form[key].Split('\x00FE');
                        cmd.CommandText = "Update_person_access";
                        cmd.Parameters.Clear();
                        cmd.Parameters.Add("@username", SqlDbType.VarChar).Value = username;
                        cmd.Parameters.Add("@person_access_ctr", SqlDbType.VarChar).Value = person_access_ctr;
                        cmd.Parameters.Add("@person_ctr", SqlDbType.VarChar).Value = valuesSplit[0];
                        cmd.Parameters.Add("@access", SqlDbType.VarChar).Value = valuesSplit[1];
                        cmd.Parameters.Add("@note", SqlDbType.VarChar).Value = valuesSplit[2];
                    }
                    con.Open();
                    cmd.ExecuteScalar().ToString();
                    con.Close();
                }
            }
            Response.Redirect("Access.aspx");
        }
    }
}

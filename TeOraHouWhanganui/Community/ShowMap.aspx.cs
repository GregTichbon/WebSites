using Generic;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace TeOraHouWhanganui.Community
{
    public partial class ShowMap : System.Web.UI.Page
    {
        public string xml;
        protected void Page_Load(object sender, EventArgs e)
        {
            string systemPrefix = "Community"; // WebConfigurationManager.AppSettings["systemPrefix"];
                                               //string systemPrefix = WebConfigurationManager.AppSettings["systemPrefix"];
            String connectionString = ConfigurationManager.ConnectionStrings[systemPrefix + "ConnectionString"].ConnectionString;
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "mapData";
            cmd.Connection = con;
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                dr.Read();
                xml = dr[0].ToString();
            }
            dr.Close();

            cmd.Dispose();
            con.Close();
            con.Dispose();

        }
    }
}
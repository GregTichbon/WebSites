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

namespace TeOraHouWhanganui.Community
{
    public partial class Search : System.Web.UI.Page
    {
        public Dictionary<string, string> options = new Dictionary<string, string>();
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btn_geocode_Click(object sender, EventArgs e)
        {
            

            string systemPrefix = "Community"; // WebConfigurationManager.AppSettings["systemPrefix"];
                                               //string systemPrefix = WebConfigurationManager.AppSettings["systemPrefix"];
            String connectionString = ConfigurationManager.ConnectionStrings[systemPrefix + "ConnectionString"].ConnectionString + ";MultipleActiveResultSets=true";


            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select location_ctr, location from location where isnull(lng,'') = ''";

            cmd.Connection = con;
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                string location_ctr = dr["location_ctr"].ToString();
                string address = dr["location"].ToString();

                options.Clear();
                google_geocodeClass google_geocode = Functions.google_geocode("AIzaSyCCpsWhkuuHlAe6EKhSi5zSlmmIVMN9M8c", address, options);
                SqlCommand cmd1 = new SqlCommand();
                cmd1.CommandType = CommandType.Text;
                cmd1.CommandText = "update location set lng = '" + google_geocode.lng + "', lat='" + google_geocode.lat + "' where location_ctr = " + location_ctr;
                cmd1.Connection = con;
                cmd1.ExecuteNonQuery();
                cmd1.Dispose();

            }
            dr.Close();
            cmd.Dispose();
            con.Close();
            con.Dispose();
        }
    }
}
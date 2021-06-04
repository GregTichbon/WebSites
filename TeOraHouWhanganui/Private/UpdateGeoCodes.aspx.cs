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

namespace TeOraHouWhanganui.Private
{
    public partial class UpdateGeoCodes : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string address = "30 Totara Street, Whanganui";
            string sql = @"select id, Address from Address where isnull([Address],'') <> '' and Longitude is null ";
            Dictionary<string, string> options = new Dictionary<string, string>();

            string connectionString = ConfigurationManager.ConnectionStrings["TOHWConnectionString"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(sql, con))
            {
                cmd.CommandType = CommandType.Text;
                //cmd.Parameters.Add("@reportname", SqlDbType.VarChar).Value = reportname;
                con.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {

                    while (dr.Read())
                    {
                        address = dr["address"].ToString();
                        if (!address.ToLower().Contains("whanganui"))
                        {
                            {
                                address += ", Whanganui";
                                //google_geocodeClass google_geocode = Functions.google_geocode("AIzaSyCCpsWhkuuHlAe6EKhSi5zSlmmIVMN9M8c", address, options);

                                //string lat = google_geocode.lat;
                                //string lng = google_geocode.lng;
                            }
                        }
                    }
                }
            }
        }
    }
}
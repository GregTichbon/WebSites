using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TeOraHouWhanganui.TestAndPlay
{
    public partial class LongRunning1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btn_submit_Click(object sender, EventArgs e)
        {
            int seconds = Convert.ToInt32(fld_seconds.Text);
            //System.Threading.Thread.Sleep(seconds);

            String connectionString = "Data Source=toh-app;Initial Catalog=DataInnovations;Integrated Security=False;user id=OnlineServices;password=Whanganui497";
            SqlConnection con = new SqlConnection(connectionString);

            SqlCommand cmd = new SqlCommand("timeoutTest", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@seconds", SqlDbType.Int).Value = seconds;

            cmd.CommandTimeout = 600;

            cmd.Connection = con;
            con.Open();
            int x = con.ConnectionTimeout;

            cmd.ExecuteNonQuery();
            cmd.Dispose();
            con.Close();
            con.Dispose();


        }
    }
}
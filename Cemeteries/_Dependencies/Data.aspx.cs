using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Cemeteries._Dependencies
{
    public partial class Data : System.Web.UI.Page
    {
        public string html = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            string mode = Request.QueryString["mode"];

            switch (mode)
            {
                case "cemetery_search":
                    html = cemeterysearch();
                    break;
               
            }
        }

        public string cemeterysearch()
        {
            string html = "";
            //string surname = Request.QueryString["surname"] ?? "";
            //string forenames = Request.QueryString["forenames"] ?? "";

            string surname = Server.UrlDecode(Request.Unvalidated["surname"]);
            string forenames = Server.UrlDecode(Request.Unvalidated["forenames"]);


            String strConnString = ConfigurationManager.ConnectionStrings["Cemetery"].ConnectionString;

            using (SqlConnection con = new SqlConnection(strConnString))
            using (SqlCommand cmd = new SqlCommand("cemetery_search", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@surname", SqlDbType.VarChar).Value = surname;
                cmd.Parameters.Add("@forenames", SqlDbType.VarChar).Value = forenames;
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    html += "<table id=\"searchtable\" class=\"table table-striped table-responsive\"><tbody>";

                    while (dr.Read())
                    {

                        string Name = dr["Name"].ToString();
                        //string Age = dr["Age"].ToString();
                        string Date_of_Death = dr["Date_of_Death"].ToString();
                        string Gender = dr["Gender"].ToString();

                        //string Warrant = dr["Warrant"].ToString();
                        string Date_of_Burial = dr["Date_of_Burial"].ToString();
                        string Date_of_Birth = dr["Date_of_Birth"].ToString();
                        //string Residence = dr["Residence"].ToString();
                        //string Occupation = dr["Occupation"].ToString();
                        //string Minister = dr["Minister"].ToString();
                        //string Director = dr["Director"].ToString();

                        //string Date_of_Medical_Certificate = dr["Date_of_Medical_Certificate"].ToString();
                        //string Marital_Status = dr["Marital_Status"].ToString();
                        string Place_of_Death = dr["Place_of_Death"].ToString();
                        //string Register_No = dr["Register_No"].ToString();

                        string Stillborn = dr["Stillborn"].ToString();
                        //string Location = dr["Location"].ToString();
                        string AccessID = dr["accessid"].ToString();

                        html += "<tr><td class=\"name\" id=\"accessid_" + AccessID + "\"><a>" + Name + "</a></td><td>" + Date_of_Birth + "</td><td>" + Date_of_Death + "</td><td>" + Date_of_Burial + "</td></tr>";

                    }
                    html += "</tbody></table>";
                }
                else
                {
                    Console.WriteLine("No rows found.");
                }
                dr.Close();
            }



            /*
            SqlConnection con = new SqlConnection(strConnString);

            SqlCommand cmd = new SqlCommand("cemeterysearch", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@surname", SqlDbType.VarChar).Value = surname;
            cmd.Parameters.Add("@forenames", SqlDbType.VarChar).Value = forenames;

            cmd.Connection = con;
            try
            {
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    html += "<table id=\"searchtable\" class=\"table table-striped table-responsive\"><tbody>";

                    while (dr.Read())
                    {

                        string Name = dr["Name"].ToString();
                        //string Age = dr["Age"].ToString();
                        string Date_of_Death = dr["Date_of_Death"].ToString();
                        string Gender = dr["Gender"].ToString();

                        //string Warrant = dr["Warrant"].ToString();
                        string Date_of_Burial = dr["Date_of_Burial"].ToString();
                        string Date_of_Birth = dr["Date_of_Birth"].ToString();
                        //string Residence = dr["Residence"].ToString();
                        //string Occupation = dr["Occupation"].ToString();
                        //string Minister = dr["Minister"].ToString();
                        //string Director = dr["Director"].ToString();

                        //string Date_of_Medical_Certificate = dr["Date_of_Medical_Certificate"].ToString();
                        //string Marital_Status = dr["Marital_Status"].ToString();
                        string Place_of_Death = dr["Place_of_Death"].ToString();
                        //string Register_No = dr["Register_No"].ToString();

                        string Stillborn = dr["Stillborn"].ToString();
                        //string Location = dr["Location"].ToString();
                        string AccessID = dr["accessid"].ToString();

                        html += "<tr><td class=\"name\" id=\"td_" + AccessID + "\"><a>" + Name + "</a></td><td>" + Date_of_Birth + "</td><td>" + Date_of_Death + "</td><td>" + Date_of_Burial + "</td></tr>";

                    }
                    html += "</tbody></table>";
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
            */
            return html;
        }
    }
}
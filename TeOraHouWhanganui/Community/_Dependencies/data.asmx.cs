using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Web.Services;

namespace TeOraHouWhanganui.Community._Dependencies
{
    /// <summary>
    /// Summary description for Data
    /// </summary>
    //[WebService(Namespace = "http://tempuri.org/")]
    //[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    //[System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class data : System.Web.Services.WebService
    {
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void postaladdress_autocomplete(string term, string option = "")
        {
            List<PostalAddressClass> PostalAddressList = new List<PostalAddressClass>();

            string systemPrefix = "Community"; // WebConfigurationManager.AppSettings["systemPrefix"];
            String connectionString = ConfigurationManager.ConnectionStrings[systemPrefix + "ConnectionString"].ConnectionString;
            SqlConnection con = new SqlConnection(connectionString);

            SqlCommand cmd = new SqlCommand("PostalAddress_AutoComplete", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@search", SqlDbType.VarChar).Value = term;
            cmd.Parameters.Add("@option", SqlDbType.VarChar).Value = option;

            cmd.Connection = con;
            try
            {
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        PostalAddressList.Add(new PostalAddressClass
                        {
                            label = dr["search_line"].ToString(),
                            delivery_point_id = dr["delivery_point_id"].ToString()
                        });
                    }
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

            JavaScriptSerializer JS = new JavaScriptSerializer();
            Context.Response.Write(JS.Serialize(PostalAddressList));
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void name_autocomplete(string term)
        {
            string Username = HttpContext.Current.User.Identity.Name.ToLower();
            List<PersonClass> PersonList = new List<PersonClass>();

            string systemPrefix = "Community"; // WebConfigurationManager.AppSettings["systemPrefix"];
            String connectionString = ConfigurationManager.ConnectionStrings[systemPrefix + "ConnectionString"].ConnectionString;
            SqlConnection con = new SqlConnection(connectionString);

            SqlCommand cmd = new SqlCommand("name_autocomplete", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@username", SqlDbType.VarChar).Value = Username;
            cmd.Parameters.Add("@search", SqlDbType.VarChar).Value = term;

            cmd.Connection = con;
            try
            {
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        PersonList.Add(new PersonClass
                        {
                            person_guid = dr["person_guid"].ToString(),
                            name = dr["name"].ToString(),
                            label = dr["name"].ToString()
                        });
                    }

                    dr.Close();
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

            JavaScriptSerializer JS = new JavaScriptSerializer();
            Context.Response.Write(JS.Serialize(PersonList));

        }
        public class PersonClass
        {
            public string person_guid;
            public string name;
            public string label;

        }

        public class PostalAddressClass
        {
            public string label;
            public string delivery_point_id;
        }
    }
}

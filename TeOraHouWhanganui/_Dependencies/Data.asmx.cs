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

namespace TeOraHouWhanganui._Dependencies
{
    /// <summary>
    /// Summary description for Data
    /// </summary>
    //[WebService(Namespace = "http://tempuri.org/")]
    //[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    //[System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class Data : System.Web.Services.WebService
    {

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void Person_name_autocomplete(string term, string options)
        {
            List<PersonClass> PersonList = new List<PersonClass>();

            string systemPrefix = WebConfigurationManager.AppSettings["systemPrefix"];
            String connectionString = ConfigurationManager.ConnectionStrings[systemPrefix + "ConnectionString"].ConnectionString;
            SqlConnection con = new SqlConnection(connectionString);

            SqlCommand cmd = new SqlCommand("person_name_autocomplete", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@search", SqlDbType.VarChar).Value = term;
            cmd.Parameters.Add("@options", SqlDbType.VarChar).Value = options;

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
                            person_ctr = dr["person_ctr"].ToString(),
                            name = dr["name"].ToString(),
                            label = dr["name"].ToString(),
                            value = dr["person_ctr"].ToString()
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
            public string person_ctr;
            public string name;
            public string label;
            public string value;

        }
    }
}

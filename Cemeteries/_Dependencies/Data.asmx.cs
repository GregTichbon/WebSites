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
        public void Cemeteries()
        {
            List<dropdownClass> dropdownlist = new List<dropdownClass>();

            String strConnString = ConfigurationManager.ConnectionStrings["Cemetery"].ConnectionString;

            SqlConnection con = new SqlConnection(strConnString);

            SqlCommand cmd = new SqlCommand("GETCemeteries", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Connection = con;
            try
            {
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        dropdownlist.Add(new dropdownClass
                        {
                            label = dr["CemeteryName"].ToString(),
                            value = dr["CemeteryID"].ToString()
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
            string passresult = JS.Serialize(dropdownlist);

            Context.Response.Write(passresult);
        }

        [WebMethod]
        //[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void Areas(string cemetery)
        {
            List<dropdownClass> dropdownlist = new List<dropdownClass>();

            String strConnString = ConfigurationManager.ConnectionStrings["Cemetery"].ConnectionString;

            SqlConnection con = new SqlConnection(strConnString);

            SqlCommand cmd = new SqlCommand("GETAreas", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@CemeteryID", SqlDbType.VarChar).Value = cemetery;

            cmd.Connection = con;
            try
            {
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        dropdownlist.Add(new dropdownClass
                        {
                            label = dr["AreaName"].ToString(),
                            value = dr["AreaID"].ToString()
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
            string passresult = JS.Serialize(dropdownlist);

            Context.Response.Write(passresult);
        }



        [WebMethod]
        //[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void Divisions(string area)
        {
            List<dropdownClass> dropdownlist = new List<dropdownClass>();

            String strConnString = ConfigurationManager.ConnectionStrings["Cemetery"].ConnectionString;

            SqlConnection con = new SqlConnection(strConnString);

            SqlCommand cmd = new SqlCommand("GETDivisions", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@AreaID", SqlDbType.VarChar).Value = area;

            cmd.Connection = con;
            try
            {
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        dropdownlist.Add(new dropdownClass
                        {
                            label = dr["DivisionName"].ToString(),
                            value = dr["DivisionID"].ToString()
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
            string passresult = JS.Serialize(dropdownlist);

            Context.Response.Write(passresult);
        }

        [WebMethod]
        //[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void Plots(string division)
        {
            List<dropdownClass> dropdownlist = new List<dropdownClass>();

            String strConnString = ConfigurationManager.ConnectionStrings["Cemetery"].ConnectionString;

            SqlConnection con = new SqlConnection(strConnString);

            SqlCommand cmd = new SqlCommand("GETPlots", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@DivisionID", SqlDbType.VarChar).Value = division;

            cmd.Connection = con;
            try
            {
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        string used = dr["used"].ToString();
                        if (used != "")
                        {
                            used = " (used)";
                        }
                        dropdownlist.Add(new dropdownClass
                        {
                            label = dr["Plot"].ToString() + used,
                            value = dr["PlotID"].ToString()
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
            string passresult = JS.Serialize(dropdownlist);

            Context.Response.Write(passresult);
        }

        [WebMethod]
        //[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void Plot(string plotid)
        {

            List<plotClass> plotlist = new List<plotClass>();

            String strConnString = ConfigurationManager.ConnectionStrings["Cemetery"].ConnectionString;
            SqlConnection con = new SqlConnection(strConnString);

            SqlCommand cmd = new SqlCommand("GETPlot", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@plotid", SqlDbType.VarChar).Value = plotid;

            cmd.Connection = con;
            try
            {
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        plotlist.Add(new plotClass
                        {
                            remarks = "",
                            gis = "",
                            person = ""
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

            plotlist.Add(new plotClass
            {
                remarks = "Test remarks",
                gis = "Ploted or not",
                person = "1,2,3"
            });

            JavaScriptSerializer JS = new JavaScriptSerializer();
            string passresult = JS.Serialize(plotlist);

            Context.Response.Write(passresult);
        }
    }
}
#region classes
public class dropdownClass
{
    public string label;
    public string value;
}
public class plotClass
{
    public string plotid;
    public string cemetery;
    public string area;
    public string block;
    public string division;
    public string plot;
    public string remarks;
    public string transactiontype;
    public string count;
    public string gis;
    public string person;
}

public class updateresultClass
{
    public string status;
    public string message;
    public string id;  //added record
}
#endregion

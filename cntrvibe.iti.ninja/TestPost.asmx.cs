using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
//using System.Web.Script.Serialization;
using System.Web.Services;

namespace cntrvibe.iti.ninja
{
    /// <summary>
    /// Summary description for TestPost
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
      [System.Web.Script.Services.ScriptService]
    public class TestPost : System.Web.Services.WebService
    {

        [WebMethod]
        public void test1()
        {
            Response resultclass = new Response();
            resultclass.id = "1";
            //JavaScriptSerializer js = new JavaScriptSerializer();
            //Context.Response.Write(js.Serialize(resultclass));
        }
        [WebMethod]
        public void test2()
        {
            dynamic response = new JObject();
            response.id = 1;
            response.reference = 2;
            response.email = 3;
            Context.Response.Write(response.ToString());
        }

        [WebMethod]
        public void test3(NameValue[] formVars)
        {
            dynamic response = new JObject();
            response.id = 1;
            response.reference = 2;
            response.email = 3;
            Context.Response.Write(response.ToString());
        }

        [WebMethod]
        public void test4(string id)
        {
            dynamic response = new JObject();
            response.id = 1;
            response.reference = 2;
            response.email = 3;
            Context.Response.Write(response.ToString());
        }
        [WebMethod]
        public void test5(NameValue[] formVars)
        {
            Response response = new Response();
            response.id = "Test5";

            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Write(js.Serialize(response));
        }
        [WebMethod]
        public Response test6(NameValue[] formVars)
        {
            Response response = new Response();
            response.id = "Test5";

            return response;

        }

        [WebMethod]
        public dynamic test7(NameValue[] formVars)
        {
            dynamic MyDynamic = new System.Dynamic.ExpandoObject();
            MyDynamic.id = "A";
            MyDynamic.B = "B";
            MyDynamic.C = "C";
            MyDynamic.Number = 12;

            return MyDynamic;

        }
        [WebMethod]
        public dynamic test8(string id)
        {
            dynamic MyDynamic = new System.Dynamic.ExpandoObject();
            MyDynamic.id = "A";
            MyDynamic.B = "B";
            MyDynamic.C = "C";
            MyDynamic.Number = 12;

            return MyDynamic;

        }

        [WebMethod]
        public Response test9(string id)
        {
            Response response = new Response();
            response.id = "Test9";

            return response;

        }



    }

    public class Response
    {
       public string id;
    }

    public class NameValue
    {
        public string name { get; set; }
        public string value { get; set; }
    }
    public static class NameValueExtensionMethods
    {
        /// <summary>
        /// Retrieves a single form variable from the list of
        /// form variables stored
        /// </summary>
        /// <param name="formVars"></param>
        /// <param name="name">formvar to retrieve</param>
        /// <returns>value or string.Empty if not found</returns>
        public static string Form(this NameValue[] formVars, string name)
        {
            var matches = formVars.Where(nv => nv.name.ToLower() == name.ToLower()).FirstOrDefault();
            if (matches != null)
                return matches.value;
            return string.Empty;
        }

        /// <summary>
        /// Retrieves multiple selection form variables from the list of 
        /// form variables stored.
        /// </summary>
        /// <param name="formVars"></param>
        /// <param name="name">The name of the form var to retrieve</param>
        /// <returns>values as string[] or null if no match is found</returns>
        public static string[] FormMultiple(this NameValue[] formVars, string name)
        {
            var matches = formVars.Where(nv => nv.name.ToLower() == name.ToLower()).Select(nv => nv.value).ToArray();
            if (matches.Length == 0)
                return null;
            return matches;
        }
    }
}
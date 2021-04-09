using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace TeOraHouWhanganui._Dependencies
{
    public class functions
    {
        public static Boolean AccessStringTest(string username, string requiredaccess)
        {
            string personaccess = "";

            string strConnString = "Data Source=toh-app;Initial Catalog=TeOraHou;Integrated Security=False;user id=OnlineServices;password=Whanganui497";
            using (SqlConnection con = new SqlConnection(strConnString))
            using (SqlCommand cmd = new SqlCommand("Get_AccessString", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@username", SqlDbType.VarChar).Value = username;
                con.Open();
                personaccess = cmd.ExecuteScalar().ToString();
            }

            int personaccesslength = personaccess.Length;
            int requiredaccesslength = requiredaccess.Length;
            if (personaccesslength < requiredaccesslength)
            {
                string pad = new String('0', requiredaccesslength - personaccesslength);
                personaccess = personaccess + pad;
            }
            else
            {
                string pad = new String('0', personaccesslength - requiredaccesslength);
                requiredaccess = requiredaccess + pad;
            }


            int result = Convert.ToInt32(personaccess, 2) & Convert.ToInt32(requiredaccess, 2);

            if (result > 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

    }
}
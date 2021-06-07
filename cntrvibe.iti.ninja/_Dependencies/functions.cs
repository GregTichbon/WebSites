using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace cntrvibe.iti.ninja._Dependencies
{
    public class functions
    {
        public static string get_entry(string reference, string entry_ctr)
        {
            string html = "";
            string systemPrefix = WebConfigurationManager.AppSettings["systemPrefix"];
            String connectionString = ConfigurationManager.ConnectionStrings[systemPrefix + "ConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("cntrvibe_get_entry", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@reference", SqlDbType.VarChar).Value = reference;
                    cmd.Parameters.Add("@entry_ctr", SqlDbType.VarChar).Value = entry_ctr;
                    con.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    if (dr.HasRows)
                    {
                        dr.Read();

                        html += "<table class=\"entry\"><tbody>";
                        html += "<tr><td>Registering as a</td><td>" + dr["type"].ToString() + "</td></tr>";
                        html += "<tr><td>Group name</td><td>" + dr["groupname"].ToString() + "</td> </tr>";
                        html += "<tr><td>Number of people in group</td><td>" + dr["groupnumber"].ToString() + "</td> </tr>";
                        html += "<tr><td>First name</td><td>" + dr["firstname"].ToString() + "</td> </tr>";
                        html += "<tr><td>Surname</td><td>" + dr["surname"].ToString() + "</td> </tr>";
                        html += "<tr><td>Email address</td><td>" + dr["emailaddress"].ToString() + "</td> </tr>";
                        html += "<tr><td>What are the best ways to get hold of you?</td><td>" + dr["contact"].ToString() + "</td> </tr>";
                        html += "<tr><td>Description</td><td>" + dr["description"].ToString() + "</td> </tr>";
                        html += "<tr><td>Requirements</td><td>" + dr["requirements"].ToString() + "</td> </tr>";
                        html += "<tr><td>Other information</td><td>" + dr["otherinformation"].ToString() + "</td> </tr>";
                        html += "<tr><td>Declaration</td><td>I confirm that:<br /> <ul> <li>All participants are between the ages of 12 and 24 on the day of CNTR VIBE and that all reside in the Whanganui Area</li> <li>I have read and agree to the rules</li> <li>Photo, video, and audio from the day of the event may be used for at the organisers discretion. </li> </ul> </td> </tr>";
                        html += "</tbody></table>";    
                    }

                    dr.Close();
                }

            }

            return (html);
        }
    }
}
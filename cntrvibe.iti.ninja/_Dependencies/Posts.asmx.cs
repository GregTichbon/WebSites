using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Services;
using System.Threading;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Net.Mail;
using HtmlAgilityPack;
using System.Net;
using System.Text.RegularExpressions;
//using System.Web.Script.Serialization;

namespace cntrvibe.iti.ninja._Dependencies
{
    /// <summary>
    /// Summary description for Data
    /// </summary>
    /// GREG [WebService(Namespace = "http://tempuri.org/")]
    /// GREG [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    /// GREG [System.ComponentModel.ToolboxItem(false)]
    /// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]   //GREG  -  THIS IS REQUIRED FOR POSTS
    
    
    public class Posts : System.Web.Services.WebService
    {

        [WebMethod]
        public Response update_registration(NameValue[] formVars)
        {
            string id = "";
            string reference = Guid.NewGuid().ToString();

            string systemPrefix = WebConfigurationManager.AppSettings["systemPrefix"];
            String connectionString = ConfigurationManager.ConnectionStrings[systemPrefix + "ConnectionString"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("cntrvibe_update_entry", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@type", SqlDbType.VarChar).Value = formVars.Form("fld_type");
                cmd.Parameters.Add("@groupname", SqlDbType.VarChar).Value = formVars.Form("fld_groupname");
                cmd.Parameters.Add("@groupnumber", SqlDbType.VarChar).Value = formVars.Form("fld_groupnumber");
                cmd.Parameters.Add("@firstname", SqlDbType.VarChar).Value = formVars.Form("fld_firstname");
                cmd.Parameters.Add("@surname", SqlDbType.VarChar).Value = formVars.Form("fld_surname");
                cmd.Parameters.Add("@emailaddress", SqlDbType.VarChar).Value = formVars.Form("fld_emailaddress");
                cmd.Parameters.Add("@contact", SqlDbType.VarChar).Value = formVars.Form("fld_contact");
                cmd.Parameters.Add("@description", SqlDbType.VarChar).Value = formVars.Form("fld_description");
                cmd.Parameters.Add("@requirements", SqlDbType.VarChar).Value = formVars.Form("fld_requirements");
                cmd.Parameters.Add("@otherinformation", SqlDbType.VarChar).Value = formVars.Form("fld_otherinformation");
                cmd.Parameters.Add("@declaration", SqlDbType.VarChar).Value = formVars.Form("fld_declaration");
                cmd.Parameters.Add("@reference", SqlDbType.VarChar).Value = reference;

                con.Open();
                id = cmd.ExecuteScalar().ToString();
                con.Close();
            }


            //string host = "datainn.co.nz";
            //string host = "70.35.207.87";
            //string emailfrom = "UnionBoatClub@datainn.co.nz";
            //string password = "39%3Zxon";

            //string host = "cp-wc03.per01.ds.network"; //"mail.unionboatclub.co.nz";
            //string emailfrom = "info@unionboatclub.co.nz";
            //string password = "R0wtheboat";
            //int port = 587; // 465; // 25;
            //Boolean enableSsl = true;


            string host = "smtp.gmail.com";
            string emailfrom = "cntrvibe@gmail.com";
            string password = "Whanganui101";
            int port = 587;
            Boolean enableSsl = true;

            string emailfromname = "CntrVibe";
            string emailBCC = emailfrom + ";greg@datainn.co.nz";
            string replyto = "";
            string emailRecipient = formVars.Form("fld_emailaddress");
            string emailsubject = "CNTRVIBE registration acknowledgment";

            string emailtemplate = Server.MapPath("\\") + "\\EmailTemplate.html";
            string emaildocument = "";

            using (StreamReader sr = new StreamReader(emailtemplate))
            {
                emaildocument = sr.ReadToEnd();
            }

            //emaildocument = emaildocument.Replace("||Content||", emaildocument);

            Regex rx = new Regex(@"\$\[([^\]]*)\]");
            foreach (Match match in rx.Matches(emaildocument))
            {
                string mv = match.Value;
                string holder = mv.Substring(2, mv.Length - 3);
                emaildocument = emaildocument.Replace(mv, formVars.Form(holder));
            }



            string[] attachments = new string[0];
            Dictionary<string, string> emailoptions = new Dictionary<string, string>();
            string emailresponse = "";

            try
            {
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(emailfrom, emailfromname);
                if (replyto != "")
                {
                    string[] rtaddresses = replyto.Split(';');

                    IEnumerable<string> distinctrtaddresses = rtaddresses.Distinct();

                    foreach (string rtaddress in distinctrtaddresses)
                    {
                        mail.ReplyToList.Add(rtaddress);
                    }
                }

                SmtpClient client = new SmtpClient();
                client.Port = port;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new System.Net.NetworkCredential(emailfrom, password);
                client.Host = host;

                string[] emailaddresses = emailRecipient.Split(';');

                IEnumerable<string> distinctemailaddresses = emailaddresses.Distinct();

                foreach (string emailaddress in distinctemailaddresses)
                {
                    mail.To.Add(emailaddress);
                }

                if (emailBCC != "")
                {
                    string[] bccaddresses = emailBCC.Split(';');

                    IEnumerable<string> distinctbccaddresses = bccaddresses.Distinct();

                    foreach (string bccaddress in distinctbccaddresses)
                    {
                        mail.Bcc.Add(bccaddress);
                    }
                }

                mail.Subject = emailsubject;

                mail.IsBodyHtml = false;
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(emaildocument);
                string text = doc.DocumentNode.SelectSingleNode("//body").InnerText;
                mail.Body = text;

                System.Net.Mime.ContentType mimeType = new System.Net.Mime.ContentType("text/html");
                AlternateView alternate = AlternateView.CreateAlternateViewFromString(emaildocument, mimeType);
                mail.AlternateViews.Add(alternate);

                foreach (string attachment in attachments)
                {
                    mail.Attachments.Add(new Attachment(attachment));
                }
                client.EnableSsl = enableSsl;
                //System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                //client.Send(mail);

                try
                {
                    client.Send(mail);
                }
                catch (SmtpFailedRecipientsException ex)
                {
                    for (int i = 0; i < ex.InnerExceptions.Length; i++)
                    {
                        SmtpStatusCode status = ex.InnerExceptions[i].StatusCode;
                        if (status == SmtpStatusCode.MailboxBusy ||
                            status == SmtpStatusCode.MailboxUnavailable)
                        {
                            Console.WriteLine("Delivery failed - retrying in 5 seconds.");
                            System.Threading.Thread.Sleep(5000);
                            client.Send(mail);
                        }
                        else
                        {
                            Console.WriteLine("Failed to deliver message to {0}",
                                ex.InnerExceptions[i].FailedRecipient);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception caught in RetryIfBusy(): {0}",
                            ex.ToString());
                }

            }
            catch (Exception e)
            {
                emailresponse = e.Message;
            }

            Response response = new Response();
            response.id = id;
            return response;


        }
    }

    public class Response
    {
        public string id;
        public string reference;
        public string email;
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

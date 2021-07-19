using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace Auction._Dependencies.Functions
{
    public class functions
    {
        public static Dictionary<string, string> get_Auction_Parameters(string url)
        {
            if(myGlobal.parameters == null)
            {
                Dictionary<string, string> parameters = new Dictionary<string, string>();

                String strConnString = ConfigurationManager.ConnectionStrings["AuctionConnectionString"].ConnectionString;
                SqlConnection con = new SqlConnection(strConnString);
                SqlCommand cmd = new SqlCommand("Get_Parameters", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@url", SqlDbType.VarChar).Value = url;
                cmd.Connection = con;
                try
                {
                    con.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    dr.Read();

                    for (int f1 = 0; f1 < dr.FieldCount; f1++)
                    {
                        parameters.Add(dr.GetName(f1).ToString(), dr.GetValue(f1).ToString());
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
                myGlobal.parameters = parameters;
            }

            return myGlobal.parameters;
        }

        public static string sendtext(string mobilenumber, string message)
        {
            string myresponse = "";
            WebRequest wr = WebRequest.Create("http://office.datainn.co.nz/sms/send?O=S&P=" + mobilenumber + "&M=" + HttpUtility.UrlEncode(message));
            wr.Timeout = 3500;

            //Console.WriteLine(i);

            WebResponse response = wr.GetResponse();
            Stream data = response.GetResponseStream();
            using (StreamReader sr = new StreamReader(data))
            {
                myresponse = sr.ReadToEnd();

            }

            data.Dispose();
            return myresponse;
        }

        public static void sendemail(string emailsubject, string emailbody, string emailRecipient, string emailbcc)
        {
            //MailMessage mail = new MailMessage("noreply@whanganui.govt.nz", emailRecipient);

            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("noreply@whanganui.govt.nz", "Whanganui District Council");

            SmtpClient client = new SmtpClient();
            client.Port = 25;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            //client.Host = "wdc-cas.local.whanganui.govt.nz";
            client.Host = "192.168.0.113"; //"192.168.1.78";

            mail.IsBodyHtml = true;

            string[] emailaddresses = emailRecipient.Split(';');

            IEnumerable<string> distinctemailaddresses = emailaddresses.Distinct();

            foreach (string emailaddress in distinctemailaddresses)
            {
                mail.To.Add(emailaddress);
            }

            if (emailbcc != "")
            {
                string[] bccaddresses = emailbcc.Split(';');

                IEnumerable<string> distinctbccaddresses = bccaddresses.Distinct();

                foreach (string bccaddress in distinctbccaddresses)
                {
                    mail.Bcc.Add(bccaddress);
                }
            }
            mail.Subject = emailsubject;
            mail.Body = emailbody;
            client.Send(mail);
        }

        public static void Log(string location, string logMessage, string EmailAddress)
        {
            String LogFileLocation = ConfigurationManager.AppSettings["Logfile.Location"];
            //String LogFileLocation = "logfile.txt";
            StreamWriter w = File.AppendText(LogFileLocation);
            w.WriteLine("{0}", DateTime.Now.ToLongTimeString() + "\t" + DateTime.Now.ToLongDateString() + "\t" + location + "\t" + logMessage + "\t" + EmailAddress);
            w.Flush();
            w.Close();
            if (EmailAddress != "")
            {
                //sendemail("Auction Error", location + "<br>" + logMessage, EmailAddress, "");
            }
        }

        public static string populateselect(string[] options, string selectedoption, string firstoption = "None")
        {
            string selected;
            string html = "";
            if (firstoption != "None")
            {
                html = html + ("<option>" + firstoption + "</option>");

            }
            string myoption;
            string[] optionparts;
            string label;
            string value;
            string selectedcompare;
            foreach (string option in options)
            {
                myoption = option + '\x0008';
                optionparts = myoption.Split('\x0008');
                label = optionparts[0];
                value = optionparts[1];
                if (value == label || value == "")
                {
                    value = "";
                    selectedcompare = label;
                }
                else
                {
                    selectedcompare = value;
                    value = " value=\"" + value + "\"";
                }

                if (selectedcompare == selectedoption)
                {
                    selected = " selected";
                }
                else
                {
                    selected = "";
                }

                html = html + ("<option" + value + selected + ">" + label + "</option>");
            }
            return html;
        }
    }
}
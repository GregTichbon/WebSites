using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//using System.Web.UI.WebControls;

using System.Net.Mail;
using System.IO;
//using System.Xml;

//using System.Web.UI;

using System.Net;
using System.Text;

using System.Data;
using System.Data.SqlClient;
//using System.Configuration;
using System.Threading.Tasks;

using System.Net.Http;
using System.Net.Http.Headers;
//using SMSProcessing.Model;
using System.Xml.Linq;
using HtmlAgilityPack;
using System.Security.Cryptography;
using System.Threading;
using System.Text.RegularExpressions;

namespace Generic
{
    public class Functions
    {
        string IPAddress = "";
        string Port = "";
        string UserName = "";
        string Password = "";

        public static google_geocodeClass google_geocode(string api_key, string address, Dictionary<string, string> options)
        {

            string requestUri = string.Format("https://maps.googleapis.com/maps/api/geocode/xml?key={1}&address={0}&sensor=false", Uri.EscapeDataString(address), api_key);

            WebRequest request = WebRequest.Create(requestUri);
            WebResponse response = request.GetResponse();
            XDocument xdoc = XDocument.Load(response.GetResponseStream());

            XElement result = xdoc.Element("GeocodeResponse").Element("result");
            XElement formatted_address = result.Element("formatted_address");
            XElement locationElement = result.Element("geometry").Element("location");
            XElement lat = locationElement.Element("lat");
            XElement lng = locationElement.Element("lng");

            google_geocodeClass google_geocode = new google_geocodeClass();

            google_geocode.address = formatted_address.Value;
            google_geocode.lat = lat.Value;
            google_geocode.lng = lng.Value;

            return google_geocode;
        }

        public static Dictionary<string, string> buildselectionlist(string strConnString, string cmdtext, Dictionary<string, string> options)
        {

            Dictionary<string, string> selectionlist = new Dictionary<string, string>();

            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd;

            if (options.ContainsKey("storedprocedure"))
            {
                if (options.ContainsKey("storedprocedurename"))
                {
                    cmd = new SqlCommand(cmdtext, con);
                    cmd.CommandType = CommandType.StoredProcedure;

                }
                else
                {
                    cmd = new SqlCommand("buildselectionlist", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@sqltext", SqlDbType.VarChar).Value = cmdtext;
                }
                if (options.ContainsKey("parameters"))
                {
                    cmd.Parameters.Add("@parameters", SqlDbType.VarChar).Value = options["parameters"];
                }
            }
            else
            {
                cmd = new SqlCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = cmdtext;
            }

            cmd.Connection = con;
            try
            {
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    string Label;
                    string Value = "";
                    while (dr.Read())
                    {
                        Label = dr["label"].ToString();
                        if (options.ContainsKey("usevalues"))
                        {
                            Value = dr["value"].ToString();
                        }
                        selectionlist.Add(Label, Value);
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

            return selectionlist;
        }
        public static string buildselection(Dictionary<string, string> optionlist, string[] selectedoption, Dictionary<string, string> options)
        {
            string html = "";

            /* Can just put the option on the page
            if (options["firstoption"] != "None")  //eg: Please Select
            {
                html = html + ("<option>" + options["firstoption"] + "</option>");
            }
            */

            string Label;
            string Value = "";
            string ValueText;
            string selectText = "";

            foreach (KeyValuePair<string, string> selectionpair in optionlist)
            {
                ValueText = "";
                Label = selectionpair.Key;
                if (options.ContainsKey("valuefield"))
                {
                    if (options["valuefield"] == "label")
                    {
                        Value = Label;
                    }
                    else if (options["valuefield"] == "value")
                    {
                        Value = selectionpair.Value;
                    }
                }
                string selected = "";

                if (options.ContainsKey("comparelabel"))
                {
                    selectText = Label;
                }
                else //compare value
                {
                    selectText = Value;
                }

                if (selectedoption.Contains(selectText))
                {
                    selected = " selected";
                }
                else
                {
                    selected = "";
                }

                if (options["type"] == "uiselectable")
                {
                    string id = "checkbox_" + Value;
                    string name = options["name"];
                    ValueText = "<label for=\"" + id + "\">" + Label + "</label>";
                    ValueText += "<input class=\"uicheckbox\" type=\"checkbox\" name=\"" + name + "\" id=\"" + id + "\">";
                }
                else if (options["type"] == "select")
                {
                    if (Value != "")
                    {
                        ValueText = " value=\"" + Value + "\"";
                    }
                    ValueText = "<option" + ValueText + selected + ">" + Label + "</option>";
                }
                else if (options["type"] == "checkbox")
                {

                }
                else if (options["type"] == "radio")
                {

                }



                html += ValueText;


            }
            return html;
        }
        public IHtmlString HTMLRaw(string source)
        {
            return new HtmlString(source);
        }
        public string HTMLtoText(string source)
        {
            var text = new HtmlDocument();
            text.LoadHtml(source);
            string pageText = text.DocumentNode.InnerText;
            return pageText;
        }
        public string formatphonenumber(string phonenumber)
        {
            phonenumber = Regex.Replace(phonenumber, "[^0-9]", "");
            return phonenumber;
        }

        public static string formatdate(string date, string format)
        {
            if (date != "")
            {
                date = Convert.ToDateTime(date).ToString(format);
            }
            return date;
        }
        public string test()
        {
            return "Test";
        }

       
        public static Boolean accessstringtest(string personaccess, string requiredaccess)
        {
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
        public static string googleanalyticstracking()
        {
            return "";
        }
        public void sendemail(string emailsubject, string emailhtml, string emailRecipient, string emailbcc, string replyto)
        {
            //MailMessage mail = new MailMessage("noreply@whanganui.govt.nz", emailRecipient);

            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("noreply@datainn.co.nz", "Data Innovations");
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
            client.Port = 25;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Host = "datainn.co.nz";

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
            mail.IsBodyHtml = true;
            mail.Subject = emailsubject;
            mail.Body = emailhtml;
            client.Send(mail);
        }

        public void sendemailV2(string host, string emailfrom, string emailfromname, string password, string emailsubject, string emailtext, string emailhtml, string emailRecipient, string emailbcc, string replyto)
        {
            //Must pass both text and html!!

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
            client.Port = 25;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential(emailfrom, password);
            client.Host = host;

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

            mail.IsBodyHtml = false;
            mail.Body = emailtext;

            System.Net.Mime.ContentType mimeType = new System.Net.Mime.ContentType("text/html");
            AlternateView alternate = AlternateView.CreateAlternateViewFromString(emailhtml, mimeType);
            mail.AlternateViews.Add(alternate);

            client.Send(mail);
        }
        public void sendemailV3(string host, string emailfrom, string emailfromname, string password, string emailsubject, string emailhtml, string emailRecipient, string emailbcc, string replyto)
        {
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
                client.Port = 25; // 587; //993; // 25 587;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(emailfrom, password);
                client.Host = host;

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

                mail.IsBodyHtml = false;
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(emailhtml);
                string text = doc.DocumentNode.SelectSingleNode("//body").InnerText;
                mail.Body = text;

                System.Net.Mime.ContentType mimeType = new System.Net.Mime.ContentType("text/html");
                AlternateView alternate = AlternateView.CreateAlternateViewFromString(emailhtml, mimeType);
                mail.AlternateViews.Add(alternate);
                client.Send(mail);
            }
            catch (Exception e)
            {
                Log("", @"generic/functions.cs/sendemailV3", "Error: Message; " + e.Message + ", StackTrace; " + e.StackTrace, "");
            }
        }
        public void sendemailV4(string host, string emailfrom, string emailfromname, string password, string emailsubject, string emailhtml, string emailRecipient, string emailbcc, string replyto, string[] attachments, Dictionary<string, string> options)
        {

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
                client.Port = 25;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(emailfrom, password);
                client.Host = host;

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

                mail.IsBodyHtml = false;
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(emailhtml);
                string text = doc.DocumentNode.SelectSingleNode("//body").InnerText;
                mail.Body = text;

                System.Net.Mime.ContentType mimeType = new System.Net.Mime.ContentType("text/html");
                AlternateView alternate = AlternateView.CreateAlternateViewFromString(emailhtml, mimeType);
                mail.AlternateViews.Add(alternate);

                foreach (string attachment in attachments)
                {
                    mail.Attachments.Add(new Attachment(attachment));
                }

                client.Send(mail);
            }
            catch (Exception e)
            {
                Log("", @"generic/functions.cs/sendemailV4", "Error: Message; " + e.Message + ", StackTrace; " + e.StackTrace, "");
            }
        }
        public void sendemailV5(string host, int port, Boolean enableSsl, string emailfrom, string emailfromname, string password, string emailsubject, string emailhtml, string emailRecipient, string emailbcc, string replyto, string[] attachments, Dictionary<string, string> options)
        {

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
                client.Credentials = new NetworkCredential(emailfrom, password);
                client.Host = host;

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

                mail.IsBodyHtml = false;
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(emailhtml);
                string text = doc.DocumentNode.SelectSingleNode("//body").InnerText;
                mail.Body = text;

                System.Net.Mime.ContentType mimeType = new System.Net.Mime.ContentType("text/html");
                AlternateView alternate = AlternateView.CreateAlternateViewFromString(emailhtml, mimeType);
                mail.AlternateViews.Add(alternate);

                foreach (string attachment in attachments)
                {
                    mail.Attachments.Add(new Attachment(attachment));
                }
                client.EnableSsl = enableSsl;
                client.Send(mail);
            }
            catch (Exception e)
            {
                Log("", @"generic/functions.cs/sendemailV5", "Error: Message; " + e.Message + ", StackTrace; " + e.StackTrace, "");
            }
        }
        public void Log(string guid, string location, string message, string EmailAddress)
        {
            //String strConnString = ConfigurationManager.AppSettings["LogConnectionString"];
            String strConnString = "Data Source=toh-app;Initial Catalog=DataInnovations;Integrated Security=False;user id=OnlineServices;password=Whanganui497";
            SqlConnection con = new SqlConnection(strConnString);

            SqlCommand cmd = new SqlCommand("insert_log", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@guid", SqlDbType.VarChar).Value = guid;
            cmd.Parameters.Add("@location", SqlDbType.VarChar).Value = location;
            cmd.Parameters.Add("@Message", SqlDbType.VarChar).Value = message;

            cmd.Connection = con;
            try
            {
                con.Open();
                cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
            }




            //String LogFileLocation = ConfigurationManager.AppSettings["Logfile.Location"];
            /*
            StreamWriter w = File.AppendText(LogFileLocation);
            w.WriteLine("{0}", DateTime.Now.ToLongTimeString() + "\t" + DateTime.Now.ToLongDateString() + "\t" + location + "\t" + logMessage + "\t" + EmailAddress);
            w.Flush();
            w.Close();
            if (EmailAddress != "")
            {
                //sendemail("Online Applications Error", location + "<br>" + logMessage, EmailAddress, "");
            }
            */

        }
        public string BrowserDetails(System.Web.HttpBrowserCapabilities browser)
        {

            string response = "";
            IPHostEntry Host = default(IPHostEntry);
            string Hostname = null;
            Hostname = System.Environment.MachineName;
            Host = Dns.GetHostEntry(Hostname);
            foreach (IPAddress IP in Host.AddressList)
            {
                if (IP.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    response = "IP Address = " + Convert.ToString(IP) + "<br />";
                }
            }
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            string ipAddress = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (!string.IsNullOrEmpty(ipAddress))
            {
                string[] addresses = ipAddress.Split(',');
                if (addresses.Length != 0)
                {
                    response += addresses[0];
                }
            }

            response += context.Request.ServerVariables["REMOTE_ADDR"];

            response += "<b>Browser Capabilities</b><br />"
                + "Type = " + browser.Type + "<br />"
                + "Name = " + browser.Browser + "<br />"
                + "Version = " + browser.Version + "<br />"
                + "Major Version = " + browser.MajorVersion + "<br />"
                + "Minor Version = " + browser.MinorVersion + "<br />"
                + "Platform = " + browser.Platform + "<br />"
                + "Is Beta = " + browser.Beta + "<br />"
                + "Is Crawler = " + browser.Crawler + "<br />"
                + "Is AOL = " + browser.AOL + "<br />"
                + "Is Win16 = " + browser.Win16 + "<br />"
                + "Is Win32 = " + browser.Win32 + "<br />"
                + "Supports Frames = " + browser.Frames + "<br />"
                + "Supports Tables = " + browser.Tables + "<br />"
                + "Supports Cookies = " + browser.Cookies + "<br />"
                + "Supports VBScript = " + browser.VBScript + "<br />"
                + "Supports JavaScript = " +
                    browser.EcmaScriptVersion.ToString() + "<br />"
                + "Supports Java Applets = " + browser.JavaApplets + "<br />"
                + "Supports ActiveX Controls = " + browser.ActiveXControls
                      + "<br />"
                + "Supports JavaScript Version = " +
                    browser["JavaScriptVersion"] + "<br />";

            return response;
        }
        public string getReference(string mode = "guid")
        {
            string reference = "";

            if (mode == "guid")
            {
                reference = Guid.NewGuid().ToString();
            }
            else if (mode == "datetime")
            {
                //reference = DateTime.Now.ToString("ddMMyyHHmmssff");
                reference = DateTime.Now.ToString("fffMMHHmmyyssdd");

            }


            return reference;
        }
        public string makelink(string url, string window = "")
        {
            string link = url.Trim().ToLower();
            if (link != "")
            {

                string desc = url.Trim().ToLower();


                if ((link + "       ").Substring(0, 7) == "http://")
                {
                    desc = desc.Substring(7);
                }
                else if ((link + "        ").Substring(0, 8) == "https://")
                {
                    desc = desc.Substring(8);
                }
                else
                {
                    link = "http://" + link;
                }

                string target = "";
                if (window != "")
                {
                    target = " target='" + window + "'";
                }
                link = "<a href='" + link + "'" + target + ">" + desc + "</a>";

            }
            return link;
        }
        public string saveattachments(string attpath, string reference, System.Web.UI.WebControls.FileUpload fucontrol, string RawUrl)
        {
            //if attpath has had reference appended then don't  probably need to pass reference
            string uploadresult = "";

            if (fucontrol.HasFiles)
            {
                if (!Directory.Exists(attpath))
                {
                    Directory.CreateDirectory(attpath);
                }

                if (reference != "")
                {
                    reference = "_" + reference;
                }

                int c1 = 0;
                int failed = 0;
                int succeeded = 0;
                string delim = "";


                foreach (HttpPostedFile postedFile in fucontrol.PostedFiles)
                {
                    c1 = c1 + 1;
                    try
                    {
                        string wpextension = System.IO.Path.GetExtension(postedFile.FileName);
                        string wpfilename = System.IO.Path.GetFileNameWithoutExtension(postedFile.FileName);
                        string newfilename = wpfilename + reference + "_" + c1.ToString("000") + wpextension;
                        postedFile.SaveAs(attpath + "\\" + newfilename);
                        uploadresult = uploadresult + delim + System.IO.Path.GetFileName(postedFile.FileName) + "\x00FD" + "Received" + "\x00FD" + newfilename;
                        delim = "\x00FE";
                        succeeded = succeeded + 1;
                    }
                    catch (Exception ex)
                    {
                        Log("", RawUrl, ex.Message, "greg.tichbon@whanganui.govt.nz");
                        uploadresult = uploadresult + delim + System.IO.Path.GetFileName(postedFile.FileName) + "\x00FD" + "Failed" + "\x00FD" + "";
                        delim = "\x00FE";
                        failed = failed + 1;
                    }
                }
            }
            else
            {
                uploadresult = "File(s) not provided";
            }
            return uploadresult;
        }

        /*
        public async Task<string> SendMessage(string PhoneNumber, string Message)
        {
            string returnval = "";
            getparams();

            using (var client = new HttpClient())
            {
                string url = ConstructBaseUri();
                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                if (!string.IsNullOrEmpty(UserName) && !string.IsNullOrEmpty(Password))
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                                "Basic",
                                 Convert.ToBase64String(
                                 ASCIIEncoding.ASCII.GetBytes(
                                 string.Format("{0}:{1}", UserName, Password))));
                }
                var postData = new List<KeyValuePair<string, string>>();
                postData.Add(new KeyValuePair<string, string>("to", PhoneNumber));
                postData.Add(new KeyValuePair<string, string>("message", Message));
                HttpContent content = new FormUrlEncodedContent(postData);

                HttpResponseMessage response = await client.PostAsync(MessagesUrlPath, content);
                if (response.IsSuccessStatusCode)
                {

                    PostMessageResponse result = await response.Content.ReadAsAsync<PostMessageResponse>();
                    if (result.IsSuccessful)
                    {
                        returnval = result.ToString();
                    }
                    else
                    {
                        returnval = result.Description;
                    }

                }
                else
                {
                    returnval = response.ToString();
                }
            }
            return (returnval);
        }
        */

        public void getparams()
        {
            String strConnString = "Data Source=192.168.10.6;Initial Catalog=SMS;Integrated Security=False;user id=OnlineServices;password=Whanganui497";
            SqlConnection con = new SqlConnection(strConnString);

            SqlCommand cmd = new SqlCommand("GET_PARAMETER", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@parameter", SqlDbType.VarChar).Value = "IPAddress";

            cmd.Connection = con;
            try
            {
                con.Open();
                //SqlDataReader dr = cmd.ExecuteReader();
                IPAddress = cmd.ExecuteScalar().ToString();

            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex.Message);
                //Console.WriteLine(ex.InnerException);
                throw ex;
            }
            finally
            {
                con.Close();
            }
            cmd.Parameters.Clear();
            cmd.Parameters.Add("@parameter", SqlDbType.VarChar).Value = "Port";

            cmd.Connection = con;
            try
            {
                con.Open();
                //SqlDataReader dr = cmd.ExecuteReader();
                Port = cmd.ExecuteScalar().ToString();

            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex.Message);
                //Console.WriteLine(ex.InnerException);
                throw ex;
            }
            finally
            {
                con.Close();
            }
        }
        public string test1()
        {
            return "Done";
        }
        public string test2()
        {
            WebRequest wr = WebRequest.Create("http://office.datainn.co.nz/sms/test2.aspx");
            wr.Timeout = 3500;
            string responsevalue = String.Empty;

            try
            {
                WebResponse response = wr.GetResponse();
                Stream data = response.GetResponseStream();
                using (StreamReader sr = new StreamReader(data))
                {
                    responsevalue = sr.ReadToEnd();
                }
            }
            catch
            {
                responsevalue = "Error sending txt message";
            }

            return responsevalue;
        }
        public string SendRemoteMessage(string PhoneNumber, string Message, string Description, string resend_id = "")
        {
            string strConnString = "Data Source=toh-app;Initial Catalog=SMS;Integrated Security=False;user id=OnlineServices;password=Whanganui497";

            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "Update_SMSLog";
            cmd.Parameters.Add("@direction", SqlDbType.VarChar).Value = "Sent";
            cmd.Parameters.Add("@phonenumber", SqlDbType.VarChar).Value = PhoneNumber;
            cmd.Parameters.Add("@message", SqlDbType.VarChar).Value = Message;
            cmd.Parameters.Add("@description", SqlDbType.VarChar).Value = Description;
            if (resend_id != "")
            {
                cmd.Parameters.Add("@resend_id", SqlDbType.VarChar).Value = resend_id;
            }
            cmd.Connection = con;

            con.Open();
            string id = cmd.ExecuteScalar().ToString();

            string response = "";
            int c1 = 0;
            Message = Message.Replace("&", "^^");
            Message = HttpUtility.UrlEncode(Message);
            string url = "http://office.datainn.co.nz/sms/send/?O=S&P=" + PhoneNumber + "&M=" + Message;

            while (!response.Contains("OK") && c1 < 3)
            {
                c1++;
                try
                {
                    var webClient = new WebClient();
                    Uri uri = new Uri(url);
                    response = webClient.DownloadString(uri);
                }
                catch (Exception e)
                {
                    response = "Could not send text:" + e.ToString();
                    Log("", "Generic Functions SendRemoteMessage", "Error", "");
                }
                if (!response.Contains("OK"))
                {
                    Thread.Sleep(500);
                }

            }
            /*
            WebRequest wr = WebRequest.Create(url);
            wr.Timeout = 3500;
            string responsevalue = String.Empty;

            try
            {
                WebResponse response = wr.GetResponse();
                Stream data = response.GetResponseStream();
                using (StreamReader sr = new StreamReader(data))
                {
                    responsevalue = sr.ReadToEnd();
                }
            }
            catch
            {
                responsevalue = "Error sending txt message";
            }

            return responsevalue;
            */
            cmd.Parameters.Clear();
            cmd.Parameters.Add("@smslog_id", SqlDbType.VarChar).Value = id;
            cmd.Parameters.Add("@id", SqlDbType.VarChar).Value = "To Do";

            cmd.Parameters.Add("@response", SqlDbType.VarChar).Value = response;

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                response += e.InnerException.ToString();
            }

            con.Close();
            con.Dispose();
            return response;
        }
        public static string populateselect(string[] selectoptions, string selectedoptions, string firstoption)
        {
            string selected;
            string html = "";
            if (firstoption != "None")  //eg: Please Select
            {
                html = html + ("<option>" + firstoption + "</option>");
            }

            foreach (string option in selectoptions)
            {
                if (option == selectedoptions)
                {
                    selected = " selected";
                }
                else
                {
                    selected = "";
                }
                html = html + ("<option" + selected + ">" + option + "</option>");
            }
         /*
            if (selectedoptions != "" && !selectoption.Contains(selectedoptions))
            {
                html = html + ("<option selected>" + selectedoption + "</option>");
            }
         */

            return html;
        }

        public static string populateselectwithvalue(string[,] options, string selectedoption, string firstoption = "None", string firstvalue = "")
        {
            string selected;
            string html = "";
            if (firstoption != "None")
            {
                html = html + ("<option value=\"" + firstvalue + "\"> " + firstoption + "</option>");

            }

            for (int f1 = 0; f1 < options.GetLength(0); f1++)
            {
                if (options[f1, 1] == selectedoption)
                {
                    selected = " selected";
                }
                else
                {
                    selected = "";
                }
                html = html + ("<option value=\"" + options[f1, 1] + "\"" + selected + ">" + options[f1, 0] + "</option>");
            }
            return html;
        }

        public static string buildandpopulateselect(string strConnString, string cmdtext, string selectedoption, Dictionary<string, string> options, string firstoption = "None")
        {
            string selected;
            string html = "";

            //string[] selectedoptions = selectedoption.Split(',');
            if (selectedoption != "")
            {
                selectedoption = "," + selectedoption + ",";
            }

            if (firstoption != "None")  // could be "Please Select" etc
            {
                html = html + ("<option>" + firstoption + "</option>");
            }

            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd;

            if (options.ContainsKey("storedprocedure"))
            {
                if (options.ContainsKey("storedprocedurename"))
                {
                    cmd = new SqlCommand(cmdtext, con);
                    cmd.CommandType = CommandType.StoredProcedure;

                }
                else
                {
                    cmd = new SqlCommand("builddropdownlist", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@sqltext", SqlDbType.VarChar).Value = cmdtext;
                }
                if (options.ContainsKey("parameters"))
                {
                    cmd.Parameters.Add("@parameters", SqlDbType.VarChar).Value = options["parameters"];

                }
            }
            else
            {
                cmd = new SqlCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = cmdtext;
            }

            cmd.Connection = con;
            try
            {
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    string Label;
                    string Value = "";
                    string ValueText = "";
                    string selectText = "";
                    while (dr.Read())
                    {
                        Label = dr["label"].ToString();
                        if (options.ContainsKey("usevalues"))
                        {
                            Value = dr["value"].ToString();
                            ValueText = " value=\"" + Value + "\"";
                        }
                        //if (options["selecttype"] == "Label")
                        if (options.ContainsKey("comparelabel"))
                        {
                            selectText = Label;
                        }
                        else //compare value
                        {
                            selectText = Value;
                        }
                        //foreach (string option in selectedoptions)
                        //{
                        //if (selectText == selectedoption) 
                        if (selectedoption.Contains("," + selectText + ","))
                        {
                            selected = " selected";
                        }
                        else
                        {
                            selected = "";
                        }
                        html = html + ("<option" + ValueText + selected + ">" + Label + "</option>");
                        //}
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

            return html;
        }

        public string[,] buildselectarray(string strConnString, string cmdtext)
        {
            string[,] SelectArray = null;

            var myList = new List<SelectList>();

            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = cmdtext;

            cmd.Connection = con;
            try
            {
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    while (dr.Read())
                    {

                        myList.Add(new SelectList()
                        {
                            Label = dr["label"].ToString(),
                            Value = dr["value"].ToString()
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

            SelectArray = new string[myList.Count, 2];
            for (int i = 0; i < myList.Count; i++)
            {
                SelectArray[i, 0] = ((SelectList)myList[i]).Label;
                SelectArray[i, 1] = ((SelectList)myList[i]).Value;
            }

            return SelectArray;
        }

        public static string populateselect_ccmonth(string selectedoption, string firstoption = "None")
        {
            string[] options = new string[12];
            for (int i = 0; i <= 11; i++)
            {
                options[i] = (i + 1).ToString("00");
            }


            return populateselect(options, selectedoption, firstoption);
        }

        public static string populateselect_ccyear(string selectedoption, string firstoption = "None")
        {
            int thisyear = DateTime.Now.Year;
            string[] options = new string[10];
            for (int i = 0; i <= 9; i++)
            {
                options[i] = (thisyear + i).ToString();
            }


            return populateselect(options, selectedoption, firstoption);
        }
        /*
        public async Task<string> SendMessage(string PhoneNumber, string Message)
        {
            string finalresponse = "";

            if (PhoneNumber == "" || Message == "")
            {
                finalresponse = "Invalid Parameters";
            }
            else
            {
                String strConnString = "Data Source=192.168.10.6;Initial Catalog=SMS;Integrated Security=False;user id=OnlineServices;password=Whanganui497";
                SqlConnection con = new SqlConnection(strConnString);

                SqlCommand cmd = new SqlCommand("GET_PARAMETER", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@parameter", SqlDbType.VarChar).Value = "IPAddress";

                cmd.Connection = con;
                try
                {
                    con.Open();
                    //SqlDataReader dr = cmd.ExecuteReader();
                    IPAddress = cmd.ExecuteScalar().ToString();

                }
                catch (Exception ex)
                {
                    //Console.WriteLine(ex.Message);
                    //Console.WriteLine(ex.InnerException);
                    throw ex;
                }
                finally
                {
                    con.Close();
                }
                cmd.Parameters.Clear();
                cmd.Parameters.Add("@parameter", SqlDbType.VarChar).Value = "Port";

                cmd.Connection = con;
                try
                {
                    con.Open();
                    //SqlDataReader dr = cmd.ExecuteReader();
                    Port = cmd.ExecuteScalar().ToString();

                }
                catch (Exception ex)
                {
                    //Console.WriteLine(ex.Message);
                    //Console.WriteLine(ex.InnerException);
                    throw ex;
                }
                finally
                {
                    con.Close();
                }


                using (var client = new HttpClient())
                {
                    string url = ConstructBaseUri();
                    client.BaseAddress = new Uri(url);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    if (!string.IsNullOrEmpty(UserName) && !string.IsNullOrEmpty(Password))
                    {
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                                    "Basic",
                                     Convert.ToBase64String(
                                     ASCIIEncoding.ASCII.GetBytes(
                                     string.Format("{0}:{1}", UserName, Password))));
                    }
                    var postData = new List<KeyValuePair<string, string>>();
                    postData.Add(new KeyValuePair<string, string>("to", PhoneNumber));
                    postData.Add(new KeyValuePair<string, string>("message", Message));
                    HttpContent content = new FormUrlEncodedContent(postData);

                    HttpResponseMessage response = await client.PostAsync(MessagesUrlPath, content);

                    if (response.IsSuccessStatusCode)
                    {
                        PostMessageResponse result = await response.Content.ReadAsAsync<PostMessageResponse>();
                        if (result.IsSuccessful)
                        {
                            finalresponse = result.ToString();
                        }
                        else
                        {
                            finalresponse = result.Description;
                        }
                    }
                    else
                    {
                        finalresponse = response.ToString();
                    }

                }
            }
            return finalresponse;

        }
        */

        protected string ConstructBaseUri()
        {
            UriBuilder uriBuilder = new UriBuilder("http", IPAddress, Convert.ToInt32(Port));
            return uriBuilder.ToString();
        }

        private const string NetworkInfoUrlPath = "services/api/status/network";

        private const string BatteryInfoUrlPath = "services/api/status/battery";

        private const string MessagesUrlPath = "services/api/messaging";

        private const string MessageStatusUrlPath = "services/api/messaging/status";
        public void populateXML(DataTable repeatertable, XElement rootXml)
        {

            DataView dv2 = new DataView(repeatertable);
            DataTable dvSites = dv2.ToTable(true, "Name");
            foreach (DataRow siterow in dvSites.Rows)
            {
                XElement repeaterXml = new XElement(siterow["Name"].ToString() + "Repeater");

                string sel = "[Name] = '" + siterow["Name"] + "'";
                DataView dv3 = new DataView(repeatertable, sel, "", DataViewRowState.CurrentRows);
                DataTable dvindexess = dv3.ToTable(true, "Index");
                foreach (DataRow indexrow in dvindexess.Rows)
                {
                    XElement subXml = new XElement(siterow["Name"].ToString());

                    subXml.Add(new XElement(siterow["Name"].ToString() + "Index", indexrow["Index"].ToString()));

                    sel = "[Name] = '" + siterow["Name"] + "' AND [Index] = '" + indexrow["Index"] + "'";
                    DataView dv4 = new DataView(repeatertable, sel, "", DataViewRowState.CurrentRows);
                    DataTable dvfields = dv4.ToTable();
                    foreach (DataRow fieldrow in dvfields.Rows)
                    {
                        subXml.Add(new XElement(fieldrow["Field"].ToString(), fieldrow["Value"].ToString()));
                    }
                    repeaterXml.Add(subXml);
                }
                rootXml.Add(repeaterXml);
            }
        }

        private static byte[] GetKey(string password)
        {
            string pwd = null;

            if (Encoding.UTF8.GetByteCount(password) < 24)
            {
                pwd = password.PadRight(24, ' ');
            }
            else
            {
                pwd = password.Substring(0, 24);
            }
            return Encoding.UTF8.GetBytes(pwd);
        }

        /// <summary>
        /// Crypto strings using 3DES (192 bits)
        /// </summary>
        public string Encrypt(string data)
        {
            TripleDESCryptoServiceProvider DES = new TripleDESCryptoServiceProvider();

            DES.Mode = CipherMode.ECB;
            DES.Key = GetKey("a1!B78s!5(");

            DES.Padding = PaddingMode.PKCS7;
            ICryptoTransform DESEncrypt = DES.CreateEncryptor();
            Byte[] Buffer = ASCIIEncoding.ASCII.GetBytes(data);

            return Convert.ToBase64String(DESEncrypt.TransformFinalBlock(Buffer, 0, Buffer.Length));
        }

        /// <summary>
        /// Descrypto strings using 3DES (192 bits)
        /// </summary>
        public string Decrypt(string data)
        {
            TripleDESCryptoServiceProvider DES = new TripleDESCryptoServiceProvider();

            DES.Mode = CipherMode.ECB;
            DES.Key = GetKey("a1!B78s!5(");

            DES.Padding = PaddingMode.PKCS7;
            ICryptoTransform DESEncrypt = DES.CreateDecryptor();
            Byte[] Buffer = Convert.FromBase64String(data.Replace(" ", "+"));

            return Encoding.UTF8.GetString(DESEncrypt.TransformFinalBlock(Buffer, 0, Buffer.Length));
        }

    }


    /*
            public static gw_Result PXPost(string cardholder, string cardnumber, string amount, string expirydate, string narrative)
            {

                gw_Result gwResult = new gw_Result();

                string URI = @"https://sec.paymentexpress.com/pxpost.aspx";
                // form the PXPost Xml message
                StringWriter sw = new StringWriter();
                XmlTextWriter xtw = new XmlTextWriter(sw);
                xtw.WriteStartElement("Txn");
                xtw.WriteElementString("PostUsername", "WanganuiDCDev");   //ANZWDC
                xtw.WriteElementString("PostPassword", "test1234");        //1A5B9A93
                xtw.WriteElementString("CardHolderName", cardholder);
                xtw.WriteElementString("CardNumber", cardnumber);
                xtw.WriteElementString("Amount", amount);
                xtw.WriteElementString("DateExpiry", expirydate);
                xtw.WriteElementString("Cvc2", "");
                xtw.WriteElementString("InputCurrency", "NZD");
                xtw.WriteElementString("TxnType", "Purchase");
                xtw.WriteElementString("TxnId", "");
                xtw.WriteElementString("MerchantReference", narrative);
                //xtw.WriteElementString("TxnData1", "This is optional data, there are 3 fields");
                //xtw.WriteElementString("TxnData2", "");
                //xtw.WriteElementString("TxnData3", "");
                xtw.WriteEndElement();
                xtw.Close();
                // Send the Xml message to PXPost
                WebRequest wrq = WebRequest.Create(URI);
                wrq.Method = "POST";
                wrq.ContentType = "application/x-www-form-urlencoded";
                byte[] b = Encoding.ASCII.GetBytes(sw.ToString());
                wrq.ContentLength = b.Length;
                Stream s = wrq.GetRequestStream();
                s.Write(b, 0, b.Length);
                s.Close();
                // Check the response
                WebResponse wrs = wrq.GetResponse();

                gwResult.xmloutput = "";
                gwResult.success = 0;
                gwResult.message = "";
                gwResult.reference = "";

                if (wrs != null)
                {
                    StreamReader sr = new StreamReader(wrs.GetResponseStream());
                    XmlDocument xd = new XmlDocument();
                    xd.LoadXml(sr.ReadToEnd().Trim());

                    gwResult.xmloutput = xd.OuterXml;

                    if (xd.SelectSingleNode("/Txn/Success") != null)
                    {
                        //ReCo = xd.SelectSingleNode("/Txn/ReCo").InnerText; //Ignore
                        gwResult.success = Convert.ToInt16(xd.SelectSingleNode("/Txn/Success").InnerText);  //1 if transaction successful - 0 if declined or unsuccessful. Will be the same value as Authorized
                    }

                    //responsetext = xd.SelectSingleNode("/Txn/ResponseText").InnerText;
                    //helptext = xd.SelectSingleNode("/Txn/HelpText").InnerText;
                    //Some response elements are in different nodes
                    //authorized = xd.SelectSingleNode("/Txn/Transaction/Authorized").InnerText; //Indicates if the transaction was authorized or not. Either False (0) or True (1)
                    gwResult.reference = xd.SelectSingleNode("/Txn/Transaction/DpsTxnRef").InnerText;
                    //authcode = xd.SelectSingleNode("/Txn/Transaction/AuthCode").InnerText;  

                    gwResult.message = "To do";

                    // further error handling code could go here



                }
                // error handling code omitted
                return gwResult;

            }
    */
}

public class SelectList
{
    public string Label { get; set; }
    public string Value { get; set; }
}

public class google_geocodeClass
{
    public String address;
    public String lat;
    public String lng;
}
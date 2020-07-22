using System;
using System.Web.Services;
using System.IO;
using Generic;
using System.Collections.Generic;

namespace TeOraHouWhanganui.Private
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
        public string send_text(string PhoneNumber, string Message)
        {
            //Generic.Functions gFunctions = new Generic.Functions();
            //string response = gFunctions.SendRemoteMessage(PhoneNumber, Message, "TOH Whanganui Communications");
            return "Not Yet Done"; // response;
        }

        [WebMethod]
        //[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public google_geocodeClass googlegeocode(string address)    //you can't pass any querystring params
        {
            Dictionary<string, string> options = new Dictionary<string, string>();
            google_geocodeClass google_geocode = Functions.google_geocode("AIzaSyCCpsWhkuuHlAe6EKhSi5zSlmmIVMN9M8c", address, options);

            return (google_geocode);
        }

        [WebMethod]
        //[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public standardResponse SaveImage(string imageData, string id)    //you can't pass any querystring params
        {
            //string dt = DateTime.Now.ToString("ddMMyyHHss");
            if (1 == 1)
            {
                string path = Server.MapPath(".\\images");

                //string path = @"F:\InetPub\Online\Assets\Cemetery\Images\" + id;
                Directory.CreateDirectory(path + "\\original");
                //string fileName = "\\" + id + "_" + dt + ".jpg";
                string fileName = "\\" + id + ".jpg";
                using (FileStream fs = new FileStream(path + "\\original" + fileName, FileMode.Create))
                {
                    using (BinaryWriter bw = new BinaryWriter(fs))
                    {
                        byte[] data = Convert.FromBase64String(imageData);
                        bw.Write(data);
                        bw.Close();
                    }
                }

                using (System.Drawing.Image original = System.Drawing.Image.FromFile(path + "\\original" + fileName))
                {
                    double scaler = Convert.ToDouble(original.Width / 640.000000);
                    int newHeight = Convert.ToInt16(original.Height / scaler);
                    int newWidth = 640;



                    using (System.Drawing.Bitmap newPic = new System.Drawing.Bitmap(newWidth, newHeight))
                    {
                        using (System.Drawing.Graphics gr = System.Drawing.Graphics.FromImage(newPic))
                        {
                            gr.DrawImage(original, 0, 0, (newWidth), (newHeight));
                            newPic.Save(path + fileName, System.Drawing.Imaging.ImageFormat.Jpeg);
                        }
                    }
                }


            }
            standardResponse resultclass = new standardResponse();
            resultclass.status = "Saved";
            resultclass.message = "";

            return (resultclass);
        }
    }
    public class standardResponse
    {
        public string status;
        public string message;
    }
    public class geocode
    {
        public string address;
        public string lat;
        public string lng;
        public string coordinates;
    }
}

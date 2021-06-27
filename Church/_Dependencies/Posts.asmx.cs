using System;
using myGeneric = Generic;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Threading;
using System.IO;

namespace Church._Dependencies
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
        //[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public standardResponse SaveImage(string imageData, string id)    //you can't pass any querystring params
        {
            //string dt = DateTime.Now.ToString("ddMMyyHHss");
            if (1 == 1)
            {
                string path = Server.MapPath("\\images");

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
}

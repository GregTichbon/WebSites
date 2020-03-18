using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using QRCoder;
using System.Data.SqlClient;
using System.Data;

namespace office.dataInn.co.nz.Raffles.UBC2019B
{
    public partial class CreateWinnersVouchers : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string folder = Server.MapPath("~\\raffles\\images");
            string sourceImage = folder + "\\ChefsChoiceWinningTicket2019B.jpg";
            //sourceImage = folder + "\\Blank.jpg";
            ImageFormat fmt = ImageFormat.Jpeg;

            string strConnString = "Data Source=toh-app;Initial Catalog=DataInnovations;Integrated Security=False;user id=OnlineServices;password=Whanganui497";
            SqlConnection con = new SqlConnection(strConnString);
            con.Open();

            string sql = @"select R.identifier, T.purchaser, t.TicketNumber, w.guid, W.Draw, W.drawndate
                from RaffleWinner W
                inner join raffleticket T on T.RaffleTicket_ID = W.RaffleTicket_ID
                inner join Raffle R on R.Raffle_ID = T.Raffle_ID where isnull(W.status,'') = 'Winner'
                order by isnull(winnerstatus,'') desc, R.identifier, w.Draw";

            SqlCommand cmd1 = new SqlCommand(sql, con);
            //cmd1.Parameters.Add("@guid", SqlDbType.VarChar).Value = guid;

            cmd1.CommandType = CommandType.Text;
            cmd1.Connection = con;

            try
            {
                SqlDataReader dr = cmd1.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        string name = dr["purchaser"].ToString();
                        //mobile = dr["mobile"].ToString();
                        //email = dr["emailaddress"].ToString();
                        string ticketnumber = dr["ticketnumber"].ToString();
                        string guid = dr["guid"].ToString();
                        string identifier = dr["identifier"].ToString();
                        string draw = dr["draw"].ToString();
                        string drawndate = "";
                        if (dr["drawndate"] != DBNull.Value)
                        {
                            drawndate = Convert.ToDateTime(dr["drawndate"]).ToString("dd MMM yy");
                        }
                        //greeting = dr["greeting"].ToString();
                        //winnerstatus = dr["winnerstatus"].ToString();
                        //winnerresponse = dr["winnerresponse"].ToString();
                        //winnernote = dr["winnernote"].ToString();
                        //string text = name + Environment.NewLine + "Ticket: " + identifier + " - " + ticketnumber;

                        string text = "Ticket: " + identifier + "/" + ticketnumber + " " + name;
                        string targetImage = folder + "\\vouchers\\" + guid + ".jpg";
                        Response.Write(name + " " + identifier + " " + ticketnumber + " <a href=\"http://office.datainn.co.nz/raffles/images/vouchers/" + guid + ".jpg" + "\">" + "http://office.datainn.co.nz/raffles/images/vouchers/" + guid + ".jpg" + "</a><br />");

                         
                        QRCodeGenerator qrGenerator = new QRCodeGenerator();
                        QRCodeData qrCodeData = qrGenerator.CreateQrCode("http://office.datainn.co.nz/raffles/UBC2019B/voucher.aspx?id=" + guid, QRCodeGenerator.ECCLevel.Q);
                        QRCode qrCode = new QRCode(qrCodeData);
                        Bitmap qrCodeImage = qrCode.GetGraphic(20);

                        Bitmap resized = new Bitmap(qrCodeImage, new Size(Convert.ToInt16(qrCodeImage.Width * .85), Convert.ToInt16(qrCodeImage.Height * .85)));

                        //using (System.Drawing.Image image = System.Drawing.Image.FromFile(sourceImage))
                        //using (Graphics imageGraphics = Graphics.FromImage(image))
                        System.Drawing.Image image = System.Drawing.Image.FromFile(sourceImage);
                        Graphics imageGraphics = Graphics.FromImage(image);

                        using (TextureBrush watermarkBrush = new TextureBrush(resized))
                        {
                            int x = 1400; // (image.Width / 2 - resized.Width / 2);
                            int y = 1350; // (image.Height / 2 - resized.Height / 2) + 200;
                            watermarkBrush.TranslateTransform(x, y);
                            imageGraphics.FillRectangle(watermarkBrush, new Rectangle(new Point(x, y), new Size(resized.Width + 1, resized.Height)));
                        }

                        using (Font Font = new Font("Arial", 26))
                        {
                            float textwidth = imageGraphics.MeasureString(text, Font, 0, StringFormat.GenericTypographic).Width;
                            int x = (image.Width / 2 - Convert.ToInt16(textwidth) / 2);
                            int y = 1200;
                            imageGraphics.DrawString(text, Font, Brushes.Blue, x, y);
                        }

                        using (Font Font = new Font("Arial", 26))
                        {
                            float textwidth = imageGraphics.MeasureString(text, Font, 0, StringFormat.GenericTypographic).Width;
                            int x = 1300;
                            int y = 50;
                            imageGraphics.DrawString("Draw " + draw + " - " + drawndate, Font, Brushes.Yellow, x, y);
                        }

                        BarcodeLib.Barcode b1 = new BarcodeLib.Barcode();
                        b1.BackColor = System.Drawing.Color.White;
                        b1.ForeColor = System.Drawing.Color.Black;
                        b1.IncludeLabel = true;
                        b1.Alignment = BarcodeLib.AlignmentPositions.CENTER;
                        b1.LabelPosition = BarcodeLib.LabelPositions.BOTTOMCENTER;
                        //b1.ImageFormat = System.Drawing.Imaging.ImageFormat.Jpeg;
                        Font font = new System.Drawing.Font("verdana", 20f);
                        b1.LabelFont = font;
                        b1.Height = 100;
                        b1.Width = 400;

                        System.Drawing.Image BarCodeimage = b1.Encode(BarcodeLib.TYPE.CODE39Extended, "20200218005005");
                        imageGraphics.DrawImage(BarCodeimage, 1250, 2300);


                        image.Save(targetImage);

                    }
                }

                dr.Close();
            }

            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}
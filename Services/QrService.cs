using QRCoder;
using System.Drawing;
using System.IO;
using System.Windows.Media.Imaging;

namespace SimGamepad.Services
{
    public static class QrService
    {
        public static BitmapImage Generate(string text)
        {
            QRCodeGenerator gen = new QRCodeGenerator();
            QRCodeData data = gen.CreateQrCode(text, QRCodeGenerator.ECCLevel.Q);
            QRCode code = new QRCode(data);

            using Bitmap bmp = code.GetGraphic(20);
            using MemoryStream ms = new MemoryStream();
            bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Png);

            BitmapImage img = new BitmapImage();
            img.BeginInit();
            img.StreamSource = new MemoryStream(ms.ToArray());
            img.EndInit();
            return img;
        }
    }
}

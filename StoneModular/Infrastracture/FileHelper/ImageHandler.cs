using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Infrastructure.FileHelper
{
    public class ImageHandler
    {
        public static byte[] ImageScale(HttpPostedFileBase MyFile, int newWidth, int newHeight)
        {
            Bitmap bitmap = new Bitmap(MyFile.InputStream);

            int oldWidth = bitmap.Width;
            int oldHeight = bitmap.Height;

            GraphicsUnit units = System.Drawing.GraphicsUnit.Pixel;
            RectangleF r = bitmap.GetBounds(ref units);
            Size newSize = new Size();

            float expectedWidth = r.Width;
            float expectedHeight = r.Height;
            float dimesion = r.Width / r.Height;

            if (newWidth < r.Width)
            {
                expectedWidth = newWidth;
                expectedHeight = expectedWidth / dimesion;
            }
            else if (newHeight < r.Height)
            {
                expectedHeight = newHeight;
                expectedWidth = dimesion * expectedHeight;
            }
            if (expectedWidth > newWidth)
            {
                expectedWidth = newWidth;
                expectedHeight = expectedHeight / expectedWidth;
            }
            else if (expectedHeight > newHeight)
            {
                expectedHeight = newHeight;
                expectedWidth = dimesion * expectedHeight;
            }
            newSize.Width = (int)Math.Round(expectedWidth);
            newSize.Height = (int)Math.Round(expectedHeight);

            Bitmap b = new Bitmap(bitmap, newSize);

            Image img = (Image)b;

            byte[] data = ImageToByte(img);

            return data;
        }
        public static byte[] ImageToByte(Image img)
        {
            byte[] byteArray = new byte[0];
            using (MemoryStream stream = new MemoryStream())
            {
                img.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                stream.Close();

                byteArray = stream.ToArray();
            }
            return byteArray;
        }
    }
}

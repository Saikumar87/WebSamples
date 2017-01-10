using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DealingWithStreams
{
    class Program
    {
        static void Main(string[] args)
        {
            //ProcessImageFile();
            CopyAnyFile("KPIs.xml");
            CopyAnyFile("SampleVideo_1280x720_10mb.mp4");
            CopyAnyFile("SampleAudio_0.7mb.mp3");
            Console.ReadLine();

        }


        #region  image

        private static void ProcessImageFile()
        {
            Console.Write("Converting Image to Bytes");
            byte[] imagebytes = ImageToByteArray(@"D:\PD\WebAPI_NET\WebAPIs_Lab\DealingWithStreams\Source\s.png");
            Console.WriteLine("Converting Bytes to Image at destination");
            ByteArrayToImageFilebyMemoryStream(imagebytes, @"D:\PD\WebAPI_NET\WebAPIs_Lab\DealingWithStreams\Destination\d.png");

            Console.WriteLine("new file created at destination");
        }


        public static byte[] ImageToByteArray(string imagefilePath)
        {
            System.Drawing.Image image = System.Drawing.Image.FromFile(imagefilePath);
            byte[] imageByte = ImageToByteArraybyMemoryStream(image);
            return imageByte;
        }

        private static byte[] ImageToByteArraybyMemoryStream(Image image)
        {
            MemoryStream ms = new MemoryStream();
            image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            return ms.ToArray();
        }
        public static Image ByteArrayToImagebyMemoryStream(byte[] imageByte)
        {
            MemoryStream ms = new MemoryStream(imageByte);
            Image image = Image.FromStream(ms);
            return image;
        }
        public static void ByteArrayToImageFilebyMemoryStream(byte[] imageByte, string ImageFilePath)
        {
            MemoryStream ms = new MemoryStream(imageByte);
            Image image = Image.FromStream(ms);
            image.Save(ImageFilePath);
        }
        #endregion


        #region AnyFile


        static void CopyAnyFile(string filename)
        {
            string sourcefullpath = @"D:\PD\WebAPI_NET\WebAPIs_Lab\DealingWithStreams\Source\" + filename;
            string destinationfullpath = @"D:\PD\WebAPI_NET\WebAPIs_Lab\DealingWithStreams\Destination\" + filename;
            FileStream stream = File.OpenRead(sourcefullpath);
            byte[] fileBytes = new byte[stream.Length];

            stream.Read(fileBytes, 0, fileBytes.Length);
            stream.Close();
            //Begins the process of writing the byte array back to a file

            using (Stream file = File.OpenWrite(destinationfullpath))
            {
                file.Write(fileBytes, 0, fileBytes.Length);
            }
        }
        #endregion
    }
}

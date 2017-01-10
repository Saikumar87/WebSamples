using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Hosting;

namespace BasicHTTPServer
{


    public class RequestHandler
    {
        public enum Authorization
        {
            Basic=1,
            Digest=2,
            Form=3,
            Anonymous=4
        }
        public RequestHandler()
        {
            
        }

        public HttpListenerResponse Handle(HttpListenerContext context,Authorization auth)
        {
            bool IsAuthorized = false;
            HttpListenerResponse response = context.Response;
           switch(auth)
            {
                case Authorization.Anonymous:IsAuthorized = true;break;
                case Authorization.Basic:IsAuthorized = CheckBasicAuthorization(context);break;
                   
            }
            if (IsAuthorized)
            {

                if (context.Request.Url.GetComponents(UriComponents.Path, UriFormat.SafeUnescaped).ToLower().Contains("images"))
                {
                    byte[] buffer = ImageRes(@"D:\s.png");
                    response.ContentLength64 = buffer.Length;
                    response.ContentType = "image/png";
                    ////Without the belowline the outstream content is inline - Commented for now
                    //response.Headers.Add("Content-Disposition", "attachment; filename=s.png");
                    System.IO.Stream output = response.OutputStream;
                    output.Write(buffer, 0, buffer.Length);
                    // You must close the output stream.
                    output.Close();
                }
                else if (context.Request.Url.GetComponents(UriComponents.Path, UriFormat.SafeUnescaped).ToLower().Contains(".html"))
                {
                    //string filepath = HostingEnvironment.MapPath("~/Content/Index.html");
                    var _assembly = System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase;
                    var _path = System.IO.Path.GetDirectoryName(_assembly);
                    string filepath = Path.Combine(_path, @"Content\Index.html");
                    FileStream fileStream = new FileStream(@"D:\PD\WebAPI_NET\WebAPIs_Lab\BasicHTTPServer\bin\Debug\Content\Index.html", FileMode.Open);
                    byte[] fileBytes = new byte[fileStream.Length];
                    fileStream.Read(fileBytes, 0, fileBytes.Length);
                    fileStream.Close();
                    // Get a response stream and write the response to it.
                    response.ContentLength64 = fileBytes.Length;
                    response.ContentType = "text/html";
                    System.IO.Stream output = response.OutputStream;
                    output.Write(fileBytes, 0, fileBytes.Length);
                    // You must close the output stream.
                    output.Close();

                }
                else if (context.Request.Url.GetComponents(UriComponents.Path, UriFormat.SafeUnescaped).ToLower().Contains("videos"))
                {
                    string sourcefullpath = @"D:\PD\WebAPI_NET\WebAPIs_Lab\DealingWithStreams\Source\SampleVideo_1280x720_10mb.mp4";
                    FileStream stream = File.OpenRead(sourcefullpath);
                    byte[] fileBytes = new byte[stream.Length];
                    stream.Read(fileBytes, 0, fileBytes.Length);
                    stream.Close();                    
                    response.ContentLength64 = fileBytes.Length;
                    response.ContentType = "video/mp4";
                    ////Without the belowline the outstream content is inline - Commented for now
                    response.Headers.Add("Content-Disposition", "attachment; filename=SampleVideo_1280x720_10mb.mp4");
                    System.IO.Stream output = response.OutputStream;
                    output.Write(fileBytes, 0, fileBytes.Length);
                   
                    // You must close the output stream.
                    output.Close();
                }
                else
                {
                    #region text response
                    //Construct a response.
                    string responseString = " <HTML><BODY> Default Content</BODY></HTML>";
                    byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
                    // Get a response stream and write the response to it.
                    response.ContentLength64 = buffer.Length;
                    response.ContentType = "text/html";
                    System.IO.Stream output = response.OutputStream;
                    output.Write(buffer, 0, buffer.Length);
                    // You must close the output stream.
                    output.Close();
                    #endregion
                }

        }
        else
        {
                #region text response
                //Construct a response.
                string responseString = "<HTML><BODY>You are not authorized to see this page</BODY></HTML>";
                byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
                // Get a response stream and write the response to it.
                response.ContentLength64 = buffer.Length;
                response.ContentType = "text/html";
                System.IO.Stream output = response.OutputStream;
                output.Write(buffer, 0, buffer.Length);
                // You must close the output stream.
                output.Close();
                #endregion
            }
            return response;

        }

        private bool CheckBasicAuthorization(HttpListenerContext context)
        {
            HttpListenerBasicIdentity identity = (HttpListenerBasicIdentity)context.User.Identity;
            if (identity.Name.Equals("v-saikum@microsoft.com"))
                return true;
            else
                return false;
        }

        #region Helper methods
        private byte[] ReadFully(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }

        private byte[] ImageRes(string filepath)
        {


            FileStream fileStream = new FileStream(filepath, FileMode.Open);
            Image image = Image.FromStream(fileStream);
            MemoryStream memoryStream = new MemoryStream();
            image.Save(memoryStream, ImageFormat.Png);
            fileStream.Close();
            return memoryStream.ToArray();
            
        }


        #endregion
    }

}
    
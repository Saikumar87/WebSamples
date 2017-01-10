using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;


namespace BasicHTTPServer
{
    class Program
    {
        static void Main(string[] args)
        {
            NoAuthListener();
            //DigestAuthListener();


        }

        static void NoAuthListener()
        {
            HttpListener listener = new HttpListener();
            listener.Prefixes.Add("http://localhost:9999/");
           
            listener.Start();

            Console.WriteLine("Listening...");
            RequestHandler handler = new RequestHandler();
            while (true)
            {
                // Note: The GetContext method blocks while waiting for a request. 
                HttpListenerContext context = listener.GetContext();
                Console.WriteLine("Got New request");
              

                handler.Handle(context, RequestHandler.Authorization.Anonymous);


            }
            listener.Stop();
        }

        static void BasicAuthListener()
        {
            HttpListener listener = new HttpListener();
            listener.Prefixes.Add("http://localhost:9999/");
            listener.AuthenticationSchemes = AuthenticationSchemes.Basic;
            listener.Start();

            Console.WriteLine("Listening...");
            RequestHandler handler = new RequestHandler();
            while (true)
            {
                // Note: The GetContext method blocks while waiting for a request. 
                HttpListenerContext context = listener.GetContext();
                Console.WriteLine("Got New request");
                HttpListenerBasicIdentity identity = (HttpListenerBasicIdentity)context.User.Identity;
                Console.WriteLine(identity.Name);
                Console.WriteLine(identity.Password);

                handler.Handle(context,RequestHandler.Authorization.Basic);


            }
            listener.Stop();
        }

        static void DigestAuthListener()
        {
            HttpListener listener = new HttpListener();
            listener.Prefixes.Add("http://localhost:9999/");
            listener.AuthenticationSchemes = AuthenticationSchemes.Digest;
            listener.Start();

            Console.WriteLine("Listening...");
            RequestHandler handler = new RequestHandler();
            while (true)
            {
                // Note: The GetContext method blocks while waiting for a request. 
                HttpListenerContext context = listener.GetContext();
                Console.WriteLine("Got New request");

                handler.Handle(context, RequestHandler.Authorization.Anonymous);


            }
            listener.Stop();
        }

    }
}

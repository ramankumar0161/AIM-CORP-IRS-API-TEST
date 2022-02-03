using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace IrsSubmission.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            //ServicePointManager.Expect100Continue = true;
            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            //WebRequest request = WebRequest.Create("https://localhost:44300/api/values/login");
            //// If required by the server, set the credentials.
            //request.Credentials = CredentialCache.DefaultCredentials;

            //// Get the response.
            //WebResponse response = request.GetResponse();
            //// Display the status.
            //Console.WriteLine(((HttpWebResponse)response).StatusDescription);

            //// Get the stream containing content returned by the server.
            //// The using block ensures the stream is automatically closed.
            //using (Stream dataStream = response.GetResponseStream())
            //{
            //    // Open the stream using a StreamReader for easy access.
            //    StreamReader reader = new StreamReader(dataStream);
            //    // Read the content.
            //    string responseFromServer = reader.ReadToEnd();
            //    // Display the content.
            //    Console.WriteLine(responseFromServer);
            //}

            //// Close the response.
            //response.Close();
            //return View(((HttpWebResponse)response).StatusDescription);
            return View();
        }
    }
}

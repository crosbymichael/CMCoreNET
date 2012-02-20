using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMCoreNET;
using CMCoreNET.Web;

namespace DebugTests
{
    class Program
    {
        private static WebServer server;

        [STAThread]
        static void Main(string[] args)
        {
            server = new WebServer(@"http://crosbymichael.com");

            Console.WriteLine("Test path:");
            Console.WriteLine(server.Path);
            TestHeaders();
            server.ContentType = "Content-Type:application/json";

            server.RegisterSimpleCallback((s) => {
                Console.WriteLine(s);
            });

            server.Get();

            Console.ReadLine();
        }

        static void TestHeaders() {
            Console.WriteLine("Test headers:");

            server.AddHeader(System.Net.HttpRequestHeader.ContentType, "json");
            server.AddHeader(System.Net.HttpRequestHeader.Authorization, "token");
        }
    }
}

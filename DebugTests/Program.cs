using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using CMCoreNET;
using CMCoreNET.Web;
using CMCoreNET.Serialization;

namespace DebugTests
{
    class Program
    {

        [STAThread]
        static void Main(string[] args)
        {
            Test(new { Name = "micahel", Age = "25" });
            /*
            var testObj = new TestObj();

            SerializationAdapter json = SerializationAdapter.GetAdapter(SerializationAdapterType.JSON);
            SerializationAdapter xml = SerializationAdapter.GetAdapter(SerializationAdapterType.XML);

            string jsonString = json.Serialize(testObj);
            string xmlString = xml.Serialize(testObj);

            Console.WriteLine(xmlString);
            Console.WriteLine("\n");
            Console.WriteLine(jsonString);

            TestObj backAgain = json.Deserialize<TestObj>(jsonString);

            json.GetMe();
            Type t = json.t;
            Console.WriteLine(t.Name);
             */
            Console.ReadLine();

            
        }

        static void Test(object ann) {
            var properties = ann.GetType().GetProperties().ToList();

            foreach (var prop in properties) {
                Console.WriteLine(prop.Name);
            }
        }
    }

    [Serializable]
    public class TestObj
    {
        public string Name;
        public List<string> Posts;

        public TestObj() {
            Name = "mike";

            Posts = new List<string>();
            Posts.Add("Rails");
            Posts.Add("C#");
            Posts.Add("Python");
        }
    }
}

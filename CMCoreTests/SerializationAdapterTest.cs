using CMCoreNET.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace CMCoreTests
{
    
    
    /// <summary>
    ///This is a test class for SerializationAdapterTest and is intended
    ///to contain all SerializationAdapterTest Unit Tests
    ///</summary>
    [TestClass()]
    public class SerializationAdapterTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion

        [TestMethod()]
        public void TestSerializerFactory() {
            var json = SerializationAdapter.GetAdapter(SerializationAdapterType.JSON);
            var xml = SerializationAdapter.GetAdapter(SerializationAdapterType.XML);


            Assert.IsNotNull(json);
            Assert.AreEqual<Type>(typeof(JsonSerializer), json.GetType(), "Incorrect type");
            Assert.IsNotNull(xml);
            Assert.AreEqual<Type>(typeof(XmlSerializer), xml.GetType(), "Incorrect type");
        }


        [TestMethod()]
        public void TestXmlSerialization() {
            var xml = SerializationAdapter.GetAdapter(SerializationAdapterType.XML);
            var test = new TestObj();
            string xmlData = xml.Serialize(test);

            Assert.IsTrue(!string.IsNullOrEmpty(xmlData));
            
        }

        [TestMethod()]
        public void TestJsonSerialization()
        {
            var xml = SerializationAdapter.GetAdapter(SerializationAdapterType.JSON);
            var test = new TestObj();
            string xmlData = xml.Serialize(test);

            Assert.IsTrue(!string.IsNullOrEmpty(xmlData));

        }

        [TestMethod()]
        public void TestXmlDeserialize()
        {
            var xml = SerializationAdapter.GetAdapter(SerializationAdapterType.XML);
            var test = new TestObj();
            string xmlData = xml.Serialize(test);

            var expect = xml.Deserialize<TestObj>(xmlData);

            Assert.AreEqual(expect.Name, test.Name);

        }

        [TestMethod()]
        public void TestJSONDeserialize()
        {
            var xml = SerializationAdapter.GetAdapter(SerializationAdapterType.JSON);
            var test = new TestObj();
            string xmlData = xml.Serialize(test);

            var expect = xml.Deserialize<TestObj>(xmlData);

            Assert.AreEqual(expect.Name, test.Name);

        }
    }

    [Serializable]
    public class TestObj
    {
        public string Name;
        public List<string> Posts;

        public TestObj()
        {
            Name = "mike";

            Posts = new List<string>();
            Posts.Add("Rails");
            Posts.Add("C#");
            Posts.Add("Python");
        }
    }
}

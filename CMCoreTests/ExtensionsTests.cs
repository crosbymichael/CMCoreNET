using CMCoreNET;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Text;
using System.Linq;

namespace CMCoreTests
{
    
    
    /// <summary>
    ///This is a test class for ByteArrayExtensionsTest and is intended
    ///to contain all ByteArrayExtensionsTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ExtensionsTests
    {


        private TestContext testContextInstance;
        private string TestString = "This is the test string";

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


        /// <summary>
        ///A test for GetString
        ///</summary>
        [TestMethod()]
        public void GetBytesTest()
        {
            byte[] actual = Encoding.UTF8.GetBytes(TestString);
            byte[] expected = TestString.GetBytes();

            Assert.IsTrue(actual.SequenceEqual(expected), "Byte arrays are not equal");
            Assert.AreEqual(expected.Length, actual.Length, "Lengths are not equal");
        }

        [TestMethod()]
        public void GetStringTest() {
            byte[] testBytes = Encoding.UTF8.GetBytes(TestString);

            string actual = TestString;
            string expected = testBytes.GetString();

            Assert.AreEqual(expected, actual, "Strings are not equal");
        }

        [TestMethod()]
        public void GetInstanceTest() {
            Type actual = typeof(StringBuilder);
            var instance = actual.GetInstance();
            Type expected = instance.GetType();

            Assert.AreEqual(expected.Name, actual.Name, "Instances are incorrect type");
        }
    }
}

using HTA.Adventures.Data.ModelValidation;
using HTA.Adventures.Models.Types;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace HTA.Adventures.Models.Tests
{


    /// <summary>
    ///This is a test class for LocationPointTest and is intended
    ///to contain all LocationPointTest Unit Tests
    ///</summary>
    [TestClass()]
    public class LocationPointTest
    {
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

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


        [TestMethod]
        public void GeoLocationValidationTest()
        {
            var target = new GeoLocationValidator();
            
            var emptyResults = target.Validate(new LocationPoint());

            Assert.AreNotEqual(0, emptyResults.Count);
        }
    }
}

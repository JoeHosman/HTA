using HTA.Adventures.Data.ModelValidation;
using HTA.Adventures.Models.Types;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace HTA.Adventures.Models.Tests
{


    /// <summary>
    ///This is a test class for AdventureRegionTest and is intended
    ///to contain all AdventureRegionTest Unit Tests
    ///</summary>
    [TestClass()]
    public class AdventureRegionTests
    {
        public const int ValidResultCount = 0;

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
        public void AdventureSpotValidationTest()
        {
            var target = new AdventureSpotValidator();

            // wants a valid name, location
            var emptyEntity = new AdventureRegion();
            Assert.AreNotEqual(ValidResultCount, target.Validate(emptyEntity).Count);

            // ...still wants a valid location
            var entity = new AdventureRegion() { Name = "Name" };
            int resultCount = target.Validate(entity).Count;
            Assert.AreNotEqual(ValidResultCount, resultCount);

            // ...still wants a valid location
            entity.Address = "address";
            Assert.AreEqual(resultCount, target.Validate(entity).Count, "These fields are not validated upon");

            // ...still wants a valid location
            entity.Picture = "picture address";
            Assert.AreEqual(resultCount, target.Validate(entity).Count, "These fields are not validated upon");

            entity.Id = "don't touch this";
            Assert.AreEqual(resultCount, target.Validate(entity).Count, "These fields are not validated upon");

            entity.LocationPoint = new LocationPoint();
            Assert.AreNotEqual(ValidResultCount, target.Validate(entity).Count, "We must have a valid location, an empty one sucks.");

            var locationValidationCount = target.Validate(entity).Count;

            entity.LocationPoint.Lat = 0;
            Assert.AreNotEqual(locationValidationCount, target.Validate(entity).Count);

            entity.LocationPoint.Lon = 0;
            Assert.AreEqual(ValidResultCount, target.Validate(entity).Count);
        }

    }
}

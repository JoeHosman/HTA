using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HTA.Adventures.BusinessLogic;
using HTA.Adventures.Models.Types;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HTA.Adventures.Busness.Tests
{
    /// <summary>
    ///This is a test class for RegionBusinessTest and is intended
    ///to contain all RegionBusiness Unit Tests
    ///</summary>
    [TestClass()]
    public class RegionBusinessTest
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


        /// <summary>
        ///Test proves validation for :
        /// Null invalid
        /// new constructed invalid
        /// Generic Valid
        ///</summary>
        [TestMethod()]
        public void ValidateTest()
        {
            var regionBusiness = new RegionBusiness();
            IList<ValidationResult> validationErrorResults = new List<ValidationResult>();
            const bool NotValid = false;
            const bool Valid = true;
            bool isValidActual;

            // check validation of null location... not valid.
            Location nullRegion = null;

            // check  validation
            validationErrorResults.Clear();
            isValidActual = regionBusiness.Validate(nullRegion, validationErrorResults);
            Assert.AreEqual(NotValid, isValidActual, "null is not valid");

            // check validation of default location constructor... not valid.
            Region defaultRegion = new Region();

            // check validation
            validationErrorResults.Clear();
            isValidActual = regionBusiness.Validate(defaultRegion, validationErrorResults);
            Assert.AreEqual(NotValid, isValidActual, "newly constructed empty class is not valid");

            // Check validation of a seemly normal location.
            Region validGenericRegion = new Region(new GeoPoint { Lat = 0.0, Lon = 0.0 }, "Location");

            // check validation
            validationErrorResults.Clear();
            isValidActual = regionBusiness.Validate(validGenericRegion, validationErrorResults);
            Assert.AreEqual(Valid, isValidActual, "Valid generic location");
        }
    }
    /// <summary>
    ///This is a test class for LocationBusinessTest and is intended
    ///to contain all LocationBusinessTest Unit Tests
    ///</summary>
    [TestClass()]
    public class LocationBusinessTest
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


        /// <summary>
        ///Test proves validation for :
        /// Null invalid
        /// new constructed invalid
        /// Generic Valid
        ///</summary>
        [TestMethod()]
        public void ValidateTest()
        {
            var locationBusiness = new LocationBusiness();
            IList<ValidationResult> validationErrorResults = new List<ValidationResult>();
            const bool NotValid = false;
            const bool Valid = true;
            bool isValidActual;

            // check validation of null location... not valid.
            Location nullLocation = null;

            // check  validation
            validationErrorResults.Clear();
            isValidActual = locationBusiness.Validate(nullLocation, validationErrorResults);
            Assert.AreEqual(NotValid, isValidActual, "null is not valid");

            // check validation of default location constructor... not valid.
            Location defaultLocation = new Location();

            // check validation
            validationErrorResults.Clear();
            isValidActual = locationBusiness.Validate(defaultLocation, validationErrorResults);
            Assert.AreEqual(NotValid, isValidActual, "newly constructed empty class is not valid");

            // Check validation of a seemly normal location.
            Location validGenericLocation = new Location(new GeoPoint { Lat = 0.0, Lon = 0.0 }, "Location");
            validGenericLocation.Region.Id = "not empty";

            // check validation
            validationErrorResults.Clear();
            isValidActual = locationBusiness.Validate(validGenericLocation, validationErrorResults);
            Assert.AreEqual(Valid, isValidActual, "Valid generic location");
        }
    }
}
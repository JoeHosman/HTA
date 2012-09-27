using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HTA.Adventures.BusinessLogic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HTA.Adventures.Models.Types;

namespace HTA.Adventures.Busness.Tests
{
    /// <summary>
    ///This is a test class for SpotBusinessTest and is intended
    ///to contain all SpotBusinessTest Unit Tests
    ///</summary>
    [TestClass()]
    public class SpotBusinessTest
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
            var spotBusiness = new SpotBusiness();

            const bool NotValid = false;
            const bool Valid = true;
            bool isValidActual;
            IList<ValidationResult> validationErrorResults = new List<ValidationResult>();


            // check validation of null location... not valid.
            Spot nullSpot = null;

            isValidActual = spotBusiness.Validate(nullSpot, null);
            Assert.AreEqual(NotValid, isValidActual, "null is not valid, null result parameter");

            // check  validation

            isValidActual = spotBusiness.Validate(nullSpot, validationErrorResults);
            Assert.AreEqual(NotValid, isValidActual, "null is not valid");
            Assert.AreNotEqual(0, validationErrorResults.Count);

            // check validation of default location constructor... not valid.
            Spot defaultLocation = new Spot();

            // check validation
            validationErrorResults.Clear();
            isValidActual = spotBusiness.Validate(defaultLocation, validationErrorResults);
            Assert.AreEqual(NotValid, isValidActual, "newly constructed empty class is not valid");
            Assert.AreNotEqual(0, validationErrorResults.Count);

            // Check validation of a seemly normal location.
            Spot validGenericLocation = new Spot(new GeoPoint { Lat = 0.0, Lon = 0.0 }, "Spot");

            // check validation
            validationErrorResults.Clear();
            isValidActual = spotBusiness.Validate(validGenericLocation, validationErrorResults);
            Assert.AreEqual(Valid, isValidActual, "Valid generic location");
            Assert.AreEqual(0, validationErrorResults.Count);

            spotBusiness.Dispose();

        }
        /// <summary>
        ///Test proves validation for :
        /// no name invalid
        /// no location invalid
        ///</summary>
        [TestMethod()]
        public void InvalidSpotTest()
        {
            var spotBusiness = new SpotBusiness();
            IList<ValidationResult> validationErrorResults = new List<ValidationResult>();
            const bool NotValid = false;
            bool isValidActual;


            // Check validation of a seemly normal location.
            var noName = new Spot(new GeoPoint { Lat = 0.0, Lon = 0.0 }, null);

            // check validation
            validationErrorResults.Clear();
            isValidActual = spotBusiness.Validate(noName, validationErrorResults);
            Assert.AreEqual(NotValid, isValidActual, "no name generic location");

            // Check validation of a seemly normal location.
            var noLocation = new Spot(null, "Spot");

            // check validation
            validationErrorResults.Clear();
            isValidActual = spotBusiness.Validate(noLocation, validationErrorResults);
            Assert.AreEqual(NotValid, isValidActual, "no location generic location");

            spotBusiness.Dispose();
        }

        /// <summary>
        ///Test proves validation for :
        /// Generic with invalid lat
        /// Generic with invalid lon  
        ///</summary>
        [TestMethod()]
        public void ValidateSpotTest()
        {
            var spotBusiness = new SpotBusiness();
            IList<ValidationResult> validationErrorResults = new List<ValidationResult>();
            const bool NotValid = false;
            const bool Valid = true;
            bool isValidActual;

            // Check validation of an invalid latitude location.
            Spot invalidLatLocation = new Spot(new GeoPoint { Lat = 90.1, Lon = 0.0 }, "Spot");

            // check validation invalid
            validationErrorResults.Clear();
            isValidActual = spotBusiness.Validate(invalidLatLocation, validationErrorResults);
            Assert.AreEqual(NotValid, isValidActual, " latitude greater than 90.0 is not valid");

            invalidLatLocation = new Spot(new GeoPoint { Lat = 90.0, Lon = 0.0 }, "Spot");

            // check validation valid
            validationErrorResults.Clear();
            isValidActual = spotBusiness.Validate(invalidLatLocation, validationErrorResults);
            Assert.AreEqual(Valid, isValidActual, "latitude less than or equal to 90.0 is valid");


            // Check validation of an invalid latitude location.
            invalidLatLocation = new Spot(new GeoPoint { Lat = -90.1, Lon = 0.0 }, "Spot");

            // check validation
            validationErrorResults.Clear();
            isValidActual = spotBusiness.Validate(invalidLatLocation, validationErrorResults);
            Assert.AreEqual(NotValid, isValidActual, "latitude less than -90.0 is not valid");

            // Check validation of an invalid latitude location.
            invalidLatLocation = new Spot(new GeoPoint { Lat = -90.0, Lon = 0.0 }, "Spot");

            // check validation
            validationErrorResults.Clear();
            isValidActual = spotBusiness.Validate(invalidLatLocation, validationErrorResults);
            Assert.AreEqual(Valid, isValidActual, "latitude greater than or equal to -90.0 is valid");

            // Check validation of an invalid latitude location.
            Spot invalidLonLocation = new Spot(new GeoPoint { Lat = 0.0, Lon = 180.1 }, "Spot");

            // check validation invalid
            validationErrorResults.Clear();
            isValidActual = spotBusiness.Validate(invalidLonLocation, validationErrorResults);
            Assert.AreEqual(NotValid, isValidActual, "longitude greater than 180.0 is not valid");

            // Check validation of an invalid latitude location.
            invalidLonLocation = new Spot(new GeoPoint { Lat = 0.0, Lon = 180.0 }, "Spot");

            // check validation valid
            validationErrorResults.Clear();
            isValidActual = spotBusiness.Validate(invalidLonLocation, validationErrorResults);
            Assert.AreEqual(Valid, isValidActual, "longitude less than or equal to 180.0 is valid");

            // Check validation of an invalid latitude location.
            invalidLonLocation = new Spot(new GeoPoint { Lat = 0.0, Lon = -180.1 }, "Spot");

            // check validation
            validationErrorResults.Clear();
            isValidActual = spotBusiness.Validate(invalidLonLocation, validationErrorResults);
            Assert.AreEqual(NotValid, isValidActual, "longitude less than -180.0 is not valid");

            // Check validation of an invalid latitude location.
            invalidLonLocation = new Spot(new GeoPoint { Lat = 0.0, Lon = -180.0 }, "Spot");

            // check validation
            validationErrorResults.Clear();
            isValidActual = spotBusiness.Validate(invalidLonLocation, validationErrorResults);
            Assert.AreEqual(Valid, isValidActual, "longitude great than or equal to -180.0 is valid");

            spotBusiness.Dispose();
        }
    }
}

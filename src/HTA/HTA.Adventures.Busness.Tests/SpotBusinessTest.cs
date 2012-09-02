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

            // check validation of null location... not valid.
            Location nullLocation = null;

            // check  validation
            isValidActual = spotBusiness.Validate(nullLocation);
            Assert.AreEqual(NotValid, isValidActual, "null is not valid");

            // check validation of default location constructor... not valid.
            Location defaultLocation = new Location();

            // check validation
            isValidActual = spotBusiness.Validate(defaultLocation);
            Assert.AreEqual(NotValid, isValidActual, "newly constructed empty class is not valid");

            // Check validation of a seemly normal location.
            Location validGenericLocation = new Location(new GeoPoint { Lat = 0.0, Lon = 0.0 }, "Spot");

            // check validation
            isValidActual = spotBusiness.Validate(validGenericLocation);
            Assert.AreEqual(Valid, isValidActual, "Valid generic location");

        }
        /// <summary>
        ///Test proves validation for :
        /// no name invalid
        /// no location invalid
        ///</summary>
        [TestMethod()]
        public void InvalidLocationTest()
        {
            var spotBusiness = new SpotBusiness();

            const bool NotValid = false;
            const bool Valid = true;
            bool isValidActual;

            // Check validation of a seemly normal location.
            Location noName = new Location(new GeoPoint { Lat = 0.0, Lon = 0.0 }, null);

            // check validation
            isValidActual = spotBusiness.Validate(noName);
            Assert.AreEqual(NotValid, isValidActual, "no name generic location");

            // Check validation of a seemly normal location.
            Location noLocation = new Location(null, "Spot");

            // check validation
            isValidActual = spotBusiness.Validate(noLocation);
            Assert.AreEqual(NotValid, isValidActual, "no location generic location");
        }

        /// <summary>
        ///Test proves validation for :
        /// Generic with invalid lat
        /// Generic with invalid lon  
        ///</summary>
        [TestMethod()]
        public void ValidateLocationTest()
        {
            var spotBusiness = new SpotBusiness();

            const bool NotValid = false;
            const bool Valid = true;
            bool isValidActual;

            // Check validation of an invalid latitude location.
            Location invalidLatLocation = new Location(new GeoPoint { Lat = 90.1, Lon = 0.0 }, "Spot");

            // check validation invalid
            isValidActual = spotBusiness.Validate(invalidLatLocation);
            Assert.AreEqual(NotValid, isValidActual, " latitude greater than 90.0 is not valid");

            invalidLatLocation = new Location(new GeoPoint { Lat = 90.0, Lon = 0.0 }, "Spot");

            // check validation valid
            isValidActual = spotBusiness.Validate(invalidLatLocation);
            Assert.AreEqual(Valid, isValidActual, "latitude less than or equal to 90.0 is valid");


            // Check validation of an invalid latitude location.
            invalidLatLocation = new Location(new GeoPoint { Lat = -90.1, Lon = 0.0 }, "Spot");

            // check validation
            isValidActual = spotBusiness.Validate(invalidLatLocation);
            Assert.AreEqual(NotValid, isValidActual, "latitude less than -90.0 is not valid");

            // Check validation of an invalid latitude location.
            invalidLatLocation = new Location(new GeoPoint { Lat = -90.0, Lon = 0.0 }, "Spot");

            // check validation
            isValidActual = spotBusiness.Validate(invalidLatLocation);
            Assert.AreEqual(Valid, isValidActual, "latitude greater than or equal to -90.0 is valid");

            // Check validation of an invalid latitude location.
            Location invalidLonLocation = new Location(new GeoPoint { Lat = 0.0, Lon = 180.1 }, "Spot");

            // check validation invalid
            isValidActual = spotBusiness.Validate(invalidLonLocation);
            Assert.AreEqual(NotValid, isValidActual, "longitude greater than 180.0 is not valid");

            // Check validation of an invalid latitude location.
            invalidLonLocation = new Location(new GeoPoint { Lat = 0.0, Lon = 180.0 }, "Spot");

            // check validation valid
            isValidActual = spotBusiness.Validate(invalidLonLocation);
            Assert.AreEqual(Valid, isValidActual, "longitude less than or equal to 180.0 is valid");

            // Check validation of an invalid latitude location.
            invalidLonLocation = new Location(new GeoPoint { Lat = 0.0, Lon = -180.1 }, "Spot");

            // check validation
            isValidActual = spotBusiness.Validate(invalidLonLocation);
            Assert.AreEqual(NotValid, isValidActual, "longitude less than -180.0 is not valid");

            // Check validation of an invalid latitude location.
            invalidLonLocation = new Location(new GeoPoint { Lat = 0.0, Lon = -180.0 }, "Spot");

            // check validation
            isValidActual = spotBusiness.Validate(invalidLonLocation);
            Assert.AreEqual(Valid, isValidActual, "longitude great than or equal to -180.0 is valid");
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HTA.Adventures.BusinessLogic;
using HTA.Adventures.Models.Types;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HTA.Adventures.Busness.Tests
{
    [TestClass]
    public class AdventureReviewBusinessTest
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
        [TestMethod]
        public void ValidateTest()
        {
            var business = new AdventureReviewBusiness();

            IList<ValidationResult> validationErrorResults = new List<ValidationResult>();
            const bool NotValid = false;
            const bool Valid = true;
            bool isValidActual;

            // check validation of null type... not valid.
            AdventureReview nullReview = null;

            //check no result handler
            isValidActual = business.Validate(nullReview, null);
            Assert.AreEqual(NotValid, isValidActual, "null input result list parameter");


            // check  validation
            validationErrorResults.Clear();
            isValidActual = business.Validate(nullReview, validationErrorResults);
            Assert.AreEqual(NotValid, isValidActual, "null is not valid");

            // check validation of default type constructor... not valid.
            var defaultReview = new AdventureReview();
            // check  validation
            validationErrorResults.Clear();
            isValidActual = business.Validate(defaultReview, validationErrorResults);
            Assert.AreEqual(NotValid, isValidActual, "default constructor is not valid");


            var validReview = new AdventureReview
                                  {
                                      AdventureName = "some adventure name",
                                      AdventureType = new AdventureType { Id = "someLegitId" },
                                      AdventureLocation = new AdventureLocation(new GeoPoint { Lat = 0, Lon = 0 }, "location"),
                                      AdventureDuration = new TimeSpan(0, 0, 0, 1),
                                      AdventureDate = DateTime.Now
                                  };

            validationErrorResults.Clear();
            isValidActual = business.Validate(validReview, validationErrorResults);
            Assert.AreEqual(Valid, isValidActual, "valid review");
        }
    }
}
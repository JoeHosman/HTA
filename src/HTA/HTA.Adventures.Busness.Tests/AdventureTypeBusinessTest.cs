using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HTA.Adventures.BusinessLogic;
using HTA.Adventures.Models.Types;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HTA.Adventures.Busness.Tests
{
    [TestClass]
    public class AdventureTypeBusinessTest
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
            var business = new AdventureTypeBusiness();

            IList<ValidationResult> validationErrorResults = new List<ValidationResult>();
            const bool NotValid = false;
            const bool Valid = true;
            bool isValidActual;

            // check validation of null type... not valid.
            AdventureType nullType = null;

            //check no result handler
            isValidActual = business.Validate(nullType, null);
            Assert.AreEqual(NotValid, isValidActual, "null input result list parameter");


            // check  validation
            validationErrorResults.Clear();
            isValidActual = business.Validate(nullType, validationErrorResults);
            Assert.AreEqual(NotValid, isValidActual, "null is not valid");

            // check validation of default type constructor... not valid.
            var defaultType = new AdventureType();

            // check validation
            validationErrorResults.Clear();
            isValidActual = business.Validate(defaultType, validationErrorResults);
            Assert.AreEqual(NotValid, isValidActual, "newly constructed empty class is not valid");

            // Check validation of a seemly normal location.
            var validType = new AdventureType
                                {
                                    Name = "TypeName",
                                    Description = "TypeDescription",
                                };

            // check validation
            validationErrorResults.Clear();
            isValidActual = business.Validate(validType, validationErrorResults);
            Assert.AreEqual(Valid, isValidActual, "Valid generic type");

            validType.PhotoLink = string.Empty;

            // check validation
            validationErrorResults.Clear();
            isValidActual = business.Validate(validType, validationErrorResults);
            Assert.AreEqual(Valid, isValidActual, "Valid generic type, null photo link");

            validType.IconLink = string.Empty;

            // check validation
            validationErrorResults.Clear();
            isValidActual = business.Validate(validType, validationErrorResults);
            Assert.AreEqual(Valid, isValidActual, "Valid generic type, null icon link");

            validType.DataCardTemplates.Add(new AdventureTypeTemplate());
            // check validation
            validationErrorResults.Clear();
            isValidActual = business.Validate(validType, validationErrorResults);
            Assert.AreEqual(Valid, isValidActual, "Valid generic type, new empty dataCardTemplate");

            validType.DataCardTemplates = null;
            // check validation
            validationErrorResults.Clear();
            isValidActual = business.Validate(validType, validationErrorResults);
            Assert.AreEqual(NotValid, isValidActual, "Valid generic type, null dataCardTemplateList");

            business.Dispose();
        }
    }
}
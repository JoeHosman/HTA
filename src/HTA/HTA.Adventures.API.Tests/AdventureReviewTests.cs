using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HTA.Adventures.BusinessLogic;
using HTA.Adventures.Models.Types;
using HTA.Adventures.Models.Types.Responses;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServiceStack.ServiceClient.Web;

namespace HTA.Websites.API.Tests
{
    [TestClass]
    public class AdventureReviewTests
    {
        private JsonServiceClient _apiProxyClient;

        [TestInitialize]
        public void StartUp()
        {
            _apiProxyClient = new JsonServiceClient("http://localhost:10768/");
        }

        /// <summary>
        /// This will test Creation of a new Adventure Location with an existing Region
        /// </summary>
        [TestMethod]
        public void CreateNewAdventureReviewTest()
        {
            var validator = new AdventureReviewBusiness();

            IList<ValidationResult> validationErrorResults = new List<ValidationResult>();
            validationErrorResults.Clear();
            Assert.IsFalse(validator.Validate(null, validationErrorResults), "Null should not be valid");
            validationErrorResults.Clear();
            Assert.IsFalse(validator.Validate(new AdventureReview(), validationErrorResults), "new AdventureReview() should not be valid");

            var nullReviewResults = _apiProxyClient.Post<AdventureReviewBaseResponse>("/Adventure/Reviews/", null);
            Assert.IsFalse(string.IsNullOrEmpty(nullReviewResults.ResponseStatus.ErrorCode));

            var validReview = new AdventureReview
                                  {
                                      AdventureName = "My Adventure",
                                      AdventureType = new AdventureType() { Name = "type" },
                                      AdventureLocation = new AdventureLocation(new Region(new GeoPoint { Lat = 50, Lon = 50 }, "Location"))
                                  };

            validationErrorResults.Clear();
            Assert.IsTrue(validator.Validate(validReview, validationErrorResults));

            var validReviewResults = _apiProxyClient.Post<AdventureReviewBaseResponse>("/Adventure/Reviews/", validReview);
            Assert.IsTrue(string.IsNullOrEmpty(validReviewResults.ResponseStatus.ErrorCode));
        }
    }
}
using HTA.Adventures.Data.ModelValidation;
using HTA.Adventures.Models.Types;
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
        public void CreateNewAdventureLocationTest()
        {
            var validator = new GenericValidator<AdventureReview>();

            Assert.AreNotEqual(0, validator.Validate(null), "Null should not be valid");

            var nullResult = _apiProxyClient.Post<AdventureReviewResponse>("/Adventure/Reviews/", null);
            Assert.IsFalse(string.IsNullOrEmpty(nullResult.ResponseStatus.ErrorCode));
        }
    }
}
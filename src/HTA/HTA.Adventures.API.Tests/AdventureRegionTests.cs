using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using HTA.Adventures.BusinessLogic;
using HTA.Adventures.Models.Types;
using HTA.Adventures.Models.Types.Responses;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServiceStack.ServiceClient.Web;

namespace HTA.Websites.API.Tests
{
    [TestClass]
    public class AdventureRegionTests
    {
        private JsonServiceClient _apiProxyClient;

        [TestInitialize]
        public void StartUp()
        {
            _apiProxyClient = new JsonServiceClient("http://localhost:10768/");
        }

        /// <summary>
        /// This will test Creation of new Adventure Regions
        /// </summary>
        [TestMethod]
        public void CreateNewRegionWithAPITest()
        {
            IList<ValidationResult> validationErrorResults = new List<ValidationResult>();
            var validator = new RegionBusiness();
            // First test sending null :(

            // Verify invalid
            validationErrorResults.Clear();
            Assert.IsFalse(validator.Validate(null, validationErrorResults));

            // send Invalid
            var nullResponse = _apiProxyClient.Post<AdventureRegionBaseResponse>("/Adventure/Regions", null);
            Assert.IsFalse(String.IsNullOrEmpty(nullResponse.ResponseStatus.ErrorCode));

            // Verify valid
            var validAdventureRegion = new Region(new GeoPoint { Lat = 0, Lon = 0 }, "Name");
            validationErrorResults.Clear();
            Assert.IsTrue(validator.Validate(validAdventureRegion, validationErrorResults));

            // send Valid
            var validResponse = _apiProxyClient.Post<AdventureRegionBaseResponse>("/Adventure/Regions", validAdventureRegion);
            Assert.IsTrue(String.IsNullOrEmpty(validResponse.ResponseStatus.ErrorCode));
            // we should have atleast one location
            //Assert.AreNotEqual(0, validResponse.Locations.Count());
        }
    }
}

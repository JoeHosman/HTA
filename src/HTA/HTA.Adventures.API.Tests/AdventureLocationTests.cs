using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HTA.Adventures.BusinessLogic;
using HTA.Adventures.Models.Types;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServiceStack.ServiceClient.Web;

namespace HTA.Websites.API.Tests
{
    [TestClass]
    public class AdventureLocationTests
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
        public void CreateNewLocationTest()
        {
            IList<ValidationResult> validationErrorResults = new List<ValidationResult>();
            var validator = new LocationBusiness();
            // First test sending null :(
            validationErrorResults.Clear();
            Assert.IsFalse(validator.Validate(null, validationErrorResults));

            var nullResponse = _apiProxyClient.Post<AdventureLocationResponse>("/Adventure/Locations", null);
            //Verify that we have errors!
            Assert.IsFalse(String.IsNullOrEmpty(nullResponse.ResponseStatus.ErrorCode));

            var newLocation = new Location()
            {
                Point = new GeoPoint { Lat = 0.5, Lon = 0.5 },
                Name = "AllByMySelf"
            };

            validationErrorResults.Clear();
            Assert.IsTrue(validator.Validate(newLocation, validationErrorResults));
            var newLocationResponse = _apiProxyClient.Post<AdventureLocationResponse>("/Adventure/Locations", newLocation);
            Assert.IsTrue(String.IsNullOrEmpty(newLocationResponse.ResponseStatus.ErrorCode));

            // test with pre-existing region
            // create a valid region, this should exist already.
            var validAdventureRegion = new Region(new GeoPoint { Lat = 0, Lon = 0 }, "Name");
            validationErrorResults.Clear();
            Assert.IsTrue(validator.Validate(validAdventureRegion, validationErrorResults));
            var validRegionResponse = _apiProxyClient.Post<AdventureRegionResponse>("/Adventure/Regions", validAdventureRegion);
            Assert.IsTrue(String.IsNullOrEmpty(validRegionResponse.ResponseStatus.ErrorCode));

            validAdventureRegion = validRegionResponse.Region;

            var validLocation = new Location(validAdventureRegion)
                                    {
                                        Point = new GeoPoint { Lat = 0.5, Lon = 0.5 }
                                    };

            var validLocationRequest = _apiProxyClient.Post<AdventureLocationResponse>("/Adventure/Locations", validLocation);
            Assert.IsTrue(String.IsNullOrEmpty(validLocationRequest.ResponseStatus.ErrorCode));

        }
    }
}
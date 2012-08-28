using System;
using HTA.Adventures.Data.ModelValidation;
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

            var validator = new AdventureSpotValidator();
            // First test sending null :(
            Assert.AreNotEqual(0, validator.Validate(null).Count);

            var nullResponse = _apiProxyClient.Post<AdventureRegionResponse>("/Adventure/Locations", null);
            // Verify that we have errors!
            Assert.IsFalse(String.IsNullOrEmpty(nullResponse.ResponseStatus.ErrorCode));


            // create a valid region, this should exist already.
            var validAdventureRegion = new AdventureRegion(new LocationPoint { Lat = 0, Lon = 0 }, "Name");
            Assert.AreEqual(0, validator.Validate(validAdventureRegion).Count);
            var validRegionResponse = _apiProxyClient.Post<AdventureRegionResponse>("/Adventure/Regions", validAdventureRegion);
            Assert.IsTrue(String.IsNullOrEmpty(validRegionResponse.ResponseStatus.ErrorCode));

            validAdventureRegion = validRegionResponse.Region;

            var validLocation = new AdventureLocation(validAdventureRegion)
                                    {
                                        LocationPoint = new LocationPoint { Lat = 0.5, Lon = 0.5 }
                                    };

            var validLocationRequest = _apiProxyClient.Post<AdventureLocationResponse>("/Adventure/Locations", validLocation);
            Assert.IsTrue(String.IsNullOrEmpty(validLocationRequest.ResponseStatus.ErrorCode));

        }
    }
}
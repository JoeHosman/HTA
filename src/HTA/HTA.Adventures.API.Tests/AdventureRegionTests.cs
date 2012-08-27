using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using HTA.Adventures.Data.ModelValidation;
using HTA.Adventures.Models.Types;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServiceStack.ServiceClient.Web;
using ServiceStack.ServiceInterface.ServiceModel;

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
        public void CreateNewRegionTest()
        {
            var validator = new AdventureSpotValidator();
            // First test sending null :(
            AdventureRegion nullAdventureRegion = null;
            Assert.AreNotEqual(0, validator.Validate(nullAdventureRegion).Count);

            var nullResponse = _apiProxyClient.Post<AdventureRegionResponse>("/Adventure/Regions", nullAdventureRegion);
            Assert.IsFalse(String.IsNullOrEmpty(nullResponse.ResponseStatus.ErrorCode));

            AdventureRegion validAdventureRegion = new AdventureRegion(new LocationPoint { Lat = 0, Lon = 0 }, "Name");
            Assert.AreEqual(0, validator.Validate(validAdventureRegion).Count);

            var validResponse = _apiProxyClient.Post<AdventureRegionResponse>("/Adventure/Regions", validAdventureRegion);
            Assert.IsTrue(String.IsNullOrEmpty(validResponse.ResponseStatus.ErrorCode));
            // we should have atleast one location
            Assert.AreNotEqual(0, validResponse.Locations.Count());
        }
    }
}

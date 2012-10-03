using System.Collections.Generic;
using HTA.Adventures.API.ServiceInterface;
using HTA.Adventures.Models;
using HTA.Adventures.Models.Types.Responses;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HTA.Adventures.Models.Types;
using Moq;

namespace HTA.Websites.API.Tests
{


    /// <summary>
    ///This is a test class for AdventureLocationServiceTest and is intended
    ///to contain all AdventureLocationServiceTest Unit Tests
    ///</summary>
    [TestClass]
    public class AdventureLocationServiceTest
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
        ///A test for OnGet
        ///</summary>
        [TestMethod]
        public void OnGetSingleTest()
        {
            var locationRequest = new AdventureLocation { Id = "existingId" };

            IList<AdventureLocation> locationList = new List<AdventureLocation>();
            locationList.Add(locationRequest);

            var mock = new Mock<IAdventureLocationRepository>();
            mock.Setup(a => a.GetAdventureLocation(locationRequest.Id)).Returns(locationRequest);

            var target = new AdventureLocationService { AdventureLocationRepository = mock.Object };

            var expected = new AdventureLocationGetResponse(locationRequest);
            expected.AdventureLocations.Add(locationRequest);



            // check get single
            var actual = target.OnGet(locationRequest) as AdventureLocationGetResponse;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void OnGetListTest()
        {
            var adventureLocation = new AdventureLocation { Id = "existingId" };
            var locationListRequest = new AdventureLocation();

            IList<AdventureLocation> locationList = new List<AdventureLocation>();
            locationList.Add(adventureLocation);

            var mock = new Mock<IAdventureLocationRepository>();
            mock.Setup(a => a.GetAdventureLocations()).Returns(locationList);

            var target = new AdventureLocationService { AdventureLocationRepository = mock.Object };

            var expectedList = new AdventureLocationGetResponse(locationListRequest)
            {
                AdventureLocations = locationList
            };

            // check get list
            var actual = target.OnGet(locationListRequest) as AdventureLocationGetResponse;
            Assert.AreEqual(expectedList, actual);

        }

        /// <summary>
        ///A test for OnPost
        ///</summary>
        [TestMethod]
        public void OnPostNewTest()
        {
            var geoPoint = new GeoPoint { Lat = 0, Lon = 0 };
            const string name = "Name";
            var newLocationRequest = new AdventureLocation(geoPoint, name);
            var newLocationResponse = new AdventureLocation(geoPoint, name) { Id = "newId" };
            var expectedNewResponse = new AdventureLocationSaveResponse(newLocationRequest) { AdventureLocation = newLocationResponse };
          
            var mock = new Mock<IAdventureLocationRepository>();
            mock.Setup(a => a.SaveAdventureLocation(newLocationRequest)).Returns(newLocationResponse);
  

            var target = new AdventureLocationService { AdventureLocationRepository = mock.Object };

            var actual = target.OnPost(newLocationRequest) as AdventureLocationSaveResponse;
            Assert.AreEqual(expectedNewResponse, actual);

        }

        [TestMethod]
        public void OnPostUpdateTest()
        {
            var geoPoint = new GeoPoint { Lat = 0, Lon = 0 };
            const string name = "Name";
            
            var existingLocationRequest = new AdventureLocation(geoPoint, name) { Id = "existingId" };
            var existingLocationResponse = new AdventureLocation(geoPoint, name) { Id = "existingId" };

            var expectedExistingResponse = new AdventureLocationSaveResponse(existingLocationRequest) { AdventureLocation = existingLocationResponse };

            var mock = new Mock<IAdventureLocationRepository>();
            mock.Setup(a => a.SaveAdventureLocation(existingLocationRequest)).Returns(existingLocationResponse);

            var target = new AdventureLocationService { AdventureLocationRepository = mock.Object };

            var actual = target.OnPost(existingLocationRequest) as AdventureLocationSaveResponse;
            Assert.AreEqual(expectedExistingResponse, actual);

        }
    }
}

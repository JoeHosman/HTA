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
    ///This is a test class for AdventureRegionServiceTest and is intended
    ///to contain all AdventureRegionServiceTest Unit Tests
    ///</summary>
    [TestClass]
    public class AdventureRegionServiceTest
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
            var getSingleRequest = new Region { Id = "existingId" };

            var singleRegion = new Region
                                   {
                                       Id = "existingId",
                                       Name = "SomeRegion",
                                       Point = new GeoPoint { Lat = 0, Lon = 0 }
                                   };

            var regionList = new List<Region> { singleRegion };

            var expectedSingleResponse = new AdventureRegionGetResponse(getSingleRequest) { AdventureRegions = regionList };

            var mock = new Mock<IAdventureRegionRepository>();

            mock.Setup(a => a.GetAdventureRegion(getSingleRequest.Id)).Returns(singleRegion);

            var target = new AdventureRegionService { AdventureRegionRepository = mock.Object };

            var actual = target.OnGet(getSingleRequest) as AdventureRegionGetResponse;
            Assert.IsNotNull(actual);
            Assert.AreEqual(expectedSingleResponse, actual);
        }

        [TestMethod]
        public void OnGetListTest()
        {
            var getListRequest = new Region();

            var singleRegion = new Region
            {
                Id = "existingId",
                Name = "SomeRegion",
                Point = new GeoPoint { Lat = 0, Lon = 0 }
            };

            var regionList = new List<Region> { singleRegion };

            var expectedListResponse = new AdventureRegionGetResponse(getListRequest) { AdventureRegions = regionList };

            var mock = new Mock<IAdventureRegionRepository>();

            mock.Setup(a => a.GetAdventureRegions()).Returns(regionList);

            var target = new AdventureRegionService { AdventureRegionRepository = mock.Object };

            var actual = target.OnGet(getListRequest) as AdventureRegionGetResponse;
            Assert.IsNotNull(actual);
            Assert.AreEqual(expectedListResponse, actual);


        }

        /// <summary>
        ///A test for OnPost
        ///</summary>
        [TestMethod]
        public void OnPostNewTest()
        {
            const string regionname = "regionName";
            var geoPoint = new GeoPoint { Lat = 0, Lon = 0 };

            var newRegionRequest = new Region
                                       {
                                           Name = regionname,
                                           Point = geoPoint
                                       };
            var newRegion = new Region
            {
                Id = "newId",
                Name = regionname,
                Point = geoPoint
            };

            var expectedNewRegionResponse = new AdventureRegionSaveResponse(newRegionRequest)
                                                {
                                                    AdventureRegion = newRegion
                                                };


            var mock = new Mock<IAdventureRegionRepository>();
            mock.Setup(a => a.SaveAdventureRegion(newRegionRequest)).Returns(newRegion);

            var target = new AdventureRegionService {AdventureRegionRepository = mock.Object};

            var actual = target.OnPost(newRegionRequest) as AdventureRegionSaveResponse;
            Assert.IsNotNull(actual);
            Assert.AreEqual(expectedNewRegionResponse, actual);

        }

        [TestMethod]
        public void OnPostUpdateTest()
        {
            const string regionname = "regionName";
            var geoPoint = new GeoPoint { Lat = 0, Lon = 0 };
            
            var updateRegionRequest = new Region
            {
                Id = "existingId",
                Name = regionname,
                Point = geoPoint
            };

            var updateRegion = new Region
            {
                Id = "existingId",
                Name = regionname,
                Point = geoPoint
            };
            
            var expectedUpdateRegionResponse = new AdventureRegionSaveResponse(updateRegionRequest)
            {
                AdventureRegion = updateRegion
            };

            var mock = new Mock<IAdventureRegionRepository>();
            mock.Setup(a => a.SaveAdventureRegion(updateRegionRequest)).Returns(updateRegion);

            var target = new AdventureRegionService {AdventureRegionRepository = mock.Object};


            var actual = target.OnPost(updateRegionRequest) as AdventureRegionSaveResponse;
            Assert.IsNotNull(actual);
            Assert.AreEqual(expectedUpdateRegionResponse, actual);

        }
    }
}

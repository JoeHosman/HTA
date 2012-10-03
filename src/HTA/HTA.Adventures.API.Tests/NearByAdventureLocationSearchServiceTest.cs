using System.Collections.Generic;
using HTA.Adventures.API.ServiceInterface;
using HTA.Adventures.Models;
using HTA.Adventures.Models.Types.Responses;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HTA.Adventures.Models.Types;

namespace HTA.Websites.API.Tests
{


    /// <summary>
    ///This is a test class for NearByAdventureLocationSearchServiceTest and is intended
    ///to contain all NearByAdventureLocationSearchServiceTest Unit Tests
    ///</summary>
    [TestClass]
    public class NearByAdventureLocationSearchServiceTest
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
        public void OnGetTest()
        {
            var validSearchRequest = new NearByAdventureLocationSearch { LatLon = "50,50" };
            var resultList = new List<AdventureLocation> { 
                new AdventureLocation(validSearchRequest.Point, "one"),
                new AdventureLocation(validSearchRequest.Point, "two")
            };

            var expectedValidSearchResponse = new NearByAdventureLocationSearchResponse(validSearchRequest)
                                                  {NearByAdventureLocations = resultList};

            var mock = new Moq.Mock<IAdventureLocationSearchRepository>();
            mock.Setup(a => a.GetNearByAdventureLocations(validSearchRequest.Point, validSearchRequest.Range)).Returns(resultList);

            var target = new NearByAdventureLocationSearchService {AdventureLocationSearchRepository = mock.Object};

            var actual = target.OnGet(validSearchRequest) as NearByAdventureLocationSearchResponse;

            Assert.AreEqual(expectedValidSearchResponse, actual);
            Assert.AreNotEqual(new NearByAdventureLocationSearchResponse(validSearchRequest), actual);
            Assert.AreNotEqual(new NearByAdventureLocationSearchResponse(null), actual);
            Assert.AreNotEqual(new NearByAdventureLocationSearchResponse(new NearByAdventureLocationSearch()), actual);
            Assert.AreEqual(new NearByAdventureLocationSearchResponse(new NearByAdventureLocationSearch()), new NearByAdventureLocationSearchResponse(new NearByAdventureLocationSearch()));
        }
    }
}

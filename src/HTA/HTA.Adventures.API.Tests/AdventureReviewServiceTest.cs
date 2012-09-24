using System;
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
    ///This is a test class for AdventureReviewServiceTest and is intended
    ///to contain all AdventureReviewServiceTest Unit Tests
    ///</summary>
    [TestClass]
    public class AdventureReviewServiceTest
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
            var singleRequest = new AdventureReview { Id = "legit id" };


            var allReviewsRequest = new AdventureReview();
            var reviewList = new List<AdventureReview> { singleRequest };

            var expectedSingleReviewResponse = new AdventureReviewGetResponse(singleRequest) { AdventureReviews = reviewList };

            var expectedAllReviewsResponse = new AdventureReviewGetResponse(allReviewsRequest) { AdventureReviews = reviewList };



            var notExpectedAllReviewsResponse = new AdventureReviewGetResponse(allReviewsRequest);

            var mock = new Mock<IAdventureReviewRepository>();
            mock.Setup(a => a.GetAdventureReviewById(singleRequest.Id)).Returns(singleRequest);
            mock.Setup(a => a.GetAdventureReviews()).Returns(reviewList);


            var target = new AdventureReviewService { AdventureReviewRepository = mock.Object };

            var actual = target.OnGet(singleRequest) as AdventureReviewGetResponse;
            Assert.AreEqual(expectedSingleReviewResponse, actual);

            actual = target.OnGet(allReviewsRequest) as AdventureReviewGetResponse;
            Assert.AreEqual(expectedAllReviewsResponse, actual);

            Assert.AreNotEqual(notExpectedAllReviewsResponse, actual);
        }

        /// <summary>
        ///A test for OnPost
        ///</summary>
        [TestMethod]
        public void OnPostTest()
        {
            const string adventurename = "AdventureName";
            var adventureDate = DateTime.Now;
            var adventureDuration = new TimeSpan(0, 0, 1);

            var adventureType = new AdventureType { Id = "typeId", Name = "AdventureType" };

            var adventureLocation = new Location(new GeoPoint { Lat = 0, Lon = 0 }, "AdventureLocation");

            var newReviewRequest = new AdventureReview
                                                   {
                                                       AdventureName = adventurename,
                                                       AdventureDate = adventureDate,
                                                       AdventureDuration = adventureDuration,
                                                       AdventureType = adventureType,
                                                       AdventureLocation = adventureLocation
                                                   };

            var newReviewRequestOutput = new AdventureReview
                                                         {
                                                             Id = "newId",
                                                             AdventureName = adventurename,
                                                             AdventureDate = adventureDate,
                                                             AdventureDuration = adventureDuration,
                                                             AdventureType = adventureType,
                                                             AdventureLocation = adventureLocation
                                                         };

            var updateReviewRequestOutput = new AdventureReview
            {
                Id = "differentId",
                AdventureName = adventurename,
                AdventureDate = adventureDate,
                AdventureDuration = adventureDuration,
                AdventureType = adventureType,
                AdventureLocation = adventureLocation
            };

            var expectedNewReviewResponse = new AdventureReviewSaveResponse(newReviewRequest)
                                                                        {
                                                                            AdventureReview = newReviewRequestOutput
                                                                        };

            var expectedUpdateReviewResponse = new AdventureReviewSaveResponse(updateReviewRequestOutput)
                                                   {
                                                       AdventureReview = updateReviewRequestOutput
                                                   };

            var mock = new Mock<IAdventureReviewRepository>();
            mock.Setup(a => a.SaveAdventureReview(newReviewRequest)).Returns(expectedNewReviewResponse.AdventureReview);
            mock.Setup(a => a.SaveAdventureReview(updateReviewRequestOutput)).Returns(updateReviewRequestOutput);

            var target = new AdventureReviewService { AdventureReviewRepository = mock.Object };


            // new review
            var actual = target.OnPost(newReviewRequest) as AdventureReviewSaveResponse;
            Assert.AreEqual(expectedNewReviewResponse, actual);

            // update
            actual = target.OnPost(updateReviewRequestOutput) as AdventureReviewSaveResponse;
            Assert.AreEqual(expectedUpdateReviewResponse, actual);
        }
    }
}

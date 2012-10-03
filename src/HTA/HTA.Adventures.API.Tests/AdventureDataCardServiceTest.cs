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
    ///This is a test class for AdventureDataCardServiceTest and is intended
    ///to contain all AdventureDataCardServiceTest Unit Tests
    ///</summary>
    [TestClass]
    public class AdventureDataCardServiceTest
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
        public void OnGetSingleDataCardTest()
        {
            const string adventureid = "adventureId";
            var adventureReview = new AdventureReview { Id = adventureid };
            var getSingleDataCardRequest = new AdventureDataCard { Id = "dataCardId" };
            var singleDataCard = new AdventureDataCard { Id = "dataCardId", AdventureReview = adventureReview };
            var singleDataCardList = new List<AdventureDataCard> { getSingleDataCardRequest };

            var expectedSingleDataCardResponse = new AdventureDataCardGetResponse(getSingleDataCardRequest)
                                                     {DataCards = singleDataCardList};



            var mock = new Mock<IAdventureDataCardRepository>();
            mock.Setup(a => a.GetAdventureDataCardById(getSingleDataCardRequest.Id)).Returns(singleDataCard);


            var target = new AdventureDataCardService { AdventureDataCardRepository = mock.Object };

            var actual = target.OnGet(getSingleDataCardRequest) as AdventureDataCardGetResponse;
            Assert.AreEqual(expectedSingleDataCardResponse, actual);
        }

        [TestMethod]
        public void OnGetReviewDataCardsTest()
        {
            const string adventureid = "adventureId";
            var adventureReview = new AdventureReview { Id = adventureid };
            var getReviewDataCardsRequest = new AdventureDataCard { AdventureReview = adventureReview };

            var reviewDataCardList = new List<AdventureDataCard>
                                         {
                                             new AdventureDataCard {Id = "dataCardOne", AdventureReview = adventureReview},
                                             new AdventureDataCard {Id = "dataCardTwo", AdventureReview = adventureReview},
                                             new AdventureDataCard {Id = "dataCardThree", AdventureReview = adventureReview}
                                         };

            var expectedReviewDataCardsResponse = new AdventureDataCardGetResponse(getReviewDataCardsRequest) { DataCards = reviewDataCardList };


            var mock = new Mock<IAdventureDataCardRepository>();
            mock.Setup(a => a.GetAdventureDataCardsByReviewId(getReviewDataCardsRequest.AdventureReview.Id)).Returns(
                reviewDataCardList);

            var target = new AdventureDataCardService { AdventureDataCardRepository = mock.Object };

            var actual = target.OnGet(getReviewDataCardsRequest) as AdventureDataCardGetResponse;
            Assert.AreEqual(expectedReviewDataCardsResponse, actual);

        }

        /// <summary>
        ///A test for OnPost
        ///</summary>
        [TestMethod]
        public void OnPostNewTest()
        {
            var adventureReview = new AdventureReview { Id = "adventureId", AdventureName = "Adventure" };
            var newDataCardRequest = new AdventureDataCard { AdventureReview = adventureReview, Title = "title", Body = "body" };
            var savedNewDataCard = new AdventureDataCard { Id = "savedId", AdventureReview = adventureReview, Title = "title", Body = "body" };

            var expectedNewDataCardResponse = new AdventureDataCardSaveResponse(newDataCardRequest) { DataCard = savedNewDataCard };

            var mock = new Mock<IAdventureDataCardRepository>();
            mock.Setup(a => a.SaveAdventureDataCard(newDataCardRequest)).Returns(savedNewDataCard);

            var target = new AdventureDataCardService { AdventureDataCardRepository = mock.Object };

            var actual = target.OnPost(newDataCardRequest) as AdventureDataCardSaveResponse;
            Assert.AreEqual(expectedNewDataCardResponse, actual);

        }

        /// <summary>
        ///A test for OnPost
        ///</summary>
        [TestMethod]
        public void OnPostUpdateTest()
        {
            var adventureReview = new AdventureReview { Id = "adventureId", AdventureName = "Adventure" };
            const string existingId = "existingId";
            var existingDataCardRequest = new AdventureDataCard { Id = existingId, AdventureReview = adventureReview, Title = "title", Body = "body" };
            var savedNewDataCard = new AdventureDataCard { Id = existingId, AdventureReview = adventureReview, Title = "title", Body = "body" };

            var expectedNewDataCardResponse = new AdventureDataCardSaveResponse(existingDataCardRequest) { DataCard = savedNewDataCard };

            var mock = new Mock<IAdventureDataCardRepository>();
            mock.Setup(a => a.SaveAdventureDataCard(existingDataCardRequest)).Returns(savedNewDataCard);

            var target = new AdventureDataCardService { AdventureDataCardRepository = mock.Object };

            var actual = target.OnPost(existingDataCardRequest) as AdventureDataCardSaveResponse;
            Assert.AreEqual(expectedNewDataCardResponse, actual);

        }
    }
}

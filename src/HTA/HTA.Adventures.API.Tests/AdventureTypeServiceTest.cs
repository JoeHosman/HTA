using HTA.Adventures.API.ServiceInterface;
using HTA.Adventures.Models;
using HTA.Adventures.Models.Types.Responses;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using HTA.Adventures.Models.Types;
using Moq;

namespace HTA.Websites.API.Tests
{


    /// <summary>
    ///This is a test class for AdventureTypeServiceTest and is intended
    ///to contain all AdventureTypeServiceTest Unit Tests
    ///</summary>
    [TestClass()]
    public class AdventureTypeServiceTest
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
        ///A test for OnPost
        ///</summary>
        [TestMethod()]
        public void OnPostTest()
        {
            AdventureType request = new AdventureType() { Name = "Valid", Description = "Type" };
            AdventureType savedRequest = new AdventureType() { Name = "Valid", Description = "Type" };
            AdventureTypeSaveResponse expected = new AdventureTypeSaveResponse(request);
            savedRequest.Id = "newId";
            expected.AdventureType = savedRequest;

            Mock<IAdventureTypeRepository> repoMock = new Mock<IAdventureTypeRepository>();
            repoMock.Setup(a => a.SaveAdventureType(request)).Returns(savedRequest);

            IAdventureTypeRepository repo = repoMock.Object;
            AdventureTypeService target = new AdventureTypeService() { AdventureTypeRepository = repo };

            AdventureTypeSaveResponse actual;
            actual = target.OnPost(request) as AdventureTypeSaveResponse;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for OnGet
        ///</summary>
        [TestMethod()]
        public void OnGetTest()
        {
            var adventureType = new AdventureType() { Name = "Valid", Description = "Type", Id = "Special" };
            AdventureType request = new AdventureType() { Id = "Special" };

            Mock<IAdventureTypeRepository> repoMock = new Mock<IAdventureTypeRepository>();
            repoMock.Setup(a => a.GetAdventureType(request.Id)).Returns(adventureType);

            IAdventureTypeRepository repo = repoMock.Object;
            AdventureTypeService target = new AdventureTypeService() { AdventureTypeRepository = repo };

            AdventureTypeGetResponse expected = new AdventureTypeGetResponse(request);
            expected.AdventureTypes.Add(adventureType);

            AdventureTypeGetResponse actual;
            actual = target.OnGet(request) as AdventureTypeGetResponse;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for OnDelete
        ///</summary>
        [TestMethod()]
        public void OnDeleteTest()
        {
            AdventureType request = new AdventureType() { Id = "Special" };

            AdventureTypeDeleteResponse expected = new AdventureTypeDeleteResponse(request) { Success = false };

            AdventureTypeService target = new AdventureTypeService(); // TODO: Initialize to an appropriate value

            AdventureTypeDeleteResponse actual;
            actual = target.OnDelete(request) as AdventureTypeDeleteResponse;
            Assert.AreEqual(expected, actual);
        }
    }
}

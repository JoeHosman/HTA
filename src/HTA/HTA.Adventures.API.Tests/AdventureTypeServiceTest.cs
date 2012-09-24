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
    ///This is a test class for AdventureTypeServiceTest and is intended
    ///to contain all AdventureTypeServiceTest Unit Tests
    ///</summary>
    [TestClass]
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
        [TestMethod]
        public void OnPostTest()
        {
            const string typeName = "Valid";
            const string typeDescription = "Type";
            var newTypeRequest = new AdventureType { Name = typeName, Description = typeDescription };
            var savedRequest = new AdventureType { Name = typeName, Description = typeDescription, Id = "newId" };
            var expectedNewResponse = new AdventureTypeSaveResponse(newTypeRequest) { AdventureType = savedRequest };

            var updateTypeRequest = new AdventureType { Name = typeName, Description = typeDescription, Id = "existingId" };
            var expectedUpdateResponse = new AdventureTypeSaveResponse(updateTypeRequest) { AdventureType = updateTypeRequest };


            var repoMock = new Mock<IAdventureTypeRepository>();
            repoMock.Setup(a => a.SaveAdventureType(newTypeRequest)).Returns(savedRequest);
            repoMock.Setup(a => a.SaveAdventureType(updateTypeRequest)).Returns(updateTypeRequest);

            IAdventureTypeRepository repo = repoMock.Object;
            var target = new AdventureTypeService { AdventureTypeRepository = repo };

            var actual = target.OnPost(newTypeRequest) as AdventureTypeSaveResponse;
            Assert.AreEqual(expectedNewResponse, actual);

            actual = target.OnPost(updateTypeRequest) as AdventureTypeSaveResponse;
            Assert.AreEqual(expectedUpdateResponse, actual);
        }

        /// <summary>
        ///A test for OnGet
        ///</summary>
        [TestMethod]
        public void OnGetTest()
        {
            var adventureType = new AdventureType { Name = "Valid", Description = "Type", Id = "Special" };
            var request = new AdventureType { Id = "Special" };

            var adventureTypeList = new List<AdventureType> { adventureType };

            var repoMock = new Mock<IAdventureTypeRepository>();
            repoMock.Setup(a => a.GetAdventureType(request.Id)).Returns(adventureType);
            repoMock.Setup(a => a.GetAdventureTypes()).Returns(adventureTypeList);

            IAdventureTypeRepository repo = repoMock.Object;
            var target = new AdventureTypeService { AdventureTypeRepository = repo };

            var expected = new AdventureTypeGetResponse(request);
            expected.AdventureTypes.Add(adventureType);

            var actual = target.OnGet(request) as AdventureTypeGetResponse;
            Assert.AreEqual(expected, actual);

            actual = target.OnGet(new AdventureType()) as AdventureTypeGetResponse;
            Assert.IsNotNull(actual);
            Assert.AreEqual(adventureTypeList.Count, actual.AdventureTypes.Count);
            Assert.AreEqual(adventureTypeList[0], actual.AdventureTypes[0]);
        }

        /// <summary>
        ///A test for OnDelete
        ///</summary>
        [TestMethod]
        public void OnDeleteTest()
        {
            var request = new AdventureType { Id = "Special" };

            var expected = new AdventureTypeDeleteResponse(request) { Success = false };

            var target = new AdventureTypeService();

            var actual = target.OnDelete(request) as AdventureTypeDeleteResponse;
            Assert.AreEqual(expected, actual);
        }
    }
}

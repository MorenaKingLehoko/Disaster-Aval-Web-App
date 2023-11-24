using Disaster_Aval.Pages.Disasters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using Disaster_Aval.Pages.Login;
using Microsoft.Extensions.Primitives;
using Disaster_Aval.Pages.Donation;
using System.ComponentModel.DataAnnotations;

namespace UnitTest_DAF
{
    [TestClass]
    public class AdminHomeModelTests
    {
        [TestMethod]
        public void OnGet_ShouldRetrieveActiveDisasters()
        {
            // Arrange
            var adminServiceMock = new Mock<IAdminService>();
            adminServiceMock.Setup(service => service.GetActiveDisasters()).Returns(new List<string> { "MockDisaster1", "MockDisaster2" });

            var pageModel = new AdminHomeModel(adminServiceMock.Object);

            // Act
            pageModel.OnGet();

            // Assert
            Assert.IsNotNull(pageModel.ActiveDisasters);
            Assert.AreEqual(2, pageModel.ActiveDisasters.Count);
            // Add more assertions based on your specific requirements
        }
    }
    [TestClass]
    public class CaptureSuccessModelTests
    {
        [TestMethod]
        public void OnGet_ShouldNotThrowException()
        {
            // Arrange
            var pageModel = new CaptureSuccessModel();

            // Act & Assert

            pageModel.OnGet();


        }

    }

    [TestClass]
    public class initializedCorrectlyTest
    {
        [TestMethod]
        public void AdminLoginModel_OnGet_ShouldInitializeProperties()
        {
            // Arrange
            var model = new AdminLoginModel();

            // Act
            model.OnGet();

            // Assert
            Assert.AreEqual(0, model.AdminId);
            Assert.IsNull(model.AdminPassword);
            Assert.IsNull(model.AdminEmail);
            Assert.IsNull(model.AdminName);
            Assert.IsNull(model.AdminSurname);
        }
        [TestClass]
        public class PopulatestheListTest
        {
            [TestMethod]
            public void DonationHomeModel_OnGet_ShouldRetrieveDisasters()
            {
                // Arrange
                var model = new DonationHomeModel();

                // Act
                model.OnGet();

                // Assert
                Assert.IsNotNull(model.Disasters);
                Assert.IsTrue(model.Disasters.Count > 0);
            }


        }
        [TestClass]
        public class RetrievesAndPopulatestheListTest
        {
            [TestMethod]
            public void StatsPageModel_OnGet_ShouldRetrieveStats()
            {
                // Arrange
                var model = new StatsPageModel();

                // Act
                model.OnGet();

                // Assert
                Assert.IsNotNull(model.ActiveDisasterStats);
                Assert.IsTrue(model.ActiveDisasterStats.Count > 0); 
            }

        }
    }
    [TestClass]
    public class DonationMessagetest
    {

    }
    

    
    













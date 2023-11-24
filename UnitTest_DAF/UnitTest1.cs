using Disaster_Aval.Pages.Disasters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using Disaster_Aval.Pages.Login;
using Microsoft.Extensions.Primitives;
using Disaster_Aval.Pages.Donation;

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
}














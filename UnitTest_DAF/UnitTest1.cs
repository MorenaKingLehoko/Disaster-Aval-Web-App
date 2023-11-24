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
    public class MoneyDonationsModelTests
    {
        [TestMethod]
        public void OnPost_ShouldRetrieveValuesFromForm()
        {
            // Arrange
            var model = new MoneyDonationsModel();
            var context = new DefaultHttpContext();
            model.PageContext = new PageContext { HttpContext = context };

            // Set up form values for testing
            context.Request.Form = new FormCollection(new Dictionary<string, StringValues>
        {
            { "UserID", "1" },
            { "DisasterID", "2" },
            { "Amount", "100" },
            { "Confirm", "1" }
        });

            // Act
            model.OnPost();

            // Assert
            Assert.AreEqual(1, model.UserID, "UserID does not match.");
            Assert.AreEqual(2, model.DisasterID, "DisasterID does not match.");
            Assert.AreEqual(100, model.Amount, "Amount does not match.");
            Assert.AreEqual(1, model.Confirm, "Confirm does not match.");
        }
    }
}













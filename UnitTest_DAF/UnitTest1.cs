using Disaster_Aval.Pages.Disasters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using Disaster_Aval.Pages.Login;
using Microsoft.Extensions.Primitives;

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
    public class AdminLoginModelTests
    {
        [TestMethod]
        public void OnPost_ShouldRetrieveValuesFromForm()
        {
            // Arrange
            var model = new AdminLoginModel();
            var context = new DefaultHttpContext();
            model.PageContext = new PageContext { HttpContext = context };

            // Simulate form values
            context.Request.Form = new FormCollection(new Dictionary<string, StringValues>
            {
                { "AdminEmail", "test@example.com" },
                { "AdminPassword", "password123" },
                { "AdminName", "John" },
                { "AdminSurname", "Doe" }
                // Add other form values as needed
            });

            // Act
            var result = model.OnPost();

            // Assert
            Assert.IsInstanceOfType(result, typeof(RedirectToPageResult));
            Assert.AreEqual("/Disasters/AdminHome", (result as RedirectToPageResult)?.PageName);  // Change "YourExpectedPage" to the expected page name

            // Add more assertions based on your specific requirements
            Assert.AreEqual("test@example.com", model.AdminEmail);
            Assert.AreEqual("password123", model.AdminPassword);
            Assert.AreEqual("John", model.AdminName);
            Assert.AreEqual("Doe", model.AdminSurname);
            // Add assertions for other properties as needed
        }
    }
}
    
    






    

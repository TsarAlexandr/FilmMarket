using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NewProject.Controllers;
using NewProject.Models;
using Moq;
using Microsoft.AspNetCore.Http;
 

namespace NewProject.Test
{
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void CheckIndex_ReturnCorrectView()
        {
            var controller = new HomeController();

            var res = controller.Index() as ViewResult;

            Assert.AreEqual("Index", res.ViewName);

        }
        [TestMethod]
        public void CheckContact_ReturnCorrectView()
        {
            var controller = new HomeController();

            var res = controller.Contact() as ViewResult;
            
            Assert.AreEqual("Contact", res.ViewName);

        }

        [TestMethod]
        public void CheckAbout_ReturnCorrectView()
        {
            var controller = new HomeController();

            var res = controller.About() as ViewResult;

            Assert.AreEqual("About", res.ViewName);

        }

        [TestMethod]
        public void CheckError_AddCorrectModel()
        {
            var http = new Mock<HttpContext>();
            http.Setup(m => m.TraceIdentifier).Returns("id1");
            
            var controller = new HomeController() {ControllerContext = new ControllerContext {HttpContext = http.Object } };

            var res = controller.Error() as ViewResult;

            Assert.IsNotNull((ErrorViewModel)res.Model);
        }
    }
}

using Microsoft.VisualStudio.TestTools.UnitTesting;
using NewProject.Models;
using NewProject.Models.AccountViewModels;
using NewProject.Controllers;
using Microsoft.AspNetCore.Identity;
using Moq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace NewProject.Test
{
    [TestClass]
    public class UnitTest1
    {
        private Mock<UserManager<ApplicationUser>> userContext;
        private Mock<SignInManager<ApplicationUser>> signContext;
        
        [TestInitialize]
        public void Initialize()
        {
            var userStoreMock = new Mock<IUserStore<ApplicationUser>>();
            var context = new HttpContextAccessor { HttpContext = new Mock<HttpContext>().Object };
            var claims = new Mock<IUserClaimsPrincipalFactory<ApplicationUser>>().Object;

            userContext = new Mock<UserManager<ApplicationUser>>(userStoreMock.Object, null, null, null, null, null, null, null, null);
            signContext = new Mock<SignInManager<ApplicationUser>>(userContext.Object, context, claims, null, null, null);
        }
       
        [TestMethod]
        public async Task CheckLogin_WithBlockedParametrs_ReturnLockout()
        {
            LoginViewModel lvm = new LoginViewModel() { UserName = "user", Password = "_aA12345"};
            signContext.Setup(m => m.PasswordSignInAsync(lvm.UserName, lvm.Password, false, false)).Returns(Task.FromResult(Microsoft.AspNetCore.Identity.SignInResult.LockedOut));

            var controller = new AccountController(userContext.Object, signContext.Object);

            var registrRes = await controller.Login(lvm) as RedirectToActionResult;

            
            Assert.AreEqual("Lockout", registrRes.ActionName);
        }

       

        [TestMethod]
        public async Task CheckLogin_WithUnExistedModel_ReturnView()
        {
            LoginViewModel lvm = new LoginViewModel() { UserName = "user", Password = "123"};
            signContext.Setup(m => m.PasswordSignInAsync(lvm.UserName, lvm.Password, false, false)).Returns(Task.FromResult(Microsoft.AspNetCore.Identity.SignInResult.Failed));

            var controller = new AccountController(userContext.Object, signContext.Object);

            var registrRes = await controller.Login(lvm) as ViewResult;


            Assert.AreEqual(1, registrRes.ViewData.ModelState.ErrorCount);
        }

        [TestMethod]
        public async Task CheckLogin_WithIncorrectModel_ReturnView()
        {
            var controller = new AccountController(userContext.Object, signContext.Object);

            controller.ModelState.AddModelError("new Error", "Error");
            
            var registrRes = await controller.Login(null, null) as ViewResult;
            

            Assert.IsTrue(registrRes.ViewData.ModelState.ContainsKey("new Error"));
        }

        [TestMethod]
        public async Task CheckLogin_WithCorrectParametrs_RedirectToHome()
        {
            LoginViewModel lvm = new LoginViewModel() { UserName = "newUser", Password = "-aA12345" };
            signContext.Setup(m => m.PasswordSignInAsync(lvm.UserName, lvm.Password, false, false)).Returns(Task.FromResult(Microsoft.AspNetCore.Identity.SignInResult.Success));

            var controller = new AccountController(userContext.Object, signContext.Object);

            var registrRes = await controller.Login(lvm) as RedirectToActionResult;

            var path = registrRes.ControllerName + "/" + registrRes.ActionName;
            Assert.AreEqual("Home/Index", path);
        }

        [TestMethod]
        public void CheckLogin_GetLoginView()
        {
            var controller = new AccountController(userContext.Object, signContext.Object);

            var res = controller.Login() as ViewResult;

            Assert.AreEqual("Login", res.ViewName);
        }
        [TestMethod]
        public void CheckRergistr_GetRegistrView()
        {
            var controller = new AccountController(userContext.Object, signContext.Object);

            var res = controller.Register() as ViewResult;

            Assert.AreEqual("Register", res.ViewName);
        }
        [TestMethod]
        public void CheckLockout_GetLockoutView()
        {
            var controller = new AccountController(userContext.Object, signContext.Object);

            var res = controller.Lockout() as ViewResult;

            Assert.AreEqual("Lockout", res.ViewName);
        }

        [TestMethod]
        public void CheckAccessDenied_GetAccessDeniedView()
        {
            var controller = new AccountController(userContext.Object, signContext.Object);

            var res = controller.AccessDenied() as ViewResult;

            Assert.AreEqual("AccessDenied", res.ViewName);
        }

        [TestMethod]
        public async Task CheckRegistration_WithCorrectModel_RedirectToHome()
        {
            RegisterViewModel rvm = new RegisterViewModel() { UserName = "email@gmail.com", Password = "_aA123456", ConfirmPassword = "_aA123456" };
            var user = new ApplicationUser { UserName = rvm.UserName };
            userContext.Setup(m => m.CreateAsync(It.IsAny<ApplicationUser>(), "_aA123456")).Returns(Task.FromResult(IdentityResult.Success));
            signContext.Setup(m => m.SignInAsync(user, false, null)).Returns(Task.CompletedTask);

            var controller = new AccountController(userContext.Object, signContext.Object);

            var registrRes = await controller.Register(rvm) as RedirectToActionResult;

            var path = registrRes.ControllerName + "/" + registrRes.ActionName;
            Assert.AreEqual("Home/Index", path);

        }
        [TestMethod]
        public async Task CheckRegistr_CannotRegistrWithIncorrectModel()
        {
            signContext.Setup(m => m.SignInAsync(It.IsAny<ApplicationUser>(), false, null)).Returns(Task.CompletedTask); 

            var controller = new AccountController(userContext.Object, signContext.Object);

            controller.ModelState.AddModelError("new Error", "Error");

            var registrRes = await controller.Register(null, null) as ViewResult;


            signContext.Verify(m => m.SignInAsync(It.IsAny<ApplicationUser>(), false, null), Times.Never());
        }

        [TestMethod]
        public async Task CheckLogout_RedirectToindex()
        {
            signContext.Setup(m => m.SignOutAsync()).Returns(Task.CompletedTask);
            var controller = new AccountController(userContext.Object, signContext.Object);

            var res = await controller.Logout() as RedirectToActionResult;
            var path = res.ControllerName + "/" + res.ActionName;

            Assert.AreEqual("Home/Index", path);

        }

        [TestMethod]
        public void CheckRedirectToLocal_UrlNull_RedirectToHome()
        {
            var controller = new AccountController(userContext.Object, signContext.Object);

            var res = controller.RedirectToLocal(null) as RedirectToActionResult;

            var path = res.ControllerName + "/" + res.ActionName;

            Assert.AreEqual("Home/Index", path);
        }

        [TestMethod]
        public void CheckRedirectToLocal_NotLocalUrl_RedirectToHome()
        {
            var urlHelper = new Mock<IUrlHelper>();
            urlHelper.Setup(m => m.IsLocalUrl("www.notLocal.url")).Returns(false);
            var controller = new AccountController(userContext.Object, signContext.Object) {Url = urlHelper.Object };

            var res = controller.RedirectToLocal("www.notLocal.url") as RedirectToActionResult;

            var path = res.ControllerName + "/" + res.ActionName;

            Assert.AreEqual("Home/Index", path);
        }

        [TestMethod]
        public void CheckRedirectToLocal_LocalUrl_RedirectToUrl()
        {
            var urlHelper = new Mock<IUrlHelper>();
            urlHelper.Setup(m => m.IsLocalUrl("Films/Index")).Returns(true);
            var controller = new AccountController(userContext.Object, signContext.Object) { Url = urlHelper.Object };

            var res = controller.RedirectToLocal("Films/Index") as RedirectResult;

            Assert.AreEqual("Films/Index", res.Url);
        }
    }
}

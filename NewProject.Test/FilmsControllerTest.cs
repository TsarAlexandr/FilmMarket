using Microsoft.VisualStudio.TestTools.UnitTesting;
using NewProject.Models;
using NewProject.Models.AccountViewModels;
using NewProject.Controllers;
using Moq;
using NewProject.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Update;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System;

namespace NewProject.Test
{
    [TestClass]
    public class FilmsControllerTest
    {
        private Mock<IRepository> repos;
        List<Film> films;

        [TestInitialize]
        public void Initialize()
        {
            films = new List<Film>()
            { new Film() {ID = 1, Description = "descr1", Name = "name1", Rating = 1, Price = "1,21" },
             new Film() {ID = 2, Description = "descr2", Name = "name2", Rating = 2, Price = "2,32" },
             new Film() {ID = 3, Description = "descr3", Name = "name3", Rating = 3, Price = "3,43" },
             new Film() {ID = 4, Description = "descr4", Name = "name4", Rating = 4, Price = "4,54" }};
            repos = new Mock<IRepository>();
            repos.Setup(m => m.Films).Returns(films);
        }

        [TestMethod]
        public void CheckIndex_ReturnValidNumberOfObjectInView()
        {
            FilmsController fContr = new FilmsController(repos.Object);

            var res = ((IEnumerable<Film>)(fContr.Index() as ViewResult).
                ViewData.Model).ToList();

            Assert.AreEqual(4, res.Count);
        }

        [TestMethod]
        public void GetDetails_WithIdNull_ReturnNotFound()
        {
            FilmsController controller = new FilmsController(repos.Object);

            var res = controller.Details(null) as NotFoundResult;

            Assert.AreEqual(404, res.StatusCode);

        }

        [TestMethod]
        public void GetDetails_OfUnexistigFilm_ReturnNotFound()
        {
            Film film5 = null;
            repos.Setup(m => m.getFilmById(5)).Returns(film5);

            FilmsController controller = new FilmsController(repos.Object);

            var res = controller.Details(5) as NotFoundResult;

            Assert.AreEqual(404, res.StatusCode);

        }

        [TestMethod]
        public void GetDetails_OfExistigFilm_ReturnViewOfValidFilm()
        {

            repos.Setup(m => m.getFilmById(1)).Returns(films.Find(x => x.ID == 1));

            FilmsController controller = new FilmsController(repos.Object);

            var res = (Film)(controller.Details(1) as ViewResult).Model;

            Assert.AreEqual(1, res.ID);

        }

        [TestMethod]
        public void CreateFilm_ReturnView()
        {
            FilmsController fContr = new FilmsController(repos.Object);

            var res = fContr.Create() as ViewResult;


            Assert.IsNotNull("Create", res.ViewName);
        }

        [TestMethod]
        public void CheckCreate_CreteValidModel_RedirectToIndex()
        {
            repos.Setup(m => m.addFilm(films.ElementAt(1))).Returns(true);

            FilmsController controller = new FilmsController(repos.Object);

            var res = controller.Create(films.ElementAt(1)) as RedirectToActionResult;

            Assert.AreEqual("Index", res.ActionName);
        }

        [TestMethod]
        public void CheckCreate_InvalidModel_CannotAdd()
        {
            Film film = new Film() { };

            repos.Setup(m => m.addFilm(film)).Returns(false);

            FilmsController controller = new FilmsController(repos.Object);

            controller.ModelState.AddModelError("Invalid price", "error");

            controller.Create(film);

            repos.Verify(m => m.addFilm(It.IsAny<Film>()), Times.Never);
        }

        [TestMethod]
        public void TryEdit_WithIdNull_ReturnNotFound()
        {
            FilmsController controller = new FilmsController(repos.Object);

            var res = controller.Edit(null) as NotFoundResult;

            Assert.AreEqual(404, res.StatusCode);

        }

        [TestMethod]
        public void TryEdit_AnUnexistigFilm_ReturnNotFound()
        {
            Film film5 = null;
            repos.Setup(m => m.getFilmById(5)).Returns(film5);

            FilmsController controller = new FilmsController(repos.Object);

            var res = controller.Edit(5) as NotFoundResult;

            Assert.AreEqual(404, res.StatusCode);

        }

        [TestMethod]
        public void TryEdit_AnExistigFilm_ReturnEditView()
        {

            repos.Setup(m => m.getFilmById(1)).Returns(films.Find(x => x.ID == 1));

            FilmsController controller = new FilmsController(repos.Object);

            var res = controller.Edit(1) as ViewResult;

            Assert.AreEqual("Edit", res.ViewName);

        }

        [TestMethod]
        public void TryEdit_ByIncorrectId_ReturnNotFound()
        {
            FilmsController controller = new FilmsController(repos.Object);

            var res = controller.Edit(1, films.ElementAt(1), null) as NotFoundResult;

            Assert.AreEqual(404, res.StatusCode);
        }

        [TestMethod]
        public void CannotUpdate_UsingIncorrectModel()
        {
            Film film = new Film() { ID = 5, Price = "price" };

            repos.Setup(m => m.updateFilm(film)).Returns(false);

            FilmsController controller = new FilmsController(repos.Object);

            controller.ModelState.AddModelError("Invalid price", "error");

            controller.Edit(5, film, null);

            repos.Verify(m => m.updateFilm(It.IsAny<Film>()), Times.Never);
        }

        [TestMethod]
        public void TryEdit_UsingCorrectModel_RedirectToIndex()
        {
            repos.Setup(m => m.updateFilm(films.ElementAt(1))).Returns(true);

            FilmsController controller = new FilmsController(repos.Object);

            var res = controller.Edit(2, films.ElementAt(1), null) as RedirectToActionResult;

            Assert.AreEqual("Index", res.ActionName);
        }

        [TestMethod]
        public void TryEdit_UnExistedFilm_ReturnNotFound()
        {
            
            repos.Setup(m => m.updateFilm(It.IsAny<Film>())).Throws(new DbUpdateException("error", new Exception()));

            Film film = new Film() { ID = 6 };

            FilmsController controller = new FilmsController(repos.Object);

            var res = controller.Edit(6, film, null) as NotFoundResult;

            Assert.AreEqual(404, res.StatusCode);
        }

        [TestMethod]
        public void TryEdit_IncorretcModel_Exception()
        {
            repos.Setup(m => m.updateFilm(It.IsAny<Film>())).Throws(new DbUpdateException("error", new Exception()));

            FilmsController controller = new FilmsController(repos.Object);

            Assert.ThrowsException<Exception>(() => controller.Edit(2, films.ElementAt(1), null));

        }

        [TestMethod]
        public void CheckDelete_WithIdNull_ReturnNotFound()
        {
            FilmsController controller = new FilmsController(repos.Object);

            var res = controller.Delete(null) as NotFoundResult;

            Assert.AreEqual(404, res.StatusCode);

        }

        [TestMethod]
        public void CheckDelete_AnUnexistigFilm_ReturnNotFound()
        {
            Film film5 = null;
            repos.Setup(m => m.getFilmById(5)).Returns(film5);

            FilmsController controller = new FilmsController(repos.Object);

            var res = controller.Delete(5) as NotFoundResult;

            Assert.AreEqual(404, res.StatusCode);

        }

        [TestMethod]
        public void CheckDelete_AnExistigFilm_ReturnDeleteView()
        {

            repos.Setup(m => m.getFilmById(1)).Returns(films.Find(x => x.ID == 1));

            FilmsController controller = new FilmsController(repos.Object);

            var res = controller.Delete(1) as ViewResult;

            Assert.AreEqual("Delete", res.ViewName);

        }
        [TestMethod]
        public void CheckDelete_DeleteCorrectModel_RedirectToIndex()
        {
            repos.Setup(m => m.getFilmById(1)).Returns(films.Find(x => x.ID == 1));
            repos.Setup(m => m.deleteFilm(It.IsAny<Film>())).Returns(true);
            FilmsController controller = new FilmsController(repos.Object);

            var res = controller.DeleteConfirmed(1) as RedirectToActionResult;

            Assert.AreEqual("Index", res.ActionName);
        }

        [TestMethod]
        public void CheckBuy_WithIdNull_ReturnNotFound()
        {
            FilmsController controller = new FilmsController(repos.Object);

            var res = controller.Buy(null) as NotFoundResult;

            Assert.AreEqual(404, res.StatusCode);

        }

        [TestMethod]
        public void CheckBuy_AnUnexistigFilm_ReturnNotFound()
        {
            Film film5 = null;
            repos.Setup(m => m.getFilmById(5)).Returns(film5);

            FilmsController controller = new FilmsController(repos.Object);

            var res = controller.Buy(5) as NotFoundResult;

            Assert.AreEqual(404, res.StatusCode);

        }

        [TestMethod]
        public void CheckBuy_AnExistigFilm_ReturnBuyView()
        {

            repos.Setup(m => m.getFilmById(1)).Returns(films.Find(x => x.ID == 1));

            FilmsController controller = new FilmsController(repos.Object);

            var res = controller.Buy(1) as ViewResult;

            Assert.AreEqual("Buy", res.ViewName);

        }

        [TestMethod]
        public void CheckFilmExist_AnExistedModel_ReturnTrue()
        {
            FilmsController controller = new FilmsController(repos.Object);

            var res = controller.FilmExists(1);

            Assert.IsTrue(res);
        }

        [TestMethod]
        public void CheckFilmExist_AnUnexistedModel_ReturnFalse()
        {
            FilmsController controller = new FilmsController(repos.Object);

            var res = controller.FilmExists(10);

            Assert.IsFalse(res);
        }

    }
}

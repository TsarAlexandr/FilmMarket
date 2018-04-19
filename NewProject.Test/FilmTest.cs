using Microsoft.VisualStudio.TestTools.UnitTesting;
using NewProject.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace NewProject.Test
{
    [TestClass]
    public class FilmTest
    {
        [TestMethod]
        public void CheckIsRatingInRange_ZeroRating_ReturnFalse()
        {
            Film film = new Film();

            var res = film.isRatingInRange(0);

            Assert.IsFalse(res);
        }

        [TestMethod]
        public void CheckIsRatingInRange_OutOfRangeRating_ReturnFalse()
        {
            Film film = new Film();

            var res = film.isRatingInRange(12);

            Assert.IsFalse(res);
        }

        [TestMethod]
        public void CheckIsRatingInRange_CorrectRating_ReturnTrue()
        {
            Film film = new Film();

            var res = film.isRatingInRange(5);

            Assert.IsTrue(res);
        }


        [TestMethod]
        public void CheckIsStringCorrect_ShortString_ReturnFalse()
        {
            Film film = new Film();

            var res = film.isStringCorrect("");

            Assert.IsFalse (res);
        }

        [TestMethod]
        public void CheckIsStringCorrect_CorrectString_ReturnTrue()
        {
            Film film = new Film();

            var res = film.isStringCorrect("string");

            Assert.IsTrue(res);
        }

        [TestMethod]
        public void CheckIsPriceCorrect_EmptyPrice_ReturnFalse()
        {
            Film film = new Film();

            var res = film.isPriceCorrect("");

            Assert.IsFalse(res);
        }
        [TestMethod]
        public void CheckIsPriceCorrect_NotNumericPrice_ReturnFalse()
        {
            Film film = new Film();

            var res = film.isPriceCorrect("price");

            Assert.IsFalse(res);
        }
        [TestMethod]
        public void CheckIsPriceCorrect_ToManyNumbersAfterDot_ReturnFalse()
        {
            Film film = new Film();

            var res = film.isPriceCorrect("2,1231");

            Assert.IsFalse(res);
        }

        [TestMethod]
        public void CheckIsPriceCorrect_CorrectPrice_ReturnTrue()
        {
            Film film = new Film();

            var res = film.isPriceCorrect("2,33");

            Assert.IsTrue(res);
        }
    }
}

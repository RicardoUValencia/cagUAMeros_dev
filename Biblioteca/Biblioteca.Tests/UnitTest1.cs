using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.ComponentModel.DataAnnotations;

namespace Biblioteca.Tests
{
    [TestClass]
    public class SugerenciaControllerTests
    {
        [TestMethod]
        public void SugerenciasUsers()
        {
            var suger = new SugerenciaControllerTests();
            var Nombre = "JeffGutierritos";
            var Email = "";
            var Comentario = "";

            var valid = new ValidationContext(new { nombre = Nombre, correo = Email, comentario = Comentario});

            var rest = suger.GetValidationResult(Nombre, Email, Comentario, valid);

            Assert.AreEqual("Los campos deben estar llenos", rest.ErrorMessages);

        }
    }
}

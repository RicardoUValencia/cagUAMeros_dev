using Biblioteca.Controllers;
using Biblioteca.DBContext;
using Biblioteca.DTOS;
using Biblioteca.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Biblioteca.Tests.PruebasUnitarias
{
    [TestClass]
    public class UsuarioControllerTests
    {
        [TestMethod]
        public void agregarUsuario()
        {
            
           

            UsuarioDTO user = new UsuarioDTO();

            var control = new UsuarioController();

            var result = (RedirectToRouteResult)control.Agregar(user);

            Assert.IsTrue(result.RouteValues.ContainsKey("action"));
            
            Assert.AreEqual("Index", result.RouteValues["action"].ToString());
            

        }
    }
}

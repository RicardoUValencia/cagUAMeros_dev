using Biblioteca.Controllers;
using Biblioteca.DBContext;
using Biblioteca.DTOS;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Biblioteca.Tests
{
    //objetivo: probar que el método envia tabla funciona correctamente cuando el usuario si tiene prestamos
    //Precondiciones: Usuario con prestamos registrado
    [TestClass]
    public class LoginControllerTests
    {
        [TestMethod]
        public void EnviaTablaConPrestamos()
        {
            //preparacion

            String nombre = "Daniel Atonal Melgarejo";
            String email = "danielatonal8@gmail.com";
            List<PrestamoDTO> prestamos = new List<PrestamoDTO>();
            LoginController control = new LoginController();
          


            //ejecucion
            var result =(RedirectToRouteResult) control.EnviarTabla(nombre, email, prestamos) /*as ViewResult*/;



            //assert
            Assert.IsTrue(result.RouteValues.ContainsKey("action"));
            Assert.IsTrue(result.RouteValues.ContainsKey("controller"));
            Assert.AreEqual("Perfil", result.RouteValues["action"].ToString());
            Assert.AreEqual("Login", result.RouteValues["controller"].ToString());
        }

        //objetivo: probar que el método envia tabla funciona correctamente cuando el usuario no tiene prestamos
        //Precondiciones: Usuario sin prestamos registrado


        [TestMethod]
        public void EnviaTablaSinPrestamos()
        {
            //preparacion
            String nombre = "Fernando Lopez Castro";
            String email = "danielatonal8@outlook.com";
            List<PrestamoDTO> prestamos = new List<PrestamoDTO>();
            LoginController control = new LoginController();
            


            //ejecucion
            var result = (RedirectToRouteResult)control.EnviarTabla(nombre, email, prestamos) /*as ViewResult*/;



            //assert
            Assert.IsTrue(result.RouteValues.ContainsKey("action"));
            Assert.IsTrue(result.RouteValues.ContainsKey("controller"));
            Assert.AreEqual("Index", result.RouteValues["action"].ToString());
            Assert.AreEqual("Buscar", result.RouteValues["controller"].ToString());
        }
        
    }


    
    
}

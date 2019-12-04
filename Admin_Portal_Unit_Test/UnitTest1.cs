using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Admin_Portal;
using System.Web;
using System.Web.Mvc;
using Admin_Portal.Controllers;
using Admin_Portal.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest
{
    [TestClass]
    public class UnitTest1
    {



        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        //[TestMethod]
        //public void TestSuperAdminLogin()

        //{
        //    var userInfo = new LoginAttempt();
        //    userInfo.Email = "superadmin@email.com";
        //    userInfo.Password = "super";
        //    var attemptLogin = new LoginController();
        //    var result = (RedirectToRouteResult) attemptLogin.Auth(userInfo);
        //    Assert.AreEqual("Home", result.RouteValues["action"]);

        //}

        [TestMethod]
        public void TestSignOut()
        {
            LoginAttempt userInfo = new LoginAttempt();
            userInfo.Email = "superadmin@email.com";
            userInfo.Password = "super";
            LoginController attemptLogout = new LoginController();
            attemptLogout.Auth(userInfo);
            var result = (RedirectToRouteResult)attemptLogout.SignOut();
            Assert.AreEqual("Home", result.RouteValues["action"]);

        }
    }
}

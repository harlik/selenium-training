using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Edge;

namespace Selenium_Training_1
{
    [TestFixture]
    public class MyFirstTest
    {
        private IWebDriver driver;

        [SetUp]
        public void start()
        {
            driver = new EdgeDriver();
        }

        [Test]
        public void FirstTest()
        {
            driver.Url = "http://customerscanvas.com/";
        }

        [TearDown]
        public void stop()
        {
            driver.Quit();
            driver = null;
        }
    }

    [TestFixture]
    public class MyLoginTest
    {
        private IWebDriver driver;

        [SetUp]
        public void start()
        {
            driver = new EdgeDriver();
        }

        [Test]
        public void LoginTest()
        {
            driver.Url = "http://localhost:9999/admin/";
            driver.FindElement(By.Name("username")).SendKeys("admin");
            driver.FindElement(By.Name("password")).SendKeys("admin");
            driver.FindElement(By.Name("login")).Click();
        }

        [TearDown]
        public void stop()
        {
            driver.Quit();
            driver = null;
        }
    }

}

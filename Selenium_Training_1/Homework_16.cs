using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Remote;
using System.Collections.Generic;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Interactions;
using System.IO;
using System.Collections;

namespace Selenium_Training_1
{
    public class ClassHomework_16
    {
        private RemoteWebDriver driver;

        [SetUp]
        public void start()
        {

            DesiredCapabilities capability = new DesiredCapabilities();
            /*            capability.SetCapability("browser", "Edge");
                        capability.SetCapability("browser_version", "16.0");
                        capability.SetCapability("os", "Windows");
                        capability.SetCapability("os_version", "10");
                        capability.SetCapability("resolution", "1024x768");
                        capability.SetCapability("browserstack.user", "olga481");
                        capability.SetCapability("browserstack.key", "S84jpUfwbnpnb1jRnsKq");
            */

            capability.SetCapability("device", "iPad 5th");
            capability.SetCapability("realMobile", "true");
            capability.SetCapability("os_version", "11.0");

            capability.SetCapability("browserstack.local", "true");
            capability.SetCapability("browserstack.user", "olga481");
            capability.SetCapability("browserstack.key", "S84jpUfwbnpnb1jRnsKq");

            driver = new RemoteWebDriver(
              new Uri("http://hub-cloud.browserstack.com/wd/hub/"), capability
            );
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(15);
        }

        [Test]
        public void Homework_16()
        {
            driver.Navigate().GoToUrl("http://customerscanvas/simplepolygraphy");

        }

        [TearDown]
        public void stop()
        {
            driver.Quit();
            driver = null;
        }
    }
}

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
                        capability.SetCapability("browserstack.key", "xxxxxxxxxxxxxxxxxxxx");
            */

            capability.SetCapability("device", "iPad 5th");
            capability.SetCapability("realMobile", "true");
            capability.SetCapability("os_version", "11.0");
//            capability.SetCapability("deviceOrientation", "landscape");
            //            capability.SetCapability("browserstack.selenium_version", "3.8.0");
//            capability.SetCapability("browserstack.debug", "true");

            capability.SetCapability("browserstack.local", "true");
            capability.SetCapability("browserstack.user", "olga481");
            capability.SetCapability("browserstack.key", "xxxxxxxxxxxxxxxxxxxx");

            driver = new RemoteWebDriver(
              new Uri("http://hub-cloud.browserstack.com/wd/hub/"), capability
            );
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
        }

        [Test]
        public void Homework_16()
        {
//            driver.Navigate().GoToUrl("http://customerscanvas/simplepolygraphy");
            driver.Navigate().GoToUrl("http://localhost:80");
//            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromMinutes(2));
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
            wait.Until(ExpectedConditions.FrameToBeAvailableAndSwitchToIt(By.CssSelector("iframe#editorFrame")));
            wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector(".objectInspector")));
            wait.Timeout = TimeSpan.FromSeconds(5);
            IList<IWebElement> oiItems = driver.FindElements(By.CssSelector(".oi-item"));
            foreach (var item in oiItems)
            {
                item.Click();
                wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("#grips")));
            }

        }

        [TearDown]
        public void stop()
        {
            driver.Quit();
            driver = null;
        }
    }
}

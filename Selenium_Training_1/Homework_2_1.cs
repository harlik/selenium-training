using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Chrome;

namespace Selenium_Training_1
{
    public static class LiteCart
    {
        public static void Login(IWebDriver driver)
        {
            driver.Url = "http://localhost:9999/admin/";
            driver.FindElement(By.Name("username")).SendKeys("admin");
            driver.FindElement(By.Name("password")).SendKeys("admin");
            driver.FindElement(By.Name("login")).Click();
        }
    }

    [TestFixture]
    public class RunEdgeClass
    {
        private EdgeDriver edgeDriver;

        [SetUp]
        public void start()
        {
            edgeDriver = new EdgeDriver();
        }

        [Test]
        public void RunEdge()
        {
            LiteCart.Login(edgeDriver);
        }

        [TearDown]
        public void stop()
        {
            edgeDriver.Quit();
            edgeDriver = null;
        }
    }

    [TestFixture]
    public class RunFFclass
    {
        private FirefoxDriver ffDriver;

        [SetUp]
        public void start()
        {
            //running ESR
            /*
            FirefoxOptions options = new FirefoxOptions();
            options.UseLegacyImplementation = true;
            options.BrowserExecutableLocation = "C:\\Program Files (x86)\\Mozilla Firefox ESR\\firefox.exe";
            ffDriver = new FirefoxDriver(options);
            */
            //running nightly
            /*
            FirefoxOptions options = new FirefoxOptions();
            options.BrowserExecutableLocation = "C:\\Program Files\\Nightly\\firefox.exe";
            ffDriver = new FirefoxDriver(options);
            */

            //running Fifrefox as usual
            FirefoxOptions options = new FirefoxOptions();
            options.BrowserExecutableLocation = "C:\\Program Files (x86)\\Mozilla Firefox\\firefox.exe";
            ffDriver = new FirefoxDriver(options);
            /*
            Console.WriteLine(ffDriver.Capabilities.ToString());
            Console.WriteLine(options.ToString());
            Console.WriteLine(options.ToCapabilities().ToString());
            */

        }

        [Test]
        public void RunFF()
        {
            LiteCart.Login(ffDriver);
        }

        [TearDown]
        public void stop()
        {
            ffDriver.Quit();
            ffDriver = null;
        }
    }

    [TestFixture]
    public class RunChromeclass
    {
        private ChromeDriver chromeDriver;

        [SetUp]
        public void start()
        {
            chromeDriver = new ChromeDriver();
        }

        [Test]
        public void RunChrome()
        {
            LiteCart.Login(chromeDriver);
        }

        [TearDown]
        public void stop()
        {
            chromeDriver.Quit();
            chromeDriver = null;
        }
    }

    [TestFixture]
    public class RunIEclass
    {
        private InternetExplorerDriver ieDriver;

        [SetUp]
        public void start()
        {
            ieDriver = new InternetExplorerDriver();
        }

        [Test]
        public void RunIE()
        {
            LiteCart.Login(ieDriver);
        }

        [TearDown]
        public void stop()
        {
            ieDriver.Quit();
            ieDriver = null;
        }
    }

}

using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace Selenium_Training_1
{
    public class ClassHomework_17
    {
        private ChromeDriver driver;

        [SetUp]
        public void start()
        {
            driver = new ChromeDriver();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
        }

        [Test]
        public void Homework_17()
        {
            driver.Url = "http://localhost:9999/admin/?app=catalog&doc=catalog&category_id=1";

            if (driver.FindElements(By.CssSelector("form[name='login_form']")).Count == 1)
            {
                driver.FindElement(By.Name("username")).SendKeys("admin");
                driver.FindElement(By.Name("password")).SendKeys("admin");
                driver.FindElement(By.Name("login")).Click();
            };


            Console.Write("Supported logs for " + driver.Capabilities.BrowserName + ": ");
            foreach (var log in driver.Manage().Logs.AvailableLogTypes)
            {
                Console.Write(log + ", ");
            }
            Console.WriteLine();

            By ducksLocator = By.CssSelector("a[title='Edit'][href*='product']");
            int duckCount = driver.FindElements(ducksLocator).Count;
            int LogCount = driver.Manage().Logs.GetLog("browser").Count;

            for (int i = 0; i < duckCount; i++)
            {
                driver.FindElements(ducksLocator)[i].Click();
                Assert.True(driver.Manage().Logs.GetLog("browser").Count == LogCount);
                driver.Navigate().Back();
            }

            Console.WriteLine("Browser log:");
            foreach (var item in driver.Manage().Logs.GetLog("browser"))
            {
                Console.WriteLine(item);
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

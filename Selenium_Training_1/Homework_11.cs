using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using System.Collections.Generic;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Interactions;

namespace Selenium_Training_1
{
    [TestFixture(typeof(InternetExplorerDriver))]
    [TestFixture(typeof(FirefoxDriver))]
    [TestFixture(typeof(EdgeDriver))]
    [TestFixture(typeof(ChromeDriver))]

    public class ClassHomework_11<TWebDriver> where TWebDriver : IWebDriver, new()
    {
        private IWebDriver driver;

        [SetUp]
        public void start()
        {
            this.driver = new TWebDriver();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(3);
        }

        [Test]
        public void Homework_11()
        {
            driver.Url = "http://localhost:9999/en/create_account";

            string email = "harlozavr" + DateTime.Now.ToBinary() + "@somemail.com";
            string password = "12345";

            //create new account
            driver.FindElement(By.CssSelector("input[name='firstname']")).SendKeys("Olga");
            driver.FindElement(By.CssSelector("input[name='lastname']")).SendKeys("Svalova");
            driver.FindElement(By.CssSelector("input[name='address1']")).SendKeys("545, SomeStreet St.");
            driver.FindElement(By.CssSelector("input[name='postcode']")).SendKeys("12345");
            driver.FindElement(By.CssSelector("input[name='city']")).SendKeys("SomeTown");
            new SelectElement(driver.FindElement(By.CssSelector("select[name='country_code']"))).SelectByValue("US");
            new SelectElement(driver.FindElement(By.CssSelector("select[name='zone_code']"))).SelectByIndex(1);
            driver.FindElement(By.CssSelector("input[name='email']")).SendKeys(email);
            driver.FindElement(By.CssSelector("input[name='phone']")).SendKeys("2345678");
            driver.FindElement(By.CssSelector("input[name='password']")).SendKeys(password);
            driver.FindElement(By.CssSelector("input[name='confirmed_password']")).SendKeys(password);
            driver.FindElement(By.CssSelector("button[name='create_account']")).Click();

            //Logout from the newly created account
            driver.FindElement(By.CssSelector("a[href *= 'logout']")).Click();

            //Login again
            driver.FindElement(By.CssSelector("input[name='email']")).SendKeys(email);
            driver.FindElement(By.CssSelector("input[name='password']")).SendKeys(password);
            driver.FindElement(By.CssSelector("button[name='login']")).Click();

            //Logout again
            driver.FindElement(By.CssSelector("a[href *= 'logout']")).Click();
        }

        [TearDown]
        public void stop()
        {
            driver.Quit();
            driver = null;
        }
    }
}

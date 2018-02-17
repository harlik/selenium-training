using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Edge;

namespace Selenium_Training_1
{

    [TestFixture(typeof(InternetExplorerDriver))]
    [TestFixture(typeof(FirefoxDriver))]
    [TestFixture(typeof(EdgeDriver))]
    [TestFixture(typeof(ChromeDriver))]

    public class ClassBitworks_Mypolk<TWebDriver> where TWebDriver : IWebDriver, new()
    {
        private IWebDriver driver;

        [SetUp]
        public void start()
        {
            this.driver = new TWebDriver();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(3);
        }

        [Test]
        public void Bitworks_Mypolk()
        {
            //переход на сайт moypolk.ru
            driver.Url = "http://google.com/";
//            driver.Url = "http://moypolk.ru/";
            //нажатие на ссылку "выбрать другой регион"
            driver.FindElement(By.CssSelector("a.autoDetCity-select")).Click();
            //выбрать "Латвийская республика"
            driver.FindElement(By.CssSelector("a[href*='latviyskaya-respublika']")).Click();
            //выбрать "Юрмала"
            driver.FindElement(By.CssSelector("a[data-city-nid='344959']")).Click();
            //в поиске найти "Петров Иван"
            driver.FindElement(By.CssSelector("input#edit-search-block-form--2")).SendKeys("Петров Иван");
            driver.FindElement(By.CssSelector("a[href='/search']")).Click();
        }

        [TearDown]
        public void stop()
        {
            driver.Quit();
            driver = null;
        }
    }
}

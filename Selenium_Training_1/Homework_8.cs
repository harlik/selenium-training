using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Chrome;
using System.Collections.Generic;
using OpenQA.Selenium.Support.UI;

namespace Selenium_Training_1
{

    [TestFixture]
    public class ClassHomework_8
    {
        private EdgeDriver driver;

        [SetUp]
        public void start()
        {
            driver = new EdgeDriver();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(3);
        }

        [Test]
        public void Homework_8()
        {
            driver.Url = "http://localhost:9999/";
            IList <IWebElement> products = driver.FindElements(By.ClassName("product"));
            for (int i = 0; i < products.Count; i++)
            {
                IList<IWebElement> stickers = products[i].FindElements(By.ClassName("sticker"));
                Assert.True(stickers.Count == 1);
                Console.WriteLine(i + ". " + stickers[0].Text);
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
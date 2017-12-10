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
    public class ClassHomework_9_1
    {
        private EdgeDriver driver;

        [SetUp]
        public void start()
        {
            driver = new EdgeDriver();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(3);
        }

        [Test]
        public void Homework_9_1()
        {
            driver.Url = "http://localhost:9999/admin/?app=countries&doc=countries";

            if (driver.FindElements(By.CssSelector("form[name='login_form']")).Count == 1)
            {
                driver.FindElement(By.Name("username")).SendKeys("admin");
                driver.FindElement(By.Name("password")).SendKeys("admin");
                driver.FindElement(By.Name("login")).Click();
            };

            //находим все страны
            IList<IWebElement> Countries = driver.FindElements(By.CssSelector(".dataTable .row>td:nth-child(5)"));
            List<string> CountryNames = new List<string>(Countries.Count);

            for (int i = 0; i < Countries.Count; i++)
                //добавляем название страны в список названий стран
                CountryNames.Add(Countries[i].Text);
            //создаём эталонный список названий стран
            List<string> CountryNamesSorted = new List<string>(CountryNames);
            CountryNamesSorted.Sort();
            //сравниваем эталонный список и реальный
            CollectionAssert.AreEqual(CountryNames, CountryNamesSorted);
        }

        [TearDown]
        public void stop()
        {
            driver.Quit();
            driver = null;
        }
    }
}

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
    public class ClassHomework_9_2
    {
        private EdgeDriver driver;

        [SetUp]
        public void start()
        {
            driver = new EdgeDriver();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(3);
        }

        [Test]
        public void Homework_9_2()
        {
            driver.Url = "http://localhost:9999/admin/?app=geo_zones&doc=geo_zones";

            if (driver.FindElements(By.CssSelector("form[name='login_form']")).Count == 1)
            {
                driver.FindElement(By.Name("username")).SendKeys("admin");
                driver.FindElement(By.Name("password")).SendKeys("admin");
                driver.FindElement(By.Name("login")).Click();
            };

            //Получаем список стран
            IList <IWebElement> DataRows = driver.FindElements(By.CssSelector(".dataTable .row>td:nth-child(3)>a"));
            IList<string> LinksToCountries = new List<string>(0);
            //Создаём список ссылок на страны
            for (int i = 0; i < DataRows.Count; i++)
                LinksToCountries.Add(DataRows[i].GetAttribute("href"));

            //Проверяем зоны для всех найденных стран
            foreach (var Link in LinksToCountries)
            {
                Console.WriteLine("Link to country: " + Link);
                //Переходим на страницу зон страны
                driver.Navigate().GoToUrl(Link);
                //Получаем список зон
                IList<IWebElement> Zones = driver.FindElements(By.CssSelector("select[name*='zone_code']>option[selected='selected']"));
                Console.WriteLine("Number of zones: " + Zones.Count);
                IList <string> ZoneNames = new List<string>(0);
                //Создаём список названий зон
                foreach (var Zone in Zones)
                {
                    ZoneNames.Add(Zone.Text);
                    Console.WriteLine(Zone.Text);
                }
                //создаём эталонный список названий зон
                List<string> ZoneNamesSorted = new List<string>(ZoneNames);
                ZoneNamesSorted.Sort();
                //сравниваем эталонный список и реальный
                CollectionAssert.AreEqual(ZoneNames, ZoneNamesSorted);
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

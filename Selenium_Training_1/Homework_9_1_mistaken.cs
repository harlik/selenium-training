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
    public class ClassHomework_9_1_mistaken
    {
        private EdgeDriver driver;

        [SetUp]
        public void start()
        {
            driver = new EdgeDriver();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(3);
        }

        [Test]
        public void Homework_9_1_mistaken()
        {
            driver.Url = "http://localhost:9999/admin/?app=countries&doc=countries";

            if (driver.FindElements(By.CssSelector("form[name='login_form']")).Count == 1)
            {
                driver.FindElement(By.Name("username")).SendKeys("admin");
                driver.FindElement(By.Name("password")).SendKeys("admin");
                driver.FindElement(By.Name("login")).Click();
            };

            IList<IWebElement> DataRows = driver.FindElements(By.CssSelector(".dataTable .row"));
            List<String> CountryNames = new List<String>(DataRows.Count);
            IList<String> LinksToCountriesHavingZones = new List<String>(0);
            for (int i = 0; i < DataRows.Count; i++)
            {
                //получаем название страны
                CountryNames.Add(DataRows[i].FindElement(By.CssSelector("td:nth-child(5)")).Text);
                    //Получаем количество зон у страны
                    int ZonesNumber = 0;
                    if (Int32.TryParse(DataRows[i].FindElement(By.CssSelector("td:nth-child(6)")).Text, out ZonesNumber))
                    {
                        //если количество зон в стране больше 0
                        if (ZonesNumber > 0)
                        {
                            //сохраняем ссылку на страницу страны в список для последующей проверки зон
                            LinksToCountriesHavingZones.Add(DataRows[i].FindElement(By.CssSelector("td:nth-child(5) a")).GetAttribute("href"));
                        }
                    };
            };
            //создаём эталонный список названий стран
            List<String> CountryNamesSorted = new List<String>(CountryNames);
            CountryNamesSorted.Sort();
            //сравниваем эталонный список и реальный
            CollectionAssert.AreEqual(CountryNames, CountryNamesSorted);

            //проверяем зоны для всех найденных стран, у которых зоны указаны
            for (int i = 0; i < LinksToCountriesHavingZones.Count; i++)
            {
                driver.Navigate().GoToUrl(LinksToCountriesHavingZones[i]);
                Console.WriteLine("Link to zoned country: " + LinksToCountriesHavingZones[i]);
                IList<IWebElement> Zones = driver.FindElements(By.CssSelector("#table-zones tr td:nth-child(3)"));
                Console.WriteLine("Number of zones: " + (Zones.Count - 1));
                IList <String> ZoneNames = new List<String>(0);
                //Создаём список названий зон
                foreach (var Zone in Zones)
                    //Проверяем есть ли у зоны название - избегаем последней строчки таблицы, оставленной для ввода новых зон
                    if (Zone.Text.Length > 0) ZoneNames.Add(Zone.Text);
                //создаём эталонный список названий зон
                List<String> ZoneNamesSorted = new List<String>(ZoneNames);
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

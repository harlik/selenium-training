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
using System.IO;
using System.Collections;

namespace Selenium_Training_1
{
    [TestFixture(typeof(InternetExplorerDriver))]
    [TestFixture(typeof(FirefoxDriver))]
    [TestFixture(typeof(EdgeDriver))]
    [TestFixture(typeof(ChromeDriver))]

    public class ClassHomework_14<TWebDriver> where TWebDriver : IWebDriver, new()
    {
        private IWebDriver driver;

        [SetUp]
        public void start()
        {

            this.driver = new TWebDriver();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
        }

        [Test]
        public void Homework_14()
        {
            driver.Url = "http://localhost:9999/admin/?app=countries&doc=countries";

            if (driver.FindElements(By.CssSelector("form[name='login_form']")).Count == 1)
            {
                driver.FindElement(By.Name("username")).SendKeys("admin");
                driver.FindElement(By.Name("password")).SendKeys("admin");
                driver.FindElement(By.Name("login")).Click();
            };

            driver.FindElement(By.CssSelector("a.button[href*='edit_country']")).Click();
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            //запоминаем хэндлер основного окна
            string mainWindow = driver.CurrentWindowHandle;
            //проходим по всем ссылкам, у которых есть "иконка в виде квадратика со стрелкой"
            foreach (var link in driver.FindElements(By.XPath("//i[contains(@class,'external-link')]/..")))
            {
                IList oldWindows = driver.WindowHandles;
                link.Click();
                wait.Until(d => {
                    if (oldWindows.Count == driver.WindowHandles.Count) return null;
                    string handler = "";
                    foreach (string h in driver.WindowHandles)
                    {
                        foreach (string oldh in oldWindows)
                        {
                            handler = string.Copy(oldh);
                            if (oldh == h) break;
                        }
                        if (handler != h) { handler = string.Copy(h); break; }
                    }
                    d.SwitchTo().Window(handler);
                    return d;
                });
                driver.Close();
                driver.SwitchTo().Window(mainWindow);
            };
        }

        [TearDown]
        public void stop()
        {
            driver.Quit();
            driver = null;
        }
    }
}

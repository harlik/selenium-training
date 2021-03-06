﻿using System;
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
    [TestFixture(typeof(ChromeOptions))]
    [TestFixture(typeof(FirefoxOptions))]
    [TestFixture(typeof(EdgeOptions))]
    [TestFixture(typeof(InternetExplorerOptions))]

    public class ClassHomework_15<TDriverOptions> where TDriverOptions : DriverOptions, new()
    {
        private IWebDriver driver;

        [SetUp]
        public void start()
        {
            driver = new RemoteWebDriver(new Uri("http://customerscanvas:4222/wd/hub"), new TDriverOptions());
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
        }

        [Test]
        public void Homework_15()
        {
            driver.Navigate().GoToUrl("http://harlik.main.aurigma.com:9999/admin/?app=countries&doc=countries");

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));

            wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("form[name='login_form']")));

            driver.FindElement(By.Name("username")).SendKeys("admin");
            driver.FindElement(By.Name("password")).SendKeys("admin");
            driver.FindElement(By.Name("login")).Click();

            driver.FindElement(By.CssSelector("a.button[href*='edit_country']")).Click();
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

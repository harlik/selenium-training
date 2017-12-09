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
    public class ClassHomework_3_1
    {
        private EdgeDriver driver;

        [SetUp]
        public void start()
        {
            driver = new EdgeDriver();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(3);
        }

        [Test]
        public void Homework_3_1()
        {
            driver.Url = "http://localhost:9999/admin/";
            driver.FindElement(By.Name("username")).SendKeys("admin");
            driver.FindElement(By.Name("password")).SendKeys("admin");
            driver.FindElement(By.Name("login")).Click();
            By MenuElementLocator = By.CssSelector("#box-apps-menu>li");
            By SubmenuElementLocator = By.CssSelector(".docs>li");
            By PageContentLocator = By.Id("content");
            By PageHeaderLocator = By.TagName("H1");
            //считаем количество элементов меню
            int menuCount = driver.FindElements(MenuElementLocator).Count;
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(3));
            IWebElement content = driver.FindElement(By.Id("content"));
            //проходим по меню
            for (int i = 0; i < menuCount; i++)
            {
                //кикаем очередной пункт меню. 
                //поскольку при переходе по ссылке в меню перезагружается вся страница, то запрашивать элементы меню приходится каждый раз
                driver.FindElements(MenuElementLocator)[i].Click();
                //ждём, пока страница перезагрузится, т.е. пропадёт содержимое предыдущей страницы и появится новое
                wait.Until(ExpectedConditions.StalenessOf(content));
                content = driver.FindElement(PageContentLocator);
                //проверяем наличие заголовка
                IWebElement header = driver.FindElement(PageHeaderLocator);
                Console.WriteLine(header.Text);
                //считаем кол-во элементов подменю
                int submenuCount = driver.FindElements(SubmenuElementLocator).Count;
                //проходим по подменю
                for (int j = 1; j < submenuCount; j++)
                {
                    //кикаем очередной пункт подменю. 
                    //поскольку при переходе по ссылке в подменю перезагружается вся страница, то запрашивать элементы меню приходится каждый раз
                    driver.FindElements(SubmenuElementLocator)[j].Click();
                    //ждём, пока страница перезагрузится, т.е. пропадёт содержимое предыдущей страницы и появится новое
                    wait.Until(ExpectedConditions.StalenessOf(content));
                    content = driver.FindElement(PageContentLocator);
                    //проверяем наличие заголовка
                    header = driver.FindElement(PageHeaderLocator);
                    Console.WriteLine(header.Text);
                }
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
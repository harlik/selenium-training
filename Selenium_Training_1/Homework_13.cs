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

namespace Selenium_Training_1
{
    [TestFixture(typeof(InternetExplorerDriver))]
    [TestFixture(typeof(FirefoxDriver))]
    [TestFixture(typeof(EdgeDriver))]
    [TestFixture(typeof(ChromeDriver))]

    public class ClassHomework_13<TWebDriver> where TWebDriver : IWebDriver, new()
    {
        private IWebDriver driver;

        [SetUp]
        public void start()
        {
            this.driver = new TWebDriver();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(3);
        }

        [Test]
        public void Homework_13()
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            By numberOfItemsLocator = By.CssSelector("span.quantity");
            for (int i = 0; i < 3; i++)
            {
                // 1) открыть главную страницу
                driver.Url = "http://localhost:9999/en/";
                // 2) открыть первый товар из списка
                driver.FindElement(By.CssSelector(".product a.link")).Click();
                // 2) добавить его в корзину (при этом может случайно добавиться товар, который там уже есть, ничего страшного)
                wait.Until(ExpectedConditions.ElementIsVisible(numberOfItemsLocator));
                int numberOfItems = Int32.Parse(driver.FindElement(numberOfItemsLocator).Text);
                IList<IWebElement> selectOptions = driver.FindElements(By.CssSelector("select[name*='options']"));
                foreach (var option in selectOptions)
                    new SelectElement(option).SelectByIndex(1);
                driver.FindElement(By.CssSelector("button[name='add_cart_product']")).Click();
                // 3) подождать, пока счётчик товаров в корзине обновится
                numberOfItems++;
                wait.Until(ExpectedConditions.TextToBePresentInElement(driver.FindElement(numberOfItemsLocator), numberOfItems.ToString()));
                // 4) вернуться на главную страницу, повторить предыдущие шаги ещё два раза, чтобы в общей сложности в корзине было 3 единицы товара
            }
            // 5) открыть корзину (в правом верхнем углу кликнуть по ссылке Checkout)
            driver.FindElement(By.CssSelector("a.link[href*='checkout']")).Click();

            By removeButtonsLocator = By.CssSelector("button[name='remove_cart_item']");
            IList<IWebElement> removeButtons = driver.FindElements(removeButtonsLocator);
            By orderSummaryLocator = By.CssSelector("table.dataTable");
            // 6) удалить все товары из корзины один за другим, после каждого удаления подождать, пока внизу обновится таблица
            while (removeButtons.Count > 0)
            {
                wait.Until(ExpectedConditions.ElementIsVisible(orderSummaryLocator));
                IWebElement orderSummary = driver.FindElement(orderSummaryLocator);
                removeButtons[0].Click();
                wait.Until(ExpectedConditions.StalenessOf(orderSummary));
                removeButtons = driver.FindElements(removeButtonsLocator);
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

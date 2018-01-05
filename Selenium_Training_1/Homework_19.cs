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

    internal class Page
    {
        protected IWebDriver driver;
        protected WebDriverWait wait;

        public Page(IWebDriver driver)
        {
            this.driver = driver;
            this.wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));

        }
    };

    internal class MainPage : Page
    {
        public MainPage(IWebDriver driver) : base(driver) { }
        internal MainPage Open()
        {
            driver.Url = "http://localhost:9999/en/";
            return this;
        }

        internal MainPage ClickAProduct()
        {
            driver.FindElement(By.CssSelector(".product a.link")).Click();
            return this;
        }
    };

    internal class ProductPage : Page
    {
        public ProductPage(IWebDriver driver) : base(driver) { }
        internal ProductPage WaitUntilNumberOfItemsIs(int numberOfItems)
        {
            wait.Until(ExpectedConditions.TextToBePresentInElement(driver.FindElement(By.CssSelector("span.quantity")), numberOfItems.ToString()));
            return this;
        }

        internal ProductPage AddItemToCart()
        {
            IList<IWebElement> selectOptions = driver.FindElements(By.CssSelector("select[name*='options']"));
            foreach (var option in selectOptions)
                new SelectElement(option).SelectByIndex(1);
            driver.FindElement(By.CssSelector("button[name='add_cart_product']")).Click();
            return this;
        }

        internal ProductPage OpenCart()
        {
            driver.FindElement(By.CssSelector("a.link[href*='checkout']")).Click();
            return this;
        }
    };

    internal class CartPage : Page
    {

        public CartPage(IWebDriver driver) : base(driver) { }
        private IWebElement orderSummary;

        internal int ItemsCount()
        {
            return driver.FindElements(By.CssSelector("button[name='remove_cart_item']")).Count;
        }

        internal CartPage DeleteItem()
        {
            driver.FindElements(By.CssSelector("button[name='remove_cart_item']"))[0].Click();
            return this;
        }

        internal CartPage WaitUntilOrderSummaryIsVisible()
        {
            wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("table.dataTable")));
            IWebElement orderSummary = driver.FindElement(By.CssSelector("table.dataTable"));
            return this;
        }

        internal CartPage WaitUntilOrderSummaryIsGone()
        {
            wait.Until(ExpectedConditions.StalenessOf(orderSummary));
            return this;
        }


    };

    public class ClassHomework_19<TWebDriver> where TWebDriver : IWebDriver, new()
    {
        private IWebDriver driver;

        [SetUp]
        public void start()
        {
            driver = new TWebDriver();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
        }

        [Test]
        public void Homework_19()
        {
            MainPage mainPage = new MainPage(driver);
            ProductPage productPage = new ProductPage(driver);
            CartPage cartPage = new CartPage(driver);

            for (int i = 0; i < 3; i++)
            {
                mainPage.Open().ClickAProduct();
                productPage.AddItemToCart().WaitUntilNumberOfItemsIs(i+1);
            }

            productPage.OpenCart();

            while (cartPage.ItemsCount() > 0)
            {
                cartPage.WaitUntilOrderSummaryIsVisible().DeleteItem().WaitUntilOrderSummaryIsGone();
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

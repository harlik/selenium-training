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

    public class ClassHomework_12<TWebDriver> where TWebDriver : IWebDriver, new()
    {
        private IWebDriver driver;

        [SetUp]
        public void start()
        {

            InternetExplorerDriverService service = InternetExplorerDriverService.CreateDefaultService();
            service.LoggingLevel = InternetExplorerDriverLogLevel.Debug;
            service.LogFile = "D:\\iedriver.log";
            driver = new InternetExplorerDriver(service);
//            this.driver = new TWebDriver();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
        }

        [Test]
        public void Homework_12()
        {
            driver.Url = "http://localhost:9999/admin/?app=catalog&doc=catalog";

            if (driver.FindElements(By.CssSelector("form[name='login_form']")).Count == 1)
            {
                driver.FindElement(By.Name("username")).SendKeys("admin");
                driver.FindElement(By.Name("password")).SendKeys("admin");
                driver.FindElement(By.Name("login")).Click();
            };

            string productName = "Donald Duck by " + driver.GetType().ToString();

            //посчитаем сколько уже насоздавали аналогичных Дональдов
            int donaldsCount = driver.FindElements(By.XPath("//a[text()='" + productName + "']")).Count;

            //Create new product
            driver.FindElement(By.CssSelector("a[href *= 'edit_product']")).Click();

            //General tab
            driver.FindElement(By.CssSelector("input[name*='name']")).SendKeys(productName);
            driver.FindElement(By.CssSelector("input[name='status'][value='1']")).Click();
            driver.FindElement(By.CssSelector("input[name='product_groups[]'][value='1-3']")).Click();
            driver.FindElement(By.CssSelector("input[name='quantity']")).Clear();
            driver.FindElement(By.CssSelector("input[name='quantity']")).SendKeys("30");
            string absoluteFileName = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.FullName;
            absoluteFileName = Path.Combine(absoluteFileName, "donald-duck-mad.png");
            driver.FindElement(By.CssSelector("input[name='new_images[]']")).SendKeys(absoluteFileName);

            //Information tab
            driver.FindElement(By.CssSelector("a[href='#tab-information']")).Click();
            new SelectElement(driver.FindElement(By.CssSelector("select[name='manufacturer_id']"))).SelectByValue("1");
            driver.FindElement(By.CssSelector("input[name*='short_description']")).SendKeys("Donald Fauntleroy Duck");
            driver.FindElement(By.CssSelector(".trumbowyg-editor")).SendKeys("Walt Disney thought up the character when he heard his friend ");
            driver.FindElement(By.CssSelector(".trumbowyg-link-button")).Click();
            driver.FindElement(By.CssSelector(".trumbowyg-createLink-dropdown-button")).Click();
            driver.FindElement(By.CssSelector("input[name='url']")).SendKeys("http://www.telegraph.co.uk/culture/disney/10885903/Donald-Duck-10-surprising-facts-about-Walt-Disneys-character.html");
            driver.FindElement(By.CssSelector("input[name='text']")).SendKeys("Clarence Nash singing a \"duck\" version of \"Mary Had a Little Lamb.\"");
            driver.FindElement(By.CssSelector("button.trumbowyg-modal-submit")).Click();
            driver.FindElement(By.CssSelector(".trumbowyg-editor")).SendKeys(Keys.End);
            driver.FindElement(By.CssSelector(".trumbowyg-editor")).SendKeys(" Disney thought having negative Donald around would be a good contrast to Mickey's happy-go-lucky demeanor.");

            //Data tab
            driver.FindElement(By.CssSelector("a[href='#tab-data']")).Click();
            driver.FindElement(By.CssSelector("input[name='weight']")).Clear();
            driver.FindElement(By.CssSelector("input[name='weight']")).SendKeys("30");
            driver.FindElement(By.CssSelector("input[name='dim_x']")).Clear();
            driver.FindElement(By.CssSelector("input[name='dim_x']")).SendKeys("20");
            driver.FindElement(By.CssSelector("input[name='dim_y']")).Clear();
            driver.FindElement(By.CssSelector("input[name='dim_y']")).SendKeys("30");
            driver.FindElement(By.CssSelector("input[name='dim_z']")).Clear();
            driver.FindElement(By.CssSelector("input[name='dim_z']")).SendKeys("10");
            driver.FindElement(By.CssSelector("textarea[name*='attributes']")).SendKeys("Other \nMaterial: Plastic");

            //Prices tab
            driver.FindElement(By.CssSelector("a[href='#tab-prices']")).Click();
            driver.FindElement(By.CssSelector("input[name='purchase_price']")).Clear();
            driver.FindElement(By.CssSelector("input[name='purchase_price']")).SendKeys("6.66");
            new SelectElement(driver.FindElement(By.CssSelector("select[name='purchase_price_currency_code']"))).SelectByValue("USD");
            driver.FindElement(By.CssSelector("input[name='prices[USD]']")).SendKeys("9.99");


            driver.FindElement(By.CssSelector("button[name='save']")).Click();

            //проверим, что действительно добавился один Дональд - не подойдёт при параллельном запуске тестов
            Assert.True(driver.FindElements(By.XPath("//a[text()='" + productName + "']")).Count == donaldsCount + 1);
        }

        [TearDown]
        public void stop()
        {
            driver.Quit();
            driver = null;
        }
    }
}

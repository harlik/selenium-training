using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using System.Collections.Generic;
using OpenQA.Selenium.Support.UI;

namespace Selenium_Training_1
{
    [TestFixture]
    public class ClassHomework_10
    {
        //        private EdgeDriver driver;
        //        private ChromeDriver driver;
        //        private FirefoxDriver driver;
        private InternetExplorerDriver driver;

        public void CheckIfRedAndStrong(IWebElement Price)
        {
            //акционная цена красная
            string RGBAstr = Price.GetCssValue("color");
            RGBAstr = RGBAstr.Split('(')[1];
            IList<string> RGBA = RGBAstr.Substring(0, RGBAstr.Length - 1).Split(',');
            RGBA[1] = RGBA[1].Trim();
            RGBA[2] = RGBA[2].Trim();
            Assert.True((RGBA[0] != "0") && (RGBA[1] == "0") && (RGBA[2] == "0"), "Campaign Price is not red.");
            //акционная цена жирная
            Assert.True(Price.TagName.ToLower() == "strong", "Campaign Price is not strong.");
        }

        public void CheckIfGreyAndCrossed(IWebElement Price)
        {
            //обычная цена серая
            string RGBAstr = Price.GetCssValue("color");
            RGBAstr = RGBAstr.Split('(')[1];
            IList<string> RGBA = RGBAstr.Substring(0, RGBAstr.Length - 1).Split(',');
            RGBA[1] = RGBA[1].Trim();
            RGBA[2] = RGBA[2].Trim();
            Assert.True((RGBA[0] == RGBA[1]) && (RGBA[1] == RGBA[2]), "Regular Price is not grey.");
            //обычная цена зачёркнутая
            Assert.True((Price.GetCssValue("text-decoration").ToLower() == "line-through") ||
                (Price.GetCssValue("text-decoration-line").ToLower() == "line-through"), "Regular Price is not crossed.");
        }

        public void CheckIfCampaignIsLarger(IWebElement RegPrice, IWebElement CampPrice)
        {
            //акционная цена крупнее, чем обычная
            string RegPriceFontSize = RegPrice.GetCssValue("font-size");
            RegPriceFontSize = RegPriceFontSize.Substring(0, RegPriceFontSize.Length - 2);
            string CampPriceFontSize = CampPrice.GetCssValue("font-size");
            CampPriceFontSize = CampPriceFontSize.Substring(0, CampPriceFontSize.Length - 2);
            Assert.True(Decimal.Parse(CampPriceFontSize) > Decimal.Parse(RegPriceFontSize), "Font-size troubles.");
        }

        [SetUp]
        public void start()
        {
//            driver = new EdgeDriver();
//            driver = new ChromeDriver();
//            driver = new FirefoxDriver();
            driver = new InternetExplorerDriver();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(3);
        }

        [Test]
        public void Homework_10()
        {
            driver.Url = "http://localhost:9999/";

            string ProductName = driver.FindElement(By.CssSelector("#box-campaigns .product .name")).Text;
            IWebElement ProductRegularPrice = driver.FindElement(By.CssSelector("#box-campaigns .product .regular-price"));
            IWebElement ProductCampaignPrice = driver.FindElement(By.CssSelector("#box-campaigns .product .campaign-price"));
            string ProductRegularPriceText = ProductRegularPrice.Text;
            string ProductCampaignPriceText = ProductCampaignPrice.Text;

            CheckIfGreyAndCrossed(ProductRegularPrice);
            CheckIfRedAndStrong(ProductCampaignPrice);
            CheckIfCampaignIsLarger(ProductRegularPrice, ProductCampaignPrice);

            driver.FindElement(By.CssSelector("#box-campaigns .product a.link")).Click();

            driver.FindElement(By.CssSelector("h1.title"));

            //на главной странице и на странице товара совпадает текст названия товара
            Assert.True(driver.FindElement(By.CssSelector("h1.title")).Text == ProductName, "Product names are different.");

            ProductRegularPrice = driver.FindElement(By.CssSelector(".regular-price"));
            ProductCampaignPrice = driver.FindElement(By.CssSelector(".campaign-price"));

            //на главной странице и на странице товара совпадают цены (обычная и акционная)
            Assert.True(ProductRegularPriceText == ProductRegularPrice.Text, "Regular prices are different.");
            Assert.True(ProductCampaignPriceText == ProductCampaignPrice.Text, "Campaign prices are different.");

            CheckIfGreyAndCrossed(ProductRegularPrice);
            CheckIfRedAndStrong(ProductCampaignPrice);
            CheckIfCampaignIsLarger(ProductRegularPrice, ProductCampaignPrice);

        }

        [TearDown]
        public void stop()
        {
            driver.Quit();
            driver = null;
        }
    }
}

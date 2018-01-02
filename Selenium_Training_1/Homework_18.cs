using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Support.UI;
using System.Collections;
using Titanium.Web.Proxy;
using Titanium.Web.Proxy.Models;
using System.Net;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Titanium.Web.Proxy.EventArguments;

namespace Selenium_Training_1
{
    [TestFixture(typeof(InternetExplorerDriver), typeof(InternetExplorerOptions))]
    [TestFixture(typeof(FirefoxDriver), typeof(FirefoxOptions))]
    [TestFixture(typeof(EdgeDriver), typeof(EdgeOptions))]
    [TestFixture(typeof(ChromeDriver), typeof(ChromeOptions))]

    public class ClassHomework_18<TWebDriver, TOptions>
        where TOptions : DriverOptions, new()
        where TWebDriver : IWebDriver, new()
    {

        //To access requestBody from OnResponse handler
        private IDictionary<Guid, string> requestBodyHistory
                = new ConcurrentDictionary<Guid, string>();

        public async Task OnRequest(object sender, SessionEventArgs e)
        {
            Console.WriteLine(e.WebSession.Request.Url);

            //read request headers
            var requestHeaders = e.WebSession.Request.Headers;

            Console.WriteLine("Request Headers: ");

            foreach (var header in requestHeaders)
            {
                Console.WriteLine(header);
            }

            var method = e.WebSession.Request.Method.ToUpper();
            if ((method == "POST" || method == "PUT" || method == "PATCH"))
            {
                //Get/Set request body bytes
                byte[] bodyBytes = await e.GetRequestBody();
                await e.SetRequestBody(bodyBytes);

                //Get/Set request body as string
                string bodyString = await e.GetRequestBodyAsString();
                await e.SetRequestBodyString(bodyString);

                //store request Body/request headers etc with request Id as key
                //so that you can find it from response handler using request Id
                requestBodyHistory[e.Id] = bodyString;
            }
        }

        //Modify response
        public async Task OnResponse(object sender, SessionEventArgs e)
        {
            //read response headers
            var responseHeaders = e.WebSession.Response.Headers;

            if (e.WebSession.Request.Method == "GET" || e.WebSession.Request.Method == "POST")
            {
                if (e.WebSession.Response.StatusCode == 200)
                {
                    if (e.WebSession.Response.ContentType != null && e.WebSession.Response.ContentType.Trim().ToLower().Contains("text/html"))
                    {
                        byte[] bodyBytes = await e.GetResponseBody();
                        await e.SetResponseBody(bodyBytes);

                        string body = await e.GetResponseBodyAsString();
                        await e.SetResponseBodyString(body);
                    }
                }
            }

            //access request body/request headers etc by looking up using requestId
            if (requestBodyHistory.ContainsKey(e.Id))
            {
                var requestBody = requestBodyHistory[e.Id];
            }
        }

        private IWebDriver driver;
        private ProxyServer proxyServer;

        [SetUp]
        public void start()
        {
            //Set up & start Titanium proxy
            proxyServer = new ProxyServer();
            proxyServer.TrustRootCertificate = true;

            proxyServer.BeforeRequest += OnRequest;
            proxyServer.BeforeResponse += OnResponse;

            var explicitEndPoint = new ExplicitProxyEndPoint(IPAddress.Any, 8888, true);

            proxyServer.AddEndPoint(explicitEndPoint);
            proxyServer.Start();

            //Set proxy for Web Driver
            TOptions options = new TOptions();
            Proxy proxy = new Proxy();
            proxy.Kind = ProxyKind.Manual;
            proxy.HttpProxy = "localhost:8888";
            options.Proxy = proxy;

            driver = (TWebDriver)Activator.CreateInstance(typeof(TWebDriver), options);

            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
        }

        [Test]
        public void Homework_18()
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
            proxyServer.Stop();
            driver.Quit();
            driver = null;
        }
    }
}

using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;

namespace ExpensesTest.WebPages
{
    [TestClass]
    public class FirefoxDriverTest
    {
        // In order to run the below test(s), 
        // please follow the instructions from http://go.microsoft.com/fwlink/?LinkId=619687
        // to install Microsoft WebDriver.

        private FirefoxDriver _driver;
        private readonly string _url = "http://localhost:50013";

        [TestInitialize]
        public void FirefoxDriverInitialize()
        {
            // Initialize edge driver 
            var options = new FirefoxOptions
            {
                PageLoadStrategy = PageLoadStrategy.Normal
            };
            _driver = new FirefoxDriver(options);
        }

        [TestMethod]
        public void EditPurchaseCard()
        {
            _driver.Url = _url+ "/Purchase/EditPurchaseCard?purchaseId_=1";

            var pPrice = _driver.FindElementById("purchPrice");
            var pCount = _driver.FindElementById("purchCount");
            var pDate = _driver.FindElementById("purchDate");

            var price = pPrice.GetAttribute("value");
            var count = pCount.GetAttribute("value");
            var date = pDate.GetAttribute("value");

            Assert.AreEqual(price, "20", "Цена неверная");
            Assert.AreEqual(count, "7", "Количество неверное");
            Assert.AreEqual(date, "2018-01-01", "Количество неверное");
        }

        [TestCleanup]
        public void FireFoxDriverCleanup()
        {
            _driver.Quit();
        }
    }
}

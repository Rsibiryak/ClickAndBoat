using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace ClickBoat
{
    /// <summary>
    /// Common procedures/tools for all web pages.
    /// </summary>
    public abstract class PageBase
    {
        protected IWebDriver Driver { get; set; }
        protected WebDriverWait Wait { get; set; }
        protected PageManager Manager { get; set; }

       


        /// <summary>
        /// Main page url.
        /// </summary>
        protected const string  BASEPAGE = "https://www.clickandboat.com/en/";
        /// <summary>
        /// Timeout for find element.
        /// </summary>
        protected const int TIMEOUT = 7;
       

        public PageBase(PageManager manager)
        {
            this.Manager = manager;
            Driver = Manager.Driver;
            Wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(TIMEOUT));
        }

        /// <summary>
        /// Element search on a page by Xpath.
        /// </summary>
        /// <param name="xpath"></param>
        /// <returns></returns>
        public IWebElement FindByXpath(string xpath)
        {
            IWebElement element = Wait.Until<IWebElement>((d) =>
            {
                return d.FindElement(By.XPath(xpath));
            });
            return element;
        }

        /// <summary>
        /// Open page. Send page name or page url.
        /// </summary>
        /// <param name="page"></param>
        public void OpenPage(string pageName)
        {
            string page = string.Empty;

            if (pageName == "main")
            {
                page = BASEPAGE;
            }
            else
            {
                page = pageName;
            }

            Driver.Manage().Window.Maximize();
            Driver.Navigate().GoToUrl(page);
        }

        //*[@id='results' and contains(@class,'mobile') ]


    }
}

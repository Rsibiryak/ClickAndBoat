using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace ClickBoat
{
    public class PageManager
    {
        public IWebDriver Driver { get; private set; }
        public MainPage MainPg { get; private set; }
        public YachtPage YachtPg { get; private set; }

        /// <summary>
        /// Instance of manager for manage all helpers. Only one.
        /// </summary>
        public static PageManager Instance { get; private set; }

        private PageManager()
        {
            Driver = new ChromeDriver();

            MainPg = new MainPage(this);
            YachtPg = new YachtPage(this);
        }

        ~PageManager()
        {
            Driver.Quit();
        }

        /// <summary>
        /// Get exixting or create new instance of PageManager.
        /// </summary>
        /// <returns></returns>
        public static PageManager GetInstance()
        {
            if (Instance == null)
            {
                Instance = new PageManager();
            }          
            return Instance;
        }     
    }
}

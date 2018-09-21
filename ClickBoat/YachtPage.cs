using System;
using OpenQA.Selenium;
using System.Globalization;

namespace ClickBoat
{
    /// <summary>
    /// Description web elements on yacht page.
    /// </summary>
    public class YachtPage : PageBase
    {    
        /// <summary>
        /// Yacht length.
        /// </summary>
        public double Length { get; set; }
        /// <summary>
        /// Yacht year. 
        /// </summary>
        public int Year { get; set; }
        /// <summary>
        /// Url yacht.
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// Broken link attribute.
        /// </summary>
        public bool BrokenLink { get; set; }
        /// <summary>
        /// Fild for error description.
        /// </summary>
        public string Error { get; set; }


        #region 
        //Xpath for page element.

        /// <summary>
        /// Fild "length".
        /// </summary>
        protected const string YACHTLENGTH = "//*[@itemprop='height']";
        /// <summary>
        /// Fild "yaer".
        /// </summary>
        protected const string YACHTYEAR = "//ul[@id='list-features']/li[text()='Year: ']/span";
        /// <summary>
        /// "Page not found" text.
        /// </summary>
        protected const string PAGENOTFOUND = "//h1[text()='Page not found']";
        #endregion

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="driver"></param>
        public YachtPage(PageManager manager) : base(manager)
        {
        }

        /// <summary>
        /// Constructor. Get yacht parameters.
        /// </summary>
        /// <param name="yachtUrl"></param>
        public YachtPage(string yachtUrl, PageManager manager) : base(manager)
        {
            string len;  //yacht length.
   
            if (Driver.FindElements(By.XPath(PAGENOTFOUND)).Count == 0) //if page open
            {
                len = FindByXpath(YACHTLENGTH).GetAttribute("innerHTML");
                Length = Double.Parse(len.Substring(0, len.Length - 1), CultureInfo.InvariantCulture);
                Url = yachtUrl;
                Year = Int32.Parse(FindByXpath(YACHTYEAR).GetAttribute("innerText"));
            }
            else //если страница не открылась
            {
                BrokenLink = true;
            }
        }

        /// <summary>
        /// Creat yacht copy.
        /// </summary>
        /// <returns></returns>
        public YachtPage Clone()
        {
            return (YachtPage)this.MemberwiseClone();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;


namespace ClickBoat
{
    /// <summary>
    /// Description web elements on main page.
    /// </summary>
    public class MainPage : PageBase
    {
        #region
        //Xpath for page element.

        /// <summary>
        /// MotoBoat page.
        /// </summary>
        protected const string MOTOBOAT = "//*[@href='https://www.clickandboat.com/en/boat-rental/type/motorboat' and @title='Motorboat charter']";
        /// <summary>
        /// Left slider "length".
        /// </summary>
        protected const string LENGTHSLIDERLEFT = "//*[@class='option advancedOption filter-length']/descendant::span[5]";
        /// <summary>
        /// Slider "length" length.
        /// </summary>
        protected const string LENGTHSLIDERWEIGHT = "//*[@class='option advancedOption filter-length']/descendant::div[4]";
        /// <summary>
        /// Right slider "length".
        /// </summary>
        protected const string LENGTHSLIDERMAX = "//*[@class='option advancedOption filter-length']/descendant::span[4]";
        /// <summary>
        /// Left slider "Year"..
        /// </summary>
        protected const string YEARSLIDERLIFT = "//*[@class='option advancedOption filter-years']/descendant::span[5]";
        /// <summary>
        /// Slider lenght "Year".
        /// </summary>
        protected const string YEARSLIDERWEIGHT = "//*[@class='option advancedOption filter-years']/descendant::div[4]";
        /// <summary>
        /// Right slider "Year".
        /// </summary>
        protected const string YEARSLIDERMAX = "//*[@class='option advancedOption filter-years']/descendant::span[4]";
        /// <summary>
        /// List div element with url.
        /// </summary>
        protected const string YACHTLINKDIV = "//article[@class='itemProduct']/div";
        /// <summary>
        /// "Next page" button.
        /// </summary>
        protected const string NEXTPAGE = "//*[@class='page-link next ']";
        /// <summary>
        /// Number last page.
        /// </summary>
        protected const string PAGECOUNT = "//*[@class='pagination']/ul/li[count(//*[@class='page-link  '])+1]/a";
        /// <summary>
        /// Sign of load page. 
        /// </summary>
        private const string PAGELOADATTRIBUTE = "//*[@id='results' and contains(@class,'loadResults') ]";
        #endregion

        public MainPage(PageManager manager) : base(manager)
        {
        }

        /// <summary>
        /// Calculate slader parameter.
        /// </summary>
        /// <param name="sliderName"></param>
        /// <param name="setValue"></param>
        /// <param name="slider"></param>
        /// <param name="value"></param>
        /// <param name="width"></param>
        /// <param name="maxSpanValue"></param>
        public void GetSliderPosition(string sliderName, int setValue, out IWebElement slider, out int value, out int width, out int maxSpanValue )
        {
            string sliderWidth;
            string sliderXpath;
            string maxValue;
            int    minVal; 

            if (sliderName == "length")
            {
                sliderWidth = LENGTHSLIDERWEIGHT;
                sliderXpath = LENGTHSLIDERLEFT;
                minVal = 0;
                maxValue = LENGTHSLIDERMAX;
            }
            else
            {
                sliderWidth = YEARSLIDERWEIGHT;
                sliderXpath = YEARSLIDERLIFT;
                minVal = 1998;
                maxValue = YEARSLIDERMAX;
            }

            width = FindByXpath(sliderWidth).Size.Width;
            slider = FindByXpath(sliderXpath);
            value = setValue - minVal;
            maxSpanValue = int.Parse(FindByXpath(maxValue).Text) - minVal;
        }
               
        /// <summary>
        /// Set left slider.
        /// </summary>
        /// <param name="filterName">Sleder name.</param> 
        /// <param name="value">Slider value.</param>  
        public void SetSlider(string sliderName, int value)
        {
            int width = 0;
            int maxSpanValue = 0;
            IWebElement slider = null;
            Actions move = new Actions(Driver);
          
            switch (sliderName)
            {
                case "length":
                    GetSliderPosition("length", value, out slider, out value, out width, out maxSpanValue);
                    break;
                case "year":
                    GetSliderPosition("year", value, out slider, out value, out width, out maxSpanValue);
                    break;
                default:
                    throw new Exception("Incorrect slider name. Expected: length, year");
            }

            move.DragAndDropToOffset(slider, value * width / maxSpanValue, 0).Build();
            move.Build().Perform();
        }

        /// <summary>
        /// Next page click.
        /// </summary>
        public void NextPage()
        {
            if (Driver.FindElements(By.XPath(NEXTPAGE)).Count != 0)
            {
                FindByXpath(NEXTPAGE).Click();
            }
            PageWait();
        }

        /// <summary>
        /// Get numder last page.
        /// </summary>
        /// <returns></returns>
        public int PageCount()
        {
            return  Int32.Parse(FindByXpath(PAGECOUNT).GetAttribute("innerText"));
        }

        /// <summary>
        /// Get yacht urls from all pages.
        /// </summary>
        /// <returns></returns>
        public string[] UrlFromPages()
        {
            int pageCount = PageCount();  //Number of page.
            string dataUrl;               //Coded url yacht.
            string pageUrl =Driver.Url;  //Url the fist page.
            IReadOnlyCollection<IWebElement> boatLinks;   //List div elements of yacht.
            List<string> urls = new List<string>();       //List yacht ulrs from all page. 

            for (int i = 1; i <= pageCount; i++)  //Open each page.
            {
                boatLinks = Driver.FindElements(By.XPath(YACHTLINKDIV));
                for (int j = 0; j < boatLinks.Count; j++) //Each data-url on the page  convert to url.
                {
                    dataUrl = boatLinks.ElementAt(j).GetAttribute("data-url");
                    urls.Add(Encoding.UTF8.GetString(Convert.FromBase64String(dataUrl)));
                }
                NextPage();
            }

            OpenPage(pageUrl); //Open the first page.
            return urls.ToArray();
        }

        /// <summary>
        /// Get parameters each yacht.
        /// </summary>
        /// <param name="urls">List yacht url.</param>
        /// <returns>List yacht with parameters.</returns>
        public List<YachtPage> GetYachtsInfo(string[] urls)
        {
            List<YachtPage> yachtsInfo = new List<YachtPage>();  //All yachts with info.

            for (int i = 0; i < urls.Length; i++)  // Open each yacht page.  
            {              
                OpenPage(urls[i]);
                YachtPage yacht = new YachtPage(urls[i], Manager);
                yachtsInfo.Add(yacht);
            }
            return yachtsInfo;
        }

        /// <summary>
        /// Open moto boat page.
        /// </summary>
        public void GetMotoBoat()
        {
            FindByXpath(MOTOBOAT).Click();
        }

        /// <summary>
        /// Wail until all yachts load. 
        /// </summary>
        public void PageWait()
        {
            int i; 
            int j=1; //Counter.
            i = Driver.FindElements(By.XPath(PAGELOADATTRIBUTE)).Count;

            while (i != 0 && j < TIMEOUT*2 )
            {
                Thread.Sleep(500);
                j++;
            }
        }
    }
}

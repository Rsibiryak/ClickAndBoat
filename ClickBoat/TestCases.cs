using System;
using System.Collections.Generic;
using System.Threading;
using NUnit.Framework;

namespace ClickBoat
{
    /// <summary>
    /// Check filtering onn moto boat page.
    /// </summary>
    [TestFixture]
    class TestCases  : TestBase
    {
        /// <summary>
        /// Yacht list with errors.
        /// </summary>
        List<YachtPage> YachtsInfoErrors { get; set; }

        /// <summary>
        /// Filter work check.
        /// </summary>
        [Test]
        public void FiltrsCheck()
        {
            YachtsInfoErrors = TestFiltrsCheck(27, 2009);
            foreach (YachtPage yacht in YachtsInfoErrors) 
            {
             Console.WriteLine(yacht.Error);
            }
            Assert.AreEqual(0, YachtsInfoErrors.Count);
        }

        /// <summary>
        /// Get yacht list with errors.
        /// </summary>
        /// <param name="lengthFrom">Yacht lenght min.</param>
        /// <param name="yearFrom">Yacht year min.</param>
        /// <returns>List yacht with errors</returns>
        public List<YachtPage> TestFiltrsCheck(int lengthFrom, int yearFrom)
        {
            string[] boatLinks;                                // Yachts links from all pages.
            List<YachtPage> yachtsInfo;                        // Yacht list for check.  

            Manager.MainPg.OpenPage("main");
            Manager.MainPg.GetMotoBoat();
            Manager.MainPg.SetSlider("length", lengthFrom);
            Manager.MainPg.SetSlider("year", yearFrom);
            Manager.MainPg.PageWait();
           
            boatLinks = Manager.MainPg.UrlFromPages();                       //Get yachts links.
            yachtsInfo = Manager.MainPg.GetYachtsInfo(boatLinks);            //Get yachts with parameters.

            return FilteringCheck(yachtsInfo, "length", lengthFrom, yearFrom);
        }

        /// <summary>
        /// Check yacht filtering. 
        /// </summary>
        /// <param name="yachts">Yachts list with parameters.</param> 
        /// <param name="sliderName">Slider name.</param>
        /// <param name="from">Minimum slider value.</param>
        /// <param name="to">Maximum slider value.</param>
        public List<YachtPage> FilteringCheck(List<YachtPage> yachts, string sliderName, int lengthFrom, int yearFrom)
        {
            List<YachtPage> yachtErrors = new List<YachtPage>();

            switch (sliderName)  //Select slider for work.
            {
                case "length":
                    foreach (YachtPage yacht in yachts)
                    {
                        if (yacht.Length < lengthFrom || yacht.Length > 60)  
                        {
                            AddYachtError(yachtErrors, yacht, lengthFrom, "length");
                        }
                    }
                    break;
                case "year":
                    foreach (YachtPage yacht in yachts)
                    {
                        if (yacht.Year < yearFrom || yacht.Year > 2018) 
                        {
                            AddYachtError(yachtErrors, yacht, yearFrom, "year");
                        }
                    }
                    break;
                default:
                    throw new Exception("Incorrect slider name. Expected: length, year.");
            }
            return yachtErrors;
        }

        /// <summary>
        /// Add yach to yachtErrors list.
        /// </summary>
        /// <param name="yachtErrors">List of yacht with errors.</param>
        /// <param name="yacht"></param>
        /// <param name="filterValue"></param>
        /// <param name="filterName"></param>
        private void AddYachtError(List<YachtPage> yachtErrors, YachtPage yacht, int filterValue, string filterName)
        {
            YachtPage errorYacht = yacht.Clone();
            if (filterName == "length")
            {
                errorYacht.Error = $"Slider from: {filterValue} - 60. Yacht lenght:  +{yacht.Length}";
            }
            else
            {
                yacht.Error = $"Slider from: {filterValue} - 2018. Yacht year:{yacht.Year}";
            }
            yachtErrors.Add(errorYacht);
        }      
    }
}

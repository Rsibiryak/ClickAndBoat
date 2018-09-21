using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;


namespace ClickBoat
{
    /// <summary>
    /// Actions which run once for all testsuite.
    /// </summary>
    [SetUpFixture]
    public class TestSuitFixture
    {
 
        //[SetUp]
        public void InitPageManager()
        {
            PageManager Manager = PageManager.GetInstance();
        }

    }
}

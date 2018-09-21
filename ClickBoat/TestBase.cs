using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using NUnit.Framework;


namespace ClickBoat
{
    /// <summary>
    /// Common  for all tests.
    /// </summary>
    public abstract class TestBase
    {
        protected PageManager Manager { get; set; }

        /// <summary>
        /// Actions before test.
        /// </summary>
        [SetUp]
        public void PrepereTest()
        {
            Manager = PageManager.GetInstance();
        }

    }
}

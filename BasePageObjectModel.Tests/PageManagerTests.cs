using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BasePageObjectModel.Tests
{
	[TestClass]
	public class PageManagerTests
	{
		[TestMethod]
		public void TestInitialize()
		{
			var mockDriver = WebDriverFactory.CreateMockWebDriver();
			PageManager.Current = new FooPages("http://localhost:12345");
			PageManager.Current.Initialize(mockDriver.Object);
			var basePages = PageManager.Current.BasePages;
		}
	}
}

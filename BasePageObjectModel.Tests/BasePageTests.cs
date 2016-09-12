using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BasePageObjectModel.Tests
{
	[TestClass]
	public class BasePageTests
	{
		[TestMethod]
		public void CompareUrlTest()
		{
			BasePage bp = new BasePage(null);

			string google = "http://www.google.com/index.html";
			string notGoogle = "http://www.google.com/notIndex.html";

			Assert.IsTrue(bp.CompareUrls(google, google));
			Assert.IsFalse(bp.CompareUrls(google, notGoogle));
		}


		[TestMethod]
		public void CompareUrlIgnoresQueryStringTest()
		{
			BasePage bp = new BasePage(null);

			string google = "http://www.google.com/index.html?Foo=Bar";
			string notGoogle = "http://www.google.com/index.html?Bar=Foo";

			Assert.IsTrue(bp.CompareUrls(google, notGoogle));
		}


		[TestMethod]
		public void CompareUrlTrailingSlashes()
		{
			BasePage bp = new BasePage(null);

			string google = "http://www.google.com/OrderCenter/CustomerOrderViewPD.aspx";
			string notGoogle = "http://www.google.com/OrderCenter/CustomerOrderViewPD.aspx?Type=O";

			Assert.IsTrue(bp.CompareUrls(google, notGoogle));
		}
	}
}

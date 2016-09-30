using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using OpenQA.Selenium;

namespace BasePageObjectModel.Tests
{
	[TestClass]
	public class BasePageTests
	{
		[TestMethod]
		public void TestIsUrlDisplayed()
		{
			var mockWebDriverFoo = CreateMockWebDriver();
			PageManager.Current = new FooPages("http://local.foo.com/");
			mockWebDriverFoo.SetupGet(wd => wd.Url).Returns("http://local.foo.com/foo");


			var mockWebDriverFooId = CreateMockWebDriver();
			mockWebDriverFooId.SetupGet(wd => wd.Url).Returns("http://local.foo.com/foo/42");

			var fooPage = new FooPage(mockWebDriverFoo.Object);
			var fooIdPage = new FooIdPage(mockWebDriverFoo.Object, 42);
			fooPage.GoTo();

			Assert.IsTrue(fooPage.IsUrlDisplayed());
			Assert.IsFalse(fooIdPage.IsUrlDisplayed());

			fooPage = new FooPage(mockWebDriverFooId.Object);
			fooIdPage = new FooIdPage(mockWebDriverFooId.Object, 42);
			fooIdPage.GoTo();

			Assert.IsFalse(fooPage.IsUrlDisplayed());
			Assert.IsTrue(fooIdPage.IsUrlDisplayed());
		}

		private static Mock<IWebDriver> CreateMockWebDriver()
		{
			var mockNavigation = new Mock<INavigation>();
			var mockWebDriver = new Mock<IWebDriver>();
			mockWebDriver.Setup(wd => wd.Navigate()).Returns(mockNavigation.Object);
			return mockWebDriver;
		}
	}

	internal class FooPage : BasePage
	{
		public FooPage(IWebDriver driver) : base(driver)
		{
			SetPageUrl("/foo");
		}
	}

	internal class FooIdPage : BasePage
	{
		public FooIdPage(IWebDriver driver, int id) 
			: base(driver)
		{
			PageUriTemplate = new UriTemplate("/foo/{id}");
			SetPageUrl(PageUriTemplate.BindByPosition(PageManager.Current.BaseUrl, id.ToString()).ToString());
		}
	}

	internal class FooPages : PageManager
	{
		public FooPages(string baseUrl) : base(baseUrl)
		{
		}

		public FooPage Foo => Current.BasePages.OfType<FooPage>().Single();
		public FooIdPage FooId => Current.BasePages.OfType<FooIdPage>().Single();
	}

}

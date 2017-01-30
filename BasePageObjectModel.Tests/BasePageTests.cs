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
			var mockWebDriverFoo = WebDriverFactory.CreateMockWebDriver();
			PageManager.Current = new FooPages("http://local.foo.com/");
			mockWebDriverFoo.SetupGet(wd => wd.Url).Returns("http://local.foo.com/foo");


			var mockWebDriverFooId = WebDriverFactory.CreateMockWebDriver();
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


		[TestMethod]
		public void TestSetPageUrl()
		{
			PageManager.Current = new FooPages("http://local.foo.com/xyz/");
			Assert.AreEqual("http://local.foo.com/xyz/foo", FooPages.Foo.PageUrl);
		}

		[TestMethod]
		public void TestHandleSpecialKeys()
		{
			var mockWebElement = new Mock<IWebElement>();
			BasePage.HandleSpecialKeys("blah~Enter~Tab~Escape", mockWebElement.Object);
			BasePage.HandleSpecialKeys("~Crap", mockWebElement.Object);
		}
	}
}

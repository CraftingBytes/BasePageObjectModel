using BasePageObjectModel.Tests.Helpers;

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

using OpenQA.Selenium;

namespace BasePageObjectModel.Tests
{
	internal class FooPage : BasePage
	{
		public FooPage(IWebDriver driver) : base(driver)
		{
			SetPageUrl("foo");
		}
	}
}
namespace BasePageObjectModel.Tests.Helpers
{
	internal class FooPage : BasePage
	{
		public FooPage(IWebDriver driver) : base(driver)
		{
			SetPageUrl("foo");
		}
	}
}
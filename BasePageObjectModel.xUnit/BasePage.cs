using OpenQA.Selenium;

namespace BasePageObjectModel.xUnit
{
	public class BasePage : BaseBasePage
	{
		static BasePage()
		{
			ServiceRegistry.Assert = new XUnitAssert();
		}

		public BasePage(IWebDriver driver) : base(driver)
		{
		}
	}
}

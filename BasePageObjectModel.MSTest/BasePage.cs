using OpenQA.Selenium;

namespace BasePageObjectModel.MSTest
{
	public class BasePage : BaseBasePage
	{
		static BasePage()
		{
			ServiceRegistry.Assert = new MsTestAssert();
		}

		public BasePage(IWebDriver driver) : base(driver)
		{
		}
	}
}
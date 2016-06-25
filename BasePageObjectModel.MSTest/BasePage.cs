using OpenQA.Selenium;
using BasePageObjectModel.MSTest;

namespace BasePageObjectModel
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
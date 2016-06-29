using OpenQA.Selenium;
using BasePageObjectModel.xUnit;

namespace BasePageObjectModel
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

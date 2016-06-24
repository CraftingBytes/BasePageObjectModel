using BasePageObjectModel.Nunit;
using OpenQA.Selenium;

namespace BasePageObjectModel
{
	public class BasePage : BaseBasePage
	{
		static BasePage()
		{
			ServiceRegistry.Assert = new NunitAssert();
		}

		public BasePage(IWebDriver driver) : base(driver)
		{
		}
	}
}
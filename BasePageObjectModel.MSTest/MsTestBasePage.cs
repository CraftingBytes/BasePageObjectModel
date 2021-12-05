using OpenQA.Selenium;

namespace BasePageObjectModel.MSTest
{
	public class MsTestBasePage : BasePage
	{
		static MsTestBasePage()
		{
			ServiceRegistry.Assert = new MsTestAssert();
		}

		public MsTestBasePage(IWebDriver driver) : base(driver)
		{
		}
	}
}
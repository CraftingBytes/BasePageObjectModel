using OpenQA.Selenium;
using BasePageObjectModel.MSTest;

namespace BasePageObjectModel
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
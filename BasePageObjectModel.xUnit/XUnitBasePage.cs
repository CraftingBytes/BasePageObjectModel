using OpenQA.Selenium;

namespace BasePageObjectModel.xUnit
{
	public class XunitBasePage : BasePage
	{
		static XunitBasePage()
		{
			ServiceRegistry.Assert = new XunitAssert();
		}

		public XunitBasePage(IWebDriver driver) : base(driver)
		{
		}
	}
}

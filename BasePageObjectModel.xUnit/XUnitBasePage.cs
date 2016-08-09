using OpenQA.Selenium;
using BasePageObjectModel.xUnit;

namespace BasePageObjectModel
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

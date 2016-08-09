using BasePageObjectModel.Nunit;
using OpenQA.Selenium;

namespace BasePageObjectModel
{
	public class NunitBasePage : BasePage
	{
		static NunitBasePage()
		{
			ServiceRegistry.Assert = new NunitAssert();
		}

		public NunitBasePage(IWebDriver driver) : base(driver)
		{
		}
	}
}
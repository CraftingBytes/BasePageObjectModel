using OpenQA.Selenium;

namespace BasePageObjectModel.Nunit
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
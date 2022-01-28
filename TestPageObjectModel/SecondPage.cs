using BasePageObjectModel;
using BasePageObjectModel.Nunit;
using OpenQA.Selenium;

namespace TestPageObjectModel
{
	public class SecondPage : NunitBasePage
	{
		public SecondPage(IWebDriver driver) : base(driver)
		{
			SetPageUrl("/second");
		}
	}
}
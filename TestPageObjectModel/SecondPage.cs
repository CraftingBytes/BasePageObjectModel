using BasePageObjectModel;
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
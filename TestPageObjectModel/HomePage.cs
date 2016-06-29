using BasePageObjectModel;
using OpenQA.Selenium;

namespace TestPageObjectModel
{
	public class HomePage :BasePage	
	{
		public HomePage(IWebDriver driver) : base(driver)
		{
			SetPageUrl("/");
		}

		public string GetTitle()
		{
			return WebDriver.FindElement(By.Id("title")).Text;
		}
	}
}
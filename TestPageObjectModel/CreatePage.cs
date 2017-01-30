using BasePageObjectModel;
using OpenQA.Selenium;

namespace TestPageObjectModel
{
	public class CreatePage : BasePage
	{
		public CreatePage(IWebDriver driver) : base(driver)
		{
			SetPageUrl("/home/create");
		}

		public IWebElement CreateButton => GetElement(By.Id("createButton"));

		public void ClickCreate()
		{
			CreateButton.Click();
		}
	}
}

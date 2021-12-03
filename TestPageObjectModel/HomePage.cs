using BasePageObjectModel;
using OpenQA.Selenium;

namespace TestPageObjectModel
{
	public class HomePage : NunitBasePage
	{
		public HomePage(IWebDriver driver) : base(driver)
		{
			SetPageUrl("/");
		}

		private IWebElement Title => GetElement(By.Id("title"));
		private IWebElement SecondPageLink => GetElement(By.Id("secondPageLink"));
		private IWebElement TableSomethings => GetElement(By.Id("tableSomethings"));

		public string GetTitle()
		{
			return Title.Text;
		}

		public void ClickSecondPageLink()
		{
			SecondPageLink.Click();
		}

		public List<string[]> GetTableBody()
		{
			return TableSomethings.ToTableBody();
		}
	}
}
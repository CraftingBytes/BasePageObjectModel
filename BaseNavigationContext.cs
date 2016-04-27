using NUnit.Framework;
using OpenQA.Selenium;

namespace BasePageObjectModel
{
	public class BaseNavigationContext<TFrom>
		where TFrom : BaseElementContainer
	{
		public IWebDriver Driver { get; set; }

		public TFrom FromPage { get; set; }

		protected BaseNavigationContext(IWebDriver driver, TFrom fromPage)
		{
			Driver = driver;
			FromPage = fromPage;
		}


		protected void AssertCorrectPageLoaded<T>(T target)
			where T : BasePage
		{
			var isDisplayed = target.IsUrlDisplayed();
			if (!isDisplayed)
			{
				Assert.Fail("Expected URL {0} but was {1}", target.PageUrl, Driver.Url);
			}
		}

		protected void AssertFailOnErrorPage<T>(T target)
			where T : BasePage
		{
			var bodyText = target.WebDriver.PageSource;
			if (bodyText.Contains("Server Error in "))
			{
				Assert.Fail("Server error while navigating\r\n\r\n {0}.", bodyText);
			}

			if (bodyText.Contains("Internet Information Services") && bodyText.Contains("Microsoft Support"))
			{
				Assert.Fail("IIS error while navigating\r\n\r\n {0}.", bodyText);
			}
		}
	}
}
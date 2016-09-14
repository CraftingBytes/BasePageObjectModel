using System;
using OpenQA.Selenium.Support.UI;

namespace BasePageObjectModel
{
	public static class WaitExtensions
	{
		public static bool ClickAndWaitForLoad<T>(this T page, Action<T> action, TimeSpan? timeout = null) where T : BasePage
		{
			var urlBeforeClick = page.WebDriver.Url;
			action(page);
			return page.WaitForLoad(urlBeforeClick,timeout);
		}

		public static bool WaitForLoad(this BasePage basePage, string urlBeforeClick, TimeSpan? timeout = null)
		{
			if (timeout == null)
			{
				timeout = TimeSpan.FromSeconds(10);
			}
			return new WebDriverWait(basePage.WebDriver, timeout.Value).Until(wd => wd.Url != urlBeforeClick);
		}

		public static bool PerformActionOnElementAndWaitForReload<T>(this T page, Action<T> action, TimeSpan? timeout = null) where T : BaseElementContainer
		{
			if (timeout == null)
			{
				timeout = TimeSpan.FromSeconds(10);
			}

			var oldPageSource = page.WebDriver.PageSource;
			action(page);
			return new WebDriverWait(page.WebDriver, timeout.Value).Until(wd => wd.PageSource != oldPageSource);
		}
	}
}

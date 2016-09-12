using OpenQA.Selenium;
using System;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using OpenQA.Selenium.Support.UI;

namespace BasePageObjectModel
{
	public static class WaitExtensions
	{
		public static bool ClickAndWaitForLoad<T>(this T page, Action<T> action) where T : BasePage
		{
			var urlBeforeClick = page.WebDriver.Url;
			action(page);
			return page.WaitForLoad(urlBeforeClick);
		}

		public static bool WaitForLoad(this BasePage basePage, string urlBeforeClick)
		{
			return new WebDriverWait(basePage.WebDriver, TimeSpan.FromSeconds(2)).Until(wd => wd.Url != urlBeforeClick);
		}

		public static bool PerformActionOnElementAndWaitForReload<T>(this T page, Action<T> action) where T : BaseElementContainer
		{
			var oldPageSource = page.WebDriver.PageSource;
			action(page);
			return new WebDriverWait(page.WebDriver, TimeSpan.FromSeconds(2)).Until(wd => wd.PageSource != oldPageSource);
		}
	}
}

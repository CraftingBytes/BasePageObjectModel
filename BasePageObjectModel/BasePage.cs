using System;
using System.Collections.Generic;
using OpenQA.Selenium;

namespace BasePageObjectModel
{
	public class BasePage : BaseElementContainer
	{
		private readonly Dictionary<string, string> queryString;

		public BasePage(IWebDriver driver)
			: base(driver)
		{
			queryString = new Dictionary<string, string>();
		}

		public string PageUrl { get; private set; }

		protected UriTemplate PageUriTemplate { get; set; }

		public void SetPageUrl(string value)
		{
			var uri = new Uri(PageManager.Current.BaseUrl, value);
			if (queryString.Count > 0)
			{
				foreach (var kvp in queryString)
				{
					uri = uri.AddQuery(kvp.Key, kvp.Value);
				}
			}

			PageUrl = uri.ToString();
		}

		public void SetQueryStringValue(string key, string value)
		{
			queryString[key] = value;
			if (PageUrl != null)
			{
				SetPageUrl(new Uri(PageUrl).AbsolutePath);
			}
		}

		public bool GoToUrl(string url)
		{
			if (WebDriver.Url == url)
			{
				return true;
			}

			WebDriver.Navigate().GoToUrl(url);
			return IsUrlDisplayed();
		}

		public void GoTo()
		{
			GoToUrl(PageUrl);
		}

		public void GoTo(string key, string value)
		{
			SetQueryStringValue(key, value);
			GoTo();
		}
		public void GoTo(bool reset)
		{
			GoTo("test", "reset");
		}

		public virtual bool IsUrlDisplayed()
		{
			if (string.IsNullOrEmpty(PageUrl))
			{
				return false;
			}

			if (PageUriTemplate == null)
			{
				return Uri.Compare(new Uri(PageUrl), new Uri(WebDriver.Url),
					UriComponents.Path, UriFormat.Unescaped,
					StringComparison.InvariantCultureIgnoreCase) == 0;
			}
			var uriActual = new Uri(WebDriver.Url);
			var actualUrlLeftPart = uriActual.GetLeftPart(UriPartial.Path);
			var match = PageUriTemplate.Match(PageManager.Current.BaseUrl, new Uri(actualUrlLeftPart));
			return match != null;
		}

		public void ScrollToBottomOfScreen()
		{
			var jse = (IJavaScriptExecutor)WebDriver;
			jse.ExecuteScript("window.scrollTo(0, document.body.scrollHeight)");
		}

		public void Refresh()
		{
			WebDriver.Navigate().Refresh();
		}
	}
}
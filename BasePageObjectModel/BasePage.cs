using System;
using System.Collections.Generic;
using OpenQA.Selenium;

namespace BasePageObjectModel
{
	public class BasePage : BaseElementContainer
	{
		private Dictionary<string, string> queryString;

		public BasePage(IWebDriver driver)
			: base(driver)
		{
			queryString = new Dictionary<string, string>();
		}

		public string PageUrl { get; private set; }

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
            return WaitExtensions.WaitFor(() => CompareUrls(WebDriver.Url, url));
        }

		private bool CompareUrls(string left, string right)
		{
			var leftUrl = new Uri(left);
			var rightUrl = new Uri(right);

			return Uri.Compare(leftUrl, rightUrl,
				UriComponents.Path, UriFormat.Unescaped,
				StringComparison.InvariantCultureIgnoreCase) == 0;
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
			return CompareUrls(WebDriver.Url, PageUrl);
		}

		public void ScrollToBottomOfScreen()
		{
			IJavaScriptExecutor jse = ((IJavaScriptExecutor)WebDriver);
			jse.ExecuteScript("window.scrollTo(0, document.body.scrollHeight)");
		}

		public void Refresh()
		{
			WebDriver.Navigate().Refresh();
		}
	}
}
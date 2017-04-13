using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace BasePageObjectModel
{
	public class BasePage : BaseElementContainer
	{
		public static char SpecialKeyPrefix { get; set; } = '~';

		public static string[] SpecialKeys { get; set; } = new[] { nameof(Keys.Enter), nameof(Keys.Escape), nameof(Keys.Tab) };
		public Dictionary<string, string> QueryStrings { get; }

		public BasePage(IWebDriver driver)
			: base(driver)
		{
			QueryStrings = new Dictionary<string, string>();
		}

		public string PageUrl { get; private set; }

		protected UriTemplate PageUriTemplate { get; set; }

		public void SetPageUrl(string value)
		{
			var uri = new Uri(PageManager.Current.BaseUrl, value);
			if (QueryStrings.Count > 0)
			{
				foreach (var kvp in QueryStrings)
				{
					uri = uri.AddQuery(kvp.Key, kvp.Value);
				}
			}

			PageUrl = uri.ToString();
		}

		public void ClearQueryStrings()
		{
			QueryStrings.Clear();
			if (PageUrl != null)
			{
				SetPageUrl(new Uri(PageUrl).AbsolutePath);
			}
		}

		public void SetQueryStringValue(string key, string value)
		{
			QueryStrings[key] = value;
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

		public void ClickLabel(string labelText)
		{
			var label = GetLabelByText(labelText);
			label.Click();
		}

		public virtual IWebElement GetLabelByText(string labelText)
		{
			return FindLabelByTextMatch(labelText);
		}

		public IWebElement FindLabelByTextMatch(string labelText)
		{
			var label = WebDriver.FindElements(By.TagName("label"))
				.FirstOrDefault(e => labelText == e.Text);
			return label;
		}

		public IWebElement FindLabelByXPathContains(string labelText)
		{
			var xpathToFind = $"//label[contains(., '{labelText}')]";
			var label = WebDriver.FindElement(By.XPath(xpathToFind));
			return label;
		}

		public IWebElement GetLabelByTextMatch(string labelText)
		{
			var label = GetElements(By.TagName("label"))
				.FirstOrDefault(e => labelText == e.Text);
			return label;
		}

		public IWebElement GetLabelByXPathContains(string labelText)
		{
			var xpathToFind = $"//label[contains(., '{labelText}')]";
			var label = GetElement(By.XPath(xpathToFind));
			return label;
		}

		public virtual IWebElement GetLabelTarget(string labelText)
		{
			return GetLabelTargetByFor(labelText);
		}

		public IWebElement GetLabelTargetByFor(string labelText)
		{
			var label = GetLabelByText(labelText);
			if (label == null)
			{
				throw new Exception($"Couldn't find label with text '{labelText}'");
			}
			var forText = label.GetAttribute("for");
			var targetElement = GetElement(By.Id(forText));
			return targetElement;
		}

		public IWebElement FindLabelTargetByFor(string labelText)
		{
			var label = GetLabelByText(labelText);
			var forText = label.GetAttribute("for");
			var targetElement = WebDriver.FindElement(By.Id(forText));
			return targetElement;
		}

		public IWebElement FindLabelTargetForChosen(string labelText)
		{
			var targetElement = FindLabelTargetByFor(labelText);
			if (!targetElement.Displayed)
			{
				//HACK for chosen
				ClickLabel(labelText);
				Thread.Sleep(100);
				targetElement = WebDriver.SwitchTo().ActiveElement();
			}
			return targetElement;
		}

		public void FillOutForm(Dictionary<string, string> labelToValue)
		{
			foreach (var kvp in labelToValue)
			{
				var targetElement = GetLabelTarget(kvp.Key);
				if (targetElement == null)
				{
					throw new Exception($"Couldn't find target element for label {kvp.Key}");
				}
				var replaced = StripKeysFromText(kvp.Value);
				targetElement.FillElement(replaced);
				HandleSpecialKeys(kvp.Value, targetElement);
			}
		}

		internal static void HandleSpecialKeys(string value, IWebElement current)
		{
			if (value.Contains(SpecialKeyPrefix))
			{
				foreach (var specialKey in SpecialKeys)
				{
					if (value.Contains(SpecialKeyPrefix + specialKey))
					{
						var keysField = typeof(Keys).GetField(specialKey);
						var seleniumKey = (string)keysField.GetValue(null);
						current.SendKeys(seleniumKey);
					}
				}
			}
		}

		private static string StripKeysFromText(string value)
		{
			string replaced = value;
			if (value.Contains(SpecialKeyPrefix))
			{
				foreach (var specialKey in SpecialKeys)
				{
					replaced = replaced.Replace(SpecialKeyPrefix + specialKey, "");
				}
			}
			return replaced;
		}

		public void FillOutFormByNames(Dictionary<string, string> namesAndValues)
		{
			foreach (var nvp in namesAndValues)
			{
				var element = GetElement(By.Name(nvp.Key));
				element.FillElement(nvp.Value);
			}
		}

		public void FillOutFormByPartialIds(Dictionary<string, string> idsAndValues)
		{
			foreach (KeyValuePair<string, string> idAndValue in idsAndValues)
			{
				var element = GetElement(this.ByPartialId(idAndValue.Key));
				element.FillElement(idAndValue.Value);
			}
		}

		public void VerifyForm(IDictionary<string, string> labelToValue)
		{
			foreach (var kvp in labelToValue)
			{
				var targetElement = FindLabelTargetByFor(kvp.Key);
				var expectedValue = StripKeysFromText(kvp.Value);
				if (targetElement.TagName == "select")
				{
					var select = new SelectElement(targetElement);
					string selectedText;
					if (select.IsMultiple)
					{
						selectedText = string.Join(",", @select.AllSelectedOptions.Select(so => so.GetAttribute("value")));
					}
					else
					{
						selectedText = select.SelectedOption.Text;
					}
					ServiceRegistry.Assert.AreEqual(expectedValue, selectedText);
				}
				if (targetElement.TagName == "input" || targetElement.TagName == "textarea")
				{
					var inputType = targetElement.GetAttribute("type");
					if (inputType == "date")
					{
						var textValue = targetElement.Text;
						ServiceRegistry.Assert.AreEqual(expectedValue, textValue);
					}
					var actualValue = targetElement.GetAttribute("value");
					ServiceRegistry.Assert.AreEqual(expectedValue, actualValue);
				}
			}
		}


	}
}
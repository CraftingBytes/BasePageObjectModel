﻿using System.Collections.Generic;
using System.Linq;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace BasePageObjectModel
{
	public static class BasePageExtensions
	{
		public static void Is<T>(this BaseBasePage page)
			where T : BaseBasePage
		{
			T castedPage = page.As<T>();
			ServiceRegistry.Assert.IsNotNull(castedPage);
		}

		public static T As<T>(this BaseBasePage page)
			where T : BaseBasePage
		{
			return page as T;
		}

		public static void ClickLabel(this BaseBasePage page, string labelText)
		{
			var label = FindLabel(page, labelText);

			label.Click();
		}

		public static IWebElement FindLabel(this BaseBasePage page, string labelText)
		{
			var xpathToFind = string.Format("//label[contains(., '{0}')]", labelText);
			var label = page.WebDriver.FindElement(By.XPath(xpathToFind));
			return label;
		}

		public static void FillOutForm(this BaseBasePage page, Dictionary<string, string> labelToValue)
		{
			foreach (var kvp in labelToValue)
			{
				var targetElement = GetTargetElementForLabel(page, kvp.Key);
				if (!targetElement.Displayed)
				{
					//HACK for chosen
					page.ClickLabel(kvp.Key);
					Thread.Sleep(100);
					targetElement = page.WebDriver.SwitchTo().ActiveElement();
				}
				var replaced = StripKeysFromText(kvp.Value);
				FillElement(targetElement, replaced);
				HandleSpecialKeys(kvp.Value, targetElement);
			}
		}

		private static void HandleSpecialKeys(string value, IWebElement current)
		{
			if (value.Contains("~ENTER"))
			{
				current.SendKeys(Keys.Enter);
			}
			if (value.Contains("~ESC"))
			{
				current.SendKeys(Keys.Escape);
			}
		}

		private static string StripKeysFromText(string value)
		{
			string replaced = value;
			replaced = replaced.Replace("~ESC", "");
			replaced = replaced.Replace("~ENTER", "");
			//replaced = replaced.Replace("~PAUSE", "");
			return replaced;
		}

		public static void FillOutFormByNames(this BaseBasePage page, Dictionary<string, string> namesAndValues)
		{
			foreach (var nvp in namesAndValues)
			{
				var element = page.WebDriver.FindElement(By.Name(nvp.Key));
				FillElement(element, nvp.Value);
			}
		}

		public static void FillOutFormByPartialNames(this BaseBasePage page, Dictionary<string, string> namesAndValues)
		{
			foreach (KeyValuePair<string, string> namesAndValue in namesAndValues)
			{
				var element = page.WebDriver.FindElement(page.ByPartialName(namesAndValue.Key));
				FillElement(element, namesAndValue.Value);
			}
		}

		public static void FillElement(IWebElement webElement, string value)
		{
			var type = webElement.GetAttribute("type").ToLower();

			if ((webElement.TagName == "input" && (type == "text" || type == "tel" || type == "email" || type == "password" || type == "search")
				|| webElement.TagName == "textarea"))
			{
                webElement.Clear();
				webElement.SendKeys(value);
			}
			else if (webElement.TagName == "select")
			{
				var select = new SelectElement(webElement);
				select.SelectByText(value);
			}
			else if (webElement.TagName == "input" && (type == "checkbox" || type == "radio"))
			{
				// TODO: Is there anyway to manage selection here? Or is that out of scope?
				webElement.Click();
			}
		}

		public static void VerifyForm(this BaseBasePage page, IDictionary<string, string> labelToValue)
		{
			foreach (var kvp in labelToValue)
			{
				var targetElement = GetTargetElementForLabel(page, kvp.Key);
				var expectedValue = StripKeysFromText(kvp.Value);
				if (targetElement.TagName == "input" || targetElement.TagName == "textarea")
				{
					var actualValue = targetElement.GetAttribute("value");
					ServiceRegistry.Assert.AreEqual(expectedValue, actualValue);
				}
				else if (targetElement.TagName == "select")
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
			}
		}

		private static IWebElement GetTargetElementForLabel(BaseBasePage page, string labelText)
		{
			var label = page.FindLabel(labelText);
			var targetElement = page.WebDriver.FindElement(By.Id(label.GetAttribute("for")));
			return targetElement;
		}

		public static By ByPartialName(this BaseBasePage page, string name)
		{
			return By.XPath($"//*[contains(@id,'{name}')]");
		}
	}
}
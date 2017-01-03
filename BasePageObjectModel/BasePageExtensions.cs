using System.Collections.Generic;
using System.Linq;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace BasePageObjectModel
{
	public static class BasePageExtensions
	{
		public static void Is<T>(this BasePage page)
			where T : BasePage
		{
			T castedPage = page.As<T>();
			ServiceRegistry.Assert.IsNotNull(castedPage);
		}

		public static T As<T>(this BasePage page)
			where T : BasePage
		{
			return page as T;
		}

		public static void ClickLabel(this BasePage page, string labelText)
		{
			var label = FindLabel(page, labelText);

			label.Click();
		}

		public static IWebElement FindLabel(this BasePage page, string labelText)
		{
			var label = page.WebDriver.FindElements(By.TagName("label"))
				.FirstOrDefault(e => labelText == e.Text);
			return label;
		}

		public static void FillOutForm(this BasePage page, Dictionary<string, string> labelToValue)
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
				targetElement.FillElement(replaced);
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
			return replaced;
		}

		public static void FillOutFormByNames(this BasePage page, Dictionary<string, string> namesAndValues)
		{
			foreach (var nvp in namesAndValues)
			{
				var element = page.GetElement(By.Name(nvp.Key));
				element.FillElement(nvp.Value);
			}
		}

		public static void FillOutFormByPartialIds(this BasePage page, Dictionary<string, string> idsAndValues)
		{
			foreach (KeyValuePair<string, string> idAndValue in idsAndValues)
			{
				var element = page.GetElement(page.ByPartialId(idAndValue.Key));
				element.FillElement(idAndValue.Value);
			}
		}

		public static void VerifyForm(this BasePage page, IDictionary<string, string> labelToValue)
		{
			foreach (var kvp in labelToValue)
			{
				var targetElement = GetTargetElementForLabel(page, kvp.Key);
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

		private static IWebElement GetTargetElementForLabel(BasePage page, string labelText)
		{
			var label = page.FindLabel(labelText);
			var targetElement = page.GetElement(By.Id(label.GetAttribute("for")));
			return targetElement;
		}

		public static By ByPartialId(this BasePage page, string id)
		{
			return By.XPath($"//*[contains(@id,'{id}')]");
		}

		public static By ByPartialName(this BasePage page, string name)
		{
			return By.XPath($"//*[contains(@name,'{name}')]");
		}
	}
}

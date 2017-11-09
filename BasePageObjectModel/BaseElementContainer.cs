using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace BasePageObjectModel
{
	/// <summary>
	/// The BaseElementContainer is either a page or a window that contains elements
	/// </summary>
	public abstract class BaseElementContainer
	{
		public static TimeSpan DefaultWaitTime = TimeSpan.FromSeconds(1);

		protected BaseElementContainer(IWebDriver webDriver)
		{
			WebDriver = webDriver;
		}

		public IWebDriver WebDriver { get; set; }

		public string Snapshot()
		{
			Screenshot ss = ((ITakesScreenshot)WebDriver).GetScreenshot();

			//Use it as you want now
			var fileName = GetType() + ".png";
			ss.SaveAsFile(fileName, ScreenshotImageFormat.Png);
			return Path.GetFullPath(fileName);
		}

		protected void SelectCheckbox(By checkboxLocator, bool select = true)
		{
			var checkbox = WebDriver.FindElement(checkboxLocator);
			if (checkbox.Selected != select)
			{
				checkbox.Click();
			}
		}

		protected void CheckHiddenControl(string labelText, bool select = true)
		{
			var xpathForClick = string.Format("//label[contains(.,'{0}')]", labelText);
			var xpathForInput = xpathForClick + "/input";
			var isSelected = IsHiddenControlChecked(By.XPath(xpathForInput));

			if (isSelected != select)
			{
				var element = WebDriver.FindElement(By.XPath(xpathForClick));
				element.Click();
			}
		}

		protected void CheckHiddenControl(By elementLocator, bool select = true)
		{
			CheckHiddenControl(WebDriver.FindElement(elementLocator), select);
		}

		protected void CheckHiddenControl(IWebElement webElement, bool select = true)
		{
			var isSelected = IsHiddenControlChecked(webElement);
			if (isSelected != select)
			{
				var parent = webElement.GetParent();
				parent.Click();
			}
		}

		private object ExecuteScript(IWebElement element, string js)
		{
			var executor = (IJavaScriptExecutor)WebDriver;
			return executor.ExecuteScript(js, element);
		}

		protected bool IsHiddenControlChecked(By elementLocator)
		{
			return IsHiddenControlChecked(WebDriver.FindElement(elementLocator));
		}

		protected bool IsHiddenControlChecked(IWebElement element)
		{
			var js = "return arguments[0].checked;";
			return (bool)ExecuteScript(element, js);
		}

		protected bool IsElementVisible(By elementLocator)
		{
			var elements = WebDriver.FindElements(elementLocator).ToList();
			return elements.Any() && elements.First().Displayed;
		}

		protected string GetTextboxValue(By elementLocator)
		{
			return WebDriver.FindElement(elementLocator).GetAttribute("value");
		}

		protected ReadOnlyCollection<IWebElement> GetCellsForTableRowById(By tableElementIdentifier, long id)
		{
			// Assumes securenet id is in first column
			var table = WebDriver.FindElement(tableElementIdentifier);
			var rows = table.FindElements(By.TagName("tr"));
			ReadOnlyCollection<IWebElement> cells = null;

			foreach (var row in rows)
			{
				var tempCells = row.FindElements(By.TagName("td"));
				var first = tempCells.FirstOrDefault();
				if (first != null && first.Text == id.ToString())
				{
					cells = tempCells;
				}
			}

			return cells;
		}

		protected void ClickATagInTableRow(string tableId, long rowId, string tagId, string linkText = null)
		{
			var tableCells = GetCellsForTableRowById(By.Id(tableId), rowId);
			IWebElement link = null;

			foreach (var element in tableCells)
			{
				var links = element.FindElements(By.TagName("a"));
				if (links.Count > 0)
				{
					link = links.FirstOrDefault(l => l.GetAttribute("id") == tagId);
					if (link != null)
					{
						if (linkText != null)
						{
							if (link.Text == linkText)
							{
								break;
							}
						}
						else
						{
							break;
						}
					}
				}
			}
			link.Click();
		}

		protected ICollection<string> GetDropDownOptions(IWebElement element)
		{
			var selector = new SelectElement(element);
			return selector.Options.Select(o => o.Text).ToList();
		}

		public IWebElement GetElement(By by, TimeSpan? waitTime = null, Func<By, Func<IWebDriver, IWebElement>> expectedCondition = null)
		{
			return GetElementInternal(by, waitTime, expectedCondition);
		}

		public IWebElement TryGetElement(By by, TimeSpan? waitTime = null, Func<By, Func<IWebDriver, IWebElement>> expectedCondition = null)
		{
			try
			{
				return GetElementInternal(by, waitTime, expectedCondition);
			}
			catch (WebDriverTimeoutException)
			{
				return null;
			}
		}

		private IWebElement GetElementInternal(By by, TimeSpan? waitTime = null, Func<By, Func<IWebDriver, IWebElement>> expectedCondition = null)
		{
			return new WebDriverWait(WebDriver, waitTime ?? DefaultWaitTime).Until(expectedCondition != null ? expectedCondition(by) : ExpectedConditions.ElementIsVisible(by));
		}

		public ReadOnlyCollection<IWebElement> GetElements(By by, TimeSpan? waitTime = null)
		{
			return new WebDriverWait(WebDriver, waitTime ?? DefaultWaitTime).Until(ExpectedConditions.PresenceOfAllElementsLocatedBy(by));
		}
	}
}
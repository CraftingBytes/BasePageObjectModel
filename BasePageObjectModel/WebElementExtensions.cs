﻿namespace BasePageObjectModel
{
	public static class WebElementExtensions
	{
		public static IWebElement GetParent(this IWebElement element)
		{
			return element.FindElement(By.XPath("parent::*"));
		}

		public static IWebElement FindParentByClassName(this IWebElement element, string className)
		{
			if (element == null || element.TagName == "html")
			{
				return null;
			}

			var classValue = element.GetAttribute("class");
			if (classValue.Contains(className))
			{
				return element;
			}

			return FindParentByClassName(element.GetParent(), className);
		}

		public static List<string[]> ToTable(this IWebElement element)
		{
			var rows = new List<string[]>();
			foreach (var tr in element.FindElements(By.TagName("tr")))
			{
				var ths = tr.FindElements(By.TagName("th"));
				var tds = tr.FindElements(By.TagName("td"));
				var thOrTds = ths.Union(tds);
				rows.Add(thOrTds.Select(c => c.Text).ToArray());
			}

			return rows;
		}

		public static IEnumerable<string> ToStringList(this List<string[]> list)
		{
			return list.Select(row => string.Join(",", row));
		}

		public static string ToTableFormattedString(this List<string[]> list)
		{
			int[] maxColumnLengths = new int[list[0].Length];
			for (int col = 0; col < maxColumnLengths.Length; col++)
			{
				maxColumnLengths[col] = list.Select(r => r.Length > col ? r[col].Length : 0).Max();
			}

			var builder = new StringBuilder();
			foreach (var row in list)
			{
				for (int col = 0; col < maxColumnLengths.Length; col++)
				{
					if (col > 0)
					{
						builder.Append("|");
					}
					string format = "{0,-" + maxColumnLengths[col] + "}";
					builder.Append(string.Format(format, row.Length > col ? row[col] : ""));
				}
				builder.AppendLine();
			}
			return builder.ToString();
		}

		public static List<string[]> ToTableBody(this IWebElement element)
		{
			return element.ToTable().Skip(1).ToList();
		}

		public static IWebElement GetElement(this IWebElement element, By by, TimeSpan? waitTime = null)
		{
			var defaultWait = new DefaultWait<IWebElement>(element)
			{
				Timeout = waitTime ?? TimeSpan.FromSeconds(5)
			};

			return defaultWait.Until(e =>
			{
				var firstOrDefault = e.FindElements(by).FirstOrDefault();
				return firstOrDefault?.Displayed == true ? firstOrDefault : null;
			});
		}

		public static ReadOnlyCollection<IWebElement> GetElements(this IWebElement element, By by, TimeSpan? waitTime = null)
		{
			var defaultWait = new DefaultWait<IWebElement>(element)
			{
				Timeout = waitTime ?? TimeSpan.FromSeconds(5)
			};
			return defaultWait.Until(e =>
			{
				var elements = e.FindElements(by);
				return elements?.Any() == true ? elements : null;
			});
		}

		public static void FillElement(this IWebElement webElement, string value, TimeSpan? waitTime = null)
		{
			var type = webElement.GetAttribute("type").ToLower();

			if (webElement.TagName == "select")
			{
				var select = new SelectElement(webElement);
				select.SelectByText(value);
			}
			else if (webElement.TagName == "input" && (type == "checkbox" || type == "radio"))
			{
				// TODO: Is there anyway to manage selection here? Or is that out of scope?
				webElement.Click();
			}
			else
			{
				if (type == "date")
				{
					value = value.Replace("/", "");
				}
				else
				{
					webElement.Clear();

					var list = webElement.GetAttribute("list");
					if (list != null)
					{
						string[] parts = value.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()).ToArray();
						var textToType = parts[0].Trim();
						var textToSelect = parts[1].Trim();

						var parent = webElement.GetParent();
						webElement.SendKeys(textToType);
						var timeToWait = waitTime ?? TimeSpan.FromSeconds(5);
						var start = DateTime.Now;

						var listElement = parent.GetElement(By.Id(list));
						if (listElement.TagName == "ul")
						{
							ReadOnlyCollection<IWebElement> anchors;
							do
							{
								Thread.Sleep(500);
								listElement = parent.FindElement(By.Id(list));
								anchors = listElement.FindElements(By.CssSelector("li a"));
							} while (anchors.Count < 1 && DateTime.Now - start < timeToWait);

							var anchor = anchors.FirstOrDefault(a => a.Text == parts[1]);
							if (anchor == null)
							{
								throw new Exception($"Couldn't find list text {parts[1]}");
							}
							anchor.Click();
						}
						else
						{
							// Datalist method (doesn't work)
							//var options = listElement.FindElements(By.CssSelector("option"));
							//var option = options.FirstOrDefault(o => o.GetAttribute("value").Trim() == textToSelect);
							//if (option == null)
							//{
							//	throw new Exception($"Couldn't find list text {parts[1]}");
							//}
							//option.Click();

							// So just type instead
							webElement.Clear();
							webElement.SendKeys(textToSelect);
						}

						return;
					}
				}
				webElement.SendKeys(value);
			}
		}

		public static bool DoesTextMatch(this IWebElement e, string labelText)
		{
			try
			{
				return labelText == e.Text;
			}
			catch (Exception)
			{
				return false;
			}
		}

		public static bool IsDisplayed(this IWebElement e)
		{
			try
			{
				return e.Displayed;
			}
			catch (Exception)
			{
				return false;
			}
		}

		public static bool IsNullOrEmpty(this IWebElement l)
		{
			try
			{
				return string.IsNullOrEmpty(l.Text);
			}
			catch (Exception)
			{
				return true;
			}
		}

		public static bool CheckForAttribute(this IWebElement l)
		{
			try
			{
				return l.GetAttribute("for") != null;
			}
			catch (Exception)
			{
				return true;
			}
		}
	}
}

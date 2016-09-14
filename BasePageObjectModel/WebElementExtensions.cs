using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace BasePageObjectModel
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
				var thOrTds = tr.FindElements(By.TagName("th")).Union(tr.FindElements(By.TagName("td")));
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
				maxColumnLengths[col] = list.Select(r => r[col].Length).Max();
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
					builder.Append(string.Format(format, row[col]));
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
				Timeout = waitTime ?? TimeSpan.FromSeconds(1)
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
				Timeout = waitTime ?? TimeSpan.FromSeconds(1)
			};
			return defaultWait.Until(e =>
			{
				var elements = e.FindElements(by);
				return elements?.Any() == true ? elements : null;
			});
		}
	}
}

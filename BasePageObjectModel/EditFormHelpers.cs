using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using OpenQA.Selenium;

namespace BasePageObjectModel
{
	public class EditFormHelper
	{
		public static string GetNewNumberText(IWebElement webElement)
		{
			var originalText = webElement.GetAttribute("value");
			var minText = webElement.GetAttribute("min");
			var maxText = webElement.GetAttribute("max");
			return GetNewNumberTextInternal(originalText, minText, maxText);
		}

		internal static string GetNewNumberTextInternal(string originalText, string minText, string maxText)
		{
			long num = 0;
			if (!string.IsNullOrEmpty(originalText))
			{
				num = Convert.ToInt64(originalText);
			}
			num = num + 1;
			if (!string.IsNullOrEmpty(maxText))
			{
				var max = Convert.ToInt64(maxText);
				if (num > max)
				{
					num = 1;
					if (!string.IsNullOrEmpty(minText))
					{
						var min = Convert.ToInt64(minText);
						if (num < min)
						{
							num = min;
						}
					}
				}
			}
			var newText = num.ToString();
			return newText;
		}

		public static string GetNewDateText(IWebElement webElement)
		{
			var originalText = webElement.GetAttribute("value");
			var placeholder = webElement.GetAttribute("placeholder");
			return GetNewDateTextInternal(originalText, placeholder);
		}

		internal static string GetNewDateTextInternal(string originalText, string placeholder)
		{
			if (string.IsNullOrEmpty(originalText))
			{
				return originalText;
			}

			var dateSplitChar = '/';
			var textParts = originalText.Split(dateSplitChar);
			if (textParts.Length < 2)
			{
				dateSplitChar = '-';
				textParts = originalText.Split(dateSplitChar);
			}
			string newText = null;
			int monthIndex;
			if (!string.IsNullOrEmpty(placeholder))
			{
				DateTime dateTime;
				if (DateTime.TryParseExact(originalText, placeholder, CultureInfo.CurrentCulture, DateTimeStyles.None, out dateTime))
				{
					return dateTime.AddMonths(1).ToString(placeholder);
				}
				var placeholderParts = placeholder.ToLower().Split(dateSplitChar);
				monthIndex = Array.IndexOf(placeholderParts, "mm");
			}
			else
			{
				DateTime dateTime;
				if (DateTime.TryParse(originalText, out dateTime))
				{
					return dateTime.AddMonths(1).ToShortDateString();
				}
				var monthParts = textParts.Where(p => p.Length <= 2 && Convert.ToInt32(p) <= 12).ToArray();
				if (monthParts.Length > 1)
				{
					monthParts = monthParts.Where(p => Convert.ToInt32(p) < 12).ToArray();
				}
				monthIndex = Array.IndexOf(textParts, monthParts[0]);
			}
			textParts[monthIndex] = GetNewMonthText(textParts[monthIndex]);
			newText = string.Join(dateSplitChar.ToString(), textParts);
			return newText;
		}

		private static string GetNewMonthText(string monthText)
		{
			var monthPart = Convert.ToInt32(monthText);
			var newMonthText = (monthPart%12 + 1).ToString("00");
			return newMonthText;
		}

		public static bool CompareDates(string date1, string date2)
		{
			var textParts1 = SplitDate(date1);
			var textParts2 = SplitDate(date2);
			var leftSet = textParts1.Except(textParts2);
			var rightSet = textParts2.Except(textParts1);
			var difference = leftSet.Union(rightSet);
			return !difference.Any();
		}

		private static string[] SplitDate(string date1)
		{
			var dateSplitChar = '/';
			var textParts = date1.Split(dateSplitChar);
			if (textParts.Length < 2)
			{
				dateSplitChar = '-';
				textParts = date1.Split(dateSplitChar);
			}
			return textParts;
		}

	}
}

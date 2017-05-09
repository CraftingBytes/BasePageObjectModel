using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading;
using BasePageObjectModel;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace BasePageObjectModel
{
	class LabelAndDone
	{
		public IWebElement Label { get; set; }
		public bool IsDone { get; set; }
	}


	public static class EditFormHelper
	{
		private static string lastRadioName = null;
		private static bool nextRadioNeedsChecking = false;
		private static IWebElement firstRadio;
		private static Dictionary<string, LabelAndDone> _dictionaryOfCurrentLabels;

		public static Dictionary<string, string> EditForm(this BasePage page)
		{
			var labelAndDones = new List<LabelAndDone>();
			var dictionaryLabelsAndValues = new Dictionary<string, string>();
			labelAndDones = page.CheckForNewLabels(labelAndDones);
			while (!IsEverythingDone(labelAndDones))
			{
				labelAndDones = page.CheckForNewLabels(labelAndDones);
				var label = GetNextUndoneLabel(labelAndDones);

				var labelText = label.Text;
				var webElement = page.SwitchToTargetElementForLabel(labelText);
				if (webElement == null || !webElement.Enabled || !string.IsNullOrEmpty(webElement.GetAttribute("readonly")))
				{
					MarkElementComplete(labelAndDones, dictionaryLabelsAndValues, labelText, "");
					continue;
				}

				var tagName = webElement.TagName;
				var inputType = webElement.GetAttribute("type").ToLower();

				if ((tagName == "select") || (tagName == "datalist"))
				{
					var selectElement = new SelectElement(webElement);
					string optionText;
					do
					{
						ChangeSelectElementValue(selectElement);
						var selectedOption = selectElement.SelectedOption;
						optionText = selectedOption.Text;
					} while (string.IsNullOrEmpty(optionText));

					MarkElementComplete(labelAndDones, dictionaryLabelsAndValues, labelText, optionText);
				}
				else if (tagName == "input" && (inputType == "checkbox"))
				{
					webElement.Click();
					var value = webElement.GetAttribute("checked");
					if (value == "true")
					{
						value = "checked";
					}
					MarkElementComplete(labelAndDones, dictionaryLabelsAndValues, labelText, value);
				}
				else if (tagName == "input" && (inputType == "radio"))
				{
					var valueToStore = "";
					var currentRadioName = webElement.GetAttribute("name");
					if (currentRadioName != lastRadioName)
					{
						lastRadioName = currentRadioName;
						firstRadio = webElement;
						nextRadioNeedsChecking = false;
					}
					if (nextRadioNeedsChecking)
					{
						webElement.Click();
						valueToStore = "checked";
					}
					var value = webElement.GetAttribute("checked");
					if (nextRadioNeedsChecking)
					{
						nextRadioNeedsChecking = false;
					}
					else if (!string.IsNullOrEmpty(value))
					{
						// if this one is checked then the next one needs to be checked
						nextRadioNeedsChecking = true;
					}
					//TODO: what if there is no next one, need to remember the first one
					MarkElementComplete(labelAndDones, dictionaryLabelsAndValues, labelText, valueToStore);
				}
				else // textarea plus input type=text, number, phone, date, email, etc.  All assumed to be typeable
				{
					string newText = null;
					var originalText = webElement.GetAttribute("value");
					if ((inputType == "number") || IsInputTextNumeric(originalText))
					{
						newText = EditFormHelper.GetNewNumberText(webElement);
					}
					else if (inputType == "date")
					{
						var dateText = EditFormHelper.GetNewDateText(webElement);

						newText = dateText.Replace("/", "");
						newText = newText.Replace("-", "");
						webElement.SendKeys(newText);
						MarkElementComplete(labelAndDones, dictionaryLabelsAndValues, labelText, dateText);
						continue;
					}
					else
					{
						var placeholder = webElement.GetAttribute("placeholder");
						if (!string.IsNullOrEmpty(placeholder) && placeholder.Contains("/"))
						{
							newText = EditFormHelper.GetNewDateText(webElement);
						}
						else
						{
							newText = originalText + "X";
						}
					}

					if (inputType != "date")
					{
						webElement.Clear();
					}
					webElement.SendKeys(newText);

					var list = webElement.GetAttribute("list");
					if (!string.IsNullOrEmpty(list))
					{
						webElement = page.GetElement(By.Id(list));
						var anchors = webElement.FindElements(By.CssSelector("li a"));
						var anchor = anchors.First();
						newText = anchor.Text;
						anchor.Click();
					}

					MarkElementComplete(labelAndDones, dictionaryLabelsAndValues, labelText, newText);
				}
			}
			return dictionaryLabelsAndValues;
		}

		private static bool IsInputTextNumeric(string originalText)
		{
			return !string.IsNullOrEmpty(originalText) && originalText.All(char.IsDigit);
		}

		private static void MarkElementComplete(List<LabelAndDone> labelAndDones, Dictionary<string, string> dictionaryLabelsAndValues, string labelText, string value)
		{
			dictionaryLabelsAndValues[labelText] = value;
			var labelAndDone = labelAndDones.FirstOrDefault(lad => lad.Label.DoesTextMatch(labelText));
			if (labelAndDone != null)
			{
				labelAndDone.IsDone = true;
			}
		}

		private static IWebElement GetNextUndoneLabel(List<LabelAndDone> labelAndDones)
		{
			return labelAndDones.First(lad => !lad.IsDone).Label;
		}

		private static List<LabelAndDone> CheckForNewLabels(this BasePage page, List<LabelAndDone> labelAndDones)
		{
			Stopwatch sw = Stopwatch.StartNew();
			var labels = page.WebDriver.FindElements(By.TagName("label"))
				.Where(l => l.CheckForAttribute() && l.IsDisplayed() && !l.IsNullOrEmpty());
			var newLabelsAndDones = labels.Select(l => new LabelAndDone { Label = l }).ToList();
			foreach (var newLad in newLabelsAndDones)
			{
				LabelAndDone oldLabelAndDone = null;
				_dictionaryOfCurrentLabels?.TryGetValue(newLad.Label.Text, out oldLabelAndDone);
				newLad.IsDone = oldLabelAndDone?.IsDone ?? false;
			}
			_dictionaryOfCurrentLabels = newLabelsAndDones
				.Where(lad => lad.Label.IsDisplayed() && !lad.Label.IsNullOrEmpty())
				.ToDictionary(lad => lad.Label.Text, lad => lad);
			Debug.WriteLine($"Finding and Merging labels: {sw.Elapsed}");
			return newLabelsAndDones;
		}

		private static bool IsEverythingDone(List<LabelAndDone> labelAndDones)
		{
			return labelAndDones.All(lad => lad.IsDone);
		}

		private static void ChangeSelectElementValue(SelectElement selectElement)
		{
			int selectedIndex = -1;
			for (int cnt = 0; cnt < selectElement.Options.Count; cnt++)
			{
				if (selectElement.Options[cnt].Text == selectElement.SelectedOption.Text)
				{
					selectedIndex = cnt;
					break;
				}
			}
			int newSelectedIndex = (selectedIndex + 1) % selectElement.Options.Count;
			selectElement.SelectByIndex(newSelectedIndex);
		}

		private static IWebElement SwitchToTargetElementForLabel(this BasePage page, string labelText)
		{
			IWebElement webElement = page.GetTargetElementForLabel(labelText);
			if (webElement == null)
			{
				return null;
			}
			if (!webElement.Displayed)
			{
				page.ClickLabel(labelText);
				Thread.Sleep(100);
				webElement = page.WebDriver.SwitchTo().ActiveElement();
			}
			return webElement;
		}

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
			var inputType = webElement.GetAttribute("type");
			var placeholder = webElement.GetAttribute("placeholder");
			string originalText;
			if (!string.IsNullOrEmpty(inputType) && inputType == "date")
			{
				originalText = webElement.GetAttribute("value");
				DateTime dateTime;
				var format = "yyyy-MM-dd";
				if (DateTime.TryParseExact(originalText, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime))
				{
					dateTime = dateTime.AddMonths(1);
				}
				placeholder = placeholder.Replace("a", "y");
				return dateTime.ToString(placeholder);
			}
			originalText = webElement.GetAttribute("value");
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
				if (DateTime.TryParseExact(originalText, placeholder, CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime))
				{
					return dateTime.AddMonths(1).ToString(placeholder);
				}
				var placeholderParts = placeholder.ToLower().Split(dateSplitChar);
				monthIndex = Array.IndexOf(placeholderParts, "mm");
			}
			else
			{
				var monthParts = textParts.Where(p => p.Length <= 2 && Convert.ToInt32(p) <= 12).ToArray();
				if (monthParts.Length > 1)
				{
					monthParts = monthParts.Where(p => Convert.ToInt32(p) < 12).ToArray();
				}
				if (monthParts.Length == 0)
				{
					monthParts = new[] { textParts.First() };
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

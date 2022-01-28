namespace BasePageObjectModel
{
	public static class BasePageExtensions
	{
		public static void Is<T>(this BasePage page)
			where T : BasePage
		{
			var castedPage = page.As<T>();
			ServiceRegistry.Assert.IsNotNull(castedPage);
		}

		public static T As<T>(this BasePage page)
			where T : BasePage
		{
			return (T)page;
		}

		public static By ByPartialId(this BasePage page, string id)
		{
			return By.XPath($"//*[contains(@id,'{id}')]");
		}

		public static By ByPartialName(this BasePage page, string name)
		{
			return By.XPath($"//*[contains(@name,'{name}')]");
		}

		private static IWebElement FindLabel(this BasePage page, string labelText)
		{
			if (string.IsNullOrEmpty(labelText))
			{
				return null;
			}
			try
			{
				var label = page.WebDriver.FindElements(By.TagName("label"))
					.Where(e => e.IsDisplayed())
					.FirstOrDefault(e => e.DoesTextMatch(labelText));
				return label;
			}
			catch (Exception)
			{
				return null;
			}
		}

		public static IWebElement GetTargetElementForLabel(this BasePage page, string labelText)
		{
			var label = page.FindLabel(labelText);
			if (label == null)
			{
				return null;
			}
			return page.WebDriver.FindElement(By.Id(label.GetAttribute("for")));
		}

	}
}

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

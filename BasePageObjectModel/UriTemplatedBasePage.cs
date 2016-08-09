using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;

namespace BasePageObjectModel
{
	public class UriTemplatedBasePage : BasePage
	{
		protected UriTemplate PageUriTemplate { get; set; }

		public UriTemplatedBasePage(IWebDriver driver) : base(driver)
		{
		}

		public override bool IsUrlDisplayed()
		{
			if (base.IsUrlDisplayed())
			{
				return true;
			}

			var uriActual = new Uri(WebDriver.Url);
			var actualUrlLeftPart = uriActual.GetLeftPart(UriPartial.Path);
			var match = PageUriTemplate.Match(PageManager.Current.BaseUrl, new Uri(actualUrlLeftPart));
			return match != null;
		}
	}
}

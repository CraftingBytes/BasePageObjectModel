using System;

namespace BasePageObjectModel.Tests.Helpers
{
	internal class FooIdPage : BasePage
	{
		public FooIdPage(IWebDriver driver, int id)
			: base(driver)
		{
			PageUriTemplate = new UriTemplate.UriTemplate("foo/{id}");
			SetPageUrl(new Uri(PageManager.Current.BaseUrl, id.ToString()).ToString());
		}
	}
}
using DotnetGap;

namespace BasePageObjectModel.Tests.Helpers
{
	internal class FooIdPage : BasePage
	{
		public FooIdPage(IWebDriver driver, int id)
			: base(driver)
		{
			PageUriTemplate = new UriTemplate("foo/{id}");
			SetPageUrl(PageUriTemplate.BindByPosition(PageManager.Current.BaseUrl, id.ToString()));
		}
	}
}
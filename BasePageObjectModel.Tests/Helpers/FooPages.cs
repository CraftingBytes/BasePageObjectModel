using System.Linq;
using OpenQA.Selenium;

namespace BasePageObjectModel.Tests
{
	internal class FooPages : PageManager
	{
		public FooPages(string baseUrl) : base(baseUrl)
		{
		}

		public override void Initialize(IWebDriver webDriver = null)
		{
			base.Initialize(webDriver);
		}

		public static FooPage Foo => Current.BasePages.OfType<FooPage>().Single();
		public static FooIdPage FooId => Current.BasePages.OfType<FooIdPage>().Single();
	}
}
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
			int i = 17;
			base.Initialize(webDriver);
		}

		public FooPage Foo => Current.BasePages.OfType<FooPage>().Single();
		public FooIdPage FooId => Current.BasePages.OfType<FooIdPage>().Single();
	}
}
using BasePageObjectModel;
using TechTalk.SpecFlow;
using TestPageObjectModel;

namespace TestSpecs
{
	[Binding]
	public class BeforeAndAfter
	{

		[BeforeFeature()]
		public static void Before()
		{
			PageManager.Current = new Pages();
			PageManager.Current.Initialize();
		}

		[AfterFeature()]
		public static void After()
		{
			PageManager.Current.Dispose();
		}
	}
}

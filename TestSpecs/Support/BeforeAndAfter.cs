namespace TestSpecs.Support;

[Binding]
public class BeforeAndAfter
{

	[BeforeFeature]
	public static void Before()
	{
		PageManager.Current = new Pages();
		PageManager.Current.Initialize();
	}

	[AfterFeature]
	public static void After()
	{
		PageManager.Current.Dispose();
	}
}
using System.Linq;
using BasePageObjectModel;

namespace TestPageObjectModel
{
	public class Pages : PageManager
	{
		public Pages(string baseUrl = "http://localhost:5001/")
			: base(baseUrl)
		{
		}

		public static HomePage Home => Current.BasePages.OfType<HomePage>().Single();
		public static SecondPage Second => Current.BasePages.OfType<SecondPage>().Single();
		public static CreatePage Create => Current.BasePages.OfType<CreatePage>().Single();
	}
}

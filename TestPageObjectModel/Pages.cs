using System.Linq;
using BasePageObjectModel;

namespace TestPageObjectModel
{
	public class Pages : PageManager
	{
		public Pages(string baseUrl = "http://localhost:49970/")
			: base(baseUrl)
		{
		}

		public static HomePage Home => Current.BasePages.OfType<HomePage>().Single();
	}
}

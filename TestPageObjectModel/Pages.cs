using System;
using System.Linq;
using BasePageObjectModel;

namespace TestPageObjectModel
{
    public class Pages : DefaultPageManager
    {
		public Pages(string baseUrl = "http://localhost:49970/")
		{
			BaseUrl = new Uri(baseUrl);
		}

		public static HomePage Home => Current.BasePages.OfType<HomePage>().Single();
    }
}

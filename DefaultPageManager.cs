using OpenQA.Selenium.Firefox;
using System;
using System.Linq;
using OpenQA.Selenium.Chrome;

namespace BasePageObjectModel
{
	public class DefaultPageManager : PageManager
	{
		public override void Initialize()
		{
			if (WebDriver == null)
			{
				var options = new ChromeOptions();
				options.AddArguments("chrome.switches", "--disable-extensions");
				options.AddArgument("-incognito");
				WebDriver = new ChromeDriver(options);
			}

			base.Initialize();
		}


		public override BasePage CurrentPage
		{
			get
			{
				return BasePages.FirstOrDefault(page => page.IsUrlDisplayed());
			}
		}


		protected override Type PageAssembly()
		{
			return GetType();
		}
	}
}

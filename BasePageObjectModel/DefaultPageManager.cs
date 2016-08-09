using OpenQA.Selenium.Chrome;

namespace BasePageObjectModel
{
	public class DefaultPageManager : PageManager
	{
		protected DefaultPageManager(string baseUrl) 
			: base(baseUrl)
		{
		}

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
		
	}
}

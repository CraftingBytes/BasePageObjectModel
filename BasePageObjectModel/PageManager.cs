using OpenQA.Selenium;
using System;
using System.Linq;
using OpenQA.Selenium.Chrome;

namespace BasePageObjectModel
{
	public class PageManager : IDisposable
	{
		private static PageManager _current;

		public PageManager(string baseUrl)
		{
			BaseUrl = new Uri(baseUrl);
		}

		public virtual void Initialize()
		{
			if (WebDriver == null)
			{
				WebDriver = new ChromeDriver();
			}
		}

		public virtual void Dispose()
		{
			if (WebDriver != null)
			{
				WebDriver.Close();
				WebDriver.Quit();
			}
		}

		public static PageManager Current
		{
			get { return _current; }
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException(nameof(value));
				}
				_current = value;
			}
		}

		public T GetMatchingPage<T>() where T : BasePage
		{
			return BasePages.FirstOrDefault(p => p.GetType() == typeof(T)) as T;
		}

		public virtual BasePage CurrentPage
		{
			get
			{
				return BasePages.FirstOrDefault(page => page.IsUrlDisplayed());
			}
		}

		protected BasePage[] GetPagesInAssembly(PageManager pageManager)
		{
			var pages = from t in GetType().Assembly.GetTypes()
						where t.IsSubclassOf(typeof(BasePage))
							  && !t.IsAbstract
						select (BasePage)Activator.CreateInstance(t, WebDriver);
			return pages.ToArray();
		}
		
		private readonly Lazy<BasePage[]> basePages = new Lazy<BasePage[]>(() => Current.GetPagesInAssembly(Current));
		public BasePage[] BasePages => basePages.Value;

		public IWebDriver WebDriver { get; set; }
		public Uri BaseUrl { get; set; }
	}
}

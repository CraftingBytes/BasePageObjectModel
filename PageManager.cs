using System.Collections.Generic;
using OpenQA.Selenium;
using System;
using System.Linq;

namespace BasePageObjectModel
{
	public abstract class PageManager
	{
		private static PageManager _current;

		static PageManager()
		{
			_current = new DefaultPageManager();
		}

		public virtual void Initialize()
		{
			BasePages = GetPagesInAssembly();
		}

		public void Dispose()
		{
			WebDriver.Close();
			WebDriver.Quit();
		}

		public static PageManager Current
		{
			get { return _current; }
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				_current = value;
			}
		}

		public T GetMatchingPage<T>() where T : BasePage
		{
			return BasePages.FirstOrDefault(p => p.GetType() == typeof(T)) as T;
		}

		private BasePage[] GetPagesInAssembly()
		{
			var pages = from t in PageAssembly().Assembly.GetTypes()
						where t.IsSubclassOf(typeof(BasePage))
							  && !t.IsAbstract
						select (BasePage)Activator.CreateInstance(t, WebDriver);
			return pages.ToArray();
		}

		public BasePage[] BasePages { get; set; }
		public abstract BasePage CurrentPage { get; }
		public IWebDriver WebDriver { get; set; }
		public Uri BaseUrl { get; set; }
		protected abstract Type PageAssembly();
	}
}

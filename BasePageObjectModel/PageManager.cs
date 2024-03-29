﻿using OpenQA.Selenium.Chrome;

namespace BasePageObjectModel
{
	public class PageManager : IDisposable
	{
		private static PageManager _current;

		public PageManager(string baseUrl)
		{
			BaseUrl = new Uri(baseUrl);
		}

		public virtual void Initialize(IWebDriver webDriver = null)
		{
			WebDriver = webDriver;
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
				try
				{
					if (!string.IsNullOrEmpty(WebDriver.Url))
					{
						return BasePages.FirstOrDefault(page => page.IsUrlDisplayed());
					}
					return null;
				}
				catch (Exception)
				{
					return null;
				}
			}
		}

		protected BasePage[] GetPagesInAssembly(PageManager pageManager)
		{
			var pageTypes = from t in GetType().Assembly.GetTypes()
							where t.IsSubclassOf(typeof(BasePage)) && !t.IsAbstract
							select t;
			var pages = new List<BasePage>();
			foreach (var pageType in pageTypes)
			{
				if (pageType.GetConstructor(Type.EmptyTypes) == null)
				{
					var minimumParameterCount = pageType.GetConstructors().Min(construct => construct.GetParameters().Count());
					var constructorToUse = pageType.GetConstructors().First(construct => construct.GetParameters().Count() == minimumParameterCount);

					var paramList = new object[minimumParameterCount];
					paramList[0] = WebDriver;

					var parameters = constructorToUse.GetParameters();
					for (int i = 1; i < minimumParameterCount; i++)
					{
						paramList[i] = parameters[i].ParameterType.IsValueType ? Activator.CreateInstance(parameters[i].ParameterType) : null;
					}

					BasePage basePage = (BasePage)Activator.CreateInstance(pageType, paramList);
					pages.Add(basePage);
				}
			}
			return pages.ToArray();
		}

		private readonly Lazy<BasePage[]> basePages = new Lazy<BasePage[]>(() => Current.GetPagesInAssembly(Current));
		public BasePage[] BasePages => basePages.Value;

		public IWebDriver WebDriver { get; set; }
		public Uri BaseUrl { get; set; }
	}
}

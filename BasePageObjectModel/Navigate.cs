namespace BasePageObjectModel
{
	public static class Navigate
	{
		public static void LoadPage<T>() where T : BasePage, new()
		{
			var page = new T { WebDriver = PageManager.Current.WebDriver };
			page.ClickAndWaitForLoad(m => m.GoTo());
		}

		public static WaitNavigationContext<T> From<T>()
			where T : BasePage
		{
			var currentPage = PageManager.Current.CurrentPage;
			ServiceRegistry.Assert.IsNotNull(currentPage, "currentPage should not be null.");
			var current = currentPage as T;
			ServiceRegistry.Assert.IsNotNull(current, "Starting page is not " + typeof(T).Name);
			return new WaitNavigationContext<T>(PageManager.Current.WebDriver, current);
		}

		public static PostbackNavigationContext<T> On<T>()
			where T : BaseElementContainer
		{
			var current = PageManager.Current.CurrentPage as T;
			ServiceRegistry.Assert.IsNotNull(current, "Starting page is not " + typeof(T).Name);
			return new PostbackNavigationContext<T>(PageManager.Current.WebDriver, current);
		}
	}
}

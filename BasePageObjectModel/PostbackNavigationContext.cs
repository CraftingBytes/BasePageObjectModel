namespace BasePageObjectModel
{
	public class PostbackNavigationContext<TFrom> : BaseNavigationContext<TFrom> where TFrom : BaseElementContainer
	{
		public PostbackNavigationContext(IWebDriver driver, TFrom fromPage)
			: base(driver, fromPage)
		{
		}

		public void PostbackUsing(Action<TFrom> func)
		{
			FromPage.PerformActionOnElementAndWaitForReload(func);
		}
	}
}
using System;
using OpenQA.Selenium;

namespace BasePageObjectModel
{
	public class WaitNavigationContext<TFrom> : BaseNavigationContext<TFrom> where TFrom : BasePage
	{
		public WaitNavigationContext(IWebDriver driver, TFrom fromPage)
			: base(driver, fromPage)
		{
		}

		public WaitNavigationContext<TFrom> Using(Action<TFrom> func)
		{
			FromPage.ClickAndWaitForLoad(func);
			return this;
		}

		public void To<T>()
			where T : BasePage
		{
			var target = PageManager.Current.GetMatchingPage<T>();
			To(target);
		}

		public void To<T>(T target)
			where T : BasePage
		{
			AssertFailOnErrorPage(target);
			AssertCorrectPageLoaded(target);
		}
	}
}
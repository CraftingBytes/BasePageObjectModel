using OpenQA.Selenium;
using System;
using System.Linq;
using System.Threading;

namespace BasePageObjectModel
{
    public static class WaitExtensions
    {
        public static void ClickAndWaitForLoad<T>(this T page, Action<T> action) where T : BasePage
        {
            string urlBeforeClick = page.WebDriver.Url;
            action(page);
            page.WaitForLoad(urlBeforeClick);
        }

        public static void WaitForLoad(this BasePage basePage, string urlBeforeClick)
        {
            WaitFor(() => basePage.WebDriver.Url != urlBeforeClick);
        }

        public static bool WaitFor(Func<bool> check)
        {
            return WaitFor(check, TimeSpan.FromSeconds(10), TimeSpan.FromMilliseconds(50));
        }

        public static bool WaitFor(Func<bool> check, TimeSpan totalTimeToWait, TimeSpan intervalToPoll)
        {
	        DateTime startTime = DateTime.UtcNow;
            TimeSpan elapsedTime = TimeSpan.Zero;
            while (elapsedTime < totalTimeToWait)
            {
                bool result = false;
                try
                {
                    result = check();
                }
                catch (Exception)
                {
                    //eating, because exceptions are just false
                }
                if (result) return true;
                Thread.Sleep(intervalToPoll);
                elapsedTime = DateTime.UtcNow - startTime;
            }
            return false;
        }

        public static IWebElement ClickAndWaitForElementVisibility<T>(this T page, Action<T> action, Func<T, By> elementIdentFunc, bool isVisible)
            where T : BaseElementContainer
        {
            action(page);

            if (isVisible)
            {
                return WaitForElementVisible(page, elementIdentFunc);
            }
            return WaitForElementInvisible(page, elementIdentFunc);
        }

        public static IWebElement WaitForElementInvisible<T>(this T page, Func<T, By> elementIdentFunc) where T : BaseElementContainer
        {
            var elementIdentifier = elementIdentFunc(page);
            WaitFor(() =>
            {

                var elements = page.WebDriver.FindElements(elementIdentifier).ToList();
                return !elements.Any() || !elements.First().Displayed;
            });

            return null;
        }

        private static IWebElement WaitForElementVisible<T>(this T page, Func<T, By> elementIdentFunc) where T : BaseElementContainer
        {
            var elementIdentifier = elementIdentFunc(page);
            WaitFor(() =>
            {
                var elements = page.WebDriver.FindElements(elementIdentifier);
                return elements.Any() && elements.First().Displayed;
            });
            return page.WebDriver.FindElement(elementIdentifier);
        }

        public static void PerformActionOnElementAndWaitForReload<T>(this T page, Action<T> action) where T : BaseElementContainer
        {
            var oldPageSource = page.WebDriver.PageSource;

            action(page);

            WaitFor(() => page.WebDriver.PageSource != oldPageSource);
        }
    }
}

using OpenQA.Selenium;
using System;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;

namespace BasePageObjectModel
{
    public static class WaitExtensions
    {
        public static bool ClickAndWaitForLoad<T>(this T page, Action<T> action) where T : BaseBasePage
        {
            string urlBeforeClick = page.WebDriver.Url;
            action(page);
            return page.WaitForLoad(urlBeforeClick);
        }

        public static bool WaitForLoad(this BaseBasePage basePage, string urlBeforeClick)
        {
            return WaitFor(() => basePage.WebDriver.Url != urlBeforeClick);
        }

        public static bool WaitFor(Func<bool> check, int timeToWaitInMilliseconds = 10000)
        {
            return WaitFor(check, TimeSpan.FromMilliseconds(timeToWaitInMilliseconds), TimeSpan.FromMilliseconds(50));
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
                if (result)  return true;
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
                var elementIsDisplayed = elements.Any() && elements.First().Displayed;

                if (!elementIsDisplayed)
                {
                    Debug.WriteLine(elementIdentifier + " is not displayed.");
                }

                return elementIsDisplayed;
            });
            return page.WebDriver.FindElement(elementIdentifier);
        }

        public static bool PerformActionOnElementAndWaitForReload<T>(this T page, Action<T> action) where T : BaseElementContainer
        {
            var oldPageSource = page.WebDriver.PageSource;

            action(page);

            return WaitFor(() => page.WebDriver.PageSource != oldPageSource);
        }
    }
}

using OpenQA.Selenium;

namespace BasePageObjectModel
{
    public class BasePopup : BaseElementContainer
    {
        protected BasePopup(IWebDriver driver)
            : base(driver)
        {
            WebDriver = driver;
        }
    }
}

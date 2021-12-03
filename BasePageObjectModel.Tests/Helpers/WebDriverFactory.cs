namespace BasePageObjectModel.Tests.Helpers
{
	public static class WebDriverFactory
	{
		public static Mock<IWebDriver> CreateMockWebDriver()
		{
			var mockNavigation = new Mock<INavigation>();
			var mockWebDriver = new Mock<IWebDriver>();
			mockWebDriver.Setup(wd => wd.Navigate()).Returns(mockNavigation.Object);
			return mockWebDriver;
		}

	}
}
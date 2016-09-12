using System;
using BasePageObjectModel;
using NUnit.Framework;
using TechTalk.SpecFlow;
using TestPageObjectModel;

namespace TestSpecs
{
	[Binding]
	public class HomePageSteps
	{
		[Given(@"I am on the home page")]
		public void GivenIAmOnTheHomePage()
		{
			Pages.Home.GoTo();
		}

		[Then(@"the title should be showing")]
		public void ThenTheTitleShouldBeShowing()
		{
			var title = Pages.Home.GetTitle();
			Assert.IsNotNull(title);
		}

		[When(@"I click on the second page link")]
		public void WhenIClickOnTheSecondPageLink()
		{
			Navigate.From<HomePage>()
				.Using(hp => hp.ClickSecondPageLink())
				.To<SecondPage>();
		}

		[Then(@"I should be on the second page")]
		public void ThenIShouldBeOnTheSecondPage()
		{
			Assert.IsTrue(Pages.Second.IsUrlDisplayed());
		}
	}
}

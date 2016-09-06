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
	}
}

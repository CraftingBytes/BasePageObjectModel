
namespace TestSpecs.StepDefinitions;

[Binding]
public class HomePageSteps
{
	public Dictionary<string, string> LabelToValue { get; private set; } = new();

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

	[Given(@"I am on the create page")]
	public void GivenIAmOnTheCreatePage()
	{
		Pages.Create.GoTo();
	}

	[When(@"I fill out the form with all kinds of characters")]
	public void WhenIFillOutTheFormWithAllKindsOfCharacters(Table table)
	{
		LabelToValue = table.ToDictionary();
		Pages.Create.FillOutForm(LabelToValue);
		//Pages.Create.FillOutFormByPartialIds(LabelToValue);
	}

	[When(@"I click Create")]
	public void WhenIClickCreate()
	{
		Pages.Create.ClickCreate();
	}

	[Then(@"I should see the new value in the list")]
	public void ThenIShouldSeeTheNewValueInTheList()
	{
		var value = LabelToValue.First().Value;
		bool contains = Pages.Home.GetTableBody().Any(s => s.Contains(value));
		Assert.IsTrue(contains);
	}

}
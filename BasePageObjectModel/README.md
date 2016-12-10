# BasePageObjectModel

The BasePageObjectModel comes from the work that CraftingBytes has done writing BDD style tests for web projects.
When we began each new project we noticed that we wanted some of the utilities that we had built in the last project.
So finally we open sourced it and published it to NuGet.

## What comes in the box

Here are the main things you get in the BasePageObjectModel:
* An easier way to manage all of the pages in the model and ask which one is currently displayed
* A more definitive way of navigating between pages that makes it more obvious if something goes wrong
* A simple way of checking multiple times if an element is present (similar to FindElement, this is GetElement)
* A way of filling out forms in a simple and readable manner
* A way of verifying that forms have been filled out correctly
* Dealing with popups
* Automating WebForms (ByPartialId, and ByPartialName)
* Adding to the query strings to URLs you are navigating to
* A way of extracting the data from tables

## Getting started

To use BasePageObjectModel start by creating a PageObjectModel project and adding a NuGet reference to BasePageObjectModel for whatever unit testing framework you will be using.
e.g. BasePageObjectModel.MsTest, BasePageObjectModel.NUnit, or BasePageObjectModel.XUnit.
NOTE: remember to set the Dependency setting to highest, or add the reference to BasePageObjectModel first, and then add the reference to the specific unit testing assembly.

Now that you have both assembly references in your project create a specific BasePage class for your system and inheriting from the specific type of testing framework 
example:
```csharp
public abstract class UcsBasePage: NunitBasePage
	{
		protected UcsBasePage(IWebDriver driver) : base(driver)
		{
		}
```
This makes it easier if you decide to switch unit testing frameworks in the future.

Next define your first page (a login page for example):
```csharp
	public class LoginPage: UcsBasePage
	{
		public LoginPage(IWebDriver driver) : base(driver)
		{
			SetPageUrl("/login");
		}

		public void EnterUserName(string userName)
		{
			var userNameTextBox = GetElement(By.Id("userName"));
			userNameTextBox.Clear();
			userNameTextBox.SendKeys(userName);
		}

		public void EnterPassword(string password)
		{
			var passwordBox = GetElement(By.Id("password"));
			passwordBox.Clear();
			passwordBox.SendKeys(password);
		}

		public void ClickLogin()
		{
			GetElement(By.Id("btnLogin")).Click();
		}

		public void LoginAs(Ucs.TestUtilities.TestUser user)
		{
			EnterUserName(user.UserName);
			EnterPassword(user.Password);
			ClickLogin();
		}
	}
```

Then define the manager to hold your pages.  This needs to define the BaseUrl used for the system.  If you want to launch your browser in a non-standard way (say in Spanish) override the Initialize method:
```csharp
	public class UcsPages : PageManager
	{
		public static LoginPage Login => Current.BasePages.OfType<LoginPage>().Single();
		// ...

#if DEBUG
		public UcsPages() : base("http://localhost:52010/")
#else
		public UcsPages() : base("http://local.universalclubsystems.com/")
#endif
		{
		}

		public override void Initialize(IWebDriver webDriver = null)
		{
			var options = new ChromeOptions();
			options.AddArguments("chrome.switches", "--disable-extensions");
			options.AddArgument("-incognito");
			options.AddArgument("--lang=es");
			WebDriver = webDriver ?? new ChromeDriver(options);
		}
	}
```

Then to use the PageObjectModel, just add a reference to the project and use it like this:
```csharp
		[Given(@"I am not logged in")]
		public void GivenIAmNotLoggedIn()
		{
			UcsPages.Home.GoTo();
			UcsPages.Home.LogOutIfNecessary();
		}

		[Given(@"I am on the login page")]
		public void GivenIAmOnTheLoginPage()
		{
			UcsPages.Login.GoTo();
		}

		[When(@"I login as a membership admin")]
		public void WhenILoginAsAMembershipAdmin()
		{
			Navigate.From<LoginPage>()
				.Using(p => p.LoginAs(TestUsers.Miguel))
				.To<MembersListPage>();
		}
```




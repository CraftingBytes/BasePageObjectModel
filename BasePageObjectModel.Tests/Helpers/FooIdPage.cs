using System;
using System.Collections.Generic;
using OpenQA.Selenium;

namespace BasePageObjectModel.Tests
{
	internal class FooIdPage : BasePage
	{
		public FooIdPage(IWebDriver driver, int id)
			: base(driver)
		{
			PageUriTemplate = new UriTemplate.Core.UriTemplate("foo/{id}");


			//TODO Not sure if this conversion if quite right... It was 
			// BindByPosition
			var dict = new Dictionary<string, string>()
			{
				{ "id", id.ToString() }
			};

			SetPageUrl(PageUriTemplate.BindByName(dict).ToString());
		}
	}
}
namespace BasePageObjectModel.Tests
{
	[TestClass]
	public class EditFormHelperTests
	{
		[TestMethod]
		public void TestGetNewDateText_NoPlaceholder()
		{
			Assert.AreEqual("", EditFormHelper.GetNewDateTextInternal("", null));
			Assert.AreEqual("02/01/1983", EditFormHelper.GetNewDateTextInternal("01/01/1983", null));
			Assert.AreEqual("02/12/1983", EditFormHelper.GetNewDateTextInternal("1/12/1983", null));
			Assert.AreEqual("12/02/1983", EditFormHelper.GetNewDateTextInternal("12/1/1983", null));
			Assert.AreEqual("01/12/1983", EditFormHelper.GetNewDateTextInternal("12/12/1983", null));
			// if one is above then the lower must be manipulated
			Assert.AreEqual("13/03/1983", EditFormHelper.GetNewDateTextInternal("13/02/1983", null));
			Assert.AreEqual("03/13/1983", EditFormHelper.GetNewDateTextInternal("02/13/1983", null));
			Assert.AreEqual("20/01/2027", EditFormHelper.GetNewDateTextInternal("20/12/2027", null));
			Assert.AreEqual("20/07/2027", EditFormHelper.GetNewDateTextInternal("20/06/2027", null));
			Assert.AreEqual("2017-07-20", EditFormHelper.GetNewDateTextInternal("2017-06-20", null));
		}

		[TestMethod]
		public void TestGetNewDateText_WithPlaceholder()
		{
			Assert.AreEqual("", EditFormHelper.GetNewDateTextInternal("", "MM/yyyy"));
			Assert.AreEqual("02/1983", EditFormHelper.GetNewDateTextInternal("01/1983", "MM/yyyy"));
			//different language
			Assert.AreEqual("02/1983", EditFormHelper.GetNewDateTextInternal("01/1983", "MM/aaaa"));
			//different separator
			Assert.AreEqual("02-1983", EditFormHelper.GetNewDateTextInternal("01-1983", "MM-yyyy"));

			// day first
			Assert.AreEqual("01/02/1983", EditFormHelper.GetNewDateTextInternal("01/01/1983", "dd/MM/yyyy"));
			//different language
			Assert.AreEqual("01/02/1983", EditFormHelper.GetNewDateTextInternal("01/01/1983", "dd/MM/aaaa"));
			Assert.AreEqual("2017-07-20", EditFormHelper.GetNewDateTextInternal("2017-06-20", "dd-MM-aaaa"));
		}


		[TestMethod]
		public void TestGetNewNumberText()
		{
			Assert.AreEqual("1", EditFormHelper.GetNewNumberTextInternal("0", null, null));
			Assert.AreEqual("18", EditFormHelper.GetNewNumberTextInternal("17", null, null));

			Assert.AreEqual("1", EditFormHelper.GetNewNumberTextInternal("5", minText: null, maxText: "5"));
			Assert.AreEqual("2", EditFormHelper.GetNewNumberTextInternal("5", minText: "2", maxText: "5"));
		}

		[TestMethod]
		public void TestCompareDates()
		{
			Assert.AreEqual(true, EditFormHelper.CompareDates("2027-12-20", "20/12/2027"));
		}
	}
}

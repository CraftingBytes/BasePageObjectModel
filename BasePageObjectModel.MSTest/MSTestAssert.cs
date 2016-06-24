using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BasePageObjectModel.MSTest
{
	public class MsTestAssert : IAssert
	{
		public void Fail(string format, params object[] args)
		{
			Assert.Fail(format, args);
		}

		public void AreEqual(string expected, string actual)
		{
			Assert.AreEqual(expected,actual);
		}

		public void IsNotNull(object actual, string message = null)
		{
			Assert.IsNotNull(actual,message);
		}
	}
}

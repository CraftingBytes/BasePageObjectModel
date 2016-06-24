using Xunit;

namespace BasePageObjectModel.xUnit
{
	public class XUnitAssert : IAssert
	{
		public void Fail(string format, params object[] args)
		{
			Assert.True(false,string.Format(format,args));
		}

		public void AreEqual(string expected, string actual)
		{
			Assert.Equal(expected,actual);
		}

		public void IsNotNull(object actual, string message = null)
		{
			Assert.NotNull(actual);
		}
	}
}
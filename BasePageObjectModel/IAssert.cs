namespace BasePageObjectModel
{
	public interface IAssert
	{
		void Fail(string format, params object[] args);
		void AreEqual(string expected, string actual);
		void IsNotNull(object actual, string message = null);
	}
}
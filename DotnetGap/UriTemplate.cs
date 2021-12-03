namespace DotnetGap;

public class UriTemplate
{
	public UriTemplate(string v)
	{
		V = v;
	}

	public string V { get; }

	public bool Match(Uri baseAddress, Uri candidate)
	{
		throw new NotImplementedException();
	}

	public string BindByPosition(Uri currentBaseUrl, string toString)
	{
		throw new NotImplementedException();
	}
}
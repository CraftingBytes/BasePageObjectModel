namespace TestSpecs.Support;

public static class TableExtensions
{
	public static Dictionary<string, string> ToDictionary(this Table table)
	{
		return table.Rows.ToDictionary(r => r[0], r => r[1]);
	}
}
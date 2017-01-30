using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace TestSpecs
{
	public static class TableExtensions
	{
		public static Dictionary<string, string> ToDictionary(this Table table)
		{
			return table.Rows.ToDictionary(r => r[0], r => r[1]);
		}
	}
}

using NUnit.Framework;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TessaTest
{
	[TestClass]
	public class Test
	{
		[TestInitialize]
		public void TestCase ()
		{
			Tessa.Tessa.PlaceIn ("asd");
		}
	}
}


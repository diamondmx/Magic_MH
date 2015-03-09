using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Magic.Core.Tests
{
	class PlayerTests
	{
		[Test]
		public void TestOpponents()
		{
			var testPlayerName = "sut";
			var _sut = new Player(testPlayerName);

			var match1 = new Match(_sut, testPlayerName, new Player("Result1"), "Result1", "TEST", 1, 0, 0, 0, false);
			var match2 = new Match(_sut, testPlayerName, new Player("Result2"), "Result2", "TEST", 1, 0, 0, 0, false);
			var match3 = new Match(new Player("Result3"), "Result3", _sut, testPlayerName, "TEST", 1, 0, 0, 0, false);
			var match4 = new Match(new Player("Result4"), "Result4", _sut, testPlayerName, "TEST", 1, 0, 0, 0, false);


		}
	}
}

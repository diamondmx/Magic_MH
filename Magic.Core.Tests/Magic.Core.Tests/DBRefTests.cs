using System;
using NUnit.Framework;
using System.Linq;

namespace Magic.Core.Tests
{
    [TestFixture]
    public class DBRefTests
    {
        [TestCase("TEST", 2, "TESTPLAYER1", "TESTPLAYER2")]
        public void MatchUpdate(string eventName, int round, string p1, string p2)
        {
            var _sut = new dbMatch();
            Assert.AreEqual(true, _sut.Read(eventName, round, p1, p2));

            int p1wins = _sut.Player1Wins;
            int p2wins = _sut.Player2Wins;
            int draws = _sut.Draws;
            bool inprog = _sut.InProgress;

            _sut.Player1Wins = 1 - p1wins;
            _sut.Player2Wins = 1 - p2wins;
            _sut.Draws = 1 - draws;
            _sut.InProgress = !_sut.InProgress;

            _sut.Update();

            var _sut2 = new dbMatch();
            Assert.AreEqual(true, _sut2.Read(eventName, round, p1, p2));

            Assert.AreEqual(1 - p1wins, _sut2.Player1Wins);
            Assert.AreEqual(1- p2wins, _sut2.Player2Wins);
            Assert.AreEqual(1- draws, _sut2.Draws);
            Assert.AreEqual(!inprog, _sut2.InProgress);
        }

        [TestCase("TEST", 1, "TESTPLAYER1", "TESTPLAYER2", 1, 0, 1, false)]
        public void MatchRead(string eventName, int round, string p1, string p2, int expectedP1Wins, int expectedP2Wins, int expectedDraws, bool expectedInProg)
        {
            var _sut = new dbMatch();
            Assert.AreEqual(true, _sut.Read(eventName, round, p1, p2));
            
            Assert.AreEqual(eventName, _sut.Event);
            Assert.AreEqual(round, _sut.Round);
            Assert.AreEqual(p1, _sut.Player1);
            Assert.AreEqual(p2, _sut.Player2);
            Assert.AreEqual(expectedP1Wins, _sut.Player1Wins);
            Assert.AreEqual(expectedP2Wins, _sut.Player2Wins);
            Assert.AreEqual(expectedDraws, _sut.Draws);
            Assert.AreEqual(expectedInProg, _sut.InProgress);            
        }

        [Test]
        public void TestEventLoad()
        {
            var _sut = new Magic.Core.Event();
            _sut.LoadEvent("FRF");
	        foreach (var p in _sut.Players)
					{
						Assert.AreEqual(p.matches.Count(), 8);
						foreach (var match in p.matches)
						{
							Assert.IsNotEmpty(match.Player1Name);
							Assert.IsNotEmpty(match.Player2Name);
						}
	        }
        }
    }
}

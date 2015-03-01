using System;
using NUnit.Framework;
using System.Linq;

namespace Magic.Core.Tests
{
    [TestFixture]
    public class DBRefTests
    {
        [Test]
        public void MatchUpdate()
        {
            var _sut = new dbMatch
            {
                Player1 = "TESTUSER1",
                Player2 = "TESTUSER2",
                Event = "TEST",
                Round = 1,
                Player1Wins = 0,
                Player2Wins = 0,
                InProgress = false,
                Draws = 0
            };
        }

        [TestCase("TEST", 1, "TESTUSER1", "TESTUSER2", 3, 4, false)]
        public void MatchRead(string eventName, int round, string p1, string p2, int expectedP1Wins, int expectedP2Wins, bool expectedInProg)
        {
            var _sut = new dbMatch();
            Assert.AreEqual(true, _sut.Read(eventName, round, p1, p2));
            
            Assert.AreEqual(eventName, _sut.Event);
            Assert.AreEqual(round, _sut.Round);
            Assert.AreEqual(p1, _sut.Player1);
            Assert.AreEqual(p2, _sut.Player2);
            Assert.AreEqual(expectedP1Wins, _sut.Player1Wins);
            Assert.AreEqual(expectedP2Wins, _sut.Player2Wins);
            Assert.AreEqual(expectedInProg, _sut.InProgress);            
        }
    }
}

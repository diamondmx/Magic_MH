using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magic.Core
{
    using NUnit.Framework;

    [TestFixture]
    public class EventTests
    {
        [Test]
        public void TestEventLoad()
        {
            var _sut = new Magic.Core.Event();
            _sut.LoadEvent("FRF");
        }
    }

    public class Event
    {
        public List<Core.Player> Players;
        public List<Core.Match> Matches;
        public string name;
        public int rounds;
        public int currentRound;

        public void LoadEvent(string eventName)
        {
            name = eventName;
            
            Players = new List<Player>();
            dbPlayer.LoadDBPlayers().Where(p => p.Active).ToList().ForEach(p => Players.Add(new Player(p)));
            
            Matches = new List<Match>();
            dbMatch.LoadDBMatches(name).Where(m=>m.Event==eventName).ToList().ForEach(m => Matches.Add(new Match(m)));

            foreach(var p in Players)
            {
                foreach(var m in Matches)
                {
                    if (m.Player1Name == p.name)
                        m.Player1 = p;

                    else if (m.Player2Name == p.name)
                        m.Player2 = p;

                    p.matches.Add(m);
                }
            }
        }
    }
}
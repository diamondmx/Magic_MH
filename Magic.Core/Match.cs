using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magic.Core
{
	public class Match
	{
        public Player Player1;
        public string Player1Name;
        public Player Player2;
        public string Player2Name;
		public int Round;
		public string Event;
		public int Player1Wins;
		public int Player2Wins;
		public int Draws;
		public bool InProgress;

		public Match()
		{ }

		public Match(string p1, string p2, string eventName, int round, int p1wins, int p2wins, int draws, bool inpr)
		{
            Player1Name = p1;
            Player2Name = p2;
			Event = eventName;
			Round = round;
			Player1Wins = p1wins;
			Player2Wins = p2wins;
			Draws = draws;
			InProgress = inpr;
		}

        public Match(Player p1, Player p2, string eventName, int round, int p1wins, int p2wins, int draws, bool inpr)
        {
            Player1 = p1;
            Player1Name = p1.name;
            Player2 = p2;
            Player2Name = p2.name;
            Event = eventName;
            Round = round;
            Player1Wins = p1wins;
            Player2Wins = p2wins;
            Draws = draws;
            InProgress = inpr;
        }

		public Match(dbMatch m)
            :this(m.Player1, m.Player2, m.Event, m.Round, m.Player1Wins, m.Player2Wins, m.Draws, m.InProgress)
        {
		}

		public Match Flipped()
		{
			return new Match(p1: Player2, p2: Player1, p1wins: Player2Wins, p2wins: Player1Wins, eventName: Event, round: Round, draws: Draws, inpr: InProgress);
		}

        public Match WithPlayerOneAs(string name)
        {
            if (Player1.name.ToLower() == name.ToLower())
                return this;
            else if (Player2.name.ToLower() == name.ToLower())
                return this.Flipped();
            else
                throw new InvalidOperationException("Bad Parameter");
        }

        public void Update()
        {
            dbMatch.Update(this);
        }
    }
}

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
        private string _player1Name;
        public string Player1Name
        {
            get { return Player1!=null ? Player1.name : _player1Name; }
        }
        public Player Player2;
        private string _player2Name;
        public string Player2Name
        {
            get { return Player2 != null ? Player2.name : _player2Name; }
        }
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
            _player1Name = p1;
            _player2Name = p2;
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
            Player2 = p2;
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

        public void Copy(Match m)
        {
            Player1 = m.Player1;
            _player1Name = m._player1Name;
            Player2 = m.Player2;
            _player2Name = m._player2Name;
            Event = m.Event;
            Round = m.Round;
            Player1Wins = m.Player1Wins;
            Player2Wins = m.Player2Wins;
            Draws = m.Draws;
            InProgress = m.InProgress;
        }

		public Match Flipped()
		{
			return new Match(p1: Player2, p2: Player1, p1wins: Player2Wins, p2wins: Player1Wins, eventName: Event, round: Round, draws: Draws, inpr: InProgress);
		}

        public Match WithPlayerOneAs(string name)
        {
            if (Player1Name.ToLower() == name.ToLower())
                return this;
            else if (Player2Name.ToLower() == name.ToLower())
                return this.Flipped();
            else
                throw new InvalidOperationException("Bad Parameter for Flipped(): Name did not match either player in match");
        }

        public void SetPlayerOneTo(string name)
        {
            Copy(WithPlayerOneAs(name));
        }

        public void Update()
        {
            var dbmatch = new dbMatch(this);
            dbmatch.Update();
        }
    }
}

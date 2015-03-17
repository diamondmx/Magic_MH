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

        public dbMatch myDbMatch;

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

        public Match(Player p1, string player1name, Player p2, string player2name, string eventName, int round, int p1wins, int p2wins, int draws, bool inpr)
        {
            Player1 = p1;
	        _player1Name = player1name;
            Player2 = p2;
	        _player2Name = player2name;
            Event = eventName;
            Round = round;
            Player1Wins = p1wins;
            Player2Wins = p2wins;
            Draws = draws;
            InProgress = inpr;

						if (String.IsNullOrEmpty(_player1Name))
							throw new Exception();
						if (String.IsNullOrEmpty(_player2Name))
							throw new Exception();

						if (Player1 == null)
							throw new Exception();
						if (Player2 == null)
							throw new Exception();
        }

		public Match(dbMatch m)
            :this(m.Player1, m.Player2, m.Event, m.Round, m.Player1Wins, m.Player2Wins, m.Draws, m.InProgress)
        {
            myDbMatch = m;
		}

        public void Save()
        {
            if(myDbMatch==null)
            {
                myDbMatch = new dbMatch();
                myDbMatch.Event = Event;
                myDbMatch.Round = Round;
                myDbMatch.Player1 = Player1.name;
                myDbMatch.Player2 = Player2.name;
            }
            
            myDbMatch.Player1Wins = Player1Wins;
            myDbMatch.Player2Wins = Player2Wins;
            myDbMatch.Draws = Draws;

            myDbMatch.Save();
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

	        if (String.IsNullOrEmpty(_player1Name))
		        throw new Exception();
					if (String.IsNullOrEmpty(_player2Name))
						throw new Exception();

					if(Player1==null)
						throw new Exception();
					if (Player2 == null)
						throw new Exception();
        }

		public Match Flipped()
		{
			return new Match(p1: Player2, player1name: Player2Name, p2: Player1, player2name:Player1Name, p1wins: Player2Wins, p2wins: Player1Wins, eventName: Event, round: Round, draws: Draws, inpr: InProgress);
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

        public bool DidPlayerWin(string name)
        {
            if(Player1Name==name)
            {
                return Player1Wins > Player2Wins;
            }
            else if(Player2Name==name)
            {
                return Player2Wins > Player1Wins;
            }
            else
            {
                throw new Exception("Player not in match");
            }
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

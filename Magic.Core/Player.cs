using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magic.Core
{
	public class Player
	{
		public string name;
		public List<Player> round1Players;
		public List<Player> round2Players;
		public List<Player> CurrentPlayers;
		public int Score;

		public Player(string newName, int newScore)
		{
			name = newName;
			Score = newScore;
			round1Players = new List<Player>();
			round2Players = new List<Player>();
			CurrentPlayers = new List<Player>();
		}
	}
}

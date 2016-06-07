using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magic.Domain
{
	public class PlayerScoreSummary
	{
		public string Name;
		public int GameWins;
		public int GameLosses;
		public int GameDraws;
		public int MatchWins;
		public int MatchLosses;
		public int MatchDraws;
		public float MatchWinPercentage
		{
			get
			{
				var matches = MatchWins + MatchDraws + MatchLosses;
				return ((float)MatchWins / matches) * 100.0f;
			}
		}

		public float GameWinPercentage
		{
			get
			{
				var games = GameWins + GameDraws + GameLosses;
				return ((float)GameWins / games) * 100.0f;
			}
		}
	}
}

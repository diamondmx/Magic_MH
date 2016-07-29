using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
		[DisplayFormat(DataFormatString = "{0:P1}")]
		public float MatchWinPercentage
		{
			get
			{
				var matches = MatchWins + MatchDraws + MatchLosses;
				return ((float)MatchWins / matches);
			}
		}

		[DisplayFormat(DataFormatString = "{0:P1}")]
		public float GameWinPercentage
		{
			get
			{
				var games = GameWins + GameDraws + GameLosses;
				return ((float)GameWins / games);
			}
		}
	}
}

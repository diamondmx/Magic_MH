using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magic.Domain
{
	public class PlayerScoreSummary
	{
		public List<PlayerScoreItem> OpponentScoreItems;
		public PlayerScoreItem Totals;
		public int LeagueTopEights;
		public int LeagueWins;
	}
}

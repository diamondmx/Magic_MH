using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magic.Domain
{
	[Table(Name = "Matches")]
	public class dbMatch
	{
		[Column()]
		public string Player1;
		[Column()]
		public string Player2;
		[Column()]
		public int Round;
		[Column()]
		public string Event;
		[Column()]
		public int Player1Wins;
		[Column()]
		public int Player2Wins;
		[Column()]
		public int Draws;
		[Column()]
		public bool InProgress;

		//public dbMatch(Magic.Core.Match m)
		//{
		//	Player1 = m.Player1Name;
		//	Player2 = m.Player2Name;
		//	Event = m.Event;
		//	Round = m.Round;
		//	Player1Wins = m.Player1Wins;
		//	Player2Wins = m.Player2Wins;
		//	Draws = m.Draws;
		//	InProgress = m.InProgress;
		//}player

		public dbMatch()
		{ }

		public void Copy(dbMatch m)
		{
			this.Player1 = m.Player1;
			this.Player2 = m.Player2;
			this.Event = m.Event;
			this.Round = m.Round;
			this.Player1Wins = m.Player1Wins;
			this.Player2Wins = m.Player2Wins;
			this.Draws = m.Draws;
			this.InProgress = m.InProgress;
		}
	}
}

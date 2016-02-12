using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magic.Domain
{
	[Table(Name = "Events")]
	public class dbEvent
	{
		[Column()]
		public string Name;
		[Column()]
		public DateTime StartDate;
		[Column()]
		public DateTime RoundEndDate;
		[Column()]
		public Int32 Rounds;
		[Column()]
		public Int32 CurrentRound;
		[Column()]
		public Int32 RoundMatches;
		[Column()]
		public bool Locked;
		public string dbName;
	}
}

using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magic.Domain
{
	[Table(Name = "EventPlayers")]
	public class dbEventPlayers
	{
		[Column()]
		public string EventName;

		[Column()]
		public string Player;

		[Column]
		public int PlayerID;

		[Column()]
		public int Dropped;
	}
}

using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magic.Domain
{
	[Table(Name="RoundPrizes")]
	public class dbRoundPrize
	{
		[Column]
		public string EventName;

		[Column]
		public int Round;

		[Column]
		public int Position;

		[Column]
		public int Packs;

		[Column]
		public string Other;
	}
}

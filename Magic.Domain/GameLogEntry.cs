using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magic.Domain
{
	[Table(Name = "GameLog")]
	public class GameLogEntry
	{
		[Column] public string Description;
		[Column] public string User;
		[Column(Name = "Event")] public string EventName;
		[Column] public object Details;
		//[Column] public readonly DateTime Timestamp;
	}
}

using Magic.Domain;
using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magic.Data
{
	public class EventPlayerRepository : IEventPlayerRepository
	{
		private readonly IDataContextWrapper _dataContext;

		public EventPlayerRepository(IDataContextWrapper dataContext)
		{
			_dataContext = dataContext;
		}

		public List<dbEventPlayers> LoadDBEventPlayers(string eventName)
		{
			return _dataContext.GetTable<dbEventPlayers>().Where(ep => ep.EventName == eventName).ToList();
		}
	}
}

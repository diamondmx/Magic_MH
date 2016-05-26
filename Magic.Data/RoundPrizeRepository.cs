using Magic.Domain;
using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magic.Data
{
	public class RoundPrizeRepository : IRoundPrizeRepository
	{
		private readonly IDataContextWrapper _dataContext;

		public RoundPrizeRepository(IDataContextWrapper dataContext)
		{
			_dataContext = dataContext;
		}

		public List<dbRoundPrize> LoadDBRoundPrizes(string eventName)
		{
			return _dataContext.GetTable<dbRoundPrize>().Where(ep => ep.EventName == eventName).ToList();
		}
	}
}

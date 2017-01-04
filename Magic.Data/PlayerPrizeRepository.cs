using Magic.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magic.Data
{
	public class PlayerPrizeRepository : IPlayerPrizeRepository
	{
		private readonly IDataContextWrapper _dataContext;
		private readonly IRoundPrizeRepository _roundPrizeRepo;

		public PlayerPrizeRepository(IDataContextWrapper dataContext)
		{
			_dataContext = dataContext;
		}

		public List<dbRoundPrize> GetAwardedPrizes(string playerName)
		{
			var playerPrizes = _dataContext.GetTable<dbPlayerPrize>();
			throw new NotImplementedException();
		}
	}
}

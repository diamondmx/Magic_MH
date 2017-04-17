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

		public PlayerPrizeRepository(IDataContextWrapper dataContext)
		{
			_dataContext = dataContext;
		}

		public List<dbPlayerPrize> GetAwardedPrizes(string playerName)
		{
			var playerPrizes = _dataContext.GetTable<Magic.Domain.dbPlayerPrize>();
			return playerPrizes.ToList();
		}

		public void MarkRecieved(List<dbPlayerPrize> acknowledgedList)
		{
			StringBuilder sqlBuilder = new StringBuilder();

			foreach(var prize in acknowledgedList)
			{
				sqlBuilder.Append($"UPDATE [PlayerPrizes] SET Recieved = {prize.Packs}, Complete = 1 WHERE ");
				sqlBuilder.Append($" ('{prize.EventName}'=EventName AND '{prize.Round}'=Round AND '{prize.Position}'=Position AND '{prize.Packs}'=Packs AND '{prize.Recieved}'=Recieved);");
      }

			var outputstring = sqlBuilder.ToString();

			_dataContext.ExecuteCommand(outputstring);
		}
	}
}

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

		public List<dbPlayerPrize> GetAllAwardedPrizes()
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
				sqlBuilder.Append($" ('{prize.EventName}'=EventName AND '{prize.PlayerID}'=PlayerID AND '{prize.Round}'=Round AND '{prize.Position}'=Position AND '{prize.Packs}'=Packs AND '{prize.Recieved}'=Recieved);");
      }
			
			var outputString = sqlBuilder.ToString();
			_dataContext.ExecuteCommand(outputString);
		}

		public void AssignPrizes(List<dbPlayerPrize> assignedPrizes)
		{
			StringBuilder sqlBuilder = new StringBuilder();

			sqlBuilder.Append("INSERT INTO [dbo].[PlayerPrizes] ([PlayerID], [Player],[EventName],[Round],[Position],[Packs],[Recieved],[Notes],[Complete]) VALUES");

			foreach(var prize in assignedPrizes)
			{
				sqlBuilder.Append($"('{prize.PlayerID}', '{prize.Player}', '{prize.EventName}',{prize.Round}, {prize.Position}, {prize.Packs}, 0, NULL, 0),");
      }

			var outputString = sqlBuilder.ToString();
			outputString = outputString.Remove(outputString.Length - 1, 1);

			// above removes the last character (a comma) from the string
			// str "ABCDEF" Length 6, index of F==5, (start 5, count 1) => remove F.

			_dataContext.ExecuteCommand(outputString);
		}


	}
}

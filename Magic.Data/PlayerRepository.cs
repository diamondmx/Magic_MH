using Magic.Domain;
using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magic.Data
{
	public class PlayerRepository : IPlayerRepository
	{
		private readonly IDataContextWrapper _dataContext;
		public PlayerRepository(IDataContextWrapper dataContext)
		{
			_dataContext = dataContext;
		}

		public List<dbPlayer> LoadDBPlayers()
		{
			var playersTable = _dataContext.GetTable<dbPlayer>().ToList();
			return playersTable;
		}

		public void Save()
		{
			//var sqlUpdate = String.Format("UPDATE Players SET CurrentRound={0}, Rounds={1}, RoundMatches={2} WHERE Name='{3}'", currentRound, rounds, roundMatches, Name);
			//db.ExecuteCommand(sqlUpdate);
		}
	}
}

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
		private List<dbPlayer> PlayerList
		{
			get
			{
				var playersTable = _dataContext.GetTable<dbPlayer>().ToList();
				return playersTable;
			}
		}

		private readonly IDataContextWrapper _dataContext;
		public PlayerRepository(IDataContextWrapper dataContext)
		{
			_dataContext = dataContext;
		}

		public List<dbPlayer> LoadDBPlayers()
		{
			return PlayerList;
		}

		public void Save(dbPlayer oldPlayer, dbPlayer newPlayer)
		{
			var sqlUpdate = String.Format("UPDATE Players SET Name='{0}', Email='{1}' WHERE ID={2}	", newPlayer.Name, newPlayer.Email, oldPlayer.ID);
			_dataContext.ExecuteCommand(sqlUpdate);
		}

		public List<Player> GetAllPlayers()
		{
			var dbPlayers = LoadDBPlayers();
			return dbPlayers.Select(dbp => new Player(dbp)).ToList();
		}

		public string GetPlayerName(int playerID)
		{
			var foundPlayer = PlayerList.FirstOrDefault(p => p.ID == playerID);
			if(foundPlayer!= null)
			{
				return foundPlayer.Name;
			}
			else
			{
				return null;
			}
		}
	}
}

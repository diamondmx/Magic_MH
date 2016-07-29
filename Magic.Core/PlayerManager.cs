using Magic.Data;
using Magic.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magic.Core
{
	public interface IPlayerManager
	{
		List<Player> GetAllPlayers();
	}

	public class PlayerManager : IPlayerManager
	{
		private readonly IPlayerRepository _playerRepo;

		public PlayerManager(IPlayerRepository playerRepo)
		{
			_playerRepo = playerRepo;
		}

		public List<Player> GetAllPlayers()
		{
			return _playerRepo.GetAllPlayers();
		}
	}
}

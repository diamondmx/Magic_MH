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
		Player GetPlayerByEmail(string email);
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

		public Player GetPlayerByEmail(string email)
		{
			var player = _playerRepo.GetAllPlayers().FirstOrDefault(p => p.Email == email);
			return player;
		}
	}
}

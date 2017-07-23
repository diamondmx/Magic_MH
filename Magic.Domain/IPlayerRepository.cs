using Magic.Domain;
using System.Collections.Generic;

namespace Magic.Domain
{
	public interface IPlayerRepository
	{
		System.Collections.Generic.List<dbPlayer> LoadDBPlayers();
		void Save(dbPlayer oldPlayer, dbPlayer newPlayer);
		List<Player> GetAllPlayers();
		string GetPlayerName(int key);
	}
}
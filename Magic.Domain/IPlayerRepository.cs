using Magic.Domain;
using System.Collections.Generic;

namespace Magic.Data
{
	public interface IPlayerRepository
	{
		System.Collections.Generic.List<dbPlayer> LoadDBPlayers();
		void Save();
		List<Player> GetAllPlayers();
	}
}
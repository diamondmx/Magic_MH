using Magic.Domain;

namespace Magic.Data
{
	public interface IPlayerRepository
	{
		System.Collections.Generic.List<dbPlayer> LoadDBPlayers();
		void Save();
	}
}
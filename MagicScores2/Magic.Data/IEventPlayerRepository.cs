using Magic.Domain;

namespace Magic.Data
{
	public interface IEventPlayerRepository
	{
		System.Collections.Generic.List<dbEventPlayers> LoadDBEventPlayers(string eventName);
	}
}
using Magic.Domain;
using System.Collections.Generic;

namespace Magic.Data
{
	public interface IMatchRepository
	{
		List<Match> LoadDBMatches(string mtgEvent);
    void Insert(Match m);
		Match Read(string eventName, int round, int p1, int p2);
    void Save(Match m);
		void Update(Match m);
		void UpdatePairing(Match m);
		void UpdateAllMatches(IEnumerable<Match> matches);
		List<dbMatch> GetAllMatches(int playerID);
		void PopulateMatch(List<Player> players, Match m);
		void Delete(Match m);
		int GetMatchCountInRound(string eventName, int round);
		void DeleteAllInRound(string eventName, int round);
	}
}
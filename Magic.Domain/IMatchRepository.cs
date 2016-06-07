using Magic.Domain;
using System.Collections.Generic;

namespace Magic.Data
{
	public interface IMatchRepository
	{
		List<Match> LoadDBMatches(string mtgEvent);
    void Insert(Match m);
		Match Read(string eventName, int round, string p1, string p2);
    void Save(Match m);
		void Update(Match m);
		void UpdateAllMatches(IEnumerable<Match> matches);
		List<dbMatch> GetAllMatches(string playerName);
	}
}
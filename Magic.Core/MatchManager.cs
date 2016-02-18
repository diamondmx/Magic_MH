using Magic.Data;
using Magic.Domain;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magic.Core
{
	public interface IMatchManager
	{
		void Update(Match m);
		void UpdateAllMatches(List<Match> matches, int round);
  }

	public class MatchManager : IMatchManager
	{
		private IMatchRepository _matchRepository;

		public MatchManager(IMatchRepository matchRepo)
		{
			_matchRepository = matchRepo;
		}
		
		public void Update(Match m)
		{
			_matchRepository.Update(m);
		}

		public void Save(Match m)
		{
			_matchRepository.Save(m);
		}

		public void UpdateAllMatches(List<Match> matches, int round)
		{
			IEnumerable<Match> relevantMatches;
      if (round==0)
			{
				relevantMatches = matches;
			}
			else
			{
				relevantMatches = matches.Where(m => m.Round == round);
			}

			_matchRepository.UpdateAllMatches(relevantMatches);
		}
	}
}

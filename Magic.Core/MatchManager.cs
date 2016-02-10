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
	}
}

using Magic.Data;
using Magic.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magic.Core
{
	public interface IPrizeManager
	{
		void SavePrizes(List<dbRoundPrize> input);
	}

	public class PrizeManager : IPrizeManager
	{
		IRoundPrizeRepository _roundPrizeRepo;

		public PrizeManager(IRoundPrizeRepository roundPrizeRepo)
		{
			_roundPrizeRepo = roundPrizeRepo;
		}

		public void SavePrizes(List<dbRoundPrize> input)
		{
			_roundPrizeRepo.SaveDBRoundPrizes(input);
		}
	}
}

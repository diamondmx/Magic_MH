using Magic.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magic.Data
{
	public interface IRoundPrizeRepository
	{
		List<dbRoundPrize> LoadDBRoundPrizes(string eventName);
		void SaveDBRoundPrizes(List<dbRoundPrize> input);
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magic.Domain
{
	public interface IPlayerPrizeRepository
	{
		List<dbRoundPrize> GetAwardedPrizes(string playerName);
	}
}

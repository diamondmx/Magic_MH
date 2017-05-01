using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magic.Domain
{
	public interface IPlayerPrizeRepository
	{
		List<dbPlayerPrize> GetAllAwardedPrizes();
		void MarkRecieved(List<dbPlayerPrize> acknowledgedList);
		void AssignPrizes(List<dbPlayerPrize> prizeAssignments);

	}
}

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
		IEnumerable<dbPlayerPrize> GetPlayerPrizes(int playerID);
		void AcknowledgeRecievedAll(int playerID, List<dbPlayerPrize> acknowledgedList);
		IEnumerable<dbPlayerPrize> GetUncollectedPlayerPrizes(int playerID);
		void AssignPrizes(List<dbPlayerPrize> prizeAssignments);
		IEnumerable<dbPlayerPrize> GetAllPlayerPrizes();
	}

	public class PrizeManager : IPrizeManager
	{
		IRoundPrizeRepository _roundPrizeRepo;
		IPlayerPrizeRepository _playerPrizeRepo;

		public PrizeManager(IRoundPrizeRepository roundPrizeRepo, IPlayerPrizeRepository playerPrizeRepo)
		{
			_roundPrizeRepo = roundPrizeRepo;
			_playerPrizeRepo = playerPrizeRepo;
		}

		public void SavePrizes(List<dbRoundPrize> input)
		{
			_roundPrizeRepo.SaveDBRoundPrizes(input);
		}

		public IEnumerable<dbPlayerPrize> GetUncollectedPlayerPrizes(int playerID)
		{
			var list = GetPlayerPrizes(playerID);
			return list.Where(pp => pp.Recieved != pp.Packs);
		}

		public IEnumerable<dbPlayerPrize> GetPlayerPrizes(int playerID)
		{
			return _playerPrizeRepo.GetAllAwardedPrizes().Where(p => p.PlayerID == playerID);
		}

		public IEnumerable<dbPlayerPrize> GetAllPlayerPrizes()
		{
			return _playerPrizeRepo.GetAllAwardedPrizes();
		}

		public static string FormatPrizeInfo(IEnumerable<dbPlayerPrize> prizes)
		{
			StringBuilder prizelist = new StringBuilder();
			
			foreach(var prize in prizes)
			{
				int packsRemaining = prize.Packs - prize.Recieved;
				if(packsRemaining!=0)
				{
					prizelist.AppendLine($"({prize.EventName}:{prize.Round}) {packsRemaining} packs");
				}
      }
			
			if(string.IsNullOrWhiteSpace(prizelist.ToString()))
			{
			 	return "";
			}

			return "Prizes to collect: " + prizelist.ToString();
		}

		public void AcknowledgeRecievedAll(int playerID, List<dbPlayerPrize> acknowledgedList)
		{
			var playerPrizes = GetPlayerPrizes(playerID);

			foreach(var prize in acknowledgedList)
			{
				if (!playerPrizes.Contains(prize))
				{
					acknowledgedList.Remove(prize);
				}
			}

			_playerPrizeRepo.MarkRecieved(acknowledgedList);

		}

		public void AssignPrizes(List<dbPlayerPrize> prizeAssignments)
		{
			_playerPrizeRepo.AssignPrizes(prizeAssignments);
		}
	}
}
